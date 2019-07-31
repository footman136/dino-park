// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Npc
{
    
    [global::System.Serializable]
    public struct DinoFSMState
    {
        public static class Serialization
        {
            public static void Serialize(DinoFSMState instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
            }
    
            public static DinoFSMState Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new DinoFSMState();
                return instance;
            }
        }
    
        
        [global::System.Serializable]
        public enum StateEnum : uint
        {
            IDLE = 0,
            WALK = 1,
            RUN = 2,
            ATTACK = 3,
            DEAD = 4,
            ON_FIRE = 5,
            NONE = 6,
        }
        
    }
    
}
