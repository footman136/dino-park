// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Fire
{
    
    [global::System.Serializable]
    public struct SetCanBeIgnitedRequest
    {
        public bool CanBeIgnited;
    
        public SetCanBeIgnitedRequest(bool canBeIgnited)
        {
            CanBeIgnited = canBeIgnited;
        }
        public static class Serialization
        {
            public static void Serialize(SetCanBeIgnitedRequest instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddBool(1, instance.CanBeIgnited);
                }
            }
    
            public static SetCanBeIgnitedRequest Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new SetCanBeIgnitedRequest();
                {
                    instance.CanBeIgnited = obj.GetBool(1);
                }
                return instance;
            }
        }
    }
    
}
