using Dinopark.Core;
using Dinopark.Fire;
using Dinopark.Life;
using Dinopark.Plants;
using DinoPark;
using Improbable;
using Improbable.Gdk.Core;
using UnityEngine;
using Improbable.Gdk.PlayerLifecycle;

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

namespace Assets.Gamelogic.Core
{
	public static class EntityTemplateFactory
    {
	    public static EntityTemplate CreateTreeTemplate(Vector3f position, uint initialRotation)
	    {
		    var serverAttribute = UnityGameLogicConnector.WorkerType;
		    
		    var entityTemplate = new EntityTemplate();
		    entityTemplate.AddComponent(new Position.Snapshot(new Coordinates(position.X, position.Y, position.Z)), serverAttribute);
		    entityTemplate.AddComponent(new Metadata.Snapshot(SimulationSettings.TreePrefabName), serverAttribute);
		    entityTemplate.AddComponent(new Persistence.Snapshot(), serverAttribute);
		    entityTemplate.AddComponent(new TransformComponent.Snapshot(initialRotation), serverAttribute);
		    entityTemplate.AddComponent(new Harvestable.Snapshot(), serverAttribute);
		    entityTemplate.AddComponent(new Health.Snapshot(SimulationSettings.TreeMaxHealth, SimulationSettings.TreeMaxHealth, true), serverAttribute);
		    entityTemplate.AddComponent(new Flammable.Snapshot(false, true, FireEffectType.BIG), serverAttribute);
		    var treeStateComponent = new Dinopark.Plants.TreeState.Snapshot((TreeType) Random.Range(0, 2), TreeFSMState.HEALTHY);
		    entityTemplate.AddComponent(treeStateComponent, serverAttribute);
		    
		    entityTemplate.SetReadAccess(UnityClientConnector.WorkerType, UnityGameLogicConnector.WorkerType, MobileClientWorkerConnector.WorkerType);
		    entityTemplate.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);
		    
		    return entityTemplate;
	    }

	    public static EntityTemplate CreatePlayerTemplate(Snapshot snapshot)
	    {
		    var serverAttribute = UnityGameLogicConnector.WorkerType;

		    var template = new EntityTemplate();
		    template.AddComponent(new Position.Snapshot(), serverAttribute);
		    template.AddComponent(new Metadata.Snapshot { EntityType = "PlayerCreator" }, serverAttribute);
		    template.AddComponent(new Persistence.Snapshot(), serverAttribute);
		    template.AddComponent(new PlayerCreator.Snapshot(), serverAttribute);

		    template.SetReadAccess(UnityClientConnector.WorkerType, UnityGameLogicConnector.WorkerType, MobileClientWorkerConnector.WorkerType);
		    template.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);

		    return template;
	    }
	}
}