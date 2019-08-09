// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Npc
{
    
    [global::System.Serializable]
    public struct AttackRequest
    {
        public global::Improbable.Gdk.Core.EntityId Attacker;
        public float Damage;
    
        public AttackRequest(global::Improbable.Gdk.Core.EntityId attacker, float damage)
        {
            Attacker = attacker;
            Damage = damage;
        }
        public static class Serialization
        {
            public static void Serialize(AttackRequest instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddEntityId(1, instance.Attacker);
                }
                {
                    obj.AddFloat(2, instance.Damage);
                }
            }
    
            public static AttackRequest Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new AttackRequest();
                {
                    instance.Attacker = obj.GetEntityIdStruct(1);
                }
                {
                    instance.Damage = obj.GetFloat(2);
                }
                return instance;
            }
        }
    }
    
}
