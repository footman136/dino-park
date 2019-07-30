using System;
using Assets.Gamelogic.Core;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;
using UnityEngine;

namespace DinoPark
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
            var isLumberJack = metadata.EntityType == SimulationSettings.NPCPrefabName; // 伐木工
            var isDino = metadata.EntityType == SimulationSettings.Dino_Brachio_PrefabName;// Dino Brachiosaurus
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
                //Debug.Log("EntityGameObjectCreator OnEntityCreated - A tree GameObject created");
            }
            else if (isLumberJack)
            {
                Debug.Log("Begin Create a LumberJack GameObject!");
                var pathPrefab = "EntityPrefabs/" + SimulationSettings.NPCPrefabName;
                var prefab = Resources.Load(pathPrefab);
                var entityGameObject = UnityEngine.Object.Instantiate(prefab);
                linker.LinkGameObjectToSpatialOSEntity(entity.SpatialOSEntityId, (GameObject)entityGameObject);
                Debug.Log("EntityGameObjectCreator OnEntityCreated - A LumberJack GameObject created");
            }
            else if (isDino)
            {
                Debug.Log("Begin Create a Dinosauer GameObject!");
                var pathPrefab = "EntityPrefabs/" + SimulationSettings.Dino_Brachio_PrefabName;
                var prefab = Resources.Load(pathPrefab);
                var entityGameObject = UnityEngine.Object.Instantiate(prefab);
                linker.LinkGameObjectToSpatialOSEntity(entity.SpatialOSEntityId, (GameObject)entityGameObject);
                Debug.Log("EntityGameObjectCreator OnEntityCreated - A Dinosauer Brachiosaurus GameObject created");
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