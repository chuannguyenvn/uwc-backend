using System.Text;
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
                CreatedTimestamp = DateTime.UtcNow,
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
        using var client = new HttpClient();
        // Convert each image byte array to Base64
        var base64Images = request.Images.Select(imageBytes => Convert.ToBase64String(imageBytes)).ToList();

        // Create a JSON payload with the image data
        var requestData = new
        {
            images = base64Images,
            // Include any other necessary data in your request
        };

        // Convert the JSON payload to string
        var jsonRequest = JsonConvert.SerializeObject(requestData);

        // Set the content type to JSON
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        // Send the request to your Azure Function
        var httpResponse = client.PostAsync(Constants.VERIFY_FACE_API, content).Result;

        // Process the Azure Function response
        var responseContent = httpResponse.Content.ReadAsStringAsync().Result;
        Console.WriteLine(responseContent);

        var account = _unitOfWork.AccountRepository.GetByUsername(request.Username);

        var loginResponse = new LoginResponse
        {
            Credentials = CreateCredentials(account),
            InitializationData = CreateInitializationData(account.Id),
        };

        return new ParamRequestResult<LoginResponse>(new Success(), loginResponse);
    }

    public ParamRequestResult<RegisterFaceResponse> RegisterFace(RegisterFaceRequest request)
    {
        using var client = new HttpClient();
        // Convert each image byte array to Base64
        var base64Images = request.Images.Select(imageBytes => Convert.ToBase64String(imageBytes)).ToList();

        // Create a JSON payload with the image data
        var requestData = new
        {
            images = base64Images,
            // Include any other necessary data in your request
        };

        // Convert the JSON payload to string
        var jsonRequest = JsonConvert.SerializeObject(requestData);

        // Set the content type to JSON
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        // Send the request to your Azure Function
        var httpResponse = client.PostAsync(Constants.REGISTER_FACE_API, content).Result;

        // Process the Azure Function response
        var responseContent = httpResponse.Content.ReadAsStringAsync().Result;
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(responseContent);
        Console.WriteLine();
        Console.WriteLine();

        return new ParamRequestResult<RegisterFaceResponse>(new Success(), new RegisterFaceResponse
        {
            Success = true,
        });
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