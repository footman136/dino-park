// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Npc
{
    
    [global::System.Serializable]
    public struct BrachioFSMState
    {
        public static class Serialization
        {
            public static void Serialize(BrachioFSMState instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
            }
    
            public static BrachioFSMState Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new BrachioFSMState();
                return instance;
            }
        }
    
        
        [global::System.Serializable]
        public enum StateEnum : uint
        {
            IDLE = 0,
            MOVING_TO_TARGET = 1,
            HARVESTING = 2,
            STOCKPILING = 3,
            ON_FIRE = 4,
        }
        
    }
    
}
