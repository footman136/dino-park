// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Collections;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Worker.CInterop;
using Entity = Unity.Entities.Entity;

namespace Dinopark.Fire
{
    [AutoRegisterSubscriptionManager]
    public class FlammableCommandSenderSubscriptionManager : SubscriptionManager<FlammableCommandSender>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;

        private Dictionary<EntityId, HashSet<Subscription<FlammableCommandSender>>>
            entityIdToSenderSubscriptions =
                new Dictionary<EntityId, HashSet<Subscription<FlammableCommandSender>>>();

        public FlammableCommandSenderSubscriptionManager(World world)
        {
            this.world = world;

            // Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();

            var constraintSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintSystem.RegisterEntityAddedCallback(entityId =>
            {
                if (!entityIdToSenderSubscriptions.TryGetValue(entityId, out var subscriptions))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out var entity);
                foreach (var subscription in subscriptions)
                {
                    if (!subscription.HasValue)
                    {
                        subscription.SetAvailable(new FlammableCommandSender(entity, world));
                    }
                }
            });

            constraintSystem.RegisterEntityRemovedCallback(entityId =>
            {
                if (!entityIdToSenderSubscriptions.TryGetValue(entityId, out var subscriptions))
                {
                    return;
                }

                foreach (var subscription in subscriptions)
                {
                    if (subscription.HasValue)
                    {
                        ResetValue(subscription);
                        subscription.SetUnavailable();
                    }
                }
            });
        }

        public override Subscription<FlammableCommandSender> Subscribe(EntityId entityId)
        {
            if (entityIdToSenderSubscriptions == null)
            {
                entityIdToSenderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<FlammableCommandSender>>>();
            }

            if (entityId.Id < 0)
            {
                throw new ArgumentException("EntityId can not be < 0");
            }

            var subscription = new Subscription<FlammableCommandSender>(this, entityId);

            if (!entityIdToSenderSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<FlammableCommandSender>>();
                entityIdToSenderSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity))
            {
                subscription.SetAvailable(new FlammableCommandSender(entity, world));
            }
            else if (entityId.Id == 0)
            {
                subscription.SetAvailable(new FlammableCommandSender(Entity.Null, world));
            }

            subscriptions.Add(subscription);
            return subscription;
        }

        public override void Cancel(ISubscription subscription)
        {
            var sub = ((Subscription<FlammableCommandSender>) subscription);
            if (sub.HasValue)
            {
                var sender = sub.Value;
                sender.IsValid = false;
            }

            var subscriptions = entityIdToSenderSubscriptions[sub.EntityId];
            subscriptions.Remove(sub);
            if (subscriptions.Count == 0)
            {
                entityIdToSenderSubscriptions.Remove(sub.EntityId);
            }
        }

        public override void ResetValue(ISubscription subscription)
        {
            var sub = ((Subscription<FlammableCommandSender>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    [AutoRegisterSubscriptionManager]
    public class FlammableCommandReceiverSubscriptionManager : SubscriptionManager<FlammableCommandReceiver>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private Dictionary<EntityId, HashSet<Subscription<FlammableCommandReceiver>>> entityIdToReceiveSubscriptions;

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public FlammableCommandReceiverSubscriptionManager(World world)
        {
            this.world = world;

            // Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();

            var constraintSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintSystem.RegisterAuthorityCallback(Flammable.ComponentId, authorityChange =>
            {
                if (authorityChange.Authority == Authority.Authoritative)
                {
                    if (!entitiesNotMatchingRequirements.Contains(authorityChange.EntityId))
                    {
                        return;
                    }

                    workerSystem.TryGetEntity(authorityChange.EntityId, out var entity);

                    foreach (var subscription in entityIdToReceiveSubscriptions[authorityChange.EntityId])
                    {
                        subscription.SetAvailable(new FlammableCommandReceiver(world, entity, authorityChange.EntityId));
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

                    foreach (var subscription in entityIdToReceiveSubscriptions[authorityChange.EntityId])
                    {
                        ResetValue(subscription);
                        subscription.SetUnavailable();
                    }

                    entitiesNotMatchingRequirements.Add(authorityChange.EntityId);
                    entitiesMatchingRequirements.Remove(authorityChange.EntityId);
                }
            });
        }

        public override Subscription<FlammableCommandReceiver> Subscribe(EntityId entityId)
        {
            if (entityIdToReceiveSubscriptions == null)
            {
                entityIdToReceiveSubscriptions = new Dictionary<EntityId, HashSet<Subscription<FlammableCommandReceiver>>>();
            }

            var subscription = new Subscription<FlammableCommandReceiver>(this, entityId);

            if (!entityIdToReceiveSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<FlammableCommandReceiver>>();
                entityIdToReceiveSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && componentUpdateSystem.HasComponent(Flammable.ComponentId, entityId)
                && componentUpdateSystem.GetAuthority(entityId, Flammable.ComponentId) != Authority.NotAuthoritative)
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new FlammableCommandReceiver(world, entity, entityId));
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
            var sub = ((Subscription<FlammableCommandReceiver>) subscription);
            if (sub.HasValue)
            {
                var receiver = sub.Value;
                receiver.IsValid = false;
                receiver.RemoveAllCallbacks();
            }

            var subscriptions = entityIdToReceiveSubscriptions[sub.EntityId];
            subscriptions.Remove(sub);
            if (subscriptions.Count == 0)
            {
                entityIdToReceiveSubscriptions.Remove(sub.EntityId);
                entitiesMatchingRequirements.Remove(sub.EntityId);
                entitiesNotMatchingRequirements.Remove(sub.EntityId);
            }
        }

        public override void ResetValue(ISubscription subscription)
        {
            var sub = ((Subscription<FlammableCommandReceiver>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    public class FlammableCommandSender
    {
        public bool IsValid;

        private readonly Entity entity;
        private readonly CommandSystem commandSender;
        private readonly CommandCallbackSystem callbackSystem;

        private int callbackEpoch;

        internal FlammableCommandSender(Entity entity, World world)
        {
            this.entity = entity;
            callbackSystem = world.GetOrCreateSystem<CommandCallbackSystem>();
            // todo check that this exists
            commandSender = world.GetExistingSystem<CommandSystem>();

            IsValid = true;
        }

        public void SendIgniteCommand(EntityId targetEntityId, global::Dinopark.Core.Nothing request, Action<global::Dinopark.Fire.Flammable.Ignite.ReceivedResponse> callback = null)
        {
            var commandRequest = new Flammable.Ignite.Request(targetEntityId, request);
            SendIgniteCommand(commandRequest, callback);
        }

        public void SendIgniteCommand(global::Dinopark.Fire.Flammable.Ignite.Request request, Action<global::Dinopark.Fire.Flammable.Ignite.ReceivedResponse> callback = null)
        {
            int validCallbackEpoch = callbackEpoch;
            var requestId = commandSender.SendCommand(request, entity);
            if (callback != null)
            {
                Action<global::Dinopark.Fire.Flammable.Ignite.ReceivedResponse> wrappedCallback = response =>
                {
                    if (!this.IsValid || validCallbackEpoch != this.callbackEpoch)
                    {
                        return;
                    }

                    callback(response);
                };
                callbackSystem.RegisterCommandResponseCallback(requestId, wrappedCallback);
            }
        }
        public void SendExtinguishCommand(EntityId targetEntityId, global::Dinopark.Fire.ExtinguishRequest request, Action<global::Dinopark.Fire.Flammable.Extinguish.ReceivedResponse> callback = null)
        {
            var commandRequest = new Flammable.Extinguish.Request(targetEntityId, request);
            SendExtinguishCommand(commandRequest, callback);
        }

        public void SendExtinguishCommand(global::Dinopark.Fire.Flammable.Extinguish.Request request, Action<global::Dinopark.Fire.Flammable.Extinguish.ReceivedResponse> callback = null)
        {
            int validCallbackEpoch = callbackEpoch;
            var requestId = commandSender.SendCommand(request, entity);
            if (callback != null)
            {
                Action<global::Dinopark.Fire.Flammable.Extinguish.ReceivedResponse> wrappedCallback = response =>
                {
                    if (!this.IsValid || validCallbackEpoch != this.callbackEpoch)
                    {
                        return;
                    }

                    callback(response);
                };
                callbackSystem.RegisterCommandResponseCallback(requestId, wrappedCallback);
            }
        }
        public void SendSetCanBeIgnitedCommand(EntityId targetEntityId, global::Dinopark.Fire.SetCanBeIgnitedRequest request, Action<global::Dinopark.Fire.Flammable.SetCanBeIgnited.ReceivedResponse> callback = null)
        {
            var commandRequest = new Flammable.SetCanBeIgnited.Request(targetEntityId, request);
            SendSetCanBeIgnitedCommand(commandRequest, callback);
        }

        public void SendSetCanBeIgnitedCommand(global::Dinopark.Fire.Flammable.SetCanBeIgnited.Request request, Action<global::Dinopark.Fire.Flammable.SetCanBeIgnited.ReceivedResponse> callback = null)
        {
            int validCallbackEpoch = callbackEpoch;
            var requestId = commandSender.SendCommand(request, entity);
            if (callback != null)
            {
                Action<global::Dinopark.Fire.Flammable.SetCanBeIgnited.ReceivedResponse> wrappedCallback = response =>
                {
                    if (!this.IsValid || validCallbackEpoch != this.callbackEpoch)
                    {
                        return;
                    }

                    callback(response);
                };
                callbackSystem.RegisterCommandResponseCallback(requestId, wrappedCallback);
            }
        }

        public void RemoveAllCallbacks()
        {
            ++callbackEpoch;
        }
    }

    public class FlammableCommandReceiver
    {
        public bool IsValid;

        private readonly EntityId entityId;
        private readonly CommandCallbackSystem callbackSystem;
        private readonly CommandSystem commandSystem;

        private Dictionary<Action<global::Dinopark.Fire.Flammable.Ignite.ReceivedRequest>, ulong> igniteCallbackToCallbackKey;

        public event Action<global::Dinopark.Fire.Flammable.Ignite.ReceivedRequest> OnIgniteRequestReceived
        {
            add
            {
                if (igniteCallbackToCallbackKey == null)
                {
                    igniteCallbackToCallbackKey = new Dictionary<Action<global::Dinopark.Fire.Flammable.Ignite.ReceivedRequest>, ulong>();
                }

                var key = callbackSystem.RegisterCommandRequestCallback(entityId, value);
                igniteCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!igniteCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                callbackSystem.UnregisterCommandRequestCallback(key);
                igniteCallbackToCallbackKey.Remove(value);
            }
        }
        private Dictionary<Action<global::Dinopark.Fire.Flammable.Extinguish.ReceivedRequest>, ulong> extinguishCallbackToCallbackKey;

        public event Action<global::Dinopark.Fire.Flammable.Extinguish.ReceivedRequest> OnExtinguishRequestReceived
        {
            add
            {
                if (extinguishCallbackToCallbackKey == null)
                {
                    extinguishCallbackToCallbackKey = new Dictionary<Action<global::Dinopark.Fire.Flammable.Extinguish.ReceivedRequest>, ulong>();
                }

                var key = callbackSystem.RegisterCommandRequestCallback(entityId, value);
                extinguishCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!extinguishCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                callbackSystem.UnregisterCommandRequestCallback(key);
                extinguishCallbackToCallbackKey.Remove(value);
            }
        }
        private Dictionary<Action<global::Dinopark.Fire.Flammable.SetCanBeIgnited.ReceivedRequest>, ulong> setCanBeIgnitedCallbackToCallbackKey;

        public event Action<global::Dinopark.Fire.Flammable.SetCanBeIgnited.ReceivedRequest> OnSetCanBeIgnitedRequestReceived
        {
            add
            {
                if (setCanBeIgnitedCallbackToCallbackKey == null)
                {
                    setCanBeIgnitedCallbackToCallbackKey = new Dictionary<Action<global::Dinopark.Fire.Flammable.SetCanBeIgnited.ReceivedRequest>, ulong>();
                }

                var key = callbackSystem.RegisterCommandRequestCallback(entityId, value);
                setCanBeIgnitedCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!setCanBeIgnitedCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                callbackSystem.UnregisterCommandRequestCallback(key);
                setCanBeIgnitedCallbackToCallbackKey.Remove(value);
            }
        }

        internal FlammableCommandReceiver(World world, Entity entity, EntityId entityId)
        {
            this.entityId = entityId;
            callbackSystem = world.GetOrCreateSystem<CommandCallbackSystem>();
            commandSystem = world.GetExistingSystem<CommandSystem>();
            // should check the system actually exists

            IsValid = true;
        }

        public void SendIgniteResponse(global::Dinopark.Fire.Flammable.Ignite.Response response)
        {
            commandSystem.SendResponse(response);
        }

        public void SendIgniteResponse(long requestId, global::Dinopark.Core.Nothing response)
        {
            commandSystem.SendResponse(new global::Dinopark.Fire.Flammable.Ignite.Response(requestId, response));
        }

        public void SendIgniteFailure(long requestId, string failureMessage)
        {
            commandSystem.SendResponse(new global::Dinopark.Fire.Flammable.Ignite.Response(requestId, failureMessage));
        }

        public void SendExtinguishResponse(global::Dinopark.Fire.Flammable.Extinguish.Response response)
        {
            commandSystem.SendResponse(response);
        }

        public void SendExtinguishResponse(long requestId, global::Dinopark.Core.Nothing response)
        {
            commandSystem.SendResponse(new global::Dinopark.Fire.Flammable.Extinguish.Response(requestId, response));
        }

        public void SendExtinguishFailure(long requestId, string failureMessage)
        {
            commandSystem.SendResponse(new global::Dinopark.Fire.Flammable.Extinguish.Response(requestId, failureMessage));
        }

        public void SendSetCanBeIgnitedResponse(global::Dinopark.Fire.Flammable.SetCanBeIgnited.Response response)
        {
            commandSystem.SendResponse(response);
        }

        public void SendSetCanBeIgnitedResponse(long requestId, global::Dinopark.Core.Nothing response)
        {
            commandSystem.SendResponse(new global::Dinopark.Fire.Flammable.SetCanBeIgnited.Response(requestId, response));
        }

        public void SendSetCanBeIgnitedFailure(long requestId, string failureMessage)
        {
            commandSystem.SendResponse(new global::Dinopark.Fire.Flammable.SetCanBeIgnited.Response(requestId, failureMessage));
        }

        public void RemoveAllCallbacks()
        {
            if (igniteCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in igniteCallbackToCallbackKey)
                {
                    callbackSystem.UnregisterCommandRequestCallback(callbackToKey.Value);
                }

                igniteCallbackToCallbackKey.Clear();
            }

            if (extinguishCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in extinguishCallbackToCallbackKey)
                {
                    callbackSystem.UnregisterCommandRequestCallback(callbackToKey.Value);
                }

                extinguishCallbackToCallbackKey.Clear();
            }

            if (setCanBeIgnitedCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in setCanBeIgnitedCallbackToCallbackKey)
                {
                    callbackSystem.UnregisterCommandRequestCallback(callbackToKey.Value);
                }

                setCanBeIgnitedCallbackToCallbackKey.Clear();
            }

        }
    }
}
