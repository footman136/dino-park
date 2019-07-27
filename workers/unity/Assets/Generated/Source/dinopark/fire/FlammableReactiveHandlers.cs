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

namespace Dinopark.Fire
{
    public partial class Flammable
    {
        internal class ReactiveComponentReplicator : IReactiveComponentReplicationHandler
        {
            public uint ComponentId => 1400;

            public EntityQueryDesc EventQuery => null;

            public EntityQueryDesc[] CommandQueries => new EntityQueryDesc[]
            {
                new EntityQueryDesc()
                {
                    All = new[]
                    {
                        ComponentType.ReadWrite<global::Dinopark.Fire.Flammable.CommandSenders.Ignite>(),
                        ComponentType.ReadWrite<global::Dinopark.Fire.Flammable.CommandResponders.Ignite>(),
                    },
                },
                new EntityQueryDesc()
                {
                    All = new[]
                    {
                        ComponentType.ReadWrite<global::Dinopark.Fire.Flammable.CommandSenders.Extinguish>(),
                        ComponentType.ReadWrite<global::Dinopark.Fire.Flammable.CommandResponders.Extinguish>(),
                    },
                },
                new EntityQueryDesc()
                {
                    All = new[]
                    {
                        ComponentType.ReadWrite<global::Dinopark.Fire.Flammable.CommandSenders.SetCanBeIgnited>(),
                        ComponentType.ReadWrite<global::Dinopark.Fire.Flammable.CommandResponders.SetCanBeIgnited>(),
                    },
                },
            };

            public void SendEvents(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system, ComponentUpdateSystem componentUpdateSystem)
            {
            }

