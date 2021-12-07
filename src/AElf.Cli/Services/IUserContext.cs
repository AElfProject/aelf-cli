using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Services
{
    public interface IUserContext
    {
        string GetEndpoint();
        string GetAddress();
        string GetPassword();

        void Init(string endpoint, string address, string password);
    }

    public class UserUserContext : IUserContext, ISingletonDependency
    {
        private readonly IConfigService _configService;

        private string _endpoint;
        private string _address;
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

        public string GetAddress()
        {
            if (string.IsNullOrWhiteSpace(_endpoint))
            {
                _address = _configService.Get(AElfCliConsts.AddressConfigKey);
            }

            return _address;
        }

        public string GetPassword()
        {
            if (string.IsNullOrWhiteSpace(_endpoint))
            {
                _password = _configService.Get(AElfCliConsts.PasswordConfigKey);
            }

            return _password;
        }

        public void Init(string endpoint, string address, string password)
        {
            _endpoint = endpoint;
            _address = endpoint;
            _password = password;
        }
    }
}