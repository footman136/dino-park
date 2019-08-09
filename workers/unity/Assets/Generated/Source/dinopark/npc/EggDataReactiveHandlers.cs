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
    public partial class EggData
    {
        internal class ReactiveComponentReplicator : IReactiveComponentReplicationHandler
        {
            public uint ComponentId => 1873;

            public EntityQueryDesc EventQuery => null;

            public EntityQueryDesc[] CommandQueries => new EntityQueryDesc[]
            {
                new EntityQueryDesc()
                {
                    All = new[]
                    {
                        ComponentType.ReadWrite<global::Dinopark.Npc.EggData.CommandSenders.Hatch>(),
                        ComponentType.ReadWrite<global::Dinopark.Npc.EggData.CommandResponders.Hatch>(),
                    },
                },
            };

            public void SendEvents(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system, ComponentUpdateSystem componentUpdateSystem)
            {
            }

            public void SendCommands(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system, CommandSystem commandSystem)
            {
                Profiler.BeginSample("EggData");
                var entityType = system.GetArchetypeChunkEntityType();
                var senderTypeHatch = system.GetArchetypeChunkComponentType<global::Dinopark.Npc.EggData.CommandSenders.Hatch>(true);
                var responderTypeHatch = system.GetArchetypeChunkComponentType<global::Dinopark.Npc.EggData.CommandResponders.Hatch>(true);

                foreach (var chunk in chunkArray)
                {
                    var entities = chunk.GetNativeArray(entityType);
                    if (chunk.Has(senderTypeHatch))
                    {
                        var senders = chunk.GetNativeArray(senderTypeHatch);
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

                        var responders = chunk.GetNativeArray(responderTypeHatch);
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
                    ComponentType.ReadWrite<ComponentAdded<global::Dinopark.Npc.EggData.Component>>(),
                    ComponentType.ReadWrite<ComponentRemoved<global::Dinopark.Npc.EggData.Component>>(),
                    ComponentType.ReadWrite<global::Dinopark.Npc.EggData.ReceivedUpdates>(),
                    ComponentType.ReadWrite<AuthorityChanges<global::Dinopark.Npc.EggData.Component>>(),
                    ComponentType.ReadWrite<CommandRequests.Hatch>(),
                    ComponentType.ReadWrite<CommandResponses.Hatch>(),
                },
            };

            public override void CleanComponents(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
                EntityCommandBuffer buffer)
            {
                var entityType = system.GetArchetypeChunkEntityType();
                var componentAddedType = system.GetArchetypeChunkComponentType<ComponentAdded<global::Dinopark.Npc.EggData.Component>>();
                var componentRemovedType = system.GetArchetypeChunkComponentType<ComponentRemoved<global::Dinopark.Npc.EggData.Component>>();
                var receivedUpdateType = system.GetArchetypeChunkComponentType<global::Dinopark.Npc.EggData.ReceivedUpdates>();
                var authorityChangeType = system.GetArchetypeChunkComponentType<AuthorityChanges<global::Dinopark.Npc.EggData.Component>>();

                var hatchRequestType = system.GetArchetypeChunkComponentType<CommandRequests.Hatch>();
                var hatchResponseType = system.GetArchetypeChunkComponentType<CommandResponses.Hatch>();

                foreach (var chunk in chunkArray)
                {
                    var entities = chunk.GetNativeArray(entityType);

                    // Updates
                    if (chunk.Has(receivedUpdateType))
                    {
                        var updateArray = chunk.GetNativeArray(receivedUpdateType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<global::Dinopark.Npc.EggData.ReceivedUpdates>(entities[i]);
                            var updateList = updateArray[i].Updates;

                            // Pool update lists to avoid excessive allocation
                            updateList.Clear();
                            global::Dinopark.Npc.EggData.Update.Pool.Push(updateList);

                            ReferenceTypeProviders.UpdatesProvider.Free(updateArray[i].handle);
                        }
                    }

                    // Component Added
                    if (chunk.Has(componentAddedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentAdded<global::Dinopark.Npc.EggData.Component>>(entities[i]);
                        }
                    }

                    // Component Removed
                    if (chunk.Has(componentRemovedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentRemoved<global::Dinopark.Npc.EggData.Component>>(entities[i]);
                        }
                    }

                    // Authority
                    if (chunk.Has(authorityChangeType))
                    {
                        var authorityChangeArray = chunk.GetNativeArray(authorityChangeType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<AuthorityChanges<global::Dinopark.Npc.EggData.Component>>(entities[i]);
                            AuthorityChangesProvider.Free(authorityChangeArray[i].Handle);
                        }
                    }

                    // Hatch Command
                    if (chunk.Has(hatchRequestType))
                    {
                        var hatchRequestArray = chunk.GetNativeArray(hatchRequestType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandRequests.Hatch>(entities[i]);
                            ReferenceTypeProviders.HatchRequestsProvider.Free(hatchRequestArray[i].CommandListHandle);
                        }
                    }

                    if (chunk.Has(hatchResponseType))
                    {
                        var hatchResponseArray = chunk.GetNativeArray(hatchResponseType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandResponses.Hatch>(entities[i]);
                            ReferenceTypeProviders.HatchResponsesProvider.Free(hatchResponseArray[i].CommandListHandle);
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
                    ComponentType.ReadOnly<AuthorityLossImminent<global::Dinopark.Npc.EggData.Component>>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
            };

            public override void AcknowledgeAuthorityLoss(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
                ComponentUpdateSystem updateSystem)
            {
                var authorityLossType = system.GetArchetypeChunkComponentType<AuthorityLossImminent<global::Dinopark.Npc.EggData.Component>>();
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
                                1873);
                        }
                    }
                }
            }
        }
    }
}
#endif
