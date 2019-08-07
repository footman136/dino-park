// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Npc
{
    
    [global::System.Serializable]
    public struct AiData
    {
        public global::Dinopark.Npc.DinoAiFSMState.StateEnum CurrentAiState;
        public global::Improbable.Gdk.Core.EntityId TargetEntityId;
        public global::Improbable.Vector3f TargetPosition;
    
        public AiData(global::Dinopark.Npc.DinoAiFSMState.StateEnum currentAiState, global::Improbable.Gdk.Core.EntityId targetEntityId, global::Improbable.Vector3f targetPosition)
        {
            CurrentAiState = currentAiState;
            TargetEntityId = targetEntityId;
            TargetPosition = targetPosition;
        }
        public static class Serialization
        {
            public static void Serialize(AiData instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddEnum(1, (uint) instance.CurrentAiState);
                }
                {
                    obj.AddEntityId(2, instance.TargetEntityId);
                }
                {
                    global::Improbable.Vector3f.Serialization.Serialize(instance.TargetPosition, obj.AddObject(3));
                }
            }
    
            public static AiData Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new AiData();
                {
                    instance.CurrentAiState = (global::Dinopark.Npc.DinoAiFSMState.StateEnum) obj.GetEnum(1);
                }
                {
                    instance.TargetEntityId = obj.GetEntityIdStruct(2);
                }
                {
                    instance.TargetPosition = global::Improbable.Vector3f.Serialization.Deserialize(obj.GetObject(3));
                }
                return instance;
            }
        }
    }
    
}
