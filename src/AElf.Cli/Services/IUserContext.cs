using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Services;

public interface IUserContext
{
    string Endpoint { get; set; }
    string Account { get; set; }
    string Password { get; set; }
}

public class UserUserContext : IUserContext, ISingletonDependency
{
    private readonly IConfigService _configService;
    private string _account;

    private string _endpoint;
    private string _password;

    public UserUserContext(IConfigService configService)
    {
        _configService = configService;
    }


    public string Endpoint
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_endpoint))
                _endpoint = _configService.Get(AElfCliConstants.EndpointConfigKey);

            return _endpoint;
        }
        set => _endpoint = value;
    }

    public string Account
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_account)) _account = _configService.Get(AElfCliConstants.AccountConfigKey);

            return _account;
        }
        set => _account = value;
    }

    public string Password
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_password))
                _password = _configService.Get(AElfCliConstants.PasswordConfigKey);

            return _password;
        }
        set => _password = value;
    }
}