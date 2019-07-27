// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Core
{
    
    [global::System.Serializable]
    public struct TeleportEvent
    {
        public global::Improbable.Coordinates TargetPosition;
    
        public TeleportEvent(global::Improbable.Coordinates targetPosition)
        {
            TargetPosition = targetPosition;
        }
        public static class Serialization
        {
            public static void Serialize(TeleportEvent instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    global::Improbable.Coordinates.Serialization.Serialize(instance.TargetPosition, obj.AddObject(1));
                }
            }
    
            public static TeleportEvent Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new TeleportEvent();
                {
                    instance.TargetPosition = global::Improbable.Coordinates.Serialization.Deserialize(obj.GetObject(1));
                }
                return instance;
            }
        }
    }
    
}
