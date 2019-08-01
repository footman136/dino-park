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
            var isDinoBrachio = metadata.EntityType == SimulationSettings.Dino_Brachio_PrefabName;// Dino Brachiosaurus
            var isDinoTRex = metadata.EntityType == SimulationSettings.Dino_TRex_PrefabName;// Dino T-Rex
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
                var pathPrefab = "EntityPrefabs/" + SimulationSettings.NPCPrefabName;
                var prefab = Resources.Load(pathPrefab);
                var entityGameObject = UnityEngine.Object.Instantiate(prefab);
                linker.LinkGameObjectToSpatialOSEntity(entity.SpatialOSEntityId, (GameObject)entityGameObject);
                Debug.Log("EntityGameObjectCreator OnEntityCreated - A LumberJack GameObject created");
            }
            else if (isDinoBrachio)
            {
                var pathPrefab = $"Prefabs/{_WorkerType}/" + SimulationSettings.Dino_Brachio_PrefabName;
                var prefab = Resources.Load(pathPrefab);
                var entityGameObject = UnityEngine.Object.Instantiate(prefab);
                entityGameObject.name = SimulationSettings.Dino_Brachio_PrefabName + "(EntityID:" + entity.SpatialOSEntityId + ", Worker: " + _WorkerType + ")";
                linker.LinkGameObjectToSpatialOSEntity(entity.SpatialOSEntityId, (GameObject)entityGameObject);
                Debug.Log("EntityGameObjectCreator OnEntityCreated - A Dinosauer Brachiosaurus GameObject created");
            }
            else if (isDinoTRex)
            {
                var pathPrefab = $"Prefabs/{_WorkerType}/" + SimulationSettings.Dino_TRex_PrefabName;
                var prefab = Resources.Load(pathPrefab);
                var entityGameObject = UnityEngine.Object.Instantiate(prefab);
                entityGameObject.name = SimulationSettings.Dino_TRex_PrefabName + "(EntityID:" + entity.SpatialOSEntityId + ", Worker: " + _WorkerType + ")";
                linker.LinkGameObjectToSpatialOSEntity(entity.SpatialOSEntityId, (GameObject)entityGameObject);
                Debug.Log("EntityGameObjectCreator OnEntityCreated - A Dinosauer T-Rex GameObject created");
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