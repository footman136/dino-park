// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

#if !DISABLE_REACTIVE_COMPONENTS
using System.Collections.Generic;
using Unity.Entities;

namespace Dinopark.Player
{
    public partial class PlayerAttrs
    {
        public class CommandSenders
        {
            public struct LayEgg : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Player.PlayerAttrs.LayEgg.Request> RequestsToSend
                {
                    get => global::Dinopark.Player.PlayerAttrs.ReferenceTypeProviders.LayEggSenderProvider.Get(CommandListHandle);
                    set => global::Dinopark.Player.PlayerAttrs.ReferenceTypeProviders.LayEggSenderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandRequests
        {
            public struct LayEgg : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Player.PlayerAttrs.LayEgg.ReceivedRequest> Requests
                {
                    get => global::Dinopark.Player.PlayerAttrs.ReferenceTypeProviders.LayEggRequestsProvider.Get(CommandListHandle);
                    set => global::Dinopark.Player.PlayerAttrs.ReferenceTypeProviders.LayEggRequestsProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponders
        {
            public struct LayEgg : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Player.PlayerAttrs.LayEgg.Response> ResponsesToSend
                {
                    get => global::Dinopark.Player.PlayerAttrs.ReferenceTypeProviders.LayEggResponderProvider.Get(CommandListHandle);
                    set => global::Dinopark.Player.PlayerAttrs.ReferenceTypeProviders.LayEggResponderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponses
        {
            public struct LayEgg : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Player.PlayerAttrs.LayEgg.ReceivedResponse> Responses
                {
                    get => global::Dinopark.Player.PlayerAttrs.ReferenceTypeProviders.LayEggResponsesProvider.Get(CommandListHandle);
                    set => global::Dinopark.Player.PlayerAttrs.ReferenceTypeProviders.LayEggResponsesProvider.Set(CommandListHandle, value);
                }
            }
        }
    }
}
#endif
