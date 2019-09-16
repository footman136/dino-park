using UnityEngine;
using Improbable;
using Assets.Gamelogic.Utils;
using Dinopark.Core;
using Improbable.Gdk.Subscriptions; // Require, WorkerType

namespace Assets.Gamelogic.Core
{
    public class TransformVisualizer : MonoBehaviour
    {
        [Require] private PositionReader positionComponent;
        [Require] private TransformComponentReader transformComponent;

        private void OnEnable()
        {
            positionComponent.OnUpdate += (VisualizePosition);
            transformComponent.OnUpdate += (VisualizeTransform);
            SetPosition(positionComponent.Data.Coords);
            SetRotation(transformComponent.Data.Rotation);
        }

        private void OnDisable()
        {
            positionComponent.OnUpdate -= (VisualizePosition);
            transformComponent.OnUpdate -= (VisualizeTransform);
        }

        private void VisualizePosition(Position.Update update)
        {
            if(update.Coords.HasValue)
            {
                SetPosition(update.Coords.Value);
            }
        }

        private void VisualizeTransform(TransformComponent.Update update)
        {
            if (update.Rotation.HasValue)
            {
                SetRotation(update.Rotation.Value);
            }
        }

        private void SetPosition(Coordinates position)
        {
            transform.position = position.ToVector3();
        }

        private void SetRotation(uint rotation)
        {
            transform.rotation = Quaternion.Euler(0f, QuantizationUtils.DequantizeAngle(rotation), 0f);
        }
    }
}
