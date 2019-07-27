// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

#if !DISABLE_REACTIVE_COMPONENTS
using System.Collections.Generic;
using Unity.Entities;

namespace Dinopark.Plants
{
    public partial class Harvestable
    {
        public class CommandSenders
        {
            public struct Harvest : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Plants.Harvestable.Harvest.Request> RequestsToSend
                {
                    get => global::Dinopark.Plants.Harvestable.ReferenceTypeProviders.HarvestSenderProvider.Get(CommandListHandle);
                    set => global::Dinopark.Plants.Harvestable.ReferenceTypeProviders.HarvestSenderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandRequests
        {
            public struct Harvest : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Plants.Harvestable.Harvest.ReceivedRequest> Requests
                {
                    get => global::Dinopark.Plants.Harvestable.ReferenceTypeProviders.HarvestRequestsProvider.Get(CommandListHandle);
                    set => global::Dinopark.Plants.Harvestable.ReferenceTypeProviders.HarvestRequestsProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponders
        {
            public struct Harvest : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Plants.Harvestable.Harvest.Response> ResponsesToSend
                {
                    get => global::Dinopark.Plants.Harvestable.ReferenceTypeProviders.HarvestResponderProvider.Get(CommandListHandle);
                    set => global::Dinopark.Plants.Harvestable.ReferenceTypeProviders.HarvestResponderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponses
        {
            public struct Harvest : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Plants.Harvestable.Harvest.ReceivedResponse> Responses
                {
                    get => global::Dinopark.Plants.Harvestable.ReferenceTypeProviders.HarvestResponsesProvider.Get(CommandListHandle);
                    set => global::Dinopark.Plants.Harvestable.ReferenceTypeProviders.HarvestResponsesProvider.Set(CommandListHandle, value);
                }
            }
        }
    }
}
#endif
