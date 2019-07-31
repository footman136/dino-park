// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Npc
{
    
    [global::System.Serializable]
    public struct DinoData
    {
        public global::Dinopark.Npc.DinoFSMState.StateEnum CurrentState;
    
        public DinoData(global::Dinopark.Npc.DinoFSMState.StateEnum currentState)
        {
            CurrentState = currentState;
        }
        public static class Serialization
        {
            public static void Serialize(DinoData instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddEnum(1, (uint) instance.CurrentState);
                }
            }
    
            public static DinoData Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new DinoData();
                {
                    instance.CurrentState = (global::Dinopark.Npc.DinoFSMState.StateEnum) obj.GetEnum(1);
                }
                return instance;
            }
        }
    }
    
}
