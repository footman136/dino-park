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
    public class DinoAttrsReaderSubscriptionManager : SubscriptionManager<DinoAttrsReader>
    {
        private readonly EntityManager entityManager;
        private readonly World world;
        private readonly WorkerSystem workerSystem;

        private Dictionary<EntityId, HashSet<Subscription<DinoAttrsReader>>> entityIdToReaderSubscriptions;

        private Dictionary<EntityId, (ulong Added, ulong Removed)> entityIdToCallbackKey =
            new Dictionary<EntityId, (ulong Added, ulong Removed)>();

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public DinoAttrsReaderSubscriptionManager(World world)
        {
            this.world = world;
            entityManager = world.EntityManager;

            // todo Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();

            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterComponentAddedCallback(DinoAttrs.ComponentId, entityId =>
            {
                if (!entitiesNotMatchingRequirements.Contains(entityId))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out var entity);

                foreach (var subscription in entityIdToReaderSubscriptions[entityId])
                {
                    subscription.SetAvailable(new DinoAttrsReader(world, entity, entityId));
                }

                entitiesMatchingRequirements.Add(entityId);
                entitiesNotMatchingRequirements.Remove(entityId);
            });

            constraintCallbackSystem.RegisterComponentRemovedCallback(DinoAttrs.ComponentId, entityId =>
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

        public override Subscription<DinoAttrsReader> Subscribe(EntityId entityId)
        {
            if (entityIdToReaderSubscriptions == null)
            {
                entityIdToReaderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<DinoAttrsReader>>>();
            }

            var subscription = new Subscription<DinoAttrsReader>(this, entityId);

            if (!entityIdToReaderSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<DinoAttrsReader>>();
                entityIdToReaderSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && entityManager.HasComponent<DinoAttrs.Component>(entity))
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new DinoAttrsReader(world, entity, entityId));
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
            var sub = ((Subscription<DinoAttrsReader>) subscription);
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
            var sub = ((Subscription<DinoAttrsReader>) subscription);
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
    public class DinoAttrsWriterSubscriptionManager : SubscriptionManager<DinoAttrsWriter>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private Dictionary<EntityId, HashSet<Subscription<DinoAttrsWriter>>> entityIdToWriterSubscriptions;

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public DinoAttrsWriterSubscriptionManager(World world)
        {
            this.world = world;

            // todo Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();

            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterAuthorityCallback(DinoAttrs.ComponentId, authorityChange =>
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
                        subscription.SetAvailable(new DinoAttrsWriter(world, entity, authorityChange.EntityId));
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

        public override Subscription<DinoAttrsWriter> Subscribe(EntityId entityId)
        {
            if (entityIdToWriterSubscriptions == null)
            {
                entityIdToWriterSubscriptions = new Dictionary<EntityId, HashSet<Subscription<DinoAttrsWriter>>>();
            }

            var subscription = new Subscription<DinoAttrsWriter>(this, entityId);

            if (!entityIdToWriterSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<DinoAttrsWriter>>();
                entityIdToWriterSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && componentUpdateSystem.HasComponent(DinoAttrs.ComponentId, entityId)
                && componentUpdateSystem.GetAuthority(entityId, DinoAttrs.ComponentId) != Authority.NotAuthoritative)
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new DinoAttrsWriter(world, entity, entityId));
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
            var sub = ((Subscription<DinoAttrsWriter>) subscription);
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
            var sub = ((Subscription<DinoAttrsWriter>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    public class DinoAttrsReader
    {
        public bool IsValid;

        protected readonly ComponentUpdateSystem ComponentUpdateSystem;
        protected readonly ComponentCallbackSystem CallbackSystem;
        protected readonly EntityManager EntityManager;
        protected readonly Entity Entity;
        protected readonly EntityId EntityId;

        public DinoAttrs.Component Data
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Oh noes!");
                }

                return EntityManager.GetComponentData<DinoAttrs.Component>(Entity);
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

                return ComponentUpdateSystem.GetAuthority(EntityId, DinoAttrs.ComponentId);
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

                var key = CallbackSystem.RegisterAuthorityCallback(EntityId, DinoAttrs.ComponentId, value);
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

        private Dictionary<Action<DinoAttrs.Update>, ulong> updateCallbackToCallbackKey;
        public event Action<DinoAttrs.Update> OnUpdate
        {
            add
            {
                if (updateCallbackToCallbackKey == null)
                {
                    updateCallbackToCallbackKey = new Dictionary<Action<DinoAttrs.Update>, ulong>();
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

        private Dictionary<Action<bool>, ulong> isDeadUpdateCallbackToCallbackKey;
        public event Action<bool> OnIsDeadUpdate
        {
            add
            {
                if (isDeadUpdateCallbackToCallbackKey == null)
                {
                    isDeadUpdateCallbackToCallbackKey = new Dictionary<Action<bool>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DinoAttrs.Update>(EntityId, update =>
                {
                    if (update.IsDead.HasValue)
                    {
                        value(update.IsDead.Value);
                    }
                });
                isDeadUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!isDeadUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                isDeadUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<float>, ulong> currentFoodUpdateCallbackToCallbackKey;
        public event Action<float> OnCurrentFoodUpdate
        {
            add
            {
                if (currentFoodUpdateCallbackToCallbackKey == null)
                {
                    currentFoodUpdateCallbackToCallbackKey = new Dictionary<Action<float>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DinoAttrs.Update>(EntityId, update =>
                {
                    if (update.CurrentFood.HasValue)
                    {
                        value(update.CurrentFood.Value);
                    }
                });
                currentFoodUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!currentFoodUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                currentFoodUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<float>, ulong> originalScentUpdateCallbackToCallbackKey;
        public event Action<float> OnOriginalScentUpdate
        {
            add
            {
                if (originalScentUpdateCallbackToCallbackKey == null)
                {
                    originalScentUpdateCallbackToCallbackKey = new Dictionary<Action<float>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DinoAttrs.Update>(EntityId, update =>
                {
                    if (update.OriginalScent.HasValue)
                    {
                        value(update.OriginalScent.Value);
                    }
                });
                originalScentUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!originalScentUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                originalScentUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<float>, ulong> originalAgressionUpdateCallbackToCallbackKey;
        public event Action<float> OnOriginalAgressionUpdate
        {
            add
            {
                if (originalAgressionUpdateCallbackToCallbackKey == null)
                {
                    originalAgressionUpdateCallbackToCallbackKey = new Dictionary<Action<float>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DinoAttrs.Update>(EntityId, update =>
                {
                    if (update.OriginalAgression.HasValue)
                    {
                        value(update.OriginalAgression.Value);
                    }
                });
                originalAgressionUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!originalAgressionUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                originalAgressionUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<int>, ulong> originalDominanceUpdateCallbackToCallbackKey;
        public event Action<int> OnOriginalDominanceUpdate
        {
            add
            {
                if (originalDominanceUpdateCallbackToCallbackKey == null)
                {
                    originalDominanceUpdateCallbackToCallbackKey = new Dictionary<Action<int>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DinoAttrs.Update>(EntityId, update =>
                {
                    if (update.OriginalDominance.HasValue)
                    {
                        value(update.OriginalDominance.Value);
                    }
                });
                originalDominanceUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!originalDominanceUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                originalDominanceUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::Improbable.Vector3f>, ulong> originPosotionUpdateCallbackToCallbackKey;
        public event Action<global::Improbable.Vector3f> OnOriginPosotionUpdate
        {
            add
            {
                if (originPosotionUpdateCallbackToCallbackKey == null)
                {
                    originPosotionUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.Vector3f>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DinoAttrs.Update>(EntityId, update =>
                {
                    if (update.OriginPosotion.HasValue)
                    {
                        value(update.OriginPosotion.Value);
                    }
                });
                originPosotionUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!originPosotionUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                originPosotionUpdateCallbackToCallbackKey.Remove(value);
            }
        }


        internal DinoAttrsReader(World world, Entity entity, EntityId entityId)
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


            if (isDeadUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in isDeadUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                isDeadUpdateCallbackToCallbackKey.Clear();
            }

            if (currentFoodUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in currentFoodUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                currentFoodUpdateCallbackToCallbackKey.Clear();
            }

            if (originalScentUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in originalScentUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                originalScentUpdateCallbackToCallbackKey.Clear();
            }

            if (originalAgressionUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in originalAgressionUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                originalAgressionUpdateCallbackToCallbackKey.Clear();
            }

            if (originalDominanceUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in originalDominanceUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                originalDominanceUpdateCallbackToCallbackKey.Clear();
            }

            if (originPosotionUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in originPosotionUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                originPosotionUpdateCallbackToCallbackKey.Clear();
            }
        }
    }

    public class DinoAttrsWriter : DinoAttrsReader
    {
        internal DinoAttrsWriter(World world, Entity entity, EntityId entityId)
            : base(world, entity, entityId)
        {
        }

        public void SendUpdate(DinoAttrs.Update update)
        {
            var component = EntityManager.GetComponentData<DinoAttrs.Component>(Entity);

            if (update.IsDead.HasValue)
            {
                component.IsDead = update.IsDead.Value;
            }

            if (update.CurrentFood.HasValue)
            {
                component.CurrentFood = update.CurrentFood.Value;
            }

            if (update.OriginalScent.HasValue)
            {
                component.OriginalScent = update.OriginalScent.Value;
            }

            if (update.OriginalAgression.HasValue)
            {
                component.OriginalAgression = update.OriginalAgression.Value;
            }

            if (update.OriginalDominance.HasValue)
            {
                component.OriginalDominance = update.OriginalDominance.Value;
            }

            if (update.OriginPosotion.HasValue)
            {
                component.OriginPosotion = update.OriginPosotion.Value;
            }

            EntityManager.SetComponentData(Entity, component);
        }


        public void AcknowledgeAuthorityLoss()
        {
            ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, DinoAttrs.ComponentId);
        }
    }
}
