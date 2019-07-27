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

namespace Improbable.Gdk.TransformSynchronization
{
    [AutoRegisterSubscriptionManager]
    public class TransformInternalReaderSubscriptionManager : SubscriptionManager<TransformInternalReader>
    {
        private readonly EntityManager entityManager;
        private readonly World world;
        private readonly WorkerSystem workerSystem;

        private Dictionary<EntityId, HashSet<Subscription<TransformInternalReader>>> entityIdToReaderSubscriptions;

        private Dictionary<EntityId, (ulong Added, ulong Removed)> entityIdToCallbackKey =
            new Dictionary<EntityId, (ulong Added, ulong Removed)>();

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public TransformInternalReaderSubscriptionManager(World world)
        {
            this.world = world;
            entityManager = world.EntityManager;

            // todo Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();

            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterComponentAddedCallback(TransformInternal.ComponentId, entityId =>
            {
                if (!entitiesNotMatchingRequirements.Contains(entityId))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out var entity);

                foreach (var subscription in entityIdToReaderSubscriptions[entityId])
                {
                    subscription.SetAvailable(new TransformInternalReader(world, entity, entityId));
                }

                entitiesMatchingRequirements.Add(entityId);
                entitiesNotMatchingRequirements.Remove(entityId);
            });

            constraintCallbackSystem.RegisterComponentRemovedCallback(TransformInternal.ComponentId, entityId =>
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

        public override Subscription<TransformInternalReader> Subscribe(EntityId entityId)
        {
            if (entityIdToReaderSubscriptions == null)
            {
                entityIdToReaderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<TransformInternalReader>>>();
            }

            var subscription = new Subscription<TransformInternalReader>(this, entityId);

            if (!entityIdToReaderSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<TransformInternalReader>>();
                entityIdToReaderSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && entityManager.HasComponent<TransformInternal.Component>(entity))
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new TransformInternalReader(world, entity, entityId));
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
            var sub = ((Subscription<TransformInternalReader>) subscription);
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
            var sub = ((Subscription<TransformInternalReader>) subscription);
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
    public class TransformInternalWriterSubscriptionManager : SubscriptionManager<TransformInternalWriter>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private Dictionary<EntityId, HashSet<Subscription<TransformInternalWriter>>> entityIdToWriterSubscriptions;

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public TransformInternalWriterSubscriptionManager(World world)
        {
            this.world = world;

            // todo Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();

            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterAuthorityCallback(TransformInternal.ComponentId, authorityChange =>
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
                        subscription.SetAvailable(new TransformInternalWriter(world, entity, authorityChange.EntityId));
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

        public override Subscription<TransformInternalWriter> Subscribe(EntityId entityId)
        {
            if (entityIdToWriterSubscriptions == null)
            {
                entityIdToWriterSubscriptions = new Dictionary<EntityId, HashSet<Subscription<TransformInternalWriter>>>();
            }

            var subscription = new Subscription<TransformInternalWriter>(this, entityId);

            if (!entityIdToWriterSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<TransformInternalWriter>>();
                entityIdToWriterSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && componentUpdateSystem.HasComponent(TransformInternal.ComponentId, entityId)
                && componentUpdateSystem.GetAuthority(entityId, TransformInternal.ComponentId) != Authority.NotAuthoritative)
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new TransformInternalWriter(world, entity, entityId));
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
            var sub = ((Subscription<TransformInternalWriter>) subscription);
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
            var sub = ((Subscription<TransformInternalWriter>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    public class TransformInternalReader
    {
        public bool IsValid;

        protected readonly ComponentUpdateSystem ComponentUpdateSystem;
        protected readonly ComponentCallbackSystem CallbackSystem;
        protected readonly EntityManager EntityManager;
        protected readonly Entity Entity;
        protected readonly EntityId EntityId;

        public TransformInternal.Component Data
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Oh noes!");
                }

                return EntityManager.GetComponentData<TransformInternal.Component>(Entity);
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

                return ComponentUpdateSystem.GetAuthority(EntityId, TransformInternal.ComponentId);
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

                var key = CallbackSystem.RegisterAuthorityCallback(EntityId, TransformInternal.ComponentId, value);
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

        private Dictionary<Action<TransformInternal.Update>, ulong> updateCallbackToCallbackKey;
        public event Action<TransformInternal.Update> OnUpdate
        {
            add
            {
                if (updateCallbackToCallbackKey == null)
                {
                    updateCallbackToCallbackKey = new Dictionary<Action<TransformInternal.Update>, ulong>();
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

        private Dictionary<Action<global::Improbable.Gdk.TransformSynchronization.FixedPointVector3>, ulong> locationUpdateCallbackToCallbackKey;
        public event Action<global::Improbable.Gdk.TransformSynchronization.FixedPointVector3> OnLocationUpdate
        {
            add
            {
                if (locationUpdateCallbackToCallbackKey == null)
                {
                    locationUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.Gdk.TransformSynchronization.FixedPointVector3>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<TransformInternal.Update>(EntityId, update =>
                {
                    if (update.Location.HasValue)
                    {
                        value(update.Location.Value);
                    }
                });
                locationUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!locationUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                locationUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion>, ulong> rotationUpdateCallbackToCallbackKey;
        public event Action<global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion> OnRotationUpdate
        {
            add
            {
                if (rotationUpdateCallbackToCallbackKey == null)
                {
                    rotationUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<TransformInternal.Update>(EntityId, update =>
                {
                    if (update.Rotation.HasValue)
                    {
                        value(update.Rotation.Value);
                    }
                });
                rotationUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!rotationUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                rotationUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::Improbable.Gdk.TransformSynchronization.FixedPointVector3>, ulong> velocityUpdateCallbackToCallbackKey;
        public event Action<global::Improbable.Gdk.TransformSynchronization.FixedPointVector3> OnVelocityUpdate
        {
            add
            {
                if (velocityUpdateCallbackToCallbackKey == null)
                {
                    velocityUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.Gdk.TransformSynchronization.FixedPointVector3>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<TransformInternal.Update>(EntityId, update =>
                {
                    if (update.Velocity.HasValue)
                    {
                        value(update.Velocity.Value);
                    }
                });
                velocityUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!velocityUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                velocityUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<uint>, ulong> physicsTickUpdateCallbackToCallbackKey;
        public event Action<uint> OnPhysicsTickUpdate
        {
            add
            {
                if (physicsTickUpdateCallbackToCallbackKey == null)
                {
                    physicsTickUpdateCallbackToCallbackKey = new Dictionary<Action<uint>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<TransformInternal.Update>(EntityId, update =>
                {
                    if (update.PhysicsTick.HasValue)
                    {
                        value(update.PhysicsTick.Value);
                    }
                });
                physicsTickUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!physicsTickUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                physicsTickUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<float>, ulong> ticksPerSecondUpdateCallbackToCallbackKey;
        public event Action<float> OnTicksPerSecondUpdate
        {
            add
            {
                if (ticksPerSecondUpdateCallbackToCallbackKey == null)
                {
                    ticksPerSecondUpdateCallbackToCallbackKey = new Dictionary<Action<float>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<TransformInternal.Update>(EntityId, update =>
                {
                    if (update.TicksPerSecond.HasValue)
                    {
                        value(update.TicksPerSecond.Value);
                    }
                });
                ticksPerSecondUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!ticksPerSecondUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                ticksPerSecondUpdateCallbackToCallbackKey.Remove(value);
            }
        }


        internal TransformInternalReader(World world, Entity entity, EntityId entityId)
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


            if (locationUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in locationUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                locationUpdateCallbackToCallbackKey.Clear();
            }

            if (rotationUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in rotationUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                rotationUpdateCallbackToCallbackKey.Clear();
            }

            if (velocityUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in velocityUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                velocityUpdateCallbackToCallbackKey.Clear();
            }

            if (physicsTickUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in physicsTickUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                physicsTickUpdateCallbackToCallbackKey.Clear();
            }

            if (ticksPerSecondUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in ticksPerSecondUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                ticksPerSecondUpdateCallbackToCallbackKey.Clear();
            }
        }
    }

    public class TransformInternalWriter : TransformInternalReader
    {
        internal TransformInternalWriter(World world, Entity entity, EntityId entityId)
            : base(world, entity, entityId)
        {
        }

        public void SendUpdate(TransformInternal.Update update)
        {
            var component = EntityManager.GetComponentData<TransformInternal.Component>(Entity);

            if (update.Location.HasValue)
            {
                component.Location = update.Location.Value;
            }

            if (update.Rotation.HasValue)
            {
                component.Rotation = update.Rotation.Value;
            }

            if (update.Velocity.HasValue)
            {
                component.Velocity = update.Velocity.Value;
            }

            if (update.PhysicsTick.HasValue)
            {
                component.PhysicsTick = update.PhysicsTick.Value;
            }

            if (update.TicksPerSecond.HasValue)
            {
                component.TicksPerSecond = update.TicksPerSecond.Value;
            }

            EntityManager.SetComponentData(Entity, component);
        }


        public void AcknowledgeAuthorityLoss()
        {
            ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, TransformInternal.ComponentId);
        }
    }
}
