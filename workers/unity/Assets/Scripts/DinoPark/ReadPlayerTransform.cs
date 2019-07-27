using Com.Infalliblecode;
using Improbable.Gdk.Subscriptions; // Require
using UnityEngine;

namespace DinoPark
{
    public class ReadPlayerTransform : MonoBehaviour
    {
        [Require] private PlayerTransformReader _reader;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.position = _reader.Data.Position.ToUnityVector();
            transform.rotation = Quaternion.Euler(_reader.Data.Rotation.ToUnityVector());
        }
    }
}