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
        private float _distanceEpsilon = 20.0f;
        private float _rotateEpsilon = 10.0f;

        private float TIME_DELAY = 0.03f;
        private float timeNow = 0;
        private void FixedUpdate()
        {
            timeNow += Time.deltaTime;
            if (timeNow < TIME_DELAY)
            {
                return;
            }

            timeNow = 0;
            
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
            return MathUtils.SqrDistance(newPosition.ToVector3(), positionComponent.Data.Coords.ToVector3()) > _distanceEpsilon;
        }

        private bool RotationNeedsUpdate(float newRotation)
        {
            return (newRotation - transformComponent.Data.Rotation) > _rotateEpsilon;
        }
    }
}
