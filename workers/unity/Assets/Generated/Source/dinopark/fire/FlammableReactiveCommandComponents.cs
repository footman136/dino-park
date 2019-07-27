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

namespace Dinopark.Fire
{
    public partial class Flammable
    {
        public class IgniteReactiveCommandComponentManager : IReactiveCommandComponentManager
        {
            public void PopulateReactiveCommandComponents(CommandSystem commandSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var receivedRequests = commandSystem.GetRequests<Ignite.ReceivedRequest>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedRequests.Count; ++i)
                {
                    if (!workerSystem.TryGetEntity(receivedRequests[i].EntityId, out var entity))
                    {
                        continue;
                    }

                    List<Ignite.ReceivedRequest> requests;
                    if (entityManager.HasComponent<global::Dinopark.Fire.Flammable.CommandRequests.Ignite>(entity))
                    {
                        requests = entityManager.GetComponentData<global::Dinopark.Fire.Flammable.CommandRequests.Ignite>(entity).Requests;
                    }
                    else
                    {
                        var data = new global::Dinopark.Fire.Flammable.CommandRequests.Ignite
                        {
                            CommandListHandle = ReferenceTypeProviders.IgniteRequestsProvider.Allocate(world)
                        };
                        data.Requests = new List<Ignite.ReceivedRequest>();
                        requests = data.Requests;
                        entityManager.AddComponentData(entity, data);
                    }

                    requests.Add(receivedRequests[i]);
                }


