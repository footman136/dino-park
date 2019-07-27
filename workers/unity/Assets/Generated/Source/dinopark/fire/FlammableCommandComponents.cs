// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

#if !DISABLE_REACTIVE_COMPONENTS
using System.Collections.Generic;
using Unity.Entities;

namespace Dinopark.Fire
{
    public partial class Flammable
    {
        public class CommandSenders
        {
            public struct Ignite : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Fire.Flammable.Ignite.Request> RequestsToSend
                {
                    get => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.IgniteSenderProvider.Get(CommandListHandle);
                    set => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.IgniteSenderProvider.Set(CommandListHandle, value);
                }
            }
            public struct Extinguish : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Fire.Flammable.Extinguish.Request> RequestsToSend
                {
                    get => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.ExtinguishSenderProvider.Get(CommandListHandle);
                    set => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.ExtinguishSenderProvider.Set(CommandListHandle, value);
                }
            }
            public struct SetCanBeIgnited : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Fire.Flammable.SetCanBeIgnited.Request> RequestsToSend
                {
                    get => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.SetCanBeIgnitedSenderProvider.Get(CommandListHandle);
                    set => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.SetCanBeIgnitedSenderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandRequests
        {
            public struct Ignite : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Fire.Flammable.Ignite.ReceivedRequest> Requests
                {
                    get => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.IgniteRequestsProvider.Get(CommandListHandle);
                    set => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.IgniteRequestsProvider.Set(CommandListHandle, value);
                }
            }
            public struct Extinguish : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Fire.Flammable.Extinguish.ReceivedRequest> Requests
                {
                    get => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.ExtinguishRequestsProvider.Get(CommandListHandle);
                    set => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.ExtinguishRequestsProvider.Set(CommandListHandle, value);
                }
            }
            public struct SetCanBeIgnited : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Fire.Flammable.SetCanBeIgnited.ReceivedRequest> Requests
                {
                    get => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.SetCanBeIgnitedRequestsProvider.Get(CommandListHandle);
                    set => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.SetCanBeIgnitedRequestsProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponders
        {
            public struct Ignite : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Fire.Flammable.Ignite.Response> ResponsesToSend
                {
                    get => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.IgniteResponderProvider.Get(CommandListHandle);
                    set => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.IgniteResponderProvider.Set(CommandListHandle, value);
                }
            }
            public struct Extinguish : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Fire.Flammable.Extinguish.Response> ResponsesToSend
                {
                    get => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.ExtinguishResponderProvider.Get(CommandListHandle);
                    set => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.ExtinguishResponderProvider.Set(CommandListHandle, value);
                }
            }
            public struct SetCanBeIgnited : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Fire.Flammable.SetCanBeIgnited.Response> ResponsesToSend
                {
                    get => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.SetCanBeIgnitedResponderProvider.Get(CommandListHandle);
                    set => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.SetCanBeIgnitedResponderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponses
        {
            public struct Ignite : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Fire.Flammable.Ignite.ReceivedResponse> Responses
                {
                    get => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.IgniteResponsesProvider.Get(CommandListHandle);
                    set => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.IgniteResponsesProvider.Set(CommandListHandle, value);
                }
            }
            public struct Extinguish : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Fire.Flammable.Extinguish.ReceivedResponse> Responses
                {
                    get => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.ExtinguishResponsesProvider.Get(CommandListHandle);
                    set => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.ExtinguishResponsesProvider.Set(CommandListHandle, value);
                }
            }
            public struct SetCanBeIgnited : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Fire.Flammable.SetCanBeIgnited.ReceivedResponse> Responses
                {
                    get => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.SetCanBeIgnitedResponsesProvider.Get(CommandListHandle);
                    set => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.SetCanBeIgnitedResponsesProvider.Set(CommandListHandle, value);
                }
            }
        }
    }
}
#endif
