// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.PlayerLifecycle
{
    
    [global::System.Serializable]
    public struct CreatePlayerRequest
    {
        public byte[] SerializedArguments;
    
        public CreatePlayerRequest(byte[] serializedArguments)
        {
            SerializedArguments = serializedArguments;
        }
        public static class Serialization
        {
            public static void Serialize(CreatePlayerRequest instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddBytes(1, instance.SerializedArguments);
                }
            }
    
            public static CreatePlayerRequest Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new CreatePlayerRequest();
                {
                    instance.SerializedArguments = obj.GetBytes(1);
                }
                return instance;
            }
        }
    }
    
}
