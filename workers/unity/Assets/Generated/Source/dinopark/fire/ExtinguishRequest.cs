// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Fire
{
    
    [global::System.Serializable]
    public struct ExtinguishRequest
    {
        public bool CanBeIgnited;
    
        public ExtinguishRequest(bool canBeIgnited)
        {
            CanBeIgnited = canBeIgnited;
        }
        public static class Serialization
        {
            public static void Serialize(ExtinguishRequest instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddBool(1, instance.CanBeIgnited);
                }
            }
    
            public static ExtinguishRequest Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new ExtinguishRequest();
                {
                    instance.CanBeIgnited = obj.GetBool(1);
                }
                return instance;
            }
        }
    }
    
}
