// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Collections;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.CodegenAdapters;

namespace Dinopark.Npc
{
    public partial class DinoAttrs
    {
        internal class ComponentReplicator : IComponentReplicationHandler
        {
            public uint ComponentId => 1872;

            public EntityQueryDesc ComponentUpdateQuery => new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<global::Dinopark.Npc.DinoAttrs.Component>(),
                    ComponentType.ReadWrite<global::Dinopark.Npc.DinoAttrs.ComponentAuthority>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
            };

            public void SendUpdates(
                NativeArray<ArchetypeChunk> chunkArray,
                ComponentSystemBase system,
                EntityManager entityManager,
                ComponentUpdateSystem componentUpdateSystem)
            {
                Profiler.BeginSample("DinoAttrs");

                var spatialOSEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>(true);
                var componentType = system.GetArchetypeChunkComponentType<global::Dinopark.Npc.DinoAttrs.Component>();

                var authorityType = system.GetArchetypeChunkSharedComponentType<ComponentAuthority>();

                foreach (var chunk in chunkArray)
                {
                    var entityIdArray = chunk.GetNativeArray(spatialOSEntityType);
                    var componentArray = chunk.GetNativeArray(componentType);

                    var authorityIndex = chunk.GetSharedComponentIndex(authorityType);

                    if (!entityManager.GetSharedComponentData<ComponentAuthority>(authorityIndex).HasAuthority)
                    {
                        continue;
                    }

                    for (var i = 0; i < componentArray.Length; i++)
                    {
                        var data = componentArray[i];
                        if (data.IsDataDirty())
                        {
                            Update update = new Update();

                            if (data.IsDataDirty(0))
                            {
                                update.IsDead = data.IsDead;
                            }

                            if (data.IsDataDirty(1))
                            {
                                update.CurrentFood = data.CurrentFood;
                            }

                            if (data.IsDataDirty(2))
                            {
                                update.OriginalScent = data.OriginalScent;
                            }

                            if (data.IsDataDirty(3))
                            {
                                update.OriginalAgression = data.OriginalAgression;
                            }

                            if (data.IsDataDirty(4))
                            {
                                update.OriginalDominance = data.OriginalDominance;
                            }

                            if (data.IsDataDirty(5))
                            {
                                update.OriginPosotion = data.OriginPosotion;
                            }

                            componentUpdateSystem.SendUpdate(in update, entityIdArray[i].EntityId);
                            data.MarkDataClean();
                            componentArray[i] = data;
                        }
                    }
                }

                Profiler.EndSample();
            }
        }
    }
}
