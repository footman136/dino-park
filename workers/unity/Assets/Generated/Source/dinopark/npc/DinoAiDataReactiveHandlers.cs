// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

#if !DISABLE_REACTIVE_COMPONENTS
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Collections;
using Improbable.Worker.CInterop;
using Improbable.Gdk.Core;
using Improbable.Gdk.ReactiveComponents;

namespace Dinopark.Npc
{
    public partial class DinoAiData
    {
        internal class ReactiveComponentReplicator : IReactiveComponentReplicationHandler
        {
            public uint ComponentId => 1870;

            public EntityQueryDesc EventQuery => null;

            public EntityQueryDesc[] CommandQueries => new EntityQueryDesc[]
            {
                new EntityQueryDesc()
                {
                    All = new[]
                    {
                        ComponentType.ReadWrite<global::Dinopark.Npc.DinoAiData.CommandSenders.Attack>(),
                        ComponentType.ReadWrite<global::Dinopark.Npc.DinoAiData.CommandResponders.Attack>(),
                    },
                },
                new EntityQueryDesc()
                {
                    All = new[]
                    {
                        ComponentType.ReadWrite<global::Dinopark.Npc.DinoAiData.CommandSenders.Eat>(),
                        ComponentType.ReadWrite<global::Dinopark.Npc.DinoAiData.CommandResponders.Eat>(),
                    },
                },
            };

            public void SendEvents(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system, ComponentUpdateSystem componentUpdateSystem)
            {
            }

            public void SendCommands(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system, CommandSystem commandSystem)
            {
                Profiler.BeginSample("DinoAiData");
                var entityType = system.GetArchetypeChunkEntityType();
                var senderTypeAttack = system.GetArchetypeChunkComponentType<global::Dinopark.Npc.DinoAiData.CommandSenders.Attack>(true);
                var responderTypeAttack = system.GetArchetypeChunkComponentType<global::Dinopark.Npc.DinoAiData.CommandResponders.Attack>(true);
                var senderTypeEat = system.GetArchetypeChunkComponentType<global::Dinopark.Npc.DinoAiData.CommandSenders.Eat>(true);
                var responderTypeEat = system.GetArchetypeChunkComponentType<global::Dinopark.Npc.DinoAiData.CommandResponders.Eat>(true);

                foreach (var chunk in chunkArray)
                {
                    var entities = chunk.GetNativeArray(entityType);
                    if (chunk.Has(senderTypeAttack))
                    {
                        var senders = chunk.GetNativeArray(senderTypeAttack);
                        for (var i = 0; i < senders.Length; i++)
                        {
                            var requests = senders[i].RequestsToSend;
                            if (requests.Count > 0)
                            {
                                foreach (var request in requests)
                                {
                                    commandSystem.SendCommand(request, entities[i]);
                                }

                                requests.Clear();
                            }
                        }

                        var responders = chunk.GetNativeArray(responderTypeAttack);
                        for (var i = 0; i < responders.Length; i++)
                        {
                            var responses = responders[i].ResponsesToSend;
                            if (responses.Count > 0)
                            {
                                foreach (var response in responses)
                                {
                                    commandSystem.SendResponse(response);
                                }

                                responses.Clear();
                            }
                        }
                    }

                    if (chunk.Has(senderTypeEat))
                    {
                        var senders = chunk.GetNativeArray(senderTypeEat);
                        for (var i = 0; i < senders.Length; i++)
                        {
                            var requests = senders[i].RequestsToSend;
                            if (requests.Count > 0)
                            {
                                foreach (var request in requests)
                                {
                                    commandSystem.SendCommand(request, entities[i]);
                                }

                                requests.Clear();
                            }
                        }

                        var responders = chunk.GetNativeArray(responderTypeEat);
                        for (var i = 0; i < responders.Length; i++)
                        {
                            var responses = responders[i].ResponsesToSend;
                            if (responses.Count > 0)
                            {
                                foreach (var response in responses)
                                {
                                    commandSystem.SendResponse(response);
                                }

                                responses.Clear();
                            }
                        }
                    }

                }

                Profiler.EndSample();
            }
        }

        internal class ComponentCleanup : ComponentCleanupHandler
        {
            public override EntityQueryDesc CleanupArchetypeQuery => new EntityQueryDesc
            {
                Any = new ComponentType[]
                {
                    ComponentType.ReadWrite<ComponentAdded<global::Dinopark.Npc.DinoAiData.Component>>(),
                    ComponentType.ReadWrite<ComponentRemoved<global::Dinopark.Npc.DinoAiData.Component>>(),
                    ComponentType.ReadWrite<global::Dinopark.Npc.DinoAiData.ReceivedUpdates>(),
                    ComponentType.ReadWrite<AuthorityChanges<global::Dinopark.Npc.DinoAiData.Component>>(),
                    ComponentType.ReadWrite<CommandRequests.Attack>(),
                    ComponentType.ReadWrite<CommandResponses.Attack>(),
                    ComponentType.ReadWrite<CommandRequests.Eat>(),
                    ComponentType.ReadWrite<CommandResponses.Eat>(),
                },
            };

