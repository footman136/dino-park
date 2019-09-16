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
    public class InitialTransformSender : MonoBehaviour
    {
        [Require] private PositionWriter positionComponent;
        [Require] private TransformComponentWriter transformComponent;

        private void Start()
        {
            SendPosition();
        }

        private void SendPosition()
        {
            var newPosition = transform.position.ToCoordinates();
            var newRotation = QuantizationUtils.QuantizeAngle(transform.rotation.eulerAngles.y); // 沿Y轴旋转
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
}
