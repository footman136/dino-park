// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Plants
{
    
    [global::System.Serializable]
    public struct HarvestResponse
    {
        public float ResourcesTaken;
    
        public HarvestResponse(float resourcesTaken)
        {
            ResourcesTaken = resourcesTaken;
        }
        public static class Serialization
        {
            public static void Serialize(HarvestResponse instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddFloat(1, instance.ResourcesTaken);
                }
            }
    
            public static HarvestResponse Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new HarvestResponse();
                {
                    instance.ResourcesTaken = obj.GetFloat(1);
                }
                return instance;
            }
        }
    }
    
}
