// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Restricted
{
    
    [global::System.Serializable]
    public struct PlayerIdentity
    {
        public string PlayerIdentifier;
        public string Provider;
        public byte[] Metadata;
    
        public PlayerIdentity(string playerIdentifier, string provider, byte[] metadata)
        {
            PlayerIdentifier = playerIdentifier;
            Provider = provider;
            Metadata = metadata;
        }
        public static class Serialization
        {
            public static void Serialize(PlayerIdentity instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddString(1, instance.PlayerIdentifier);
                }
                {
                    obj.AddString(2, instance.Provider);
                }
                {
                    obj.AddBytes(3, instance.Metadata);
                }
            }
    
            public static PlayerIdentity Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new PlayerIdentity();
                {
                    instance.PlayerIdentifier = obj.GetString(1);
                }
                {
                    instance.Provider = obj.GetString(2);
                }
                {
                    instance.Metadata = obj.GetBytes(3);
                }
                return instance;
            }
        }
    }
    
}
