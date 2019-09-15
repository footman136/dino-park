using Assets.Gamelogic.Fire;
using Assets.Gamelogic.Life;
using Assets.Gamelogic.Tree;
using Assets.Gamelogic.Utils;
using System;
using UnityEngine;
using Improbable.Gdk.Core;
using Dinopark.Plants;
using Improbable.Gdk.Subscriptions;
using Improbable.Unity.Entity;

namespace Assets.Gamelogic.NPC
{
    public static class NPCUtils
    {
        private static Collider[] nearbyColliders = new Collider[32];

        public static bool TargetExistsLocally(EntityId targetEntityId)
        {
            return LocalEntities.Instance.ContainsEntity(targetEntityId);
        }

        public static GameObject GetTargetGameObject(EntityId targetEntityId)
        {
            if (!TargetExistsLocally(targetEntityId))
            {
                return null;
            }
            return LocalEntities.Instance.Get(targetEntityId).UnderlyingGameObject;
        }

        public static bool IsWithinInteractionRange(Vector3 currentPosition, Vector3 targetPosition, float interactionSqrDistance)
        {
            return MathUtils.SqrDistance(currentPosition, targetPosition) <= interactionSqrDistance;
        }

        public static GameObject FindNearestTarget(GameObject referenceGameObject, float radius, Func<GameObject, GameObject, bool> conditionForSuccess, int layerMask)
        {
            var currentPosition = referenceGameObject.transform.position;
            var gameObjectCount = Physics.OverlapSphereNonAlloc(currentPosition, radius, nearbyColliders, layerMask);

            GameObject closestTarget = null;
            var minimumDistanceFound = Mathf.Infinity;

            for (var nearbyColliderIndex = 0; nearbyColliderIndex < gameObjectCount; nearbyColliderIndex++)
            {
                var targetObject = nearbyColliders[nearbyColliderIndex].gameObject;
                //if (!targetObject.IsSpatialOsEntity())
                if (targetObject.GetComponent<LinkedEntityComponent>() == null)
                {
                    continue;
                }

                var distance = (targetObject.transform.position - currentPosition).sqrMagnitude;
                if (distance < minimumDistanceFound && conditionForSuccess(referenceGameObject, targetObject))
                {
                    minimumDistanceFound = distance;
                    closestTarget = targetObject;
                }
            }
            return closestTarget;
        }

//        public static bool IsTargetAttackable(GameObject reference, GameObject target)
//        {
//            var teamAssignment = reference.GetComponent<TeamAssignmentVisualizerUnityWorker>();
//            if (teamAssignment == null)
//            {
//                Debug.LogError("Failed to find TeamAssignmentVisualizerUnityWorker in IsTargetAttackable.");
//                return false;
//            }
//            var targetTeamAssignment = target.GetComponent<TeamAssignmentVisualizerUnityWorker>();
//            var targetFlammable = target.GetComponent<FlammableBehaviour>();
//            var targetHealth = target.GetComponent<HealthVisualizer>();
//
//            return targetTeamAssignment != null &&
//                   teamAssignment.TeamId != targetTeamAssignment.TeamId &&
//                   targetFlammable != null &&
//                   !targetFlammable.IsOnFire &&
//                   targetHealth != null && 
//                   targetHealth.CurrentHealth > 0;
//        }

//        public static bool IsTargetDefendable(GameObject reference, GameObject target)
//        {
//            var teamAssignment = reference.GetComponent<TeamAssignmentVisualizerUnityWorker>();
//            if (teamAssignment == null)
//            {
//                Debug.LogError("Failed to find TeamAssignmentVisualizerUnityWorker in IsTargetDefendable.");
//                return false;
//            }
//            var targetTeamAssignment = target.GetComponent<TeamAssignmentVisualizerUnityWorker>();
//            var targetFlammable = target.GetComponent<FlammableBehaviour>();
//            var targetHealth = target.GetComponent<HealthVisualizer>();
//            
//            return targetTeamAssignment != null &&
//                   teamAssignment.TeamId == targetTeamAssignment.TeamId &&
//                   targetFlammable != null && 
//                   targetFlammable.IsOnFire &&
//                   targetHealth != null && 
//                   targetHealth.CurrentHealth > 0;
//        }

        public static bool IsTargetATeamStockpile(GameObject reference, GameObject target)
        {
//            var teamAssignment = reference.GetComponent<TeamAssignmentVisualizerUnityWorker>();
//            if (teamAssignment == null)
//            {
//                Debug.LogError("Failed to find TeamAssignmentVisualizerUnityWorker in IsTargetATeamStockpile.");
//                return false;
//            }
//            var targetBarracksInfoVisualizer = target.GetComponent<BarracksInfoVisualizer>();
//            var targetTeamAssignmentVisualizer = target.GetComponent<TeamAssignmentVisualizerUnityWorker>();
//
//            return targetBarracksInfoVisualizer != null && 
//                   targetTeamAssignmentVisualizer != null && 
//                   targetBarracksInfoVisualizer.BarracksState == BarracksState.UNDER_CONSTRUCTION &&
//                   teamAssignment.TeamId == targetTeamAssignmentVisualizer.TeamId;
            return false;
        }

        public static bool IsTargetAHealthyTree(GameObject reference, GameObject target)
        {
            var targetTreeModelVisualizer = target.GetComponent<TreeModelVisualizer>();
            var targetHealthVisualizer = target.GetComponent<HealthVisualizer>();
            return targetTreeModelVisualizer != null && 
                   targetHealthVisualizer != null &&
                   targetTreeModelVisualizer.CurrentState != null &&
                   targetTreeModelVisualizer.CurrentState.Data.CurrentState == TreeFSMState.HEALTHY &&
                   targetHealthVisualizer.CurrentHealth > 0;
        }
    }
}
