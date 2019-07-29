// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Npc
{
    
    [global::System.Serializable]
    public struct TargetNavigationData
    {
        public global::Dinopark.Npc.NavigationState NavigationState;
        public global::Improbable.Vector3f TargetPosition;
        public global::Improbable.Gdk.Core.EntityId TargetEntityId;
        public float InteractionSqrDistance;
    
        public TargetNavigationData(global::Dinopark.Npc.NavigationState navigationState, global::Improbable.Vector3f targetPosition, global::Improbable.Gdk.Core.EntityId targetEntityId, float interactionSqrDistance)
        {
            NavigationState = navigationState;
            TargetPosition = targetPosition;
            TargetEntityId = targetEntityId;
            InteractionSqrDistance = interactionSqrDistance;
        }
        public static class Serialization
        {
            public static void Serialize(TargetNavigationData instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddEnum(1, (uint) instance.NavigationState);
                }
                {
                    global::Improbable.Vector3f.Serialization.Serialize(instance.TargetPosition, obj.AddObject(2));
                }
                {
                    obj.AddEntityId(3, instance.TargetEntityId);
                }
                {
                    obj.AddFloat(4, instance.InteractionSqrDistance);
                }
            }
    
            public static TargetNavigationData Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new TargetNavigationData();
                {
                    instance.NavigationState = (global::Dinopark.Npc.NavigationState) obj.GetEnum(1);
                }
                {
                    instance.TargetPosition = global::Improbable.Vector3f.Serialization.Deserialize(obj.GetObject(2));
                }
                {
                    instance.TargetEntityId = obj.GetEntityIdStruct(3);
                }
                {
                    instance.InteractionSqrDistance = obj.GetFloat(4);
                }
                return instance;
            }
        }
    }
    
}
