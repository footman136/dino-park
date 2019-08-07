// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Worker.CInterop;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Dinopark.Npc
{
    [AutoRegisterSubscriptionManager]
    public class DinoAiDataReaderSubscriptionManager : SubscriptionManager<DinoAiDataReader>
    {
        private readonly EntityManager entityManager;
        private readonly World world;
        private readonly WorkerSystem workerSystem;

        private Dictionary<EntityId, HashSet<Subscription<DinoAiDataReader>>> entityIdToReaderSubscriptions;

        private Dictionary<EntityId, (ulong Added, ulong Removed)> entityIdToCallbackKey =
            new Dictionary<EntityId, (ulong Added, ulong Removed)>();

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public DinoAiDataReaderSubscriptionManager(World world)
        {
            this.world = world;
            entityManager = world.EntityManager;

            // todo Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();

            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterComponentAddedCallback(DinoAiData.ComponentId, entityId =>
            {
                if (!entitiesNotMatchingRequirements.Contains(entityId))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out var entity);

                foreach (var subscription in entityIdToReaderSubscriptions[entityId])
                {
                    subscription.SetAvailable(new DinoAiDataReader(world, entity, entityId));
                }

                entitiesMatchingRequirements.Add(entityId);
                entitiesNotMatchingRequirements.Remove(entityId);
            });

            constraintCallbackSystem.RegisterComponentRemovedCallback(DinoAiData.ComponentId, entityId =>
            {
                if (!entitiesMatchingRequirements.Contains(entityId))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out var entity);

                foreach (var subscription in entityIdToReaderSubscriptions[entityId])
                {
                    ResetValue(subscription);
                    subscription.SetUnavailable();
                }

                entitiesNotMatchingRequirements.Add(entityId);
                entitiesMatchingRequirements.Remove(entityId);
            });
        }

        public override Subscription<DinoAiDataReader> Subscribe(EntityId entityId)
        {
            if (entityIdToReaderSubscriptions == null)
            {
                entityIdToReaderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<DinoAiDataReader>>>();
            }

            var subscription = new Subscription<DinoAiDataReader>(this, entityId);

            if (!entityIdToReaderSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<DinoAiDataReader>>();
                entityIdToReaderSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && entityManager.HasComponent<DinoAiData.Component>(entity))
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new DinoAiDataReader(world, entity, entityId));
            }
            else
            {
                entitiesNotMatchingRequirements.Add(entityId);
            }

            subscriptions.Add(subscription);
            return subscription;
        }

        public override void Cancel(ISubscription subscription)
        {
            var sub = ((Subscription<DinoAiDataReader>) subscription);
            if (sub.HasValue)
            {
                var reader = sub.Value;
                reader.IsValid = false;
                reader.RemoveAllCallbacks();
            }

            var subscriptions = entityIdToReaderSubscriptions[sub.EntityId];
            subscriptions.Remove(sub);
            if (subscriptions.Count == 0)
            {
                entityIdToReaderSubscriptions.Remove(sub.EntityId);
                entitiesMatchingRequirements.Remove(sub.EntityId);
                entitiesNotMatchingRequirements.Remove(sub.EntityId);
            }
        }

        public override void ResetValue(ISubscription subscription)
        {
            var sub = ((Subscription<DinoAiDataReader>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }

        private void OnComponentAdded(EntityId entityId)
        {
        }

        private void OnComponentRemoved(EntityId entityId)
        {
        }
    }

    [AutoRegisterSubscriptionManager]
    public class DinoAiDataWriterSubscriptionManager : SubscriptionManager<DinoAiDataWriter>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private Dictionary<EntityId, HashSet<Subscription<DinoAiDataWriter>>> entityIdToWriterSubscriptions;

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public DinoAiDataWriterSubscriptionManager(World world)
        {
            this.world = world;

            // todo Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();

            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterAuthorityCallback(DinoAiData.ComponentId, authorityChange =>
            {
                if (authorityChange.Authority == Authority.Authoritative)
                {
                    if (!entitiesNotMatchingRequirements.Contains(authorityChange.EntityId))
                    {
                        return;
                    }

                    workerSystem.TryGetEntity(authorityChange.EntityId, out var entity);

                    foreach (var subscription in entityIdToWriterSubscriptions[authorityChange.EntityId])
                    {
                        subscription.SetAvailable(new DinoAiDataWriter(world, entity, authorityChange.EntityId));
                    }

                    entitiesMatchingRequirements.Add(authorityChange.EntityId);
                    entitiesNotMatchingRequirements.Remove(authorityChange.EntityId);
                }
                else if (authorityChange.Authority == Authority.NotAuthoritative)
                {
                    if (!entitiesMatchingRequirements.Contains(authorityChange.EntityId))
                    {
                        return;
                    }

                    workerSystem.TryGetEntity(authorityChange.EntityId, out var entity);

                    foreach (var subscription in entityIdToWriterSubscriptions[authorityChange.EntityId])
                    {
                        ResetValue(subscription);
                        subscription.SetUnavailable();
                    }

                    entitiesNotMatchingRequirements.Add(authorityChange.EntityId);
                    entitiesMatchingRequirements.Remove(authorityChange.EntityId);
                }
            });
        }

        public override Subscription<DinoAiDataWriter> Subscribe(EntityId entityId)
        {
            if (entityIdToWriterSubscriptions == null)
            {
                entityIdToWriterSubscriptions = new Dictionary<EntityId, HashSet<Subscription<DinoAiDataWriter>>>();
            }

            var subscription = new Subscription<DinoAiDataWriter>(this, entityId);

            if (!entityIdToWriterSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<DinoAiDataWriter>>();
                entityIdToWriterSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && componentUpdateSystem.HasComponent(DinoAiData.ComponentId, entityId)
                && componentUpdateSystem.GetAuthority(entityId, DinoAiData.ComponentId) != Authority.NotAuthoritative)
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new DinoAiDataWriter(world, entity, entityId));
            }
            else
            {
                entitiesNotMatchingRequirements.Add(entityId);
            }

            subscriptions.Add(subscription);
            return subscription;
        }

        public override void Cancel(ISubscription subscription)
        {
            var sub = ((Subscription<DinoAiDataWriter>) subscription);
            if (sub.HasValue)
            {
                var reader = sub.Value;
                reader.IsValid = false;
                reader.RemoveAllCallbacks();
            }

            var subscriptions = entityIdToWriterSubscriptions[sub.EntityId];
            subscriptions.Remove(sub);
            if (subscriptions.Count == 0)
            {
                entityIdToWriterSubscriptions.Remove(sub.EntityId);
                entitiesMatchingRequirements.Remove(sub.EntityId);
                entitiesNotMatchingRequirements.Remove(sub.EntityId);
            }
        }

        public override void ResetValue(ISubscription subscription)
        {
            var sub = ((Subscription<DinoAiDataWriter>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    public class DinoAiDataReader
    {
        public bool IsValid;

        protected readonly ComponentUpdateSystem ComponentUpdateSystem;
        protected readonly ComponentCallbackSystem CallbackSystem;
        protected readonly EntityManager EntityManager;
        protected readonly Entity Entity;
        protected readonly EntityId EntityId;

        public DinoAiData.Component Data
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Oh noes!");
                }

                return EntityManager.GetComponentData<DinoAiData.Component>(Entity);
            }
        }

        public Authority Authority
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Oh noes!");
                }

                return ComponentUpdateSystem.GetAuthority(EntityId, DinoAiData.ComponentId);
            }
        }

        private Dictionary<Action<Authority>, ulong> authorityCallbackToCallbackKey;
        public event Action<Authority> OnAuthorityUpdate
        {
            add
            {
                if (authorityCallbackToCallbackKey == null)
                {
                    authorityCallbackToCallbackKey = new Dictionary<Action<Authority>, ulong>();
                }

                var key = CallbackSystem.RegisterAuthorityCallback(EntityId, DinoAiData.ComponentId, value);
                authorityCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!authorityCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                authorityCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<DinoAiData.Update>, ulong> updateCallbackToCallbackKey;
        public event Action<DinoAiData.Update> OnUpdate
        {
            add
            {
                if (updateCallbackToCallbackKey == null)
                {
                    updateCallbackToCallbackKey = new Dictionary<Action<DinoAiData.Update>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback(EntityId, value);
                updateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!updateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                updateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::Dinopark.Npc.DinoAiFSMState.StateEnum>, ulong> currentAiStateUpdateCallbackToCallbackKey;
        public event Action<global::Dinopark.Npc.DinoAiFSMState.StateEnum> OnCurrentAiStateUpdate
        {
            add
            {
                if (currentAiStateUpdateCallbackToCallbackKey == null)
                {
                    currentAiStateUpdateCallbackToCallbackKey = new Dictionary<Action<global::Dinopark.Npc.DinoAiFSMState.StateEnum>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DinoAiData.Update>(EntityId, update =>
                {
                    if (update.CurrentAiState.HasValue)
                    {
                        value(update.CurrentAiState.Value);
                    }
                });
                currentAiStateUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!currentAiStateUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                currentAiStateUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::Improbable.Gdk.Core.EntityId>, ulong> targetEntityIdUpdateCallbackToCallbackKey;
        public event Action<global::Improbable.Gdk.Core.EntityId> OnTargetEntityIdUpdate
        {
            add
            {
                if (targetEntityIdUpdateCallbackToCallbackKey == null)
                {
                    targetEntityIdUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.Gdk.Core.EntityId>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DinoAiData.Update>(EntityId, update =>
                {
                    if (update.TargetEntityId.HasValue)
                    {
                        value(update.TargetEntityId.Value);
                    }
                });
                targetEntityIdUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!targetEntityIdUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                targetEntityIdUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::Improbable.Vector3f>, ulong> targetPositionUpdateCallbackToCallbackKey;
        public event Action<global::Improbable.Vector3f> OnTargetPositionUpdate
        {
            add
            {
                if (targetPositionUpdateCallbackToCallbackKey == null)
                {
                    targetPositionUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.Vector3f>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DinoAiData.Update>(EntityId, update =>
                {
                    if (update.TargetPosition.HasValue)
                    {
                        value(update.TargetPosition.Value);
                    }
                });
                targetPositionUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!targetPositionUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                targetPositionUpdateCallbackToCallbackKey.Remove(value);
            }
        }


        internal DinoAiDataReader(World world, Entity entity, EntityId entityId)
        {
            Entity = entity;
            EntityId = entityId;

            IsValid = true;

            ComponentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();
            CallbackSystem = world.GetExistingSystem<ComponentCallbackSystem>();
            EntityManager = world.EntityManager;
        }

        public void RemoveAllCallbacks()
        {
            if (authorityCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in authorityCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                authorityCallbackToCallbackKey.Clear();
            }

            if (updateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in updateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                updateCallbackToCallbackKey.Clear();
            }


            if (currentAiStateUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in currentAiStateUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                currentAiStateUpdateCallbackToCallbackKey.Clear();
            }

            if (targetEntityIdUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in targetEntityIdUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                targetEntityIdUpdateCallbackToCallbackKey.Clear();
            }

            if (targetPositionUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in targetPositionUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                targetPositionUpdateCallbackToCallbackKey.Clear();
            }
        }
    }

    public class DinoAiDataWriter : DinoAiDataReader
    {
        internal DinoAiDataWriter(World world, Entity entity, EntityId entityId)
            : base(world, entity, entityId)
        {
        }

        public void SendUpdate(DinoAiData.Update update)
        {
            var component = EntityManager.GetComponentData<DinoAiData.Component>(Entity);

            if (update.CurrentAiState.HasValue)
            {
                component.CurrentAiState = update.CurrentAiState.Value;
            }

            if (update.TargetEntityId.HasValue)
            {
                component.TargetEntityId = update.TargetEntityId.Value;
            }

            if (update.TargetPosition.HasValue)
            {
                component.TargetPosition = update.TargetPosition.Value;
            }

            EntityManager.SetComponentData(Entity, component);
        }


        public void AcknowledgeAuthorityLoss()
        {
            ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, DinoAiData.ComponentId);
        }
    }
}
