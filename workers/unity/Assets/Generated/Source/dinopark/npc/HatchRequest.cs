// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Npc
{
    
    [global::System.Serializable]
    public struct HatchRequest
    {
        public global::Dinopark.Npc.EggTypeEnum EggType;
        public global::Improbable.Vector3f EggPosition;
    
        public HatchRequest(global::Dinopark.Npc.EggTypeEnum eggType, global::Improbable.Vector3f eggPosition)
        {
            EggType = eggType;
            EggPosition = eggPosition;
        }
        public static class Serialization
        {
            public static void Serialize(HatchRequest instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddEnum(1, (uint) instance.EggType);
                }
                {
                    global::Improbable.Vector3f.Serialization.Serialize(instance.EggPosition, obj.AddObject(2));
                }
            }
    
            public static HatchRequest Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new HatchRequest();
                {
                    instance.EggType = (global::Dinopark.Npc.EggTypeEnum) obj.GetEnum(1);
                }
                {
                    instance.EggPosition = global::Improbable.Vector3f.Serialization.Deserialize(obj.GetObject(2));
                }
                return instance;
            }
        }
    }
    
}
