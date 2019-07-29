using Assets.Gamelogic.Core;
using Assets.Gamelogic.Utils;
using Dinopark.Fire;
using Dinopark.Npc;
using UnityEngine;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.Core;

namespace Assets.Gamelogic.NPC
{
    [WorkerType(WorkerUtils.UnityGameLogic)]
    public class TargetNavigationBehaviour : MonoBehaviour
    {
        [Require] private TargetNavigationWriter targetNavigation;
        [Require] private FlammableReader flammable;

        [SerializeField] private Rigidbody myRigidbody;
        [SerializeField] private Transform myTransform;

        private Vector3 targetPosition = SimulationSettings.InvalidPosition;
        
        private void Awake()
        {
            myRigidbody = gameObject.GetComponentIfUnassigned(myRigidbody);
            myTransform = gameObject.GetComponentIfUnassigned(myTransform);
        }

        public static bool IsInTransit(TargetNavigationReader targetNavigation)
        {
            return targetNavigation.Data.NavigationState != NavigationState.INACTIVE;
        }

        public void StartNavigation(Vector3 position, float interactionSqrDistance)
        {
            var flatPosition = position.FlattenVector();
            var update = new TargetNavigation.Update
            {
                NavigationState = NavigationState.POSITION,
                TargetPosition = flatPosition.ToVector3f(),
                TargetEntityId = new EntityId(), // 不知道该给什么
                InteractionSqrDistance = interactionSqrDistance
            };
            targetNavigation.SendUpdate(update);
        }

        public void StartNavigation(EntityId targetEntityId, float interactionSqrDistance)
        {
            var update = new TargetNavigation.Update
            {
                NavigationState = NavigationState.ENTITY,
                TargetPosition = SimulationSettings.InvalidPosition.ToVector3f(),
                TargetEntityId = targetEntityId, // 不知道该给什么
                InteractionSqrDistance = interactionSqrDistance
            };
            targetNavigation.SendUpdate(update);
        }

        public void StopNavigation()
        {
            if (IsInTransit(targetNavigation))
            {
                var update = new TargetNavigation.Update
                {
                    NavigationState = NavigationState.INACTIVE,
                    TargetPosition = SimulationSettings.InvalidPosition.ToVector3f(),
                    TargetEntityId = new EntityId(), // 不知道该给什么
                    InteractionSqrDistance = 0
                };
                targetNavigation.SendUpdate(update);
            }
        }

        public void FinishNavigation(bool success)
        {
            StopNavigation();
            if (success)
            {
                var update = new TargetNavigation.Update
                {
                    NavigationState = NavigationState.INACTIVE,
                    TargetPosition = SimulationSettings.InvalidPosition.ToVector3f(),
                    TargetEntityId = new EntityId(), // 不知道该给什么
                    InteractionSqrDistance = 0
                };
                targetNavigation.SendUpdate(update);
            }
        }

        private void Update()
        {
            TargetNavigationTick();
        }

        private void TargetNavigationTick()
        {
            if (!IsInTransit(targetNavigation))
            {
                return;
            }
            
            if (targetNavigation.Data.NavigationState == NavigationState.ENTITY)
            {
                var targetGameObject = NPCUtils.GetTargetGameObject(targetNavigation.Data.TargetEntityId);
                targetPosition = targetGameObject != null ? targetGameObject.transform.position.FlattenVector() : SimulationSettings.InvalidPosition;
            }

            if (targetNavigation.Data.NavigationState == NavigationState.POSITION)
            {
                targetPosition = targetNavigation.Data.TargetPosition.ToVector3();
            }

            if (MathUtils.CompareEqualityEpsilon(targetPosition, SimulationSettings.InvalidPosition))
            {
                FinishNavigation(false);
            }

            if (TargetPositionReached())
            {
                FinishNavigation(true);
            }

            MoveTowardsTargetPosition(Time.deltaTime);
        }

        private bool TargetPositionReached()
        {
            return MathUtils.SqrDistance(myTransform.position, targetPosition) < targetNavigation.Data.InteractionSqrDistance;
        }

        private void MoveTowardsTargetPosition(float deltaTime)
        {
            var movementSpeed = SimulationSettings.NPCMovementSpeed * (flammable.Data.IsOnFire ? SimulationSettings.OnFireMovementSpeedIncreaseFactor : 1f);
            var sqrDistanceToTarget = MathUtils.SqrDistance(targetPosition, myTransform.position);
            var distanceToTravel = movementSpeed * deltaTime;
            if ((distanceToTravel * distanceToTravel) < sqrDistanceToTarget)
            {
                myRigidbody.MovePosition(myTransform.position + (targetPosition - myTransform.position).normalized*distanceToTravel);
            }
            else
            {
                myRigidbody.MovePosition(targetPosition);
            }
            if (sqrDistanceToTarget > 0.01f)
            {
                myTransform.LookAt(targetPosition, Vector3.up);
            }
        }
    }
}
