using Dinopark.Core;
using Dinopark.Fire;
using DinoPark;
using Improbable.Gdk.Subscriptions;
using UnityEngine;
using Assets.Gamelogic.Utils;
// WorkerUtils
// Quaternion, MathUtils, TimeUtils
// SimulationSettings

// Require, WorkerType

namespace Assets.Gamelogic.Core
{
    [WorkerType(WorkerUtils.UnityClient)]
    public class TransformReceiverClientControllableAuthoritative : MonoBehaviour
    {
        //[Require] private TransformComponentWriter clientAuthorityCheck;
        [Require] private TransformComponentReader transformComponent;
        [Require] private FlammableReader flammable;

        private Vector3 targetVelocity;

        [SerializeField] private Rigidbody myRigidbody;

        private void Awake()
        {
            myRigidbody = gameObject.GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            //transformComponent.OnUpdate += (OnTransformComponentUpdated);
            transformComponent.OnTeleportEventEvent += (OnTeleportEvent);
        }

        private void OnDisable()
        {
            //transformComponent.OnUpdate -= (OnTransformComponentUpdated);
            transformComponent.OnTeleportEventEvent -= (OnTeleportEvent);
        }

//        private void OnTransformComponentUpdated(TransformComponent.Update update)
//        {
//            for (int i = 0; i < update.teleportEvent.Count; i++)
//            {
//                TeleportTo(update.teleportEvent[i].targetPosition.ToVector3());
//            }
//        }
        private void OnTeleportEvent(TeleportEvent tevent)
        {
            TeleportTo(tevent.TargetPosition.ToVector3());
        }

        private void TeleportTo(Vector3 position)
        {
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.MovePosition(position);
        }

        public void SetTargetVelocity(Vector3 direction)
        {
            bool isOnFire = flammable != null && flammable.Data.IsOnFire;
            var movementSpeed = SimulationSettings.PlayerMovementSpeed * (isOnFire ? SimulationSettings.OnFireMovementSpeedIncreaseFactor : 1f);
            targetVelocity = direction * movementSpeed;
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        public void MovePlayer()
        {
            var currentVelocity = myRigidbody.velocity;
            var velocityChange = targetVelocity - currentVelocity;
            if (ShouldMovePlayerAuthoritativeClient(velocityChange))
            {
                transform.LookAt(myRigidbody.position + targetVelocity);
                myRigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
            }
        }

        private bool ShouldMovePlayerAuthoritativeClient(Vector3 velocityChange)
        {
            return velocityChange.sqrMagnitude > Mathf.Epsilon && PlayerMovementCheatSafeguardPassedAuthoritativeClient(velocityChange);
        }

        private bool PlayerMovementCheatSafeguardPassedAuthoritativeClient(Vector3 velocityChange)
        {
            var result = velocityChange.sqrMagnitude < SimulationSettings.PlayerPositionUpdateMaxSqrDistance;
            if (!result)
            {
                Debug.LogError("Player movement cheat safeguard failed on Client. " + velocityChange.sqrMagnitude);
            }
            return result;
        }
    }
}
