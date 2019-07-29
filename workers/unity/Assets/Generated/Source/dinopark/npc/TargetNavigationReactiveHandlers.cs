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
    public partial class TargetNavigation
    {
        internal class ReactiveComponentReplicator : IReactiveComponentReplicationHandler
        {
            public uint ComponentId => 1800;

            public EntityQueryDesc EventQuery => new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<EventSender.NavigationFinished>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
            };

            public EntityQueryDesc[] CommandQueries => null;

            public void SendEvents(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system, ComponentUpdateSystem componentUpdateSystem)
            {
                Profiler.BeginSample("TargetNavigation");

                var spatialOSEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>(true);
                var eventNavigationFinishedType = system.GetArchetypeChunkComponentType<EventSender.NavigationFinished>(true);
                foreach (var chunk in chunkArray)
                {
                    var entityIdArray = chunk.GetNativeArray(spatialOSEntityType);
                    var eventNavigationFinishedArray = chunk.GetNativeArray(eventNavigationFinishedType);
                    for (var i = 0; i < entityIdArray.Length; i++)
                    {
                        foreach (var e in eventNavigationFinishedArray[i].Events)
                        {
                            componentUpdateSystem.SendEvent(new NavigationFinished.Event(e), entityIdArray[i].EntityId);
                        }

                        eventNavigationFinishedArray[i].Events.Clear();
                    }
                }

                Profiler.EndSample();
            }

            public void SendCommands(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system, CommandSystem commandSystem)
            {
            }
        }

        internal class ComponentCleanup : ComponentCleanupHandler
        {
            public override EntityQueryDesc CleanupArchetypeQuery => new EntityQueryDesc
            {
                Any = new ComponentType[]
                {
                    ComponentType.ReadWrite<ComponentAdded<global::Dinopark.Npc.TargetNavigation.Component>>(),
                    ComponentType.ReadWrite<ComponentRemoved<global::Dinopark.Npc.TargetNavigation.Component>>(),
                    ComponentType.ReadWrite<global::Dinopark.Npc.TargetNavigation.ReceivedUpdates>(),
                    ComponentType.ReadWrite<AuthorityChanges<global::Dinopark.Npc.TargetNavigation.Component>>(),
                    ComponentType.ReadWrite<ReceivedEvents.NavigationFinished>(),
                },
            };

            public override void CleanComponents(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
                EntityCommandBuffer buffer)
            {
                var entityType = system.GetArchetypeChunkEntityType();
                var componentAddedType = system.GetArchetypeChunkComponentType<ComponentAdded<global::Dinopark.Npc.TargetNavigation.Component>>();
                var componentRemovedType = system.GetArchetypeChunkComponentType<ComponentRemoved<global::Dinopark.Npc.TargetNavigation.Component>>();
                var receivedUpdateType = system.GetArchetypeChunkComponentType<global::Dinopark.Npc.TargetNavigation.ReceivedUpdates>();
                var authorityChangeType = system.GetArchetypeChunkComponentType<AuthorityChanges<global::Dinopark.Npc.TargetNavigation.Component>>();
                var navigationFinishedEventType = system.GetArchetypeChunkComponentType<ReceivedEvents.NavigationFinished>();

                foreach (var chunk in chunkArray)
                {
                    var entities = chunk.GetNativeArray(entityType);

                    // Updates
                    if (chunk.Has(receivedUpdateType))
                    {
                        var updateArray = chunk.GetNativeArray(receivedUpdateType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<global::Dinopark.Npc.TargetNavigation.ReceivedUpdates>(entities[i]);
                            var updateList = updateArray[i].Updates;

                            // Pool update lists to avoid excessive allocation
                            updateList.Clear();
                            global::Dinopark.Npc.TargetNavigation.Update.Pool.Push(updateList);

                            ReferenceTypeProviders.UpdatesProvider.Free(updateArray[i].handle);
                        }
                    }

                    // Component Added
                    if (chunk.Has(componentAddedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentAdded<global::Dinopark.Npc.TargetNavigation.Component>>(entities[i]);
                        }
                    }

                    // Component Removed
                    if (chunk.Has(componentRemovedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentRemoved<global::Dinopark.Npc.TargetNavigation.Component>>(entities[i]);
                        }
                    }

                    // Authority
                    if (chunk.Has(authorityChangeType))
                    {
                        var authorityChangeArray = chunk.GetNativeArray(authorityChangeType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<AuthorityChanges<global::Dinopark.Npc.TargetNavigation.Component>>(entities[i]);
                            AuthorityChangesProvider.Free(authorityChangeArray[i].Handle);
                        }
                    }

                    // NavigationFinished Event
                    if (chunk.Has(navigationFinishedEventType))
                    {
                        var navigationFinishedEventArray = chunk.GetNativeArray(navigationFinishedEventType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ReceivedEvents.NavigationFinished>(entities[i]);
                            ReferenceTypeProviders.NavigationFinishedProvider.Free(navigationFinishedEventArray[i].handle);
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
                    ComponentType.ReadOnly<AuthorityLossImminent<global::Dinopark.Npc.TargetNavigation.Component>>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
            };

            public override void AcknowledgeAuthorityLoss(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
                ComponentUpdateSystem updateSystem)
            {
                var authorityLossType = system.GetArchetypeChunkComponentType<AuthorityLossImminent<global::Dinopark.Npc.TargetNavigation.Component>>();
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
                                1800);
                        }
                    }
                }
            }
        }
    }
}
#endif
