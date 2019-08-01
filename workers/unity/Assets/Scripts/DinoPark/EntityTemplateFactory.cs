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
		    template.AddComponent(new PlayerTransform.Snapshot(), serverAttribute);

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
		    template.AddComponent(new PlayerTransform.Snapshot(), serverAttribute);
		    template.AddComponent(new Health.Snapshot(SimulationSettings.LumberjackMaxHealth, SimulationSettings.LumberjackMaxHealth, true), serverAttribute);
		    template.AddComponent(new Flammable.Snapshot(false, true, FireEffectType.SMALL), serverAttribute);
		    template.AddComponent(new Inventory.Snapshot(0), serverAttribute);
		    template.AddComponent(new DinoBrachio.Snapshot(0), serverAttribute);
		    
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
		    template.AddComponent(new PlayerTransform.Snapshot(), serverAttribute);
		    template.AddComponent(new Health.Snapshot(SimulationSettings.LumberjackMaxHealth, SimulationSettings.LumberjackMaxHealth, true), serverAttribute);
		    template.AddComponent(new Flammable.Snapshot(false, true, FireEffectType.SMALL), serverAttribute);
		    //template.AddComponent(new TargetNavigation.Snapshot(NavigationState.INACTIVE, Vector3f.Zero, new EntityId(), 0f), serverAttribute);
		    template.AddComponent(new Inventory.Snapshot(0), serverAttribute);
		    template.AddComponent(new DinoBrachio.Snapshot(0), serverAttribute);
		    
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
	    
//		    var template = EntityBuilder.Begin()
//			    .AddPositionComponent(initialPosition.ToUnityVector(), CommonRequirementSets.PhysicsOnly)
//			    .AddMetadataComponent(SimulationSettings.NPCPrefabName)
//			    .SetPersistence(true)
//			    .SetReadAcl(CommonRequirementSets.PhysicsOrVisual)
//			    .AddComponent(new TransformComponent.Data(0), CommonRequirementSets.PhysicsOnly)
//			    .AddComponent(new UnityWorkerAuthorityCheck.Data(), CommonRequirementSets.PhysicsOnly)
//			    .AddComponent(new Health.Data(SimulationSettings.LumberjackMaxHealth, SimulationSettings.LumberjackMaxHealth, true), CommonRequirementSets.PhysicsOnly)
//			    .AddComponent(new Flammable.Data(false, true, FireEffectType.SMALL), CommonRequirementSets.PhysicsOnly)
//			    .AddComponent(new TargetNavigation.Data(NavigationState.INACTIVE, Vector3f.ZERO, new EntityId(), 0f), CommonRequirementSets.PhysicsOnly)
//			    .AddComponent(new Inventory.Data(0), CommonRequirementSets.PhysicsOnly)
//			    .AddComponent(new NPCLumberjack.Data(LumberjackFSMState.StateEnum.IDLE, new EntityId(), SimulationSettings.InvalidPosition.ToVector3f()), CommonRequirementSets.PhysicsOnly)
//			    .AddComponent(new TeamAssignment.Data(teamId), CommonRequirementSets.PhysicsOnly)
//			    .Build();
//
//		    return template;
    }
}