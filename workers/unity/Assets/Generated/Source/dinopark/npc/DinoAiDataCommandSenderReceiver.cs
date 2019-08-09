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

namespace Dinopark.Npc
{
    [AutoRegisterSubscriptionManager]
    public class DinoAiDataCommandSenderSubscriptionManager : SubscriptionManager<DinoAiDataCommandSender>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;

        private Dictionary<EntityId, HashSet<Subscription<DinoAiDataCommandSender>>>
            entityIdToSenderSubscriptions =
                new Dictionary<EntityId, HashSet<Subscription<DinoAiDataCommandSender>>>();

        public DinoAiDataCommandSenderSubscriptionManager(World world)
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
                        subscription.SetAvailable(new DinoAiDataCommandSender(entity, world));
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

        public override Subscription<DinoAiDataCommandSender> Subscribe(EntityId entityId)
        {
            if (entityIdToSenderSubscriptions == null)
            {
                entityIdToSenderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<DinoAiDataCommandSender>>>();
            }

            if (entityId.Id < 0)
            {
                throw new ArgumentException("EntityId can not be < 0");
            }

            var subscription = new Subscription<DinoAiDataCommandSender>(this, entityId);

            if (!entityIdToSenderSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<DinoAiDataCommandSender>>();
                entityIdToSenderSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity))
            {
                subscription.SetAvailable(new DinoAiDataCommandSender(entity, world));
            }
            else if (entityId.Id == 0)
            {
                subscription.SetAvailable(new DinoAiDataCommandSender(Entity.Null, world));
            }

