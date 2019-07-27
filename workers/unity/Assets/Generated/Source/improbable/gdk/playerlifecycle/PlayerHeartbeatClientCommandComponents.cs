// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

#if !DISABLE_REACTIVE_COMPONENTS
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    public partial class PlayerHeartbeatClient
    {
        public class CommandSenders
        {
            public struct PlayerHeartbeat : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.PlayerHeartbeat.Request> RequestsToSend
                {
                    get => global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatSenderProvider.Get(CommandListHandle);
                    set => global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatSenderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandRequests
        {
            public struct PlayerHeartbeat : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.PlayerHeartbeat.ReceivedRequest> Requests
                {
                    get => global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatRequestsProvider.Get(CommandListHandle);
                    set => global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatRequestsProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponders
        {
            public struct PlayerHeartbeat : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.PlayerHeartbeat.Response> ResponsesToSend
                {
                    get => global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatResponderProvider.Get(CommandListHandle);
                    set => global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatResponderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponses
        {
            public struct PlayerHeartbeat : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.PlayerHeartbeat.ReceivedResponse> Responses
                {
                    get => global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatResponsesProvider.Get(CommandListHandle);
                    set => global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatResponsesProvider.Set(CommandListHandle, value);
                }
            }
        }
    }
}
#endif
