// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable
{
    
    [global::System.Serializable]
    public struct WorkerAttributeSet
    {
        public global::System.Collections.Generic.List<string> Attribute;
    
        public WorkerAttributeSet(global::System.Collections.Generic.List<string> attribute)
        {
            Attribute = attribute;
        }
        public static class Serialization
        {
            public static void Serialize(WorkerAttributeSet instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    foreach (var value in instance.Attribute)
                    {
                        obj.AddString(1, value);
                    }
                    
                }
            }
    
            public static WorkerAttributeSet Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new WorkerAttributeSet();
                {
                    instance.Attribute = new global::System.Collections.Generic.List<string>();
                    var list = instance.Attribute;
                    var listLength = obj.GetStringCount(1);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexString(1, (uint) i));
                    }
                    
                }
                return instance;
            }
        }
    }
    
}
