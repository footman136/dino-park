// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

#if !DISABLE_REACTIVE_COMPONENTS
using System.Collections.Generic;
using Unity.Entities;

namespace Dinopark.Npc
{
    public partial class EggData
    {
        public class CommandSenders
        {
            public struct Hatch : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Npc.EggData.Hatch.Request> RequestsToSend
                {
                    get => global::Dinopark.Npc.EggData.ReferenceTypeProviders.HatchSenderProvider.Get(CommandListHandle);
                    set => global::Dinopark.Npc.EggData.ReferenceTypeProviders.HatchSenderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandRequests
        {
            public struct Hatch : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Npc.EggData.Hatch.ReceivedRequest> Requests
                {
                    get => global::Dinopark.Npc.EggData.ReferenceTypeProviders.HatchRequestsProvider.Get(CommandListHandle);
                    set => global::Dinopark.Npc.EggData.ReferenceTypeProviders.HatchRequestsProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponders
        {
            public struct Hatch : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Npc.EggData.Hatch.Response> ResponsesToSend
                {
                    get => global::Dinopark.Npc.EggData.ReferenceTypeProviders.HatchResponderProvider.Get(CommandListHandle);
                    set => global::Dinopark.Npc.EggData.ReferenceTypeProviders.HatchResponderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponses
        {
            public struct Hatch : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Npc.EggData.Hatch.ReceivedResponse> Responses
                {
                    get => global::Dinopark.Npc.EggData.ReferenceTypeProviders.HatchResponsesProvider.Get(CommandListHandle);
                    set => global::Dinopark.Npc.EggData.ReferenceTypeProviders.HatchResponsesProvider.Set(CommandListHandle, value);
                }
            }
        }
    }
}
#endif
