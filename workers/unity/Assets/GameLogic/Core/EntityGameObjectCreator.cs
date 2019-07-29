using System;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;
using UnityEngine;

namespace Assets.Gamelogic.Core
{
    public class EntityGameObjectCreator : IEntityGameObjectCreator
    {
        private readonly IEntityGameObjectCreator _fallbackCreator;
        private readonly World _world;
        private string _WorkerType;

        public EntityGameObjectCreator(IEntityGameObjectCreator fallbackCreator, World world, String workerType)
        {
            _fallbackCreator = fallbackCreator;
            _world = world;
            _WorkerType = workerType;
        }
        
        public void OnEntityCreated(SpatialOSEntity entity, EntityGameObjectLinker linker)
        {
            if (!entity.HasComponent<Metadata.Component>()) return;

            var metadata = entity.GetComponent<Metadata.Component>();
            var isPlayer = metadata.EntityType == "Player";// 玩家
            var isTree = metadata.EntityType == SimulationSettings.TreePrefabName;// 树
            var hasAuthority = PlayerLifecycleHelper.IsOwningWorker(entity.SpatialOSEntityId, _world);
            if (isPlayer && hasAuthority)
            {
                var pathPrefab = $"Prefabs/{_WorkerType}/Authoritative/Player";
                var prefab = Resources.Load(pathPrefab);
                var playerGameObject = UnityEngine.Object.Instantiate(prefab);
                linker.LinkGameObjectToSpatialOSEntity(entity.SpatialOSEntityId, (GameObject)playerGameObject);
                Debug.Log("EntityGameObjectCreator OnEntityCreated - A Player GameObject created!");
            }
            else if (isTree)
            {
                var pathPrefab = "EntityPrefabs/" + SimulationSettings.TreePrefabName;
                var prefab = Resources.Load(pathPrefab);
                var entityGameObject = UnityEngine.Object.Instantiate(prefab);
                linker.LinkGameObjectToSpatialOSEntity(entity.SpatialOSEntityId, (GameObject)entityGameObject);
                Debug.Log("PlayerGameObjectCreator OnEntityCreated - A tree GameObject created");
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