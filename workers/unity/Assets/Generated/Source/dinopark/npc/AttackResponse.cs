// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Npc
{
    
    [global::System.Serializable]
    public struct AttackResponse
    {
        public float DamageTaken;
    
        public AttackResponse(float damageTaken)
        {
            DamageTaken = damageTaken;
        }
        public static class Serialization
        {
            public static void Serialize(AttackResponse instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddFloat(1, instance.DamageTaken);
                }
            }
    
            public static AttackResponse Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new AttackResponse();
                {
                    instance.DamageTaken = obj.GetFloat(1);
                }
                return instance;
            }
        }
    }
    
}
