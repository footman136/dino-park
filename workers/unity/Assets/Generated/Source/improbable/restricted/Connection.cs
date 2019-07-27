// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Restricted
{
    
    [global::System.Serializable]
    public struct Connection
    {
        public global::Improbable.Restricted.Connection.ConnectionStatus Status;
        public uint DataLatencyMs;
        public ulong ConnectedSinceUtc;
    
        public Connection(global::Improbable.Restricted.Connection.ConnectionStatus status, uint dataLatencyMs, ulong connectedSinceUtc)
        {
            Status = status;
            DataLatencyMs = dataLatencyMs;
            ConnectedSinceUtc = connectedSinceUtc;
        }
        public static class Serialization
        {
            public static void Serialize(Connection instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddEnum(1, (uint) instance.Status);
                }
                {
                    obj.AddUint32(2, instance.DataLatencyMs);
                }
                {
                    obj.AddUint64(3, instance.ConnectedSinceUtc);
                }
            }
    
            public static Connection Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new Connection();
                {
                    instance.Status = (global::Improbable.Restricted.Connection.ConnectionStatus) obj.GetEnum(1);
                }
                {
                    instance.DataLatencyMs = obj.GetUint32(2);
                }
                {
                    instance.ConnectedSinceUtc = obj.GetUint64(3);
                }
                return instance;
            }
        }
    
        
        [global::System.Serializable]
        public enum ConnectionStatus : uint
        {
            UNKNOWN = 0,
            AWAITING_WORKER_CONNECTION = 1,
            CONNECTED = 2,
            DISCONNECTED = 3,
        }
        
    }
    
}
