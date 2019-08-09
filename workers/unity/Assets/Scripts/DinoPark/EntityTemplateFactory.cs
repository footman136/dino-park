using Assets.Gamelogic.Core;
using Assets.Gamelogic.Utils;
using Com.Infalliblecode;
using Dinopark.Core;
using Dinopark.Fire;
using Dinopark.Life;
using Dinopark.Npc;
using Dinopark.Plants;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using UnityEngine;

// Random
// Coordinates
// WorkerUtils
// SimulationSettings
// treeState, Harvestable
// Health
// TransformComponent
// Flammable
// EntityTemplate
// PlayerCreator

namespace DinoPark
{
	public static class EntityTemplateFactory
    {
	    public static EntityTemplate CreatePlayerTemplate(Snapshot snapshot)
	    {
		    var serverAttribute = UnityGameLogicConnector.WorkerType;

		    var template = new EntityTemplate();
		    template.AddComponent(new Position.Snapshot(), serverAttribute);
		    template.AddComponent(new Metadata.Snapshot { EntityType = "PlayerCreator" }, serverAttribute);
		    template.AddComponent(new Persistence.Snapshot(), serverAttribute);
		    template.AddComponent(new PlayerCreator.Snapshot(), serverAttribute);
		    template.AddComponent(new TransformComponent.Snapshot(0), serverAttribute);

		    template.SetReadAccess(UnityClientConnector.WorkerType, UnityGameLogicConnector.WorkerType, MobileClientWorkerConnector.WorkerType);
		    template.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);

		    return template;
	    }
	    
	    public static EntityTemplate CreateTreeTemplate(Vector3f position, uint initialRotation)
	    {
		    var serverAttribute = UnityGameLogicConnector.WorkerType;
		    
		    var template = new EntityTemplate();
		    template.AddComponent(new Position.Snapshot(new Coordinates(position.X, position.Y, position.Z)), serverAttribute);
		    template.AddComponent(new Metadata.Snapshot(SimulationSettings.TreePrefabName), serverAttribute);
		    template.AddComponent(new Persistence.Snapshot(), serverAttribute);
		    template.AddComponent(new TransformComponent.Snapshot(initialRotation), serverAttribute);
		    
		    template.AddComponent(new Harvestable.Snapshot(), serverAttribute);
		    template.AddComponent(new Health.Snapshot(SimulationSettings.TreeMaxHealth, SimulationSettings.TreeMaxHealth, true), serverAttribute);
		    template.AddComponent(new Flammable.Snapshot(false, true, FireEffectType.BIG), serverAttribute);
		    var treeStateComponent = new Dinopark.Plants.TreeState.Snapshot((TreeType) Random.Range(0, 2), TreeFSMState.HEALTHY);
		    template.AddComponent(treeStateComponent, serverAttribute);
		    
		    template.SetReadAccess(UnityClientConnector.WorkerType, UnityGameLogicConnector.WorkerType, MobileClientWorkerConnector.WorkerType);
		    template.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);
		    
