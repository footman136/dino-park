using Assets.Gamelogic.Utils;
using Improbable;
using Dinopark.Core;
using UnityEngine;
using Improbable.Gdk.Subscriptions;

namespace Assets.Gamelogic.Core
{
    public class InitialPositionVisualizer : MonoBehaviour {

        [Require] private PositionReader positionComponent;
        [Require] private TransformComponentReader transformComponent;


        private void OnEnable ()
        {
            InitializeTransform();
        }

        private void InitializeTransform()
        {
            transform.position = positionComponent.Data.Coords.ToVector3();
            transform.rotation = Quaternion.Euler(0f, transformComponent.Data.Rotation, 0f);
        }
    }
}