                var receivedResponses = commandSystem.GetResponses<Ignite.ReceivedResponse>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedResponses.Count; ++i)
                {
                    ref readonly var response = ref receivedResponses[i];

                    if (response.SendingEntity == Unity.Entities.Entity.Null || !entityManager.Exists(response.SendingEntity))
                    {
                        continue;
                    }

                    List<Ignite.ReceivedResponse> responses;
                    if (entityManager.HasComponent<global::Dinopark.Fire.Flammable.CommandResponses.Ignite>(response.SendingEntity))
                    {
                        responses = entityManager.GetComponentData<global::Dinopark.Fire.Flammable.CommandResponses.Ignite>(response.SendingEntity).Responses;
                    }
                    else
                    {
                        var data = new global::Dinopark.Fire.Flammable.CommandResponses.Ignite
                        {
                            CommandListHandle = ReferenceTypeProviders.IgniteResponsesProvider.Allocate(world)
                        };
                        data.Responses = new List<Ignite.ReceivedResponse>();
                        responses = data.Responses;
                        entityManager.AddComponentData(response.SendingEntity, data);
                    }

                    responses.Add(response);
                }
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.IgniteRequestsProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.IgniteResponsesProvider.CleanDataInWorld(world);
            }
        }

        public class IgniteCommandSenderComponentManager : ICommandSenderComponentManager
        {
            private Dictionary<EntityId, (uint Sender, uint Responder)> entityIdToAllocatedHandles =
                new Dictionary<EntityId, (uint Sender, uint Responder)>();

            public void AddComponents(Entity entity, EntityManager entityManager, World world)
            {
                // todo error message if not the worker entity or spatial entity
                EntityId entityId = entityManager.HasComponent<SpatialEntityId>(entity)
                    ? entityManager.GetComponentData<SpatialEntityId>(entity).EntityId
                    : new EntityId(0);

                var commandSender = new global::Dinopark.Fire.Flammable.CommandSenders.Ignite();
                commandSender.CommandListHandle = global::Dinopark.Fire.Flammable.ReferenceTypeProviders.IgniteSenderProvider.Allocate(world);
                commandSender.RequestsToSend = new List<global::Dinopark.Fire.Flammable.Ignite.Request>();

                entityManager.AddComponentData(entity, commandSender);

                var commandResponder = new global::Dinopark.Fire.Flammable.CommandResponders.Ignite();
                commandResponder.CommandListHandle =
                    global::Dinopark.Fire.Flammable.ReferenceTypeProviders.IgniteResponderProvider.Allocate(world);
                commandResponder.ResponsesToSend = new List<global::Dinopark.Fire.Flammable.Ignite.Response>();

                entityManager.AddComponentData(entity, commandResponder);

                entityIdToAllocatedHandles.Add(entityId, (commandSender.CommandListHandle, commandResponder.CommandListHandle));
            }

            public void RemoveComponents(EntityId entityId, EntityManager entityManager, World world)
            {
                var workerSystem = world.GetExistingSystem<WorkerSystem>();

                workerSystem.TryGetEntity(entityId, out var entity);

                if (entity != Entity.Null)
                {
                    entityManager.RemoveComponent<CommandSenders.Ignite>(entity);
                    entityManager.RemoveComponent<CommandResponders.Ignite>(entity);
                }

                if (!entityIdToAllocatedHandles.TryGetValue(entityId, out var handles))
                {
                    throw new ArgumentException("Command components not added to entity");
                }

                entityIdToAllocatedHandles.Remove(entityId);

                ReferenceTypeProviders.IgniteSenderProvider.Free(handles.Sender);
                ReferenceTypeProviders.IgniteResponderProvider.Free(handles.Responder);
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.IgniteSenderProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.IgniteResponderProvider.CleanDataInWorld(world);
            }
        }

        public class ExtinguishReactiveCommandComponentManager : IReactiveCommandComponentManager
        {
            public void PopulateReactiveCommandComponents(CommandSystem commandSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var receivedRequests = commandSystem.GetRequests<Extinguish.ReceivedRequest>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedRequests.Count; ++i)
                {
                    if (!workerSystem.TryGetEntity(receivedRequests[i].EntityId, out var entity))
                    {
                        continue;
                    }

                    List<Extinguish.ReceivedRequest> requests;
                    if (entityManager.HasComponent<global::Dinopark.Fire.Flammable.CommandRequests.Extinguish>(entity))
                    {
                        requests = entityManager.GetComponentData<global::Dinopark.Fire.Flammable.CommandRequests.Extinguish>(entity).Requests;
                    }
                    else
                    {
                        var data = new global::Dinopark.Fire.Flammable.CommandRequests.Extinguish
                        {
                            CommandListHandle = ReferenceTypeProviders.ExtinguishRequestsProvider.Allocate(world)
                        };
                        data.Requests = new List<Extinguish.ReceivedRequest>();
                        requests = data.Requests;
                        entityManager.AddComponentData(entity, data);
                    }

                    requests.Add(receivedRequests[i]);
                }


                var receivedResponses = commandSystem.GetResponses<Extinguish.ReceivedResponse>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedResponses.Count; ++i)
                {
                    ref readonly var response = ref receivedResponses[i];

                    if (response.SendingEntity == Unity.Entities.Entity.Null || !entityManager.Exists(response.SendingEntity))
                    {
                        continue;
                    }

                    List<Extinguish.ReceivedResponse> responses;
                    if (entityManager.HasComponent<global::Dinopark.Fire.Flammable.CommandResponses.Extinguish>(response.SendingEntity))
                    {
                        responses = entityManager.GetComponentData<global::Dinopark.Fire.Flammable.CommandResponses.Extinguish>(response.SendingEntity).Responses;
                    }
                    else
                    {
                        var data = new global::Dinopark.Fire.Flammable.CommandResponses.Extinguish
                        {
                            CommandListHandle = ReferenceTypeProviders.ExtinguishResponsesProvider.Allocate(world)
                        };
                        data.Responses = new List<Extinguish.ReceivedResponse>();
                        responses = data.Responses;
                        entityManager.AddComponentData(response.SendingEntity, data);
                    }

                    responses.Add(response);
                }
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.ExtinguishRequestsProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.ExtinguishResponsesProvider.CleanDataInWorld(world);
            }
        }

        public class ExtinguishCommandSenderComponentManager : ICommandSenderComponentManager
        {
            private Dictionary<EntityId, (uint Sender, uint Responder)> entityIdToAllocatedHandles =
                new Dictionary<EntityId, (uint Sender, uint Responder)>();

            public void AddComponents(Entity entity, EntityManager entityManager, World world)
            {
                // todo error message if not the worker entity or spatial entity
                EntityId entityId = entityManager.HasComponent<SpatialEntityId>(entity)
                    ? entityManager.GetComponentData<SpatialEntityId>(entity).EntityId
                    : new EntityId(0);

                var commandSender = new global::Dinopark.Fire.Flammable.CommandSenders.Extinguish();
                commandSender.CommandListHandle = global::Dinopark.Fire.Flammable.ReferenceTypeProviders.ExtinguishSenderProvider.Allocate(world);
                commandSender.RequestsToSend = new List<global::Dinopark.Fire.Flammable.Extinguish.Request>();

                entityManager.AddComponentData(entity, commandSender);

                var commandResponder = new global::Dinopark.Fire.Flammable.CommandResponders.Extinguish();
                commandResponder.CommandListHandle =
                    global::Dinopark.Fire.Flammable.ReferenceTypeProviders.ExtinguishResponderProvider.Allocate(world);
                commandResponder.ResponsesToSend = new List<global::Dinopark.Fire.Flammable.Extinguish.Response>();

                entityManager.AddComponentData(entity, commandResponder);

                entityIdToAllocatedHandles.Add(entityId, (commandSender.CommandListHandle, commandResponder.CommandListHandle));
            }

            public void RemoveComponents(EntityId entityId, EntityManager entityManager, World world)
            {
                var workerSystem = world.GetExistingSystem<WorkerSystem>();

                workerSystem.TryGetEntity(entityId, out var entity);

                if (entity != Entity.Null)
                {
                    entityManager.RemoveComponent<CommandSenders.Extinguish>(entity);
                    entityManager.RemoveComponent<CommandResponders.Extinguish>(entity);
                }

                if (!entityIdToAllocatedHandles.TryGetValue(entityId, out var handles))
                {
                    throw new ArgumentException("Command components not added to entity");
                }

                entityIdToAllocatedHandles.Remove(entityId);

                ReferenceTypeProviders.ExtinguishSenderProvider.Free(handles.Sender);
                ReferenceTypeProviders.ExtinguishResponderProvider.Free(handles.Responder);
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.ExtinguishSenderProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.ExtinguishResponderProvider.CleanDataInWorld(world);
            }
        }

        public class SetCanBeIgnitedReactiveCommandComponentManager : IReactiveCommandComponentManager
        {
            public void PopulateReactiveCommandComponents(CommandSystem commandSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var receivedRequests = commandSystem.GetRequests<SetCanBeIgnited.ReceivedRequest>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedRequests.Count; ++i)
                {
                    if (!workerSystem.TryGetEntity(receivedRequests[i].EntityId, out var entity))
                    {
                        continue;
                    }

                    List<SetCanBeIgnited.ReceivedRequest> requests;
                    if (entityManager.HasComponent<global::Dinopark.Fire.Flammable.CommandRequests.SetCanBeIgnited>(entity))
                    {
                        requests = entityManager.GetComponentData<global::Dinopark.Fire.Flammable.CommandRequests.SetCanBeIgnited>(entity).Requests;
                    }
                    else
                    {
                        var data = new global::Dinopark.Fire.Flammable.CommandRequests.SetCanBeIgnited
                        {
                            CommandListHandle = ReferenceTypeProviders.SetCanBeIgnitedRequestsProvider.Allocate(world)
                        };
                        data.Requests = new List<SetCanBeIgnited.ReceivedRequest>();
                        requests = data.Requests;
                        entityManager.AddComponentData(entity, data);
                    }

                    requests.Add(receivedRequests[i]);
                }


                var receivedResponses = commandSystem.GetResponses<SetCanBeIgnited.ReceivedResponse>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedResponses.Count; ++i)
                {
                    ref readonly var response = ref receivedResponses[i];

                    if (response.SendingEntity == Unity.Entities.Entity.Null || !entityManager.Exists(response.SendingEntity))
                    {
                        continue;
                    }

                    List<SetCanBeIgnited.ReceivedResponse> responses;
                    if (entityManager.HasComponent<global::Dinopark.Fire.Flammable.CommandResponses.SetCanBeIgnited>(response.SendingEntity))
                    {
                        responses = entityManager.GetComponentData<global::Dinopark.Fire.Flammable.CommandResponses.SetCanBeIgnited>(response.SendingEntity).Responses;
                    }
                    else
                    {
                        var data = new global::Dinopark.Fire.Flammable.CommandResponses.SetCanBeIgnited
                        {
                            CommandListHandle = ReferenceTypeProviders.SetCanBeIgnitedResponsesProvider.Allocate(world)
                        };
                        data.Responses = new List<SetCanBeIgnited.ReceivedResponse>();
                        responses = data.Responses;
                        entityManager.AddComponentData(response.SendingEntity, data);
                    }

                    responses.Add(response);
                }
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.SetCanBeIgnitedRequestsProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.SetCanBeIgnitedResponsesProvider.CleanDataInWorld(world);
            }
        }

        public class SetCanBeIgnitedCommandSenderComponentManager : ICommandSenderComponentManager
        {
            private Dictionary<EntityId, (uint Sender, uint Responder)> entityIdToAllocatedHandles =
                new Dictionary<EntityId, (uint Sender, uint Responder)>();

            public void AddComponents(Entity entity, EntityManager entityManager, World world)
            {
                // todo error message if not the worker entity or spatial entity
                EntityId entityId = entityManager.HasComponent<SpatialEntityId>(entity)
                    ? entityManager.GetComponentData<SpatialEntityId>(entity).EntityId
                    : new EntityId(0);

                var commandSender = new global::Dinopark.Fire.Flammable.CommandSenders.SetCanBeIgnited();
                commandSender.CommandListHandle = global::Dinopark.Fire.Flammable.ReferenceTypeProviders.SetCanBeIgnitedSenderProvider.Allocate(world);
                commandSender.RequestsToSend = new List<global::Dinopark.Fire.Flammable.SetCanBeIgnited.Request>();

                entityManager.AddComponentData(entity, commandSender);

                var commandResponder = new global::Dinopark.Fire.Flammable.CommandResponders.SetCanBeIgnited();
                commandResponder.CommandListHandle =
                    global::Dinopark.Fire.Flammable.ReferenceTypeProviders.SetCanBeIgnitedResponderProvider.Allocate(world);
                commandResponder.ResponsesToSend = new List<global::Dinopark.Fire.Flammable.SetCanBeIgnited.Response>();

                entityManager.AddComponentData(entity, commandResponder);

                entityIdToAllocatedHandles.Add(entityId, (commandSender.CommandListHandle, commandResponder.CommandListHandle));
            }

            public void RemoveComponents(EntityId entityId, EntityManager entityManager, World world)
            {
                var workerSystem = world.GetExistingSystem<WorkerSystem>();

                workerSystem.TryGetEntity(entityId, out var entity);

                if (entity != Entity.Null)
                {
                    entityManager.RemoveComponent<CommandSenders.SetCanBeIgnited>(entity);
                    entityManager.RemoveComponent<CommandResponders.SetCanBeIgnited>(entity);
                }

                if (!entityIdToAllocatedHandles.TryGetValue(entityId, out var handles))
                {
                    throw new ArgumentException("Command components not added to entity");
                }

                entityIdToAllocatedHandles.Remove(entityId);

                ReferenceTypeProviders.SetCanBeIgnitedSenderProvider.Free(handles.Sender);
                ReferenceTypeProviders.SetCanBeIgnitedResponderProvider.Free(handles.Responder);
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.SetCanBeIgnitedSenderProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.SetCanBeIgnitedResponderProvider.CleanDataInWorld(world);
            }
        }

    }
}
#endif
