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
            EAT = 1,
            WALK = 2,
            RUN = 3,
            ATTACK = 4,
            DEAD = 5,
            ROAR = 6,
            ON_FIRE = 7,
            NONE = 8,
        }
        
    }
    
}
