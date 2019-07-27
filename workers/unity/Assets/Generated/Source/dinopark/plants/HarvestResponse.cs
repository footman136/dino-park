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
        public int ResourcesTaken;
    
        public HarvestResponse(int resourcesTaken)
        {
            ResourcesTaken = resourcesTaken;
        }
        public static class Serialization
        {
            public static void Serialize(HarvestResponse instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddInt32(1, instance.ResourcesTaken);
                }
            }
    
            public static HarvestResponse Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new HarvestResponse();
                {
                    instance.ResourcesTaken = obj.GetInt32(1);
                }
                return instance;
            }
        }
    }
    
}
