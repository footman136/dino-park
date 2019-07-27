// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

#if !DISABLE_REACTIVE_COMPONENTS
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    public partial class PlayerCreator
    {
        public class CommandSenders
        {
            public struct CreatePlayer : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.CreatePlayer.Request> RequestsToSend
                {
                    get => global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerSenderProvider.Get(CommandListHandle);
                    set => global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerSenderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandRequests
        {
            public struct CreatePlayer : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.CreatePlayer.ReceivedRequest> Requests
                {
                    get => global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerRequestsProvider.Get(CommandListHandle);
                    set => global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerRequestsProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponders
        {
            public struct CreatePlayer : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.CreatePlayer.Response> ResponsesToSend
                {
                    get => global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerResponderProvider.Get(CommandListHandle);
                    set => global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerResponderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponses
        {
            public struct CreatePlayer : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.CreatePlayer.ReceivedResponse> Responses
                {
                    get => global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerResponsesProvider.Get(CommandListHandle);
                    set => global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerResponsesProvider.Set(CommandListHandle, value);
                }
            }
        }
    }
}
#endif
