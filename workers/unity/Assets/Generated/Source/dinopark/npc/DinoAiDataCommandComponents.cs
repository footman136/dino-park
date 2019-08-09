// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

#if !DISABLE_REACTIVE_COMPONENTS
using System.Collections.Generic;
using Unity.Entities;

namespace Dinopark.Npc
{
    public partial class DinoAiData
    {
        public class CommandSenders
        {
            public struct Attack : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Npc.DinoAiData.Attack.Request> RequestsToSend
                {
                    get => global::Dinopark.Npc.DinoAiData.ReferenceTypeProviders.AttackSenderProvider.Get(CommandListHandle);
                    set => global::Dinopark.Npc.DinoAiData.ReferenceTypeProviders.AttackSenderProvider.Set(CommandListHandle, value);
                }
            }
            public struct Eat : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Npc.DinoAiData.Eat.Request> RequestsToSend
                {
                    get => global::Dinopark.Npc.DinoAiData.ReferenceTypeProviders.EatSenderProvider.Get(CommandListHandle);
                    set => global::Dinopark.Npc.DinoAiData.ReferenceTypeProviders.EatSenderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandRequests
        {
            public struct Attack : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Npc.DinoAiData.Attack.ReceivedRequest> Requests
                {
                    get => global::Dinopark.Npc.DinoAiData.ReferenceTypeProviders.AttackRequestsProvider.Get(CommandListHandle);
                    set => global::Dinopark.Npc.DinoAiData.ReferenceTypeProviders.AttackRequestsProvider.Set(CommandListHandle, value);
                }
            }
            public struct Eat : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Npc.DinoAiData.Eat.ReceivedRequest> Requests
                {
                    get => global::Dinopark.Npc.DinoAiData.ReferenceTypeProviders.EatRequestsProvider.Get(CommandListHandle);
                    set => global::Dinopark.Npc.DinoAiData.ReferenceTypeProviders.EatRequestsProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponders
        {
            public struct Attack : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Npc.DinoAiData.Attack.Response> ResponsesToSend
                {
                    get => global::Dinopark.Npc.DinoAiData.ReferenceTypeProviders.AttackResponderProvider.Get(CommandListHandle);
                    set => global::Dinopark.Npc.DinoAiData.ReferenceTypeProviders.AttackResponderProvider.Set(CommandListHandle, value);
                }
            }
            public struct Eat : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Npc.DinoAiData.Eat.Response> ResponsesToSend
                {
                    get => global::Dinopark.Npc.DinoAiData.ReferenceTypeProviders.EatResponderProvider.Get(CommandListHandle);
                    set => global::Dinopark.Npc.DinoAiData.ReferenceTypeProviders.EatResponderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponses
        {
            public struct Attack : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Npc.DinoAiData.Attack.ReceivedResponse> Responses
                {
                    get => global::Dinopark.Npc.DinoAiData.ReferenceTypeProviders.AttackResponsesProvider.Get(CommandListHandle);
                    set => global::Dinopark.Npc.DinoAiData.ReferenceTypeProviders.AttackResponsesProvider.Set(CommandListHandle, value);
                }
            }
            public struct Eat : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Dinopark.Npc.DinoAiData.Eat.ReceivedResponse> Responses
                {
                    get => global::Dinopark.Npc.DinoAiData.ReferenceTypeProviders.EatResponsesProvider.Get(CommandListHandle);
                    set => global::Dinopark.Npc.DinoAiData.ReferenceTypeProviders.EatResponsesProvider.Set(CommandListHandle, value);
                }
            }
        }
    }
}
#endif
