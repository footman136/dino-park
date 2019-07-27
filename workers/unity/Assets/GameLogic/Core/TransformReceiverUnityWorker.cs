using UnityEngine;
using Improbable;
using DinoPark; // WorkerUtils
using GameLogic.Core; // GetComponentIfUnassigned
using GameLogic.Utils; // Quaternion, MathUtils, TimeUtils
using Dinopark.Core; // TransformComponentReader
using Improbable.Gdk.Subscriptions; // Require, WorkerType
using Improbable.Worker.CInterop; // Authority.NotAuthoritative

namespace Gamelogic.Core
{
    [WorkerType(WorkerUtils.UnityGameLogic)]
    public class TransformReceiverUnityWorker : MonoBehaviour
    {
        [Require] private PositionReader positionComponent;
        [Require] private TransformComponentReader rotationComponent;

        [SerializeField] private Rigidbody myRigidbody;

        private void Awake()
        {
            myRigidbody = gameObject.GetComponentIfUnassigned(myRigidbody);
        }

        private void OnEnable()
        {
            positionComponent.OnUpdate += (OnPositionUpdated);
            rotationComponent.OnUpdate += (OnRotationUpdated);
        }

        private void OnDisable()
        {
            positionComponent.OnUpdate -= (OnPositionUpdated);
            rotationComponent.OnUpdate -= (OnRotationUpdated);
        }

        private void OnPositionUpdated(Position.Update update)
        {
            if (positionComponent.Authority == Authority.NotAuthoritative)
            {
                // 好神奇！update.Coords竟然是一个map，不能直接获取
                Coordinates coords;
                if (update.Coords.TryGetValue(out coords))
                {
                    myRigidbody.MovePosition(coords.ToVector3());
                }
            }
        }

        private void OnRotationUpdated(TransformComponent.Update update)
        {
            if (rotationComponent.Authority == Authority.NotAuthoritative)
            {
                myRigidbody.MoveRotation(Quaternion.Euler(0f, QuantizationUtils.DequantizeAngle(update.Rotation), 0f));
            }
        }
    }
}
