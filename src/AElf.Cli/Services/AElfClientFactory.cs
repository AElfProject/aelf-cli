using AElf.Client.Service;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Services
{
    public class AElfClientFactory : ISingletonDependency
    {
        private AElfClient _client;
        private readonly IUserContext _userContext;

        public AElfClientFactory(IUserContext userContext)
        {
            _userContext = userContext;
        }

        public AElfClient CreateClient(string endpoint = null)
        {
            if (endpoint == null)
            {
                if (_client == null)
                {
                    _client = new AElfClient(_userContext.Endpoint);
                }
            }
            else
            {
                _client = new AElfClient(endpoint);
            }

            return _client;
        }
    }
}