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
    public partial class PlayerCreator
    {
        public class CreatePlayerReactiveCommandComponentManager : IReactiveCommandComponentManager
        {
            public void PopulateReactiveCommandComponents(CommandSystem commandSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var receivedRequests = commandSystem.GetRequests<CreatePlayer.ReceivedRequest>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedRequests.Count; ++i)
                {
                    if (!workerSystem.TryGetEntity(receivedRequests[i].EntityId, out var entity))
                    {
                        continue;
                    }

                    List<CreatePlayer.ReceivedRequest> requests;
                    if (entityManager.HasComponent<global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.CommandRequests.CreatePlayer>(entity))
                    {
                        requests = entityManager.GetComponentData<global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.CommandRequests.CreatePlayer>(entity).Requests;
                    }
                    else
                    {
                        var data = new global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.CommandRequests.CreatePlayer
                        {
                            CommandListHandle = ReferenceTypeProviders.CreatePlayerRequestsProvider.Allocate(world)
                        };
                        data.Requests = new List<CreatePlayer.ReceivedRequest>();
                        requests = data.Requests;
                        entityManager.AddComponentData(entity, data);
                    }

                    requests.Add(receivedRequests[i]);
                }


                var receivedResponses = commandSystem.GetResponses<CreatePlayer.ReceivedResponse>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedResponses.Count; ++i)
                {
                    ref readonly var response = ref receivedResponses[i];

                    if (response.SendingEntity == Unity.Entities.Entity.Null || !entityManager.Exists(response.SendingEntity))
                    {
                        continue;
                    }

                    List<CreatePlayer.ReceivedResponse> responses;
                    if (entityManager.HasComponent<global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.CommandResponses.CreatePlayer>(response.SendingEntity))
                    {
                        responses = entityManager.GetComponentData<global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.CommandResponses.CreatePlayer>(response.SendingEntity).Responses;
                    }
                    else
                    {
                        var data = new global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.CommandResponses.CreatePlayer
                        {
                            CommandListHandle = ReferenceTypeProviders.CreatePlayerResponsesProvider.Allocate(world)
                        };
                        data.Responses = new List<CreatePlayer.ReceivedResponse>();
                        responses = data.Responses;
                        entityManager.AddComponentData(response.SendingEntity, data);
                    }

                    responses.Add(response);
                }
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.CreatePlayerRequestsProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.CreatePlayerResponsesProvider.CleanDataInWorld(world);
            }
        }

        public class CreatePlayerCommandSenderComponentManager : ICommandSenderComponentManager
        {
            private Dictionary<EntityId, (uint Sender, uint Responder)> entityIdToAllocatedHandles =
                new Dictionary<EntityId, (uint Sender, uint Responder)>();

            public void AddComponents(Entity entity, EntityManager entityManager, World world)
            {
                // todo error message if not the worker entity or spatial entity
                EntityId entityId = entityManager.HasComponent<SpatialEntityId>(entity)
                    ? entityManager.GetComponentData<SpatialEntityId>(entity).EntityId
                    : new EntityId(0);

                var commandSender = new global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.CommandSenders.CreatePlayer();
                commandSender.CommandListHandle = global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerSenderProvider.Allocate(world);
                commandSender.RequestsToSend = new List<global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.CreatePlayer.Request>();

                entityManager.AddComponentData(entity, commandSender);

                var commandResponder = new global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.CommandResponders.CreatePlayer();
                commandResponder.CommandListHandle =
                    global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerResponderProvider.Allocate(world);
                commandResponder.ResponsesToSend = new List<global::Improbable.Gdk.PlayerLifecycle.PlayerCreator.CreatePlayer.Response>();

                entityManager.AddComponentData(entity, commandResponder);

                entityIdToAllocatedHandles.Add(entityId, (commandSender.CommandListHandle, commandResponder.CommandListHandle));
            }

            public void RemoveComponents(EntityId entityId, EntityManager entityManager, World world)
            {
                var workerSystem = world.GetExistingSystem<WorkerSystem>();

                workerSystem.TryGetEntity(entityId, out var entity);

                if (entity != Entity.Null)
                {
                    entityManager.RemoveComponent<CommandSenders.CreatePlayer>(entity);
                    entityManager.RemoveComponent<CommandResponders.CreatePlayer>(entity);
                }

                if (!entityIdToAllocatedHandles.TryGetValue(entityId, out var handles))
                {
                    throw new ArgumentException("Command components not added to entity");
                }

                entityIdToAllocatedHandles.Remove(entityId);

                ReferenceTypeProviders.CreatePlayerSenderProvider.Free(handles.Sender);
                ReferenceTypeProviders.CreatePlayerResponderProvider.Free(handles.Responder);
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.CreatePlayerSenderProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.CreatePlayerResponderProvider.CleanDataInWorld(world);
            }
        }

    }
}
#endif
