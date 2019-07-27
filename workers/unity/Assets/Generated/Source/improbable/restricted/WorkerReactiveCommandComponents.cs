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

namespace Improbable.Restricted
{
    public partial class Worker
    {
        public class DisconnectReactiveCommandComponentManager : IReactiveCommandComponentManager
        {
            public void PopulateReactiveCommandComponents(CommandSystem commandSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var receivedRequests = commandSystem.GetRequests<Disconnect.ReceivedRequest>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedRequests.Count; ++i)
                {
                    if (!workerSystem.TryGetEntity(receivedRequests[i].EntityId, out var entity))
                    {
                        continue;
                    }

                    List<Disconnect.ReceivedRequest> requests;
                    if (entityManager.HasComponent<global::Improbable.Restricted.Worker.CommandRequests.Disconnect>(entity))
                    {
                        requests = entityManager.GetComponentData<global::Improbable.Restricted.Worker.CommandRequests.Disconnect>(entity).Requests;
                    }
                    else
                    {
                        var data = new global::Improbable.Restricted.Worker.CommandRequests.Disconnect
                        {
                            CommandListHandle = ReferenceTypeProviders.DisconnectRequestsProvider.Allocate(world)
                        };
                        data.Requests = new List<Disconnect.ReceivedRequest>();
                        requests = data.Requests;
                        entityManager.AddComponentData(entity, data);
                    }

                    requests.Add(receivedRequests[i]);
                }


                var receivedResponses = commandSystem.GetResponses<Disconnect.ReceivedResponse>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedResponses.Count; ++i)
                {
                    ref readonly var response = ref receivedResponses[i];

                    if (response.SendingEntity == Unity.Entities.Entity.Null || !entityManager.Exists(response.SendingEntity))
                    {
                        continue;
                    }

                    List<Disconnect.ReceivedResponse> responses;
                    if (entityManager.HasComponent<global::Improbable.Restricted.Worker.CommandResponses.Disconnect>(response.SendingEntity))
                    {
                        responses = entityManager.GetComponentData<global::Improbable.Restricted.Worker.CommandResponses.Disconnect>(response.SendingEntity).Responses;
                    }
                    else
                    {
                        var data = new global::Improbable.Restricted.Worker.CommandResponses.Disconnect
                        {
                            CommandListHandle = ReferenceTypeProviders.DisconnectResponsesProvider.Allocate(world)
                        };
                        data.Responses = new List<Disconnect.ReceivedResponse>();
                        responses = data.Responses;
                        entityManager.AddComponentData(response.SendingEntity, data);
                    }

                    responses.Add(response);
                }
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.DisconnectRequestsProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.DisconnectResponsesProvider.CleanDataInWorld(world);
            }
        }

        public class DisconnectCommandSenderComponentManager : ICommandSenderComponentManager
        {
            private Dictionary<EntityId, (uint Sender, uint Responder)> entityIdToAllocatedHandles =
                new Dictionary<EntityId, (uint Sender, uint Responder)>();

            public void AddComponents(Entity entity, EntityManager entityManager, World world)
            {
                // todo error message if not the worker entity or spatial entity
                EntityId entityId = entityManager.HasComponent<SpatialEntityId>(entity)
                    ? entityManager.GetComponentData<SpatialEntityId>(entity).EntityId
                    : new EntityId(0);

                var commandSender = new global::Improbable.Restricted.Worker.CommandSenders.Disconnect();
                commandSender.CommandListHandle = global::Improbable.Restricted.Worker.ReferenceTypeProviders.DisconnectSenderProvider.Allocate(world);
                commandSender.RequestsToSend = new List<global::Improbable.Restricted.Worker.Disconnect.Request>();

                entityManager.AddComponentData(entity, commandSender);

                var commandResponder = new global::Improbable.Restricted.Worker.CommandResponders.Disconnect();
                commandResponder.CommandListHandle =
                    global::Improbable.Restricted.Worker.ReferenceTypeProviders.DisconnectResponderProvider.Allocate(world);
                commandResponder.ResponsesToSend = new List<global::Improbable.Restricted.Worker.Disconnect.Response>();

                entityManager.AddComponentData(entity, commandResponder);

                entityIdToAllocatedHandles.Add(entityId, (commandSender.CommandListHandle, commandResponder.CommandListHandle));
            }

            public void RemoveComponents(EntityId entityId, EntityManager entityManager, World world)
            {
                var workerSystem = world.GetExistingSystem<WorkerSystem>();

                workerSystem.TryGetEntity(entityId, out var entity);

                if (entity != Entity.Null)
                {
                    entityManager.RemoveComponent<CommandSenders.Disconnect>(entity);
                    entityManager.RemoveComponent<CommandResponders.Disconnect>(entity);
                }

                if (!entityIdToAllocatedHandles.TryGetValue(entityId, out var handles))
                {
                    throw new ArgumentException("Command components not added to entity");
                }

                entityIdToAllocatedHandles.Remove(entityId);

                ReferenceTypeProviders.DisconnectSenderProvider.Free(handles.Sender);
                ReferenceTypeProviders.DisconnectResponderProvider.Free(handles.Responder);
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.DisconnectSenderProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.DisconnectResponderProvider.CleanDataInWorld(world);
            }
        }

    }
}
#endif
