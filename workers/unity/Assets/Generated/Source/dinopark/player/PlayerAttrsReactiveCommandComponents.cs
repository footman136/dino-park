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

namespace Dinopark.Player
{
    public partial class PlayerAttrs
    {
        public class LayEggReactiveCommandComponentManager : IReactiveCommandComponentManager
        {
            public void PopulateReactiveCommandComponents(CommandSystem commandSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var receivedRequests = commandSystem.GetRequests<LayEgg.ReceivedRequest>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedRequests.Count; ++i)
                {
                    if (!workerSystem.TryGetEntity(receivedRequests[i].EntityId, out var entity))
                    {
                        continue;
                    }

                    List<LayEgg.ReceivedRequest> requests;
                    if (entityManager.HasComponent<global::Dinopark.Player.PlayerAttrs.CommandRequests.LayEgg>(entity))
                    {
                        requests = entityManager.GetComponentData<global::Dinopark.Player.PlayerAttrs.CommandRequests.LayEgg>(entity).Requests;
                    }
                    else
                    {
                        var data = new global::Dinopark.Player.PlayerAttrs.CommandRequests.LayEgg
                        {
                            CommandListHandle = ReferenceTypeProviders.LayEggRequestsProvider.Allocate(world)
                        };
                        data.Requests = new List<LayEgg.ReceivedRequest>();
                        requests = data.Requests;
                        entityManager.AddComponentData(entity, data);
                    }

                    requests.Add(receivedRequests[i]);
                }


                var receivedResponses = commandSystem.GetResponses<LayEgg.ReceivedResponse>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedResponses.Count; ++i)
                {
                    ref readonly var response = ref receivedResponses[i];

                    if (response.SendingEntity == Unity.Entities.Entity.Null || !entityManager.Exists(response.SendingEntity))
                    {
                        continue;
                    }

                    List<LayEgg.ReceivedResponse> responses;
                    if (entityManager.HasComponent<global::Dinopark.Player.PlayerAttrs.CommandResponses.LayEgg>(response.SendingEntity))
                    {
                        responses = entityManager.GetComponentData<global::Dinopark.Player.PlayerAttrs.CommandResponses.LayEgg>(response.SendingEntity).Responses;
                    }
                    else
                    {
                        var data = new global::Dinopark.Player.PlayerAttrs.CommandResponses.LayEgg
                        {
                            CommandListHandle = ReferenceTypeProviders.LayEggResponsesProvider.Allocate(world)
                        };
                        data.Responses = new List<LayEgg.ReceivedResponse>();
                        responses = data.Responses;
                        entityManager.AddComponentData(response.SendingEntity, data);
                    }

                    responses.Add(response);
                }
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.LayEggRequestsProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.LayEggResponsesProvider.CleanDataInWorld(world);
            }
        }

        public class LayEggCommandSenderComponentManager : ICommandSenderComponentManager
        {
            private Dictionary<EntityId, (uint Sender, uint Responder)> entityIdToAllocatedHandles =
                new Dictionary<EntityId, (uint Sender, uint Responder)>();

            public void AddComponents(Entity entity, EntityManager entityManager, World world)
            {
                // todo error message if not the worker entity or spatial entity
                EntityId entityId = entityManager.HasComponent<SpatialEntityId>(entity)
                    ? entityManager.GetComponentData<SpatialEntityId>(entity).EntityId
                    : new EntityId(0);

                var commandSender = new global::Dinopark.Player.PlayerAttrs.CommandSenders.LayEgg();
                commandSender.CommandListHandle = global::Dinopark.Player.PlayerAttrs.ReferenceTypeProviders.LayEggSenderProvider.Allocate(world);
                commandSender.RequestsToSend = new List<global::Dinopark.Player.PlayerAttrs.LayEgg.Request>();

                entityManager.AddComponentData(entity, commandSender);

                var commandResponder = new global::Dinopark.Player.PlayerAttrs.CommandResponders.LayEgg();
                commandResponder.CommandListHandle =
                    global::Dinopark.Player.PlayerAttrs.ReferenceTypeProviders.LayEggResponderProvider.Allocate(world);
                commandResponder.ResponsesToSend = new List<global::Dinopark.Player.PlayerAttrs.LayEgg.Response>();

                entityManager.AddComponentData(entity, commandResponder);

                entityIdToAllocatedHandles.Add(entityId, (commandSender.CommandListHandle, commandResponder.CommandListHandle));
            }

            public void RemoveComponents(EntityId entityId, EntityManager entityManager, World world)
            {
                var workerSystem = world.GetExistingSystem<WorkerSystem>();

                workerSystem.TryGetEntity(entityId, out var entity);

                if (entity != Entity.Null)
                {
                    entityManager.RemoveComponent<CommandSenders.LayEgg>(entity);
                    entityManager.RemoveComponent<CommandResponders.LayEgg>(entity);
                }

                if (!entityIdToAllocatedHandles.TryGetValue(entityId, out var handles))
                {
                    throw new ArgumentException("Command components not added to entity");
                }

                entityIdToAllocatedHandles.Remove(entityId);

                ReferenceTypeProviders.LayEggSenderProvider.Free(handles.Sender);
                ReferenceTypeProviders.LayEggResponderProvider.Free(handles.Responder);
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.LayEggSenderProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.LayEggResponderProvider.CleanDataInWorld(world);
            }
        }

    }
}
#endif
