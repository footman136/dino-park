using Com.Infalliblecode;
using Improbable.Gdk.Subscriptions; // Require
using UnityEngine;
using Improbable;
namespace Assets.Gamelogic.Core
{
    public class ReadPlayerTransform : MonoBehaviour
    {
        [Require] private PlayerTransformReader _reader;
        [Require] private PositionReader spatialPosition;

        // Update is called once per frame
        void Update()
        {
            //transform.position = _reader.Data.Position.ToUnityVector();
            transform.position = spatialPosition.Data.Coords.ToUnityVector();
            transform.rotation = Quaternion.Euler(_reader.Data.Rotation.ToUnityVector());
        }
    }
}