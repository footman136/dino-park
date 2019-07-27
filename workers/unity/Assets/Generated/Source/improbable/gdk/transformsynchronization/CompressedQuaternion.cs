// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    
    [global::System.Serializable]
    public struct CompressedQuaternion
    {
        public uint Data;
    
        public CompressedQuaternion(uint data)
        {
            Data = data;
        }
        public static class Serialization
        {
            public static void Serialize(CompressedQuaternion instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddUint32(1, instance.Data);
                }
            }
    
            public static CompressedQuaternion Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new CompressedQuaternion();
                {
                    instance.Data = obj.GetUint32(1);
                }
                return instance;
            }
        }
    }
    
}
