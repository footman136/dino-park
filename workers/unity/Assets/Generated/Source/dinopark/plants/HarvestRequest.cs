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
    
        public HarvestRequest(global::Improbable.Gdk.Core.EntityId harvester)
        {
            Harvester = harvester;
        }
        public static class Serialization
        {
            public static void Serialize(HarvestRequest instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddEntityId(1, instance.Harvester);
                }
            }
    
            public static HarvestRequest Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new HarvestRequest();
                {
                    instance.Harvester = obj.GetEntityIdStruct(1);
                }
                return instance;
            }
        }
    }
    
}
