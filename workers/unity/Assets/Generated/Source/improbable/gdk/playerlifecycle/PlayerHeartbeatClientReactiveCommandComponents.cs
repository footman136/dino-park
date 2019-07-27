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

namespace Improbable.Gdk.PlayerLifecycle
{
    public partial class PlayerHeartbeatClient
    {
        public class PlayerHeartbeatReactiveCommandComponentManager : IReactiveCommandComponentManager
        {
            public void PopulateReactiveCommandComponents(CommandSystem commandSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var receivedRequests = commandSystem.GetRequests<PlayerHeartbeat.ReceivedRequest>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedRequests.Count; ++i)
                {
                    if (!workerSystem.TryGetEntity(receivedRequests[i].EntityId, out var entity))
                    {
                        continue;
                    }

                    List<PlayerHeartbeat.ReceivedRequest> requests;
                    if (entityManager.HasComponent<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.CommandRequests.PlayerHeartbeat>(entity))
                    {
                        requests = entityManager.GetComponentData<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.CommandRequests.PlayerHeartbeat>(entity).Requests;
                    }
                    else
                    {
                        var data = new global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.CommandRequests.PlayerHeartbeat
                        {
                            CommandListHandle = ReferenceTypeProviders.PlayerHeartbeatRequestsProvider.Allocate(world)
                        };
                        data.Requests = new List<PlayerHeartbeat.ReceivedRequest>();
                        requests = data.Requests;
                        entityManager.AddComponentData(entity, data);
                    }

                    requests.Add(receivedRequests[i]);
                }


                var receivedResponses = commandSystem.GetResponses<PlayerHeartbeat.ReceivedResponse>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedResponses.Count; ++i)
                {
                    ref readonly var response = ref receivedResponses[i];

                    if (response.SendingEntity == Unity.Entities.Entity.Null || !entityManager.Exists(response.SendingEntity))
                    {
                        continue;
                    }

                    List<PlayerHeartbeat.ReceivedResponse> responses;
                    if (entityManager.HasComponent<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.CommandResponses.PlayerHeartbeat>(response.SendingEntity))
                    {
                        responses = entityManager.GetComponentData<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.CommandResponses.PlayerHeartbeat>(response.SendingEntity).Responses;
                    }
                    else
                    {
                        var data = new global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.CommandResponses.PlayerHeartbeat
                        {
                            CommandListHandle = ReferenceTypeProviders.PlayerHeartbeatResponsesProvider.Allocate(world)
                        };
                        data.Responses = new List<PlayerHeartbeat.ReceivedResponse>();
                        responses = data.Responses;
                        entityManager.AddComponentData(response.SendingEntity, data);
                    }

                    responses.Add(response);
                }
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.PlayerHeartbeatRequestsProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.PlayerHeartbeatResponsesProvider.CleanDataInWorld(world);
            }
        }

        public class PlayerHeartbeatCommandSenderComponentManager : ICommandSenderComponentManager
        {
            private Dictionary<EntityId, (uint Sender, uint Responder)> entityIdToAllocatedHandles =
                new Dictionary<EntityId, (uint Sender, uint Responder)>();

            public void AddComponents(Entity entity, EntityManager entityManager, World world)
            {
                // todo error message if not the worker entity or spatial entity
                EntityId entityId = entityManager.HasComponent<SpatialEntityId>(entity)
                    ? entityManager.GetComponentData<SpatialEntityId>(entity).EntityId
                    : new EntityId(0);

                var commandSender = new global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.CommandSenders.PlayerHeartbeat();
                commandSender.CommandListHandle = global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatSenderProvider.Allocate(world);
                commandSender.RequestsToSend = new List<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.PlayerHeartbeat.Request>();

                entityManager.AddComponentData(entity, commandSender);

                var commandResponder = new global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.CommandResponders.PlayerHeartbeat();
                commandResponder.CommandListHandle =
                    global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatResponderProvider.Allocate(world);
                commandResponder.ResponsesToSend = new List<global::Improbable.Gdk.PlayerLifecycle.PlayerHeartbeatClient.PlayerHeartbeat.Response>();

                entityManager.AddComponentData(entity, commandResponder);

                entityIdToAllocatedHandles.Add(entityId, (commandSender.CommandListHandle, commandResponder.CommandListHandle));
            }

            public void RemoveComponents(EntityId entityId, EntityManager entityManager, World world)
            {
                var workerSystem = world.GetExistingSystem<WorkerSystem>();

                workerSystem.TryGetEntity(entityId, out var entity);

                if (entity != Entity.Null)
                {
                    entityManager.RemoveComponent<CommandSenders.PlayerHeartbeat>(entity);
                    entityManager.RemoveComponent<CommandResponders.PlayerHeartbeat>(entity);
                }

                if (!entityIdToAllocatedHandles.TryGetValue(entityId, out var handles))
                {
                    throw new ArgumentException("Command components not added to entity");
                }

                entityIdToAllocatedHandles.Remove(entityId);

                ReferenceTypeProviders.PlayerHeartbeatSenderProvider.Free(handles.Sender);
                ReferenceTypeProviders.PlayerHeartbeatResponderProvider.Free(handles.Responder);
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.PlayerHeartbeatSenderProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.PlayerHeartbeatResponderProvider.CleanDataInWorld(world);
            }
        }

    }
}
#endif