            subscriptions.Add(subscription);
            return subscription;
        }

        public override void Cancel(ISubscription subscription)
        {
            var sub = ((Subscription<DinoAiDataCommandSender>) subscription);
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
            var sub = ((Subscription<DinoAiDataCommandSender>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    [AutoRegisterSubscriptionManager]
    public class DinoAiDataCommandReceiverSubscriptionManager : SubscriptionManager<DinoAiDataCommandReceiver>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private Dictionary<EntityId, HashSet<Subscription<DinoAiDataCommandReceiver>>> entityIdToReceiveSubscriptions;

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public DinoAiDataCommandReceiverSubscriptionManager(World world)
        {
            this.world = world;

            // Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();

            var constraintSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintSystem.RegisterAuthorityCallback(DinoAiData.ComponentId, authorityChange =>
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
                        subscription.SetAvailable(new DinoAiDataCommandReceiver(world, entity, authorityChange.EntityId));
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

        public override Subscription<DinoAiDataCommandReceiver> Subscribe(EntityId entityId)
        {
            if (entityIdToReceiveSubscriptions == null)
            {
                entityIdToReceiveSubscriptions = new Dictionary<EntityId, HashSet<Subscription<DinoAiDataCommandReceiver>>>();
            }

            var subscription = new Subscription<DinoAiDataCommandReceiver>(this, entityId);

            if (!entityIdToReceiveSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<DinoAiDataCommandReceiver>>();
                entityIdToReceiveSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && componentUpdateSystem.HasComponent(DinoAiData.ComponentId, entityId)
                && componentUpdateSystem.GetAuthority(entityId, DinoAiData.ComponentId) != Authority.NotAuthoritative)
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new DinoAiDataCommandReceiver(world, entity, entityId));
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
            var sub = ((Subscription<DinoAiDataCommandReceiver>) subscription);
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
            var sub = ((Subscription<DinoAiDataCommandReceiver>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    public class DinoAiDataCommandSender
    {
        public bool IsValid;

        private readonly Entity entity;
        private readonly CommandSystem commandSender;
        private readonly CommandCallbackSystem callbackSystem;

        private int callbackEpoch;

        internal DinoAiDataCommandSender(Entity entity, World world)
        {
            this.entity = entity;
            callbackSystem = world.GetOrCreateSystem<CommandCallbackSystem>();
            // todo check that this exists
            commandSender = world.GetExistingSystem<CommandSystem>();

            IsValid = true;
        }

        public void SendAttackCommand(EntityId targetEntityId, global::Dinopark.Npc.AttackRequest request, Action<global::Dinopark.Npc.DinoAiData.Attack.ReceivedResponse> callback = null)
        {
            var commandRequest = new DinoAiData.Attack.Request(targetEntityId, request);
            SendAttackCommand(commandRequest, callback);
        }

        public void SendAttackCommand(global::Dinopark.Npc.DinoAiData.Attack.Request request, Action<global::Dinopark.Npc.DinoAiData.Attack.ReceivedResponse> callback = null)
        {
            int validCallbackEpoch = callbackEpoch;
            var requestId = commandSender.SendCommand(request, entity);
            if (callback != null)
            {
                Action<global::Dinopark.Npc.DinoAiData.Attack.ReceivedResponse> wrappedCallback = response =>
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
        public void SendEatCommand(EntityId targetEntityId, global::Dinopark.Npc.EatRequest request, Action<global::Dinopark.Npc.DinoAiData.Eat.ReceivedResponse> callback = null)
        {
            var commandRequest = new DinoAiData.Eat.Request(targetEntityId, request);
            SendEatCommand(commandRequest, callback);
        }

        public void SendEatCommand(global::Dinopark.Npc.DinoAiData.Eat.Request request, Action<global::Dinopark.Npc.DinoAiData.Eat.ReceivedResponse> callback = null)
        {
            int validCallbackEpoch = callbackEpoch;
            var requestId = commandSender.SendCommand(request, entity);
            if (callback != null)
            {
                Action<global::Dinopark.Npc.DinoAiData.Eat.ReceivedResponse> wrappedCallback = response =>
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

    public class DinoAiDataCommandReceiver
    {
        public bool IsValid;

        private readonly EntityId entityId;
        private readonly CommandCallbackSystem callbackSystem;
        private readonly CommandSystem commandSystem;

        private Dictionary<Action<global::Dinopark.Npc.DinoAiData.Attack.ReceivedRequest>, ulong> attackCallbackToCallbackKey;

        public event Action<global::Dinopark.Npc.DinoAiData.Attack.ReceivedRequest> OnAttackRequestReceived
        {
            add
            {
                if (attackCallbackToCallbackKey == null)
                {
                    attackCallbackToCallbackKey = new Dictionary<Action<global::Dinopark.Npc.DinoAiData.Attack.ReceivedRequest>, ulong>();
                }

                var key = callbackSystem.RegisterCommandRequestCallback(entityId, value);
                attackCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!attackCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                callbackSystem.UnregisterCommandRequestCallback(key);
                attackCallbackToCallbackKey.Remove(value);
            }
        }
        private Dictionary<Action<global::Dinopark.Npc.DinoAiData.Eat.ReceivedRequest>, ulong> eatCallbackToCallbackKey;

        public event Action<global::Dinopark.Npc.DinoAiData.Eat.ReceivedRequest> OnEatRequestReceived
        {
            add
            {
                if (eatCallbackToCallbackKey == null)
                {
                    eatCallbackToCallbackKey = new Dictionary<Action<global::Dinopark.Npc.DinoAiData.Eat.ReceivedRequest>, ulong>();
                }

                var key = callbackSystem.RegisterCommandRequestCallback(entityId, value);
                eatCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!eatCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                callbackSystem.UnregisterCommandRequestCallback(key);
                eatCallbackToCallbackKey.Remove(value);
            }
        }

        internal DinoAiDataCommandReceiver(World world, Entity entity, EntityId entityId)
        {
            this.entityId = entityId;
            callbackSystem = world.GetOrCreateSystem<CommandCallbackSystem>();
            commandSystem = world.GetExistingSystem<CommandSystem>();
            // should check the system actually exists

            IsValid = true;
        }

        public void SendAttackResponse(global::Dinopark.Npc.DinoAiData.Attack.Response response)
        {
            commandSystem.SendResponse(response);
        }

        public void SendAttackResponse(long requestId, global::Dinopark.Npc.AttackResponse response)
        {
            commandSystem.SendResponse(new global::Dinopark.Npc.DinoAiData.Attack.Response(requestId, response));
        }

        public void SendAttackFailure(long requestId, string failureMessage)
        {
            commandSystem.SendResponse(new global::Dinopark.Npc.DinoAiData.Attack.Response(requestId, failureMessage));
        }

        public void SendEatResponse(global::Dinopark.Npc.DinoAiData.Eat.Response response)
        {
            commandSystem.SendResponse(response);
        }

        public void SendEatResponse(long requestId, global::Dinopark.Npc.EatResponse response)
        {
            commandSystem.SendResponse(new global::Dinopark.Npc.DinoAiData.Eat.Response(requestId, response));
        }

        public void SendEatFailure(long requestId, string failureMessage)
        {
            commandSystem.SendResponse(new global::Dinopark.Npc.DinoAiData.Eat.Response(requestId, failureMessage));
        }

        public void RemoveAllCallbacks()
        {
            if (attackCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in attackCallbackToCallbackKey)
                {
                    callbackSystem.UnregisterCommandRequestCallback(callbackToKey.Value);
                }

                attackCallbackToCallbackKey.Clear();
            }

            if (eatCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in eatCallbackToCallbackKey)
                {
                    callbackSystem.UnregisterCommandRequestCallback(callbackToKey.Value);
                }

                eatCallbackToCallbackKey.Clear();
            }

        }
    }
}