		    return template;
	    }

	    public static EntityTemplate CreateDinoBrachioTemplate(Coordinates initialPosition, uint teamId)
	    {
		    var serverAttribute = UnityGameLogicConnector.WorkerType;
		    var template = new EntityTemplate();
		    template.AddComponent(new Position.Snapshot(initialPosition), serverAttribute);
		    template.AddComponent(new Metadata.Snapshot(SimulationSettings.Dino_Brachio_PrefabName), serverAttribute);
		    template.AddComponent(new Persistence.Snapshot(), serverAttribute);
		    var spawnRotation = (uint)Mathf.CeilToInt(Random.Range(0, 360));
		    Debug.Log("DinoBrachio rotation : "+spawnRotation);
		    template.AddComponent(new TransformComponent.Snapshot(spawnRotation), serverAttribute);

		    template.AddComponent(new DinoAiData.Snapshot(), serverAttribute);
		    template.AddComponent(new DinoAttrs.Snapshot(), serverAttribute);
		    
		    template.AddComponent(new Health.Snapshot(), serverAttribute);
//		    template.AddComponent(new Flammable.Snapshot(false, true, FireEffectType.SMALL), serverAttribute);
//		    template.AddComponent(new Inventory.Snapshot(0), serverAttribute);
//		    template.AddComponent(new DinoBrachio.Snapshot(), serverAttribute);
		    
		    template.SetReadAccess(UnityClientConnector.WorkerType, UnityGameLogicConnector.WorkerType, MobileClientWorkerConnector.WorkerType);
		    template.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);
		    
		    return template;
	    }

	    public static EntityTemplate CreateDinoTRexTemplate(Coordinates initialPosition, uint teamId)
	    {
		    var serverAttribute = UnityGameLogicConnector.WorkerType;
		    var template = new EntityTemplate();
		    template.AddComponent(new Position.Snapshot(initialPosition), serverAttribute);
		    template.AddComponent(new Metadata.Snapshot(SimulationSettings.Dino_TRex_PrefabName), serverAttribute);
		    template.AddComponent(new Persistence.Snapshot(), serverAttribute);
		    var spawnRotation = (uint)Mathf.CeilToInt(Random.Range(0, 360));
		    Debug.Log("DinoTRex rotation : "+spawnRotation);
		    template.AddComponent(new TransformComponent.Snapshot(spawnRotation), serverAttribute);
		    
		    template.AddComponent(new DinoAiData.Snapshot(), serverAttribute);
		    template.AddComponent(new DinoAttrs.Snapshot(), serverAttribute);
		    
		    template.AddComponent(new Health.Snapshot(), serverAttribute);
//		    template.AddComponent(new Flammable.Snapshot(false, true, FireEffectType.SMALL), serverAttribute);
//		    template.AddComponent(new Inventory.Snapshot(0), serverAttribute);
//		    template.AddComponent(new DinoBrachio.Snapshot(), serverAttribute);
		    
		    template.SetReadAccess(UnityClientConnector.WorkerType, UnityGameLogicConnector.WorkerType, MobileClientWorkerConnector.WorkerType);
		    template.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);
		    
		    return template;
	    }

	    public static EntityTemplate CreateNPCLumberjackTemplate(Coordinates initialPosition, uint teamId)
	    {
		    var serverAttribute = UnityGameLogicConnector.WorkerType;
		    var template = new EntityTemplate();
		    template.AddComponent(new Position.Snapshot(initialPosition), serverAttribute);
		    template.AddComponent(new Metadata.Snapshot(SimulationSettings.NPCPrefabName), serverAttribute);
		    template.AddComponent(new Persistence.Snapshot(), serverAttribute);
		    template.AddComponent(new TransformComponent.Snapshot(), serverAttribute);
		    template.AddComponent(new Health.Snapshot(SimulationSettings.LumberjackMaxHealth, SimulationSettings.LumberjackMaxHealth, true), serverAttribute);
		    template.AddComponent(new Flammable.Snapshot(false, true, FireEffectType.SMALL), serverAttribute);
		    template.AddComponent(new TargetNavigation.Snapshot(NavigationState.INACTIVE, Vector3f.Zero, new EntityId(), 0f), serverAttribute);
		    template.AddComponent(new Inventory.Snapshot(0), serverAttribute);
		    template.AddComponent(new NPCLumberjack.Snapshot(LumberjackFSMState.StateEnum.IDLE, new EntityId(), SimulationSettings.InvalidPosition.ToVector3f()), serverAttribute);
		    //template.AddComponent(new TeamAssignment.Snapshot(teamId), serverAttribute);

		    template.SetReadAccess(UnityClientConnector.WorkerType, UnityGameLogicConnector.WorkerType, MobileClientWorkerConnector.WorkerType);
		    template.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);
		    
		    return template;
	    }
    }
}