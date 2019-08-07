using Assets.Gamelogic.Utils;
using UnityEngine; // Vector3
using Improbable; // Position PositionWriter Coords
using DinoPark; // WorkerUtils
// Quaternion, MathUtils, TimeUtils
using Dinopark.Core; // TransformComponentWriter
using Improbable.Gdk.Subscriptions; // Require, WorkerType

namespace Assets.Gamelogic.Core
{
    [WorkerType(WorkerUtils.UnityGameLogic)]
    public class TransformSender : MonoBehaviour
    {
        [Require] private PositionWriter positionComponent;
        [Require] private TransformComponentWriter transformComponent;

        private int fixedFramesSinceLastUpdate = 0;

//        // 传送门
//        public void TriggerTeleport(Vector3 position, uint rotation)
//        {
//            transform.position = position;
//            var update = new Position.Update()
//            {
//                Coords = position.ToCoordinates()
//            };
//            positionComponent.SendUpdate(update);
//
//            transform.rotation = Quaternion.Euler(0, rotation, 0); // 沿Y轴旋转
//            var update2 = new TransformComponent.Update()
//            {
//                Rotation = rotation
//            };
//            transformComponent.SendUpdate(update2);
//            
//            var tevent = new TeleportEvent {TargetPosition = position.ToCoordinates()}; // 事件用此方法传递
//            transformComponent.SendTeleportEventEvent(tevent);
//        }

        private void OnEnable()
        {
//            var newPosition = positionComponent.Data.Coords.ToVector3();
//            var newRotation = transformComponent.Data.Rotation; // 沿Y轴旋转
//            transform.position = newPosition;
//            transform.rotation = Quaternion.EulerAngles(0, newRotation, 0);
        }

        private void FixedUpdate()
        {
            var newPosition = transform.position.ToCoordinates();
            var newRotation = QuantizationUtils.QuantizeAngle(transform.rotation.eulerAngles.y); // 沿Y轴旋转
            fixedFramesSinceLastUpdate++;
            if ((PositionNeedsUpdate(newPosition) || RotationNeedsUpdate(newRotation)) && fixedFramesSinceLastUpdate > SimulationSettings.TransformUpdatesToSkipBetweenSends)
            {
                fixedFramesSinceLastUpdate = 0;
                var update = new Position.Update()
                {
                    Coords = newPosition
                };
                positionComponent.SendUpdate(update);
                var update2 = new TransformComponent.Update()
                {
                    Rotation = newRotation
                };
                transformComponent.SendUpdate(update2);
            }
        }

        private bool PositionNeedsUpdate(Coordinates newPosition)
        {
            return !MathUtils.CompareEqualityEpsilon(newPosition.ToVector3(), positionComponent.Data.Coords.ToVector3());
        }

        private bool RotationNeedsUpdate(float newRotation)
        {
            return !MathUtils.CompareEqualityEpsilon(newRotation, transformComponent.Data.Rotation);
        }
    }
}
