// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Player
{
    
    [global::System.Serializable]
    public struct LayEggRequest
    {
        public global::Dinopark.Npc.EggTypeEnum EggType;
        public global::Improbable.Vector3f EggPosition;
        public long OwnerEntityId;
    
        public LayEggRequest(global::Dinopark.Npc.EggTypeEnum eggType, global::Improbable.Vector3f eggPosition, long ownerEntityId)
        {
            EggType = eggType;
            EggPosition = eggPosition;
            OwnerEntityId = ownerEntityId;
        }
        public static class Serialization
        {
            public static void Serialize(LayEggRequest instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddEnum(1, (uint) instance.EggType);
                }
                {
                    global::Improbable.Vector3f.Serialization.Serialize(instance.EggPosition, obj.AddObject(2));
                }
                {
                    obj.AddInt64(3, instance.OwnerEntityId);
                }
            }
    
            public static LayEggRequest Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new LayEggRequest();
                {
                    instance.EggType = (global::Dinopark.Npc.EggTypeEnum) obj.GetEnum(1);
                }
                {
                    instance.EggPosition = global::Improbable.Vector3f.Serialization.Deserialize(obj.GetObject(2));
                }
                {
                    instance.OwnerEntityId = obj.GetInt64(3);
                }
                return instance;
            }
        }
    }
    
}
