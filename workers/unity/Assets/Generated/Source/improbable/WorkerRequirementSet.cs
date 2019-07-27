// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable
{
    
    [global::System.Serializable]
    public struct WorkerRequirementSet
    {
        public global::System.Collections.Generic.List<global::Improbable.WorkerAttributeSet> AttributeSet;
    
        public WorkerRequirementSet(global::System.Collections.Generic.List<global::Improbable.WorkerAttributeSet> attributeSet)
        {
            AttributeSet = attributeSet;
        }
        public static class Serialization
        {
            public static void Serialize(WorkerRequirementSet instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    foreach (var value in instance.AttributeSet)
                    {
                        global::Improbable.WorkerAttributeSet.Serialization.Serialize(value, obj.AddObject(1));
                    }
                    
                }
            }
    
            public static WorkerRequirementSet Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new WorkerRequirementSet();
                {
                    instance.AttributeSet = new global::System.Collections.Generic.List<global::Improbable.WorkerAttributeSet>();
                    var list = instance.AttributeSet;
                    var listLength = obj.GetObjectCount(1);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(global::Improbable.WorkerAttributeSet.Serialization.Deserialize(obj.IndexObject(1, (uint) i)));
                    }
                    
                }
                return instance;
            }
        }
    }
    
}
