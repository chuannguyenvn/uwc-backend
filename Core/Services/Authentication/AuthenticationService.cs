using Commons.Categories;
using Repositories.Managers;
using Commons.Communications.Authentication;
using Commons.Communications.Map;
using Commons.Communications.Mcps;
using Commons.Communications.Status;
using Commons.Models;
using Commons.RequestStatuses;
using Commons.RequestStatuses.Authentication;
using Commons.Types;
using Commons.Types.SettingOptions;
using Hubs;
using Newtonsoft.Json;
using Services.Mcps;

namespace Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Commons.Types.Settings _settings;

    public AuthenticationService(IUnitOfWork unitOfWork, Commons.Types.Settings settings)
    {
        _unitOfWork = unitOfWork;
        _settings = settings;
    }

    public ParamRequestResult<LoginResponse> Login(LoginRequest request)
    {
        if (!_unitOfWork.AccountRepository.DoesUsernameExist(request.Username)) return new ParamRequestResult<LoginResponse>(new UsernameNotExist());

        var account = _unitOfWork.AccountRepository.GetByUsername(request.Username);
        if (account.PasswordHash != AuthenticationHelpers.ComputeHash(request.Password, account.PasswordSalt))
            return new ParamRequestResult<LoginResponse>(new IncorrectPassword());

        var userProfile = _unitOfWork.UserProfileRepository.GetById(account.UserProfileId);

        var userRole = userProfile.UserRole;
        if (request.IsFromDesktop && userRole != UserRole.Supervisor)
            return new ParamRequestResult<LoginResponse>(new InvalidRoleWhenLogin());
        if (!request.IsFromDesktop && userRole == UserRole.Supervisor)
            return new ParamRequestResult<LoginResponse>(new InvalidRoleWhenLogin());

        var loginResponse = new LoginResponse
        {
            Credentials = CreateCredentials(account),
            InitializationData = CreateInitializationData(account.Id),
        };

        return new ParamRequestResult<LoginResponse>(new Success(), loginResponse);
    }

    public ParamRequestResult<RegisterResponse> Register(RegisterRequest request)
    {
        if (_unitOfWork.AccountRepository.DoesUsernameExist(request.Username))
            return new ParamRequestResult<RegisterResponse>(new UsernameAlreadyExist());

        var account = new Account
        {
            Username = request.Username,
            PasswordHash = request.Password,
            UserProfile = new UserProfile
            {
                UserRole = request.UserRole,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                Address = request.Address,
                CreatedTimestamp = DateTime.Now,
                AvatarColorHue = Random.Shared.NextSingle() * 360f,
            }
        };
        account.GenerateSaltAndHash();
        _unitOfWork.AccountRepository.Add(account);

        var setting = new Setting
        {
            Account = account,
            DarkMode = ToggleOption.Off,
            ColorblindMode = ToggleOption.Off,
            ReducedMotionMode = ToggleOption.Off,
            Language = LanguageOption.English,
            Messages = ToggleOption.On,
            EmployeesLoggedIn = ToggleOption.On,
            EmployeesLoggedOut = ToggleOption.On,
            McpsAlmostFull = ToggleOption.On,
            McpsFull = ToggleOption.On,
            McpsEmptied = ToggleOption.On,
            SoftwareUpdateAvailable = ToggleOption.On,
            OnlineStatus = OnlineStatusOption.Online
        };
        _unitOfWork.SettingRepository.Add(setting);

        _unitOfWork.Complete();

        var registerResponse = new RegisterResponse
        {
            Credentials = CreateCredentials(account),
            InitializationData = CreateInitializationData(account.Id),
        };

        return new ParamRequestResult<RegisterResponse>(new Success(), registerResponse);
    }

    public ParamRequestResult<LoginResponse> LoginWithFace(LoginWithFaceRequest request)
    {
        Console.WriteLine(JsonConvert.SerializeObject(request));
        return new ParamRequestResult<LoginResponse>(new Success());
    }

    public ParamRequestResult<RegisterFaceResponse> RegisterFace(RegisterFaceRequest request)
    {
        Console.WriteLine(JsonConvert.SerializeObject(request));
        return new ParamRequestResult<RegisterFaceResponse>(new Success());
    }

    private Credentials CreateCredentials(Account account)
    {
        return new Credentials()
        {
            JwtToken = AuthenticationHelpers.GenerateJwtToken(account, _settings.BearerKey),
            AccountId = account.Id,
        };
    }

    private InitializationData CreateInitializationData(int accountId)
    {
        var allMcps = _unitOfWork.McpDataRepository.GetAll().ToList();
        var setting = _unitOfWork.SettingRepository.GetById(accountId);
        setting.Account = null;

        return new InitializationData
        {
            McpLocationBroadcastData = new McpLocationBroadcastData() { LocationByIds = allMcps.ToDictionary(mcp => mcp.Id, mcp => mcp.Coordinate) },
            McpFillLevelBroadcastData = new McpFillLevelBroadcastData() { FillLevelsById = McpFillLevelService.FillLevelsById },
            OnlineStatusBroadcastData = new OnlineStatusBroadcastData() { OnlineAccountIds = BaseHub.ConnectionIds.Keys.ToList() },
            Setting = setting,
        };
    }
}