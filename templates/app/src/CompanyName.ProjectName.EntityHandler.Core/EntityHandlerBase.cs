using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.ObjectMapping;

namespace CompanyName.ProjectName.EntityHandler.Core
{
    public class EntityHandlerBase : ITransientDependency
    {
        protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetRequiredService<IObjectMapper>();
        protected IDistributedEventBus DistributedEventBus => LazyServiceProvider.LazyGetRequiredService<IDistributedEventBus>();
        
        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }
    }
}