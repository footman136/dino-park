using System.Collections;
using System.Collections.Generic;
using Com.Infalliblecode;
using Improbable;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

namespace DinoPark
{
    public class WritePlayerTransform : MonoBehaviour
    {
        [Require] private PlayerTransformWriter _writer;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var update = new PlayerTransform.Update()
            {
                Position = Vector3f.FromUnityVector(transform.position),
                Rotation = Vector3f.FromUnityVector(transform.eulerAngles)
            };
            _writer.SendUpdate(update);
        }
    }
}