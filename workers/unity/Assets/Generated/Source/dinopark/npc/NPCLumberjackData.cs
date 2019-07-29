// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Npc
{
    
    [global::System.Serializable]
    public struct NPCLumberjackData
    {
        public global::Dinopark.Npc.LumberjackFSMState.StateEnum CurrentState;
        public global::Improbable.Gdk.Core.EntityId TargetEntityId;
        public global::Improbable.Vector3f TargetPosition;
    
        public NPCLumberjackData(global::Dinopark.Npc.LumberjackFSMState.StateEnum currentState, global::Improbable.Gdk.Core.EntityId targetEntityId, global::Improbable.Vector3f targetPosition)
        {
            CurrentState = currentState;
            TargetEntityId = targetEntityId;
            TargetPosition = targetPosition;
        }
        public static class Serialization
        {
            public static void Serialize(NPCLumberjackData instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddEnum(1, (uint) instance.CurrentState);
                }
                {
                    obj.AddEntityId(2, instance.TargetEntityId);
                }
                {
                    global::Improbable.Vector3f.Serialization.Serialize(instance.TargetPosition, obj.AddObject(3));
                }
            }
    
            public static NPCLumberjackData Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new NPCLumberjackData();
                {
                    instance.CurrentState = (global::Dinopark.Npc.LumberjackFSMState.StateEnum) obj.GetEnum(1);
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
