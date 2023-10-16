using Commons.Categories;
using Repositories.Managers;
using Commons.Communications.Authentication;
using Commons.Models;
using Commons.RequestStatuses;
using Commons.RequestStatuses.Authentication;

namespace Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Settings _settings;

    public AuthenticationService(IUnitOfWork unitOfWork, Settings settings)
    {
        _unitOfWork = unitOfWork;
        _settings = settings;
    }

    public ParamRequestResult<LoginResponse> Login(LoginRequest request)
    {
        if (!_unitOfWork.Accounts.DoesUsernameExist(request.Username)) return new ParamRequestResult<LoginResponse>(new UsernameNotExist());

        var account = _unitOfWork.Accounts.GetByUsername(request.Username);
        if (account.PasswordHash != AuthenticationHelpers.ComputeHash(request.Password, account.PasswordSalt))
            return new ParamRequestResult<LoginResponse>(new IncorrectPassword());

        var userRole = account.UserRole;
        if (request.IsFromDesktop && userRole != UserRole.Supervisor)
            return new ParamRequestResult<LoginResponse>(new InvalidRoleWhenLogin());
        if (!request.IsFromDesktop && userRole == UserRole.Supervisor)
            return new ParamRequestResult<LoginResponse>(new InvalidRoleWhenLogin());

        var loginResponse = new LoginResponse
        {
            Credentials = CreateCredentials(account),
            InitializationData = CreateInitializationData(),
        };

        return new ParamRequestResult<LoginResponse>(new Success(), loginResponse);
    }

    public ParamRequestResult<RegisterResponse> Register(RegisterRequest request)
    {
        if (_unitOfWork.Accounts.DoesUsernameExist(request.Username)) return new ParamRequestResult<RegisterResponse>(new UsernameAlreadyExist());

        var account = new Account
        {
            Username = request.Username,
            PasswordHash = request.Password,
        };
        account.GenerateSaltAndHash();
        _unitOfWork.Accounts.Add(account);
        _unitOfWork.Complete();

        var registerResponse = new RegisterResponse
        {
            Credentials = CreateCredentials(account),
            InitializationData = CreateInitializationData(),
        };

        return new ParamRequestResult<RegisterResponse>(new Success(), registerResponse);
    }

    private Credentials CreateCredentials(Account account)
    {
        return new Credentials()
        {
            JwtToken = AuthenticationHelpers.GenerateJwtToken(account, _settings.BearerKey),
            AccountId = account.Id,
        };
    }

    private InitializationData CreateInitializationData()
    {
        var allMcps = _unitOfWork.McpData.GetAll().ToList();
        return new InitializationData
        {
            McpLocationByIds = allMcps.ToDictionary(mcp => mcp.Id, mcp => mcp.Coordinate)
        };
    }
}