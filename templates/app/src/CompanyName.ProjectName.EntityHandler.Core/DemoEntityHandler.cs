using CompanyName.ProjectName.Demo.Eto;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;

namespace CompanyName.ProjectName.EntityHandler.Core
{
    public class DemoEntityHandler : EntityHandlerBase,
        IDistributedEventHandler<EntityCreatedEto<DemoEto>>,
        IDistributedEventHandler<EntityUpdatedEto<DemoEto>>,
        IDistributedEventHandler<EntityDeletedEto<DemoEto>>
    {
        public DemoEntityHandler()
        {
        }
        
        public async Task HandleEventAsync(EntityCreatedEto<DemoEto> eventData)
        {
        }

        public async Task HandleEventAsync(EntityDeletedEto<DemoEto> eventData)
        {
        }

        public async Task HandleEventAsync(EntityUpdatedEto<DemoEto> eventData)
        {
        }
    }
}