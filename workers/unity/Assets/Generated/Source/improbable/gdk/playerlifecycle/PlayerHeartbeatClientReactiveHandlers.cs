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

namespace Improbable.Gdk.PlayerLifecycle
{
    public partial class PlayerHeartbeatClient
    {
        internal class ReactiveComponentReplicator : IReactiveComponentReplicationHandler
        {
            public uint ComponentId => 13001;

            public EntityQueryDesc EventQuery => null;

            public EntityQueryDesc[] CommandQueries => new EntityQueryDesc[]
            {
                new EntityQueryDesc()
                {
                    All = new[]
                    {
                        ComponentType.ReadWrite<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.CommandSenders.PlayerHeartbeat>(),
                        ComponentType.ReadWrite<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.CommandResponders.PlayerHeartbeat>(),
                    },
                },
            };

            public void SendEvents(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system, ComponentUpdateSystem componentUpdateSystem)
            {
            }

            public void SendCommands(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system, CommandSystem commandSystem)
            {
                Profiler.BeginSample("PlayerHeartbeatClient");
                var entityType = system.GetArchetypeChunkEntityType();
                var senderTypePlayerHeartbeat = system.GetArchetypeChunkComponentType<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.CommandSenders.PlayerHeartbeat>(true);
                var responderTypePlayerHeartbeat = system.GetArchetypeChunkComponentType<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.CommandResponders.PlayerHeartbeat>(true);

                foreach (var chunk in chunkArray)
                {
                    var entities = chunk.GetNativeArray(entityType);
                    if (chunk.Has(senderTypePlayerHeartbeat))
                    {
                        var senders = chunk.GetNativeArray(senderTypePlayerHeartbeat);
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

                        var responders = chunk.GetNativeArray(responderTypePlayerHeartbeat);
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
                    ComponentType.ReadWrite<ComponentAdded<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.Component>>(),
                    ComponentType.ReadWrite<ComponentRemoved<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.Component>>(),
                    ComponentType.ReadWrite<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.ReceivedUpdates>(),
                    ComponentType.ReadWrite<AuthorityChanges<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.Component>>(),
                    ComponentType.ReadWrite<CommandRequests.PlayerHeartbeat>(),
                    ComponentType.ReadWrite<CommandResponses.PlayerHeartbeat>(),
                },
            };

            public override void CleanComponents(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
                EntityCommandBuffer buffer)
            {
                var entityType = system.GetArchetypeChunkEntityType();
                var componentAddedType = system.GetArchetypeChunkComponentType<ComponentAdded<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.Component>>();
                var componentRemovedType = system.GetArchetypeChunkComponentType<ComponentRemoved<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.Component>>();
                var receivedUpdateType = system.GetArchetypeChunkComponentType<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.ReceivedUpdates>();
                var authorityChangeType = system.GetArchetypeChunkComponentType<AuthorityChanges<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.Component>>();

                var playerHeartbeatRequestType = system.GetArchetypeChunkComponentType<CommandRequests.PlayerHeartbeat>();
                var playerHeartbeatResponseType = system.GetArchetypeChunkComponentType<CommandResponses.PlayerHeartbeat>();

                foreach (var chunk in chunkArray)
                {
                    var entities = chunk.GetNativeArray(entityType);

                    // Updates
                    if (chunk.Has(receivedUpdateType))
                    {
                        var updateArray = chunk.GetNativeArray(receivedUpdateType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.ReceivedUpdates>(entities[i]);
                            var updateList = updateArray[i].Updates;

                            // Pool update lists to avoid excessive allocation
                            updateList.Clear();
                            global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.Update.Pool.Push(updateList);

                            ReferenceTypeProviders.UpdatesProvider.Free(updateArray[i].handle);
                        }
                    }

                    // Component Added
                    if (chunk.Has(componentAddedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentAdded<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entities[i]);
                        }
                    }

                    // Component Removed
                    if (chunk.Has(componentRemovedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentRemoved<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entities[i]);
                        }
                    }

                    // Authority
                    if (chunk.Has(authorityChangeType))
                    {
                        var authorityChangeArray = chunk.GetNativeArray(authorityChangeType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<AuthorityChanges<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entities[i]);
                            AuthorityChangesProvider.Free(authorityChangeArray[i].Handle);
                        }
                    }

                    // PlayerHeartbeat Command
                    if (chunk.Has(playerHeartbeatRequestType))
                    {
                        var playerHeartbeatRequestArray = chunk.GetNativeArray(playerHeartbeatRequestType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandRequests.PlayerHeartbeat>(entities[i]);
                            ReferenceTypeProviders.PlayerHeartbeatRequestsProvider.Free(playerHeartbeatRequestArray[i].CommandListHandle);
                        }
                    }

                    if (chunk.Has(playerHeartbeatResponseType))
                    {
                        var playerHeartbeatResponseArray = chunk.GetNativeArray(playerHeartbeatResponseType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandResponses.PlayerHeartbeat>(entities[i]);
                            ReferenceTypeProviders.PlayerHeartbeatResponsesProvider.Free(playerHeartbeatResponseArray[i].CommandListHandle);
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
                    ComponentType.ReadOnly<AuthorityLossImminent<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.Component>>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
            };

            public override void AcknowledgeAuthorityLoss(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
                ComponentUpdateSystem updateSystem)
            {
                var authorityLossType = system.GetArchetypeChunkComponentType<AuthorityLossImminent<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.Component>>();
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
                                13001);
                        }
                    }
                }
            }
        }
    }
}
#endif
