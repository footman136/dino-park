// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Npc
{
    
    [global::System.Serializable]
    public struct EatResponse
    {
        public float ResTaken;
    
        public EatResponse(float resTaken)
        {
            ResTaken = resTaken;
        }
        public static class Serialization
        {
            public static void Serialize(EatResponse instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddFloat(1, instance.ResTaken);
                }
            }
    
            public static EatResponse Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new EatResponse();
                {
                    instance.ResTaken = obj.GetFloat(1);
                }
                return instance;
            }
        }
    }
    
}
