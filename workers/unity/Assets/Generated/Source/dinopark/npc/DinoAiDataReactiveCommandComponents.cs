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
    public partial class DinoAiData
    {
        public class AttackReactiveCommandComponentManager : IReactiveCommandComponentManager
        {
            public void PopulateReactiveCommandComponents(CommandSystem commandSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var receivedRequests = commandSystem.GetRequests<Attack.ReceivedRequest>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedRequests.Count; ++i)
                {
                    if (!workerSystem.TryGetEntity(receivedRequests[i].EntityId, out var entity))
                    {
                        continue;
                    }

                    List<Attack.ReceivedRequest> requests;
                    if (entityManager.HasComponent<global::Dinopark.Npc.DinoAiData.CommandRequests.Attack>(entity))
                    {
                        requests = entityManager.GetComponentData<global::Dinopark.Npc.DinoAiData.CommandRequests.Attack>(entity).Requests;
                    }
                    else
                    {
                        var data = new global::Dinopark.Npc.DinoAiData.CommandRequests.Attack
                        {
                            CommandListHandle = ReferenceTypeProviders.AttackRequestsProvider.Allocate(world)
                        };
                        data.Requests = new List<Attack.ReceivedRequest>();
                        requests = data.Requests;
                        entityManager.AddComponentData(entity, data);
                    }

                    requests.Add(receivedRequests[i]);
                }


                var receivedResponses = commandSystem.GetResponses<Attack.ReceivedResponse>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedResponses.Count; ++i)
                {
                    ref readonly var response = ref receivedResponses[i];

                    if (response.SendingEntity == Unity.Entities.Entity.Null || !entityManager.Exists(response.SendingEntity))
                    {
                        continue;
                    }

                    List<Attack.ReceivedResponse> responses;
                    if (entityManager.HasComponent<global::Dinopark.Npc.DinoAiData.CommandResponses.Attack>(response.SendingEntity))
                    {
                        responses = entityManager.GetComponentData<global::Dinopark.Npc.DinoAiData.CommandResponses.Attack>(response.SendingEntity).Responses;
                    }
                    else
                    {
                        var data = new global::Dinopark.Npc.DinoAiData.CommandResponses.Attack
                        {
                            CommandListHandle = ReferenceTypeProviders.AttackResponsesProvider.Allocate(world)
                        };
                        data.Responses = new List<Attack.ReceivedResponse>();
                        responses = data.Responses;
                        entityManager.AddComponentData(response.SendingEntity, data);
                    }

                    responses.Add(response);
                }
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.AttackRequestsProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.AttackResponsesProvider.CleanDataInWorld(world);
            }
        }

        public class AttackCommandSenderComponentManager : ICommandSenderComponentManager
        {
            private Dictionary<EntityId, (uint Sender, uint Responder)> entityIdToAllocatedHandles =
                new Dictionary<EntityId, (uint Sender, uint Responder)>();

            public void AddComponents(Entity entity, EntityManager entityManager, World world)
            {
                // todo error message if not the worker entity or spatial entity
                EntityId entityId = entityManager.HasComponent<SpatialEntityId>(entity)
                    ? entityManager.GetComponentData<SpatialEntityId>(entity).EntityId
                    : new EntityId(0);

                var commandSender = new global::Dinopark.Npc.DinoAiData.CommandSenders.Attack();
                commandSender.CommandListHandle = global::Dinopark.Npc.DinoAiData.ReferenceTypeProviders.AttackSenderProvider.Allocate(world);
                commandSender.RequestsToSend = new List<global::Dinopark.Npc.DinoAiData.Attack.Request>();

                entityManager.AddComponentData(entity, commandSender);

                var commandResponder = new global::Dinopark.Npc.DinoAiData.CommandResponders.Attack();
                commandResponder.CommandListHandle =
                    global::Dinopark.Npc.DinoAiData.ReferenceTypeProviders.AttackResponderProvider.Allocate(world);
                commandResponder.ResponsesToSend = new List<global::Dinopark.Npc.DinoAiData.Attack.Response>();

                entityManager.AddComponentData(entity, commandResponder);

                entityIdToAllocatedHandles.Add(entityId, (commandSender.CommandListHandle, commandResponder.CommandListHandle));
            }

            public void RemoveComponents(EntityId entityId, EntityManager entityManager, World world)
            {
                var workerSystem = world.GetExistingSystem<WorkerSystem>();

                workerSystem.TryGetEntity(entityId, out var entity);

                if (entity != Entity.Null)
                {
                    entityManager.RemoveComponent<CommandSenders.Attack>(entity);
                    entityManager.RemoveComponent<CommandResponders.Attack>(entity);
                }

                if (!entityIdToAllocatedHandles.TryGetValue(entityId, out var handles))
                {
                    throw new ArgumentException("Command components not added to entity");
                }

                entityIdToAllocatedHandles.Remove(entityId);

                ReferenceTypeProviders.AttackSenderProvider.Free(handles.Sender);
                ReferenceTypeProviders.AttackResponderProvider.Free(handles.Responder);
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.AttackSenderProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.AttackResponderProvider.CleanDataInWorld(world);
            }
        }

        public class EatReactiveCommandComponentManager : IReactiveCommandComponentManager
        {
            public void PopulateReactiveCommandComponents(CommandSystem commandSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var receivedRequests = commandSystem.GetRequests<Eat.ReceivedRequest>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedRequests.Count; ++i)
                {
                    if (!workerSystem.TryGetEntity(receivedRequests[i].EntityId, out var entity))
                    {
                        continue;
                    }

                    List<Eat.ReceivedRequest> requests;
                    if (entityManager.HasComponent<global::Dinopark.Npc.DinoAiData.CommandRequests.Eat>(entity))
                    {
                        requests = entityManager.GetComponentData<global::Dinopark.Npc.DinoAiData.CommandRequests.Eat>(entity).Requests;
                    }
                    else
                    {
                        var data = new global::Dinopark.Npc.DinoAiData.CommandRequests.Eat
                        {
                            CommandListHandle = ReferenceTypeProviders.EatRequestsProvider.Allocate(world)
                        };
                        data.Requests = new List<Eat.ReceivedRequest>();
                        requests = data.Requests;
                        entityManager.AddComponentData(entity, data);
                    }

                    requests.Add(receivedRequests[i]);
                }


                var receivedResponses = commandSystem.GetResponses<Eat.ReceivedResponse>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedResponses.Count; ++i)
                {
                    ref readonly var response = ref receivedResponses[i];

                    if (response.SendingEntity == Unity.Entities.Entity.Null || !entityManager.Exists(response.SendingEntity))
                    {
                        continue;
                    }

                    List<Eat.ReceivedResponse> responses;
                    if (entityManager.HasComponent<global::Dinopark.Npc.DinoAiData.CommandResponses.Eat>(response.SendingEntity))
                    {
                        responses = entityManager.GetComponentData<global::Dinopark.Npc.DinoAiData.CommandResponses.Eat>(response.SendingEntity).Responses;
                    }
                    else
                    {
                        var data = new global::Dinopark.Npc.DinoAiData.CommandResponses.Eat
                        {
                            CommandListHandle = ReferenceTypeProviders.EatResponsesProvider.Allocate(world)
                        };
                        data.Responses = new List<Eat.ReceivedResponse>();
                        responses = data.Responses;
                        entityManager.AddComponentData(response.SendingEntity, data);
                    }

                    responses.Add(response);
                }
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.EatRequestsProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.EatResponsesProvider.CleanDataInWorld(world);
            }
        }

        public class EatCommandSenderComponentManager : ICommandSenderComponentManager
        {
            private Dictionary<EntityId, (uint Sender, uint Responder)> entityIdToAllocatedHandles =
                new Dictionary<EntityId, (uint Sender, uint Responder)>();

            public void AddComponents(Entity entity, EntityManager entityManager, World world)
            {
                // todo error message if not the worker entity or spatial entity
                EntityId entityId = entityManager.HasComponent<SpatialEntityId>(entity)
                    ? entityManager.GetComponentData<SpatialEntityId>(entity).EntityId
                    : new EntityId(0);

                var commandSender = new global::Dinopark.Npc.DinoAiData.CommandSenders.Eat();
                commandSender.CommandListHandle = global::Dinopark.Npc.DinoAiData.ReferenceTypeProviders.EatSenderProvider.Allocate(world);
                commandSender.RequestsToSend = new List<global::Dinopark.Npc.DinoAiData.Eat.Request>();

                entityManager.AddComponentData(entity, commandSender);

                var commandResponder = new global::Dinopark.Npc.DinoAiData.CommandResponders.Eat();
                commandResponder.CommandListHandle =
                    global::Dinopark.Npc.DinoAiData.ReferenceTypeProviders.EatResponderProvider.Allocate(world);
                commandResponder.ResponsesToSend = new List<global::Dinopark.Npc.DinoAiData.Eat.Response>();

                entityManager.AddComponentData(entity, commandResponder);

                entityIdToAllocatedHandles.Add(entityId, (commandSender.CommandListHandle, commandResponder.CommandListHandle));
            }

            public void RemoveComponents(EntityId entityId, EntityManager entityManager, World world)
            {
                var workerSystem = world.GetExistingSystem<WorkerSystem>();

                workerSystem.TryGetEntity(entityId, out var entity);

                if (entity != Entity.Null)
                {
                    entityManager.RemoveComponent<CommandSenders.Eat>(entity);
                    entityManager.RemoveComponent<CommandResponders.Eat>(entity);
                }

                if (!entityIdToAllocatedHandles.TryGetValue(entityId, out var handles))
                {
                    throw new ArgumentException("Command components not added to entity");
                }

                entityIdToAllocatedHandles.Remove(entityId);

                ReferenceTypeProviders.EatSenderProvider.Free(handles.Sender);
                ReferenceTypeProviders.EatResponderProvider.Free(handles.Responder);
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.EatSenderProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.EatResponderProvider.CleanDataInWorld(world);
            }
        }

    }
}
#endif
