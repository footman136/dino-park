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
    public class TargetNavigationReaderSubscriptionManager : SubscriptionManager<TargetNavigationReader>
    {
        private readonly EntityManager entityManager;
        private readonly World world;
        private readonly WorkerSystem workerSystem;

        private Dictionary<EntityId, HashSet<Subscription<TargetNavigationReader>>> entityIdToReaderSubscriptions;

        private Dictionary<EntityId, (ulong Added, ulong Removed)> entityIdToCallbackKey =
            new Dictionary<EntityId, (ulong Added, ulong Removed)>();

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public TargetNavigationReaderSubscriptionManager(World world)
        {
            this.world = world;
            entityManager = world.EntityManager;

            // todo Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();

            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterComponentAddedCallback(TargetNavigation.ComponentId, entityId =>
            {
                if (!entitiesNotMatchingRequirements.Contains(entityId))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out var entity);

                foreach (var subscription in entityIdToReaderSubscriptions[entityId])
                {
                    subscription.SetAvailable(new TargetNavigationReader(world, entity, entityId));
                }

                entitiesMatchingRequirements.Add(entityId);
                entitiesNotMatchingRequirements.Remove(entityId);
            });

            constraintCallbackSystem.RegisterComponentRemovedCallback(TargetNavigation.ComponentId, entityId =>
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

        public override Subscription<TargetNavigationReader> Subscribe(EntityId entityId)
        {
            if (entityIdToReaderSubscriptions == null)
            {
                entityIdToReaderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<TargetNavigationReader>>>();
            }

            var subscription = new Subscription<TargetNavigationReader>(this, entityId);

            if (!entityIdToReaderSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<TargetNavigationReader>>();
                entityIdToReaderSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && entityManager.HasComponent<TargetNavigation.Component>(entity))
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new TargetNavigationReader(world, entity, entityId));
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
            var sub = ((Subscription<TargetNavigationReader>) subscription);
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
            var sub = ((Subscription<TargetNavigationReader>) subscription);
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
    public class TargetNavigationWriterSubscriptionManager : SubscriptionManager<TargetNavigationWriter>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private Dictionary<EntityId, HashSet<Subscription<TargetNavigationWriter>>> entityIdToWriterSubscriptions;

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public TargetNavigationWriterSubscriptionManager(World world)
        {
            this.world = world;

            // todo Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();

            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterAuthorityCallback(TargetNavigation.ComponentId, authorityChange =>
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
                        subscription.SetAvailable(new TargetNavigationWriter(world, entity, authorityChange.EntityId));
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

        public override Subscription<TargetNavigationWriter> Subscribe(EntityId entityId)
        {
            if (entityIdToWriterSubscriptions == null)
            {
                entityIdToWriterSubscriptions = new Dictionary<EntityId, HashSet<Subscription<TargetNavigationWriter>>>();
            }

            var subscription = new Subscription<TargetNavigationWriter>(this, entityId);

            if (!entityIdToWriterSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<TargetNavigationWriter>>();
                entityIdToWriterSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && componentUpdateSystem.HasComponent(TargetNavigation.ComponentId, entityId)
                && componentUpdateSystem.GetAuthority(entityId, TargetNavigation.ComponentId) != Authority.NotAuthoritative)
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new TargetNavigationWriter(world, entity, entityId));
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
            var sub = ((Subscription<TargetNavigationWriter>) subscription);
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
            var sub = ((Subscription<TargetNavigationWriter>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    public class TargetNavigationReader
    {
        public bool IsValid;

        protected readonly ComponentUpdateSystem ComponentUpdateSystem;
        protected readonly ComponentCallbackSystem CallbackSystem;
        protected readonly EntityManager EntityManager;
        protected readonly Entity Entity;
        protected readonly EntityId EntityId;

        public TargetNavigation.Component Data
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Oh noes!");
                }

                return EntityManager.GetComponentData<TargetNavigation.Component>(Entity);
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

                return ComponentUpdateSystem.GetAuthority(EntityId, TargetNavigation.ComponentId);
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

                var key = CallbackSystem.RegisterAuthorityCallback(EntityId, TargetNavigation.ComponentId, value);
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

        private Dictionary<Action<TargetNavigation.Update>, ulong> updateCallbackToCallbackKey;
        public event Action<TargetNavigation.Update> OnUpdate
        {
            add
            {
                if (updateCallbackToCallbackKey == null)
                {
                    updateCallbackToCallbackKey = new Dictionary<Action<TargetNavigation.Update>, ulong>();
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

        private Dictionary<Action<global::Dinopark.Npc.NavigationState>, ulong> navigationStateUpdateCallbackToCallbackKey;
        public event Action<global::Dinopark.Npc.NavigationState> OnNavigationStateUpdate
        {
            add
            {
                if (navigationStateUpdateCallbackToCallbackKey == null)
                {
                    navigationStateUpdateCallbackToCallbackKey = new Dictionary<Action<global::Dinopark.Npc.NavigationState>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<TargetNavigation.Update>(EntityId, update =>
                {
                    if (update.NavigationState.HasValue)
                    {
                        value(update.NavigationState.Value);
                    }
                });
                navigationStateUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!navigationStateUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                navigationStateUpdateCallbackToCallbackKey.Remove(value);
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

                var key = CallbackSystem.RegisterComponentUpdateCallback<TargetNavigation.Update>(EntityId, update =>
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

        private Dictionary<Action<global::Improbable.Gdk.Core.EntityId>, ulong> targetEntityIdUpdateCallbackToCallbackKey;
        public event Action<global::Improbable.Gdk.Core.EntityId> OnTargetEntityIdUpdate
        {
            add
            {
                if (targetEntityIdUpdateCallbackToCallbackKey == null)
                {
                    targetEntityIdUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.Gdk.Core.EntityId>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<TargetNavigation.Update>(EntityId, update =>
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

        private Dictionary<Action<float>, ulong> interactionSqrDistanceUpdateCallbackToCallbackKey;
        public event Action<float> OnInteractionSqrDistanceUpdate
        {
            add
            {
                if (interactionSqrDistanceUpdateCallbackToCallbackKey == null)
                {
                    interactionSqrDistanceUpdateCallbackToCallbackKey = new Dictionary<Action<float>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<TargetNavigation.Update>(EntityId, update =>
                {
                    if (update.InteractionSqrDistance.HasValue)
                    {
                        value(update.InteractionSqrDistance.Value);
                    }
                });
                interactionSqrDistanceUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!interactionSqrDistanceUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                interactionSqrDistanceUpdateCallbackToCallbackKey.Remove(value);
            }
        }


        private Dictionary<Action<global::Dinopark.Npc.NavigationFinished>, ulong> navigationFinishedEventCallbackToCallbackKey;
        public event Action<global::Dinopark.Npc.NavigationFinished> OnNavigationFinishedEvent
        {
            add
            {
                if (navigationFinishedEventCallbackToCallbackKey == null)
                {
                    navigationFinishedEventCallbackToCallbackKey = new Dictionary<Action<global::Dinopark.Npc.NavigationFinished>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentEventCallback<TargetNavigation.NavigationFinished.Event>(EntityId, ev => value(ev.Payload));
                navigationFinishedEventCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!navigationFinishedEventCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                navigationFinishedEventCallbackToCallbackKey.Remove(value);
            }
        }

        internal TargetNavigationReader(World world, Entity entity, EntityId entityId)
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


            if (navigationStateUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in navigationStateUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                navigationStateUpdateCallbackToCallbackKey.Clear();
            }

            if (targetPositionUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in targetPositionUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                targetPositionUpdateCallbackToCallbackKey.Clear();
            }

            if (targetEntityIdUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in targetEntityIdUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                targetEntityIdUpdateCallbackToCallbackKey.Clear();
            }

            if (interactionSqrDistanceUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in interactionSqrDistanceUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                interactionSqrDistanceUpdateCallbackToCallbackKey.Clear();
            }

            if (navigationFinishedEventCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in navigationFinishedEventCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                navigationFinishedEventCallbackToCallbackKey.Clear();
            }
        }
    }

    public class TargetNavigationWriter : TargetNavigationReader
    {
        internal TargetNavigationWriter(World world, Entity entity, EntityId entityId)
            : base(world, entity, entityId)
        {
        }

        public void SendUpdate(TargetNavigation.Update update)
        {
            var component = EntityManager.GetComponentData<TargetNavigation.Component>(Entity);

            if (update.NavigationState.HasValue)
            {
                component.NavigationState = update.NavigationState.Value;
            }

            if (update.TargetPosition.HasValue)
            {
                component.TargetPosition = update.TargetPosition.Value;
            }

            if (update.TargetEntityId.HasValue)
            {
                component.TargetEntityId = update.TargetEntityId.Value;
            }

            if (update.InteractionSqrDistance.HasValue)
            {
                component.InteractionSqrDistance = update.InteractionSqrDistance.Value;
            }

            EntityManager.SetComponentData(Entity, component);
        }

        public void SendNavigationFinishedEvent(global::Dinopark.Npc.NavigationFinished navigationFinished)
        {
            var eventToSend = new TargetNavigation.NavigationFinished.Event(navigationFinished);
            ComponentUpdateSystem.SendEvent(eventToSend, EntityId);
        }

        public void AcknowledgeAuthorityLoss()
        {
            ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, TargetNavigation.ComponentId);
        }
    }
}
