using Com.Infalliblecode;
using Dinopark.Npc;
using Improbable.Gdk.Subscriptions; // Require
using UnityEngine;
using Improbable;
using LowPolyAnimalPack;

namespace Assets.Gamelogic.Core
{
    public class ReadDinoTransform : MonoBehaviour
    {
        [Require] private PlayerTransformReader _reader;
        [Require] private PositionReader spatialPosition; // 这个是系统自带的坐标类，只有使用了这个，才能在网页地图上显示出正确的坐标

        // Update is called once per frame
        void Update()
        {
            //transform.position = _reader.Data.Position.ToUnityVector();
            transform.position = spatialPosition.Data.Coords.ToUnityVector();
            transform.rotation = Quaternion.Euler(_reader.Data.Rotation.ToUnityVector());
        }
    }
}