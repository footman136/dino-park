// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Plants
{
    
    [global::System.Serializable]
    public struct HarvestRequest
    {
        public global::Improbable.Gdk.Core.EntityId Harvester;
        public float ResourcesNeed;
    
        public HarvestRequest(global::Improbable.Gdk.Core.EntityId harvester, float resourcesNeed)
        {
            Harvester = harvester;
            ResourcesNeed = resourcesNeed;
        }
        public static class Serialization
        {
            public static void Serialize(HarvestRequest instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddEntityId(1, instance.Harvester);
                }
                {
                    obj.AddFloat(2, instance.ResourcesNeed);
                }
            }
    
            public static HarvestRequest Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new HarvestRequest();
                {
                    instance.Harvester = obj.GetEntityIdStruct(1);
                }
                {
                    instance.ResourcesNeed = obj.GetFloat(2);
                }
                return instance;
            }
        }
    }
    
}
