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

namespace Dinopark.Plants
{
    public partial class Harvestable
    {
        public class HarvestReactiveCommandComponentManager : IReactiveCommandComponentManager
        {
            public void PopulateReactiveCommandComponents(CommandSystem commandSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var receivedRequests = commandSystem.GetRequests<Harvest.ReceivedRequest>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedRequests.Count; ++i)
                {
                    if (!workerSystem.TryGetEntity(receivedRequests[i].EntityId, out var entity))
                    {
                        continue;
                    }

                    List<Harvest.ReceivedRequest> requests;
                    if (entityManager.HasComponent<global::Dinopark.Plants.Harvestable.CommandRequests.Harvest>(entity))
                    {
                        requests = entityManager.GetComponentData<global::Dinopark.Plants.Harvestable.CommandRequests.Harvest>(entity).Requests;
                    }
                    else
                    {
                        var data = new global::Dinopark.Plants.Harvestable.CommandRequests.Harvest
                        {
                            CommandListHandle = ReferenceTypeProviders.HarvestRequestsProvider.Allocate(world)
                        };
                        data.Requests = new List<Harvest.ReceivedRequest>();
                        requests = data.Requests;
                        entityManager.AddComponentData(entity, data);
                    }

                    requests.Add(receivedRequests[i]);
                }


                var receivedResponses = commandSystem.GetResponses<Harvest.ReceivedResponse>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedResponses.Count; ++i)
                {
                    ref readonly var response = ref receivedResponses[i];

                    if (response.SendingEntity == Unity.Entities.Entity.Null || !entityManager.Exists(response.SendingEntity))
                    {
                        continue;
                    }

                    List<Harvest.ReceivedResponse> responses;
                    if (entityManager.HasComponent<global::Dinopark.Plants.Harvestable.CommandResponses.Harvest>(response.SendingEntity))
                    {
                        responses = entityManager.GetComponentData<global::Dinopark.Plants.Harvestable.CommandResponses.Harvest>(response.SendingEntity).Responses;
                    }
                    else
                    {
                        var data = new global::Dinopark.Plants.Harvestable.CommandResponses.Harvest
                        {
                            CommandListHandle = ReferenceTypeProviders.HarvestResponsesProvider.Allocate(world)
                        };
                        data.Responses = new List<Harvest.ReceivedResponse>();
                        responses = data.Responses;
                        entityManager.AddComponentData(response.SendingEntity, data);
                    }

                    responses.Add(response);
                }
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.HarvestRequestsProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.HarvestResponsesProvider.CleanDataInWorld(world);
            }
        }

        public class HarvestCommandSenderComponentManager : ICommandSenderComponentManager
        {
            private Dictionary<EntityId, (uint Sender, uint Responder)> entityIdToAllocatedHandles =
                new Dictionary<EntityId, (uint Sender, uint Responder)>();

            public void AddComponents(Entity entity, EntityManager entityManager, World world)
            {
                // todo error message if not the worker entity or spatial entity
                EntityId entityId = entityManager.HasComponent<SpatialEntityId>(entity)
                    ? entityManager.GetComponentData<SpatialEntityId>(entity).EntityId
                    : new EntityId(0);

                var commandSender = new global::Dinopark.Plants.Harvestable.CommandSenders.Harvest();
                commandSender.CommandListHandle = global::Dinopark.Plants.Harvestable.ReferenceTypeProviders.HarvestSenderProvider.Allocate(world);
                commandSender.RequestsToSend = new List<global::Dinopark.Plants.Harvestable.Harvest.Request>();

                entityManager.AddComponentData(entity, commandSender);

                var commandResponder = new global::Dinopark.Plants.Harvestable.CommandResponders.Harvest();
                commandResponder.CommandListHandle =
                    global::Dinopark.Plants.Harvestable.ReferenceTypeProviders.HarvestResponderProvider.Allocate(world);
                commandResponder.ResponsesToSend = new List<global::Dinopark.Plants.Harvestable.Harvest.Response>();

                entityManager.AddComponentData(entity, commandResponder);

                entityIdToAllocatedHandles.Add(entityId, (commandSender.CommandListHandle, commandResponder.CommandListHandle));
            }

            public void RemoveComponents(EntityId entityId, EntityManager entityManager, World world)
            {
                var workerSystem = world.GetExistingSystem<WorkerSystem>();

                workerSystem.TryGetEntity(entityId, out var entity);

                if (entity != Entity.Null)
                {
                    entityManager.RemoveComponent<CommandSenders.Harvest>(entity);
                    entityManager.RemoveComponent<CommandResponders.Harvest>(entity);
                }

                if (!entityIdToAllocatedHandles.TryGetValue(entityId, out var handles))
                {
                    throw new ArgumentException("Command components not added to entity");
                }

                entityIdToAllocatedHandles.Remove(entityId);

                ReferenceTypeProviders.HarvestSenderProvider.Free(handles.Sender);
                ReferenceTypeProviders.HarvestResponderProvider.Free(handles.Responder);
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.HarvestSenderProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.HarvestResponderProvider.CleanDataInWorld(world);
            }
        }

    }
}
#endif
