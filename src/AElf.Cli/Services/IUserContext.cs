using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Services
{
    public interface IUserContext
    {
        string GetEndpoint();
        string GetAccount();
        string GetPassword();

        void Init(string endpoint, string address, string password);
    }

    public class UserUserContext : IUserContext, ISingletonDependency
    {
        private readonly IConfigService _configService;

        private string _endpoint;
        private string _account;
        private string _password;

        public UserUserContext(IConfigService configService)
        {
            _configService = configService;
        }

        public string GetEndpoint()
        {
            if (string.IsNullOrWhiteSpace(_endpoint))
            {
                _endpoint = _configService.Get(AElfCliConsts.EndpointConfigKey);
            }

            return _endpoint;
        }

        public string GetAccount()
        {
            if (string.IsNullOrWhiteSpace(_account))
            {
                _account = _configService.Get(AElfCliConsts.AccountConfigKey);
            }

            return _account;
        }

        public string GetPassword()
        {
            if (string.IsNullOrWhiteSpace(_password))
            {
                _password = _configService.Get(AElfCliConsts.PasswordConfigKey);
            }

            return _password;
        }

        public void Init(string endpoint, string account, string password)
        {
            _endpoint = endpoint;
            _account = account;
            _password = password;
        }
    }
}