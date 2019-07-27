// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.PlayerLifecycle
{
    
    [global::System.Serializable]
    public struct CreatePlayerResponse
    {
        public global::Improbable.Gdk.Core.EntityId CreatedEntityId;
    
        public CreatePlayerResponse(global::Improbable.Gdk.Core.EntityId createdEntityId)
        {
            CreatedEntityId = createdEntityId;
        }
        public static class Serialization
        {
            public static void Serialize(CreatePlayerResponse instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddEntityId(1, instance.CreatedEntityId);
                }
            }
    
            public static CreatePlayerResponse Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new CreatePlayerResponse();
                {
                    instance.CreatedEntityId = obj.GetEntityIdStruct(1);
                }
                return instance;
            }
        }
    }
    
}
