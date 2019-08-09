// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Npc
{
    
    [global::System.Serializable]
    public struct EatRequest
    {
        public global::Improbable.Gdk.Core.EntityId Attacker;
        public float ResNeed;
    
        public EatRequest(global::Improbable.Gdk.Core.EntityId attacker, float resNeed)
        {
            Attacker = attacker;
            ResNeed = resNeed;
        }
        public static class Serialization
        {
            public static void Serialize(EatRequest instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddEntityId(1, instance.Attacker);
                }
                {
                    obj.AddFloat(2, instance.ResNeed);
                }
            }
    
            public static EatRequest Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new EatRequest();
                {
                    instance.Attacker = obj.GetEntityIdStruct(1);
                }
                {
                    instance.ResNeed = obj.GetFloat(2);
                }
                return instance;
            }
        }
    }
    
}
