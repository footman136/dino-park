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

namespace Improbable
{
    public partial class EntityAcl
    {
        internal class ReactiveComponentReplicator : IReactiveComponentReplicationHandler
        {
            public uint ComponentId => 50;

            public EntityQueryDesc EventQuery => null;

            public EntityQueryDesc[] CommandQueries => null;

            public void SendEvents(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system, ComponentUpdateSystem componentUpdateSystem)
            {
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
                    ComponentType.ReadWrite<ComponentAdded<global::Improbable.EntityAcl.Component>>(),
                    ComponentType.ReadWrite<ComponentRemoved<global::Improbable.EntityAcl.Component>>(),
                    ComponentType.ReadWrite<global::Improbable.EntityAcl.ReceivedUpdates>(),
                    ComponentType.ReadWrite<AuthorityChanges<global::Improbable.EntityAcl.Component>>(),
                },
            };

            public override void CleanComponents(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
                EntityCommandBuffer buffer)
            {
                var entityType = system.GetArchetypeChunkEntityType();
                var componentAddedType = system.GetArchetypeChunkComponentType<ComponentAdded<global::Improbable.EntityAcl.Component>>();
                var componentRemovedType = system.GetArchetypeChunkComponentType<ComponentRemoved<global::Improbable.EntityAcl.Component>>();
                var receivedUpdateType = system.GetArchetypeChunkComponentType<global::Improbable.EntityAcl.ReceivedUpdates>();
                var authorityChangeType = system.GetArchetypeChunkComponentType<AuthorityChanges<global::Improbable.EntityAcl.Component>>();

                foreach (var chunk in chunkArray)
                {
                    var entities = chunk.GetNativeArray(entityType);

                    // Updates
                    if (chunk.Has(receivedUpdateType))
                    {
                        var updateArray = chunk.GetNativeArray(receivedUpdateType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<global::Improbable.EntityAcl.ReceivedUpdates>(entities[i]);
                            var updateList = updateArray[i].Updates;

                            // Pool update lists to avoid excessive allocation
                            updateList.Clear();
                            global::Improbable.EntityAcl.Update.Pool.Push(updateList);

                            ReferenceTypeProviders.UpdatesProvider.Free(updateArray[i].handle);
                        }
                    }

                    // Component Added
                    if (chunk.Has(componentAddedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentAdded<global::Improbable.EntityAcl.Component>>(entities[i]);
                        }
                    }

                    // Component Removed
                    if (chunk.Has(componentRemovedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentRemoved<global::Improbable.EntityAcl.Component>>(entities[i]);
                        }
                    }

                    // Authority
                    if (chunk.Has(authorityChangeType))
                    {
                        var authorityChangeArray = chunk.GetNativeArray(authorityChangeType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<AuthorityChanges<global::Improbable.EntityAcl.Component>>(entities[i]);
                            AuthorityChangesProvider.Free(authorityChangeArray[i].Handle);
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
                    ComponentType.ReadOnly<AuthorityLossImminent<global::Improbable.EntityAcl.Component>>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
            };

            public override void AcknowledgeAuthorityLoss(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
                ComponentUpdateSystem updateSystem)
            {
                var authorityLossType = system.GetArchetypeChunkComponentType<AuthorityLossImminent<global::Improbable.EntityAcl.Component>>();
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
                                50);
                        }
                    }
                }
            }
        }
    }
}
#endif
