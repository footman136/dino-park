// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Plants
{
    
    [global::System.Serializable]
    public struct TreeStateData
    {
        public global::Dinopark.Plants.TreeType TreeType;
        public global::Dinopark.Plants.TreeFSMState CurrentState;
    
        public TreeStateData(global::Dinopark.Plants.TreeType treeType, global::Dinopark.Plants.TreeFSMState currentState)
        {
            TreeType = treeType;
            CurrentState = currentState;
        }
        public static class Serialization
        {
            public static void Serialize(TreeStateData instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddEnum(1, (uint) instance.TreeType);
                }
                {
                    obj.AddEnum(2, (uint) instance.CurrentState);
                }
            }
    
            public static TreeStateData Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new TreeStateData();
                {
                    instance.TreeType = (global::Dinopark.Plants.TreeType) obj.GetEnum(1);
                }
                {
                    instance.CurrentState = (global::Dinopark.Plants.TreeFSMState) obj.GetEnum(2);
                }
                return instance;
            }
        }
    }
    
}
