// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

#if !DISABLE_REACTIVE_COMPONENTS
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Restricted
{
    public partial class Worker
    {
        public class CommandSenders
        {
            public struct Disconnect : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.Restricted.Worker.Disconnect.Request> RequestsToSend
                {
                    get => global::Improbable.Restricted.Worker.ReferenceTypeProviders.DisconnectSenderProvider.Get(CommandListHandle);
                    set => global::Improbable.Restricted.Worker.ReferenceTypeProviders.DisconnectSenderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandRequests
        {
            public struct Disconnect : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.Restricted.Worker.Disconnect.ReceivedRequest> Requests
                {
                    get => global::Improbable.Restricted.Worker.ReferenceTypeProviders.DisconnectRequestsProvider.Get(CommandListHandle);
                    set => global::Improbable.Restricted.Worker.ReferenceTypeProviders.DisconnectRequestsProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponders
        {
            public struct Disconnect : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.Restricted.Worker.Disconnect.Response> ResponsesToSend
                {
                    get => global::Improbable.Restricted.Worker.ReferenceTypeProviders.DisconnectResponderProvider.Get(CommandListHandle);
                    set => global::Improbable.Restricted.Worker.ReferenceTypeProviders.DisconnectResponderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponses
        {
            public struct Disconnect : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.Restricted.Worker.Disconnect.ReceivedResponse> Responses
                {
                    get => global::Improbable.Restricted.Worker.ReferenceTypeProviders.DisconnectResponsesProvider.Get(CommandListHandle);
                    set => global::Improbable.Restricted.Worker.ReferenceTypeProviders.DisconnectResponsesProvider.Set(CommandListHandle, value);
                }
            }
        }
    }
}
#endif
