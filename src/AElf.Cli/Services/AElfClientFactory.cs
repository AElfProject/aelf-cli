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

        public AElfClient CreateClient()
        {
            if (_client == null)
            {
                _client = new AElfClient(_userContext.Endpoint);
            }

            return _client;
        }
    }
}