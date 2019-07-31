using Assets.Gamelogic.Utils;
using Com.Infalliblecode;
using Improbable;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

namespace Assets.Gamelogic.Core
{
    public class WriteTransform : MonoBehaviour
    {
        [Require] private PlayerTransformWriter _writer;
        [Require] private PositionWriter spatialPosition;

        // Update is called once per frame
        void Update()
        {
            var update = new Position.Update()
            {
                Coords = transform.position.ToCoordinates()
            };
            spatialPosition.SendUpdate(update);
            var update2 = new PlayerTransform.Update()
            {
                //Position = Vector3f.FromUnityVector(transform.position),
                Rotation = Vector3f.FromUnityVector(transform.eulerAngles)
            };
            _writer.SendUpdate(update2);
        }
    }
}