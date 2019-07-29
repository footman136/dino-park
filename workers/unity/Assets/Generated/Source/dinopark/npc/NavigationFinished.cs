// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Npc
{
    
    [global::System.Serializable]
    public struct NavigationFinished
    {
        public bool Success;
    
        public NavigationFinished(bool success)
        {
            Success = success;
        }
        public static class Serialization
        {
            public static void Serialize(NavigationFinished instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddBool(1, instance.Success);
                }
            }
    
            public static NavigationFinished Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new NavigationFinished();
                {
                    instance.Success = obj.GetBool(1);
                }
                return instance;
            }
        }
    }
    
}