            public override void CleanComponents(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
                EntityCommandBuffer buffer)
            {
                var entityType = system.GetArchetypeChunkEntityType();
                var componentAddedType = system.GetArchetypeChunkComponentType<ComponentAdded<global::Dinopark.Npc.DinoAiData.Component>>();
                var componentRemovedType = system.GetArchetypeChunkComponentType<ComponentRemoved<global::Dinopark.Npc.DinoAiData.Component>>();
                var receivedUpdateType = system.GetArchetypeChunkComponentType<global::Dinopark.Npc.DinoAiData.ReceivedUpdates>();
                var authorityChangeType = system.GetArchetypeChunkComponentType<AuthorityChanges<global::Dinopark.Npc.DinoAiData.Component>>();

                var attackRequestType = system.GetArchetypeChunkComponentType<CommandRequests.Attack>();
                var attackResponseType = system.GetArchetypeChunkComponentType<CommandResponses.Attack>();

                var eatRequestType = system.GetArchetypeChunkComponentType<CommandRequests.Eat>();
                var eatResponseType = system.GetArchetypeChunkComponentType<CommandResponses.Eat>();

                foreach (var chunk in chunkArray)
                {
                    var entities = chunk.GetNativeArray(entityType);

                    // Updates
                    if (chunk.Has(receivedUpdateType))
                    {
                        var updateArray = chunk.GetNativeArray(receivedUpdateType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<global::Dinopark.Npc.DinoAiData.ReceivedUpdates>(entities[i]);
                            var updateList = updateArray[i].Updates;

                            // Pool update lists to avoid excessive allocation
                            updateList.Clear();
                            global::Dinopark.Npc.DinoAiData.Update.Pool.Push(updateList);

                            ReferenceTypeProviders.UpdatesProvider.Free(updateArray[i].handle);
                        }
                    }

                    // Component Added
                    if (chunk.Has(componentAddedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentAdded<global::Dinopark.Npc.DinoAiData.Component>>(entities[i]);
                        }
                    }

                    // Component Removed
                    if (chunk.Has(componentRemovedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentRemoved<global::Dinopark.Npc.DinoAiData.Component>>(entities[i]);
                        }
                    }

                    // Authority
                    if (chunk.Has(authorityChangeType))
                    {
                        var authorityChangeArray = chunk.GetNativeArray(authorityChangeType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<AuthorityChanges<global::Dinopark.Npc.DinoAiData.Component>>(entities[i]);
                            AuthorityChangesProvider.Free(authorityChangeArray[i].Handle);
                        }
                    }

                    // Attack Command
                    if (chunk.Has(attackRequestType))
                    {
                        var attackRequestArray = chunk.GetNativeArray(attackRequestType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandRequests.Attack>(entities[i]);
                            ReferenceTypeProviders.AttackRequestsProvider.Free(attackRequestArray[i].CommandListHandle);
                        }
                    }

                    if (chunk.Has(attackResponseType))
                    {
                        var attackResponseArray = chunk.GetNativeArray(attackResponseType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandResponses.Attack>(entities[i]);
                            ReferenceTypeProviders.AttackResponsesProvider.Free(attackResponseArray[i].CommandListHandle);
                        }
                    }
                    // Eat Command
                    if (chunk.Has(eatRequestType))
                    {
                        var eatRequestArray = chunk.GetNativeArray(eatRequestType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandRequests.Eat>(entities[i]);
                            ReferenceTypeProviders.EatRequestsProvider.Free(eatRequestArray[i].CommandListHandle);
                        }
                    }

                    if (chunk.Has(eatResponseType))
                    {
                        var eatResponseArray = chunk.GetNativeArray(eatResponseType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandResponses.Eat>(entities[i]);
                            ReferenceTypeProviders.EatResponsesProvider.Free(eatResponseArray[i].CommandListHandle);
                        }
                    }
                }
            }
        }

        internal class AcknowledgeAuthorityLossHandler : AbstractAcknowledgeAuthorityLossHandler
       {
            public override EntityQueryDesc Query => new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<AuthorityLossImminent<global::Dinopark.Npc.DinoAiData.Component>>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
            };

            public override void AcknowledgeAuthorityLoss(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
                ComponentUpdateSystem updateSystem)
            {
                var authorityLossType = system.GetArchetypeChunkComponentType<AuthorityLossImminent<global::Dinopark.Npc.DinoAiData.Component>>();
                var spatialEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>();

                foreach (var chunk in chunkArray)
                {
                    var authorityArray = chunk.GetNativeArray(authorityLossType);
                    var spatialEntityIdArray = chunk.GetNativeArray(spatialEntityType);

                    for (int i = 0; i < authorityArray.Length; ++i)
                    {
                        if (authorityArray[i].AcknowledgeAuthorityLoss)
                        {
                            updateSystem.AcknowledgeAuthorityLoss(spatialEntityIdArray[i].EntityId,
                                1870);
                        }
                    }
                }
            }
        }
    }
}
#endif
