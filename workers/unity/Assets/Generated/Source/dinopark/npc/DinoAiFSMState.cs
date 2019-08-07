// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Npc
{
    
    [global::System.Serializable]
    public struct DinoAiFSMState
    {
        public static class Serialization
        {
            public static void Serialize(DinoAiFSMState instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
            }
    
            public static DinoAiFSMState Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new DinoAiFSMState();
                return instance;
            }
        }
    
        
        [global::System.Serializable]
        public enum StateEnum : uint
        {
            IDLE = 0,
            EAT = 1,
            WANDER = 2,
            RUN_AWAY = 3,
            CHASE = 4,
            ATTACK = 5,
            BE_ATTACK = 6,
            DEAD = 7,
            ROAR = 8,
            BREED = 9,
            ON_FIRE = 19,
            NONE = 20,
        }
        
    }
    
}
