using AElf.AElfNode.EventHandler.BackgroundJob;
using AElf.AElfNode.EventHandler.BackgroundJob.Processors;
using AElf.Contracts.ProjectName;

namespace CompanyName.ProjectName.ContractEventHandler.Core.Processors
{
    internal class TransferredProcessor : AElfEventProcessorBase<DemoEvent>
    {
        //private readonly IService _service;
        public TransferredProcessor(
            //IService service
            )
        {
            //_service = service;
        }

        protected override async Task HandleEventAsync(DemoEvent eventDetailsEto, EventContext txInfoDto)
        {
            //await _service.DoSomething();
        }
    }
}