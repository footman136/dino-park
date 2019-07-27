using UnityEngine; // Random
using Improbable; // Coordinates
using DinoPark; // WorkerUtils
using Dinopark.Plants; // treeState, Harvestable
using Dinopark.Life; // Health
using Dinopark.Core; // TransformComponent
using Dinopark.Fire; // Flammable
using Improbable.Gdk.Core; // EntityTemplate

namespace GameLogic.Core
{
	public static class EntityTemplateFactory
    {
	    public static EntityTemplate CreateTreeTemplate(Vector3f position, uint initialRotation)
	    {
		    var entityTemplate = new EntityTemplate();
		    entityTemplate.AddComponent(new Position.Snapshot(new Coordinates(position.X, position.Y, position.Z)), WorkerUtils.UnityGameLogic);
		    entityTemplate.AddComponent(new Metadata.Snapshot(SimulationSettings.TreePrefabName), WorkerUtils.UnityGameLogic);
		    entityTemplate.AddComponent(new Persistence.Snapshot(), WorkerUtils.UnityGameLogic);
		    entityTemplate.AddComponent(new TransformComponent.Snapshot(initialRotation), WorkerUtils.UnityGameLogic);
		    entityTemplate.AddComponent(new Harvestable.Snapshot(), WorkerUtils.UnityGameLogic);
		    entityTemplate.AddComponent(new Health.Snapshot(SimulationSettings.TreeMaxHealth, SimulationSettings.TreeMaxHealth, true), WorkerUtils.UnityGameLogic);
		    entityTemplate.AddComponent(new Flammable.Snapshot(false, true, FireEffectType.BIG), WorkerUtils.UnityGameLogic);
		    var treeStateComponent = new Dinopark.Plants.TreeState.Snapshot((TreeType) Random.Range(0, 2), TreeFSMState.HEALTHY);
		    entityTemplate.AddComponent(treeStateComponent, WorkerUtils.UnityGameLogic);
		    
		    entityTemplate.SetReadAccess(WorkerUtils.UnityGameLogic, WorkerUtils.UnityClient);
		    entityTemplate.SetComponentWriteAccess(EntityAcl.ComponentId, WorkerUtils.UnityGameLogic);
		    
		    return entityTemplate;
//		    var template = EntityBuilder.Begin()
//			    .AddPositionComponent(initialPosition.ToUnityVector(), CommonRequirementSets.PhysicsOnly)
//			    .AddMetadataComponent(SimulationSettings.TreePrefabName)
//			    .SetPersistence(true)
//			    .SetReadAcl(CommonRequirementSets.PhysicsOrVisual)
//			    .AddComponent(new UnityWorkerAuthorityCheck.Data(), CommonRequirementSets.PhysicsOnly)
//			    .AddComponent(new TransformComponent.Data(initialRotation), CommonRequirementSets.PhysicsOnly)
//			    .AddComponent(new Harvestable.Data(), CommonRequirementSets.PhysicsOnly)
//			    .AddComponent(new Health.Data(SimulationSettings.TreeMaxHealth, SimulationSettings.TreeMaxHealth, true), CommonRequirementSets.PhysicsOnly)
//			    .AddComponent(new Flammable.Data(false, true, FireEffectType.BIG), CommonRequirementSets.PhysicsOnly)
//			    .AddComponent(new TreeState.Data((TreeType) Random.Range(0, 2), TreeFSMState.HEALTHY), CommonRequirementSets.PhysicsOnly)
//			    .Build();
//		    return template;
	    }
	}
}