// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Npc
{
    
    [global::System.Serializable]
    public struct HatchResponse
    {
        public global::Dinopark.Npc.EggTypeEnum HatchOutType;
        public global::Improbable.Vector3f HatchOutPosition;
    
        public HatchResponse(global::Dinopark.Npc.EggTypeEnum hatchOutType, global::Improbable.Vector3f hatchOutPosition)
        {
            HatchOutType = hatchOutType;
            HatchOutPosition = hatchOutPosition;
        }
        public static class Serialization
        {
            public static void Serialize(HatchResponse instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddEnum(1, (uint) instance.HatchOutType);
                }
                {
                    global::Improbable.Vector3f.Serialization.Serialize(instance.HatchOutPosition, obj.AddObject(2));
                }
            }
    
            public static HatchResponse Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new HatchResponse();
                {
                    instance.HatchOutType = (global::Dinopark.Npc.EggTypeEnum) obj.GetEnum(1);
                }
                {
                    instance.HatchOutPosition = global::Improbable.Vector3f.Serialization.Deserialize(obj.GetObject(2));
                }
                return instance;
            }
        }
    }
    
}