            public void SendCommands(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system, CommandSystem commandSystem)
            {
                Profiler.BeginSample("Flammable");
                var entityType = system.GetArchetypeChunkEntityType();
                var senderTypeIgnite = system.GetArchetypeChunkComponentType<global::Dinopark.Fire.Flammable.CommandSenders.Ignite>(true);
                var responderTypeIgnite = system.GetArchetypeChunkComponentType<global::Dinopark.Fire.Flammable.CommandResponders.Ignite>(true);
                var senderTypeExtinguish = system.GetArchetypeChunkComponentType<global::Dinopark.Fire.Flammable.CommandSenders.Extinguish>(true);
                var responderTypeExtinguish = system.GetArchetypeChunkComponentType<global::Dinopark.Fire.Flammable.CommandResponders.Extinguish>(true);
                var senderTypeSetCanBeIgnited = system.GetArchetypeChunkComponentType<global::Dinopark.Fire.Flammable.CommandSenders.SetCanBeIgnited>(true);
                var responderTypeSetCanBeIgnited = system.GetArchetypeChunkComponentType<global::Dinopark.Fire.Flammable.CommandResponders.SetCanBeIgnited>(true);

                foreach (var chunk in chunkArray)
                {
                    var entities = chunk.GetNativeArray(entityType);
                    if (chunk.Has(senderTypeIgnite))
                    {
                        var senders = chunk.GetNativeArray(senderTypeIgnite);
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

                        var responders = chunk.GetNativeArray(responderTypeIgnite);
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

                    if (chunk.Has(senderTypeExtinguish))
                    {
                        var senders = chunk.GetNativeArray(senderTypeExtinguish);
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

                        var responders = chunk.GetNativeArray(responderTypeExtinguish);
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

                    if (chunk.Has(senderTypeSetCanBeIgnited))
                    {
                        var senders = chunk.GetNativeArray(senderTypeSetCanBeIgnited);
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

                        var responders = chunk.GetNativeArray(responderTypeSetCanBeIgnited);
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
                    ComponentType.ReadWrite<ComponentAdded<global::Dinopark.Fire.Flammable.Component>>(),
                    ComponentType.ReadWrite<ComponentRemoved<global::Dinopark.Fire.Flammable.Component>>(),
                    ComponentType.ReadWrite<global::Dinopark.Fire.Flammable.ReceivedUpdates>(),
                    ComponentType.ReadWrite<AuthorityChanges<global::Dinopark.Fire.Flammable.Component>>(),
                    ComponentType.ReadWrite<CommandRequests.Ignite>(),
                    ComponentType.ReadWrite<CommandResponses.Ignite>(),
                    ComponentType.ReadWrite<CommandRequests.Extinguish>(),
                    ComponentType.ReadWrite<CommandResponses.Extinguish>(),
                    ComponentType.ReadWrite<CommandRequests.SetCanBeIgnited>(),
                    ComponentType.ReadWrite<CommandResponses.SetCanBeIgnited>(),
                },
            };

            public override void CleanComponents(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
                EntityCommandBuffer buffer)
            {
                var entityType = system.GetArchetypeChunkEntityType();
                var componentAddedType = system.GetArchetypeChunkComponentType<ComponentAdded<global::Dinopark.Fire.Flammable.Component>>();
                var componentRemovedType = system.GetArchetypeChunkComponentType<ComponentRemoved<global::Dinopark.Fire.Flammable.Component>>();
                var receivedUpdateType = system.GetArchetypeChunkComponentType<global::Dinopark.Fire.Flammable.ReceivedUpdates>();
                var authorityChangeType = system.GetArchetypeChunkComponentType<AuthorityChanges<global::Dinopark.Fire.Flammable.Component>>();

                var igniteRequestType = system.GetArchetypeChunkComponentType<CommandRequests.Ignite>();
                var igniteResponseType = system.GetArchetypeChunkComponentType<CommandResponses.Ignite>();

                var extinguishRequestType = system.GetArchetypeChunkComponentType<CommandRequests.Extinguish>();
                var extinguishResponseType = system.GetArchetypeChunkComponentType<CommandResponses.Extinguish>();

                var setCanBeIgnitedRequestType = system.GetArchetypeChunkComponentType<CommandRequests.SetCanBeIgnited>();
                var setCanBeIgnitedResponseType = system.GetArchetypeChunkComponentType<CommandResponses.SetCanBeIgnited>();

                foreach (var chunk in chunkArray)
                {
                    var entities = chunk.GetNativeArray(entityType);

                    // Updates
                    if (chunk.Has(receivedUpdateType))
                    {
                        var updateArray = chunk.GetNativeArray(receivedUpdateType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<global::Dinopark.Fire.Flammable.ReceivedUpdates>(entities[i]);
                            var updateList = updateArray[i].Updates;

                            // Pool update lists to avoid excessive allocation
                            updateList.Clear();
                            global::Dinopark.Fire.Flammable.Update.Pool.Push(updateList);

                            ReferenceTypeProviders.UpdatesProvider.Free(updateArray[i].handle);
                        }
                    }

                    // Component Added
                    if (chunk.Has(componentAddedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentAdded<global::Dinopark.Fire.Flammable.Component>>(entities[i]);
                        }
                    }

                    // Component Removed
                    if (chunk.Has(componentRemovedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentRemoved<global::Dinopark.Fire.Flammable.Component>>(entities[i]);
                        }
                    }

                    // Authority
                    if (chunk.Has(authorityChangeType))
                    {
                        var authorityChangeArray = chunk.GetNativeArray(authorityChangeType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<AuthorityChanges<global::Dinopark.Fire.Flammable.Component>>(entities[i]);
                            AuthorityChangesProvider.Free(authorityChangeArray[i].Handle);
                        }
                    }

                    // Ignite Command
                    if (chunk.Has(igniteRequestType))
                    {
                        var igniteRequestArray = chunk.GetNativeArray(igniteRequestType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandRequests.Ignite>(entities[i]);
                            ReferenceTypeProviders.IgniteRequestsProvider.Free(igniteRequestArray[i].CommandListHandle);
                        }
                    }

                    if (chunk.Has(igniteResponseType))
                    {
                        var igniteResponseArray = chunk.GetNativeArray(igniteResponseType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandResponses.Ignite>(entities[i]);
                            ReferenceTypeProviders.IgniteResponsesProvider.Free(igniteResponseArray[i].CommandListHandle);
                        }
                    }
                    // Extinguish Command
                    if (chunk.Has(extinguishRequestType))
                    {
                        var extinguishRequestArray = chunk.GetNativeArray(extinguishRequestType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandRequests.Extinguish>(entities[i]);
                            ReferenceTypeProviders.ExtinguishRequestsProvider.Free(extinguishRequestArray[i].CommandListHandle);
                        }
                    }

                    if (chunk.Has(extinguishResponseType))
                    {
                        var extinguishResponseArray = chunk.GetNativeArray(extinguishResponseType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandResponses.Extinguish>(entities[i]);
                            ReferenceTypeProviders.ExtinguishResponsesProvider.Free(extinguishResponseArray[i].CommandListHandle);
                        }
                    }
                    // SetCanBeIgnited Command
                    if (chunk.Has(setCanBeIgnitedRequestType))
                    {
                        var setCanBeIgnitedRequestArray = chunk.GetNativeArray(setCanBeIgnitedRequestType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandRequests.SetCanBeIgnited>(entities[i]);
                            ReferenceTypeProviders.SetCanBeIgnitedRequestsProvider.Free(setCanBeIgnitedRequestArray[i].CommandListHandle);
                        }
                    }

                    if (chunk.Has(setCanBeIgnitedResponseType))
                    {
                        var setCanBeIgnitedResponseArray = chunk.GetNativeArray(setCanBeIgnitedResponseType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandResponses.SetCanBeIgnited>(entities[i]);
                            ReferenceTypeProviders.SetCanBeIgnitedResponsesProvider.Free(setCanBeIgnitedResponseArray[i].CommandListHandle);
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
                    ComponentType.ReadOnly<AuthorityLossImminent<global::Dinopark.Fire.Flammable.Component>>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
            };

            public override void AcknowledgeAuthorityLoss(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
                ComponentUpdateSystem updateSystem)
            {
                var authorityLossType = system.GetArchetypeChunkComponentType<AuthorityLossImminent<global::Dinopark.Fire.Flammable.Component>>();
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
                                1400);
                        }
                    }
                }
            }
        }
    }
}
#endif
