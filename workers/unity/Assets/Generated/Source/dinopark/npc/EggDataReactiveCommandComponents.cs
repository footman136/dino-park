// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

#if !DISABLE_REACTIVE_COMPONENTS
using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.ReactiveComponents;
using Improbable.Worker.CInterop;
using Entity = Unity.Entities.Entity;

namespace Dinopark.Npc
{
    public partial class EggData
    {
        public class HatchReactiveCommandComponentManager : IReactiveCommandComponentManager
        {
            public void PopulateReactiveCommandComponents(CommandSystem commandSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var receivedRequests = commandSystem.GetRequests<Hatch.ReceivedRequest>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedRequests.Count; ++i)
                {
                    if (!workerSystem.TryGetEntity(receivedRequests[i].EntityId, out var entity))
                    {
                        continue;
                    }

                    List<Hatch.ReceivedRequest> requests;
                    if (entityManager.HasComponent<global::Dinopark.Npc.EggData.CommandRequests.Hatch>(entity))
                    {
                        requests = entityManager.GetComponentData<global::Dinopark.Npc.EggData.CommandRequests.Hatch>(entity).Requests;
                    }
                    else
                    {
                        var data = new global::Dinopark.Npc.EggData.CommandRequests.Hatch
                        {
                            CommandListHandle = ReferenceTypeProviders.HatchRequestsProvider.Allocate(world)
                        };
                        data.Requests = new List<Hatch.ReceivedRequest>();
                        requests = data.Requests;
                        entityManager.AddComponentData(entity, data);
                    }

                    requests.Add(receivedRequests[i]);
                }


                var receivedResponses = commandSystem.GetResponses<Hatch.ReceivedResponse>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedResponses.Count; ++i)
                {
                    ref readonly var response = ref receivedResponses[i];

                    if (response.SendingEntity == Unity.Entities.Entity.Null || !entityManager.Exists(response.SendingEntity))
                    {
                        continue;
                    }

                    List<Hatch.ReceivedResponse> responses;
                    if (entityManager.HasComponent<global::Dinopark.Npc.EggData.CommandResponses.Hatch>(response.SendingEntity))
                    {
                        responses = entityManager.GetComponentData<global::Dinopark.Npc.EggData.CommandResponses.Hatch>(response.SendingEntity).Responses;
                    }
                    else
                    {
                        var data = new global::Dinopark.Npc.EggData.CommandResponses.Hatch
                        {
                            CommandListHandle = ReferenceTypeProviders.HatchResponsesProvider.Allocate(world)
                        };
                        data.Responses = new List<Hatch.ReceivedResponse>();
                        responses = data.Responses;
                        entityManager.AddComponentData(response.SendingEntity, data);
                    }

                    responses.Add(response);
                }
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.HatchRequestsProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.HatchResponsesProvider.CleanDataInWorld(world);
            }
        }

        public class HatchCommandSenderComponentManager : ICommandSenderComponentManager
        {
            private Dictionary<EntityId, (uint Sender, uint Responder)> entityIdToAllocatedHandles =
                new Dictionary<EntityId, (uint Sender, uint Responder)>();

            public void AddComponents(Entity entity, EntityManager entityManager, World world)
            {
                // todo error message if not the worker entity or spatial entity
                EntityId entityId = entityManager.HasComponent<SpatialEntityId>(entity)
                    ? entityManager.GetComponentData<SpatialEntityId>(entity).EntityId
                    : new EntityId(0);

                var commandSender = new global::Dinopark.Npc.EggData.CommandSenders.Hatch();
                commandSender.CommandListHandle = global::Dinopark.Npc.EggData.ReferenceTypeProviders.HatchSenderProvider.Allocate(world);
                commandSender.RequestsToSend = new List<global::Dinopark.Npc.EggData.Hatch.Request>();

                entityManager.AddComponentData(entity, commandSender);

                var commandResponder = new global::Dinopark.Npc.EggData.CommandResponders.Hatch();
                commandResponder.CommandListHandle =
                    global::Dinopark.Npc.EggData.ReferenceTypeProviders.HatchResponderProvider.Allocate(world);
                commandResponder.ResponsesToSend = new List<global::Dinopark.Npc.EggData.Hatch.Response>();

                entityManager.AddComponentData(entity, commandResponder);

                entityIdToAllocatedHandles.Add(entityId, (commandSender.CommandListHandle, commandResponder.CommandListHandle));
            }

            public void RemoveComponents(EntityId entityId, EntityManager entityManager, World world)
            {
                var workerSystem = world.GetExistingSystem<WorkerSystem>();

                workerSystem.TryGetEntity(entityId, out var entity);

                if (entity != Entity.Null)
                {
                    entityManager.RemoveComponent<CommandSenders.Hatch>(entity);
                    entityManager.RemoveComponent<CommandResponders.Hatch>(entity);
                }

                if (!entityIdToAllocatedHandles.TryGetValue(entityId, out var handles))
                {
                    throw new ArgumentException("Command components not added to entity");
                }

                entityIdToAllocatedHandles.Remove(entityId);

                ReferenceTypeProviders.HatchSenderProvider.Free(handles.Sender);
                ReferenceTypeProviders.HatchResponderProvider.Free(handles.Responder);
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.HatchSenderProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.HatchResponderProvider.CleanDataInWorld(world);
            }
        }

    }
}
#endif
