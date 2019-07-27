using System;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;
using UnityEngine;

namespace DinoPark
{
    public class PlayerGameObjectCreator : IEntityGameObjectCreator
    {
        private readonly IEntityGameObjectCreator _fallbackCreator;
        private readonly World _world;
        private string _WorkerType;

        public PlayerGameObjectCreator(IEntityGameObjectCreator fallbackCreator, World world, String workerType)
        {
            _fallbackCreator = fallbackCreator;
            _world = world;
            _WorkerType = workerType;
        }
        
        public void OnEntityCreated(SpatialOSEntity entity, EntityGameObjectLinker linker)
        {
            if (!entity.HasComponent<Metadata.Component>()) return;

            var metadata = entity.GetComponent<Metadata.Component>();
            var isPlayer = metadata.EntityType == "Player";
            var hasAuthority = PlayerLifecycleHelper.IsOwningWorker(entity.SpatialOSEntityId, _world);
            if (isPlayer && hasAuthority)
            {
                var pathPrefab = $"Prefabs/{_WorkerType}/Authoritative/Player";
                var prefab = Resources.Load(pathPrefab);
                var playerGameObject = UnityEngine.Object.Instantiate(prefab);
                linker.LinkGameObjectToSpatialOSEntity(entity.SpatialOSEntityId, (GameObject)playerGameObject);
            }
            else
            {
                _fallbackCreator.OnEntityCreated(entity, linker);
            }
        }

        public void OnEntityRemoved(EntityId entityId)
        {
            _fallbackCreator.OnEntityRemoved(entityId);
        }
    }
}