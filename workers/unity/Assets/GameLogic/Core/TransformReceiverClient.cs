using UnityEngine;
using Improbable;
using DinoPark; // WorkerUtils
using GameLogic.Utils; // Quaternion, MathUtils, TimeUtils
using Dinopark.Core;
using Improbable.Gdk.Subscriptions; // Require, WorkerType
using Improbable.Worker.CInterop; // Authority.NotAuthoritative

namespace Gamelogic.Core
{
    [WorkerType(WorkerUtils.UnityClient)]
    public class TransformReceiverClient : MonoBehaviour
    {
        [Require] private PositionReader positionComponent;
        [Require] private TransformComponentReader transformComponent;

        private bool isRemote;

        [SerializeField] private Rigidbody myRigidbody;

        private void Awake()
        {
            myRigidbody = gameObject.GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            //positionComponent.OnUpdate += (OnPositionComponentUpdated); 坐标不需要事件来处理，直接在Update()里更新就够了
            //transformComponent.OnUpdate += (OnTransformComponentUpdated);
            transformComponent.OnTeleportEventEvent += (OnTeleportEvent);
            if (IsNotAnAuthoritativePlayer())
            {
                SetUpRemoteTransform();
            }     
        }

        private void OnDisable()
        {
            //positionComponent.OnUpdate -= (OnPositionComponentUpdated);
            //transformComponent.OnUpdate -= (OnTransformComponentUpdated);
            transformComponent.OnTeleportEventEvent -= (OnTeleportEvent);
            if (isRemote)
            {
                TearDownRemoveTransform();
            }
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

        private bool IsNotAnAuthoritativePlayer()
        {
            return transformComponent.Authority == Authority.NotAuthoritative;
            //return gameObject.GetAuthority(ClientAuthorityCheck.ComponentId) == Authority.NotAuthoritative;
        }

        private void Update()
        {
            if (IsNotAnAuthoritativePlayer())
            {
                myRigidbody.MovePosition(Vector3.Lerp(myRigidbody.position, positionComponent.Data.Coords.ToVector3(), 0.2f));
                myRigidbody.MoveRotation(Quaternion.Euler(0f, QuantizationUtils.DequantizeAngle(transformComponent.Data.Rotation), 0f));
            }
            else if(isRemote)
            {
                TearDownRemoveTransform();
            }
        }

        private void SetUpRemoteTransform()
        {
            isRemote = true;
            myRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            myRigidbody.isKinematic = true;
        }

        private void TearDownRemoveTransform()
        {
            isRemote = false;
            myRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            myRigidbody.isKinematic = false;
        }
    }
}
