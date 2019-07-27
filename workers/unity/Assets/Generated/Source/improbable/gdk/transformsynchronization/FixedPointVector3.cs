// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    
    [global::System.Serializable]
    public struct FixedPointVector3
    {
        public int X;
        public int Y;
        public int Z;
    
        public FixedPointVector3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public static class Serialization
        {
            public static void Serialize(FixedPointVector3 instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddSint32(1, instance.X);
                }
                {
                    obj.AddSint32(2, instance.Y);
                }
                {
                    obj.AddSint32(3, instance.Z);
                }
            }
    
            public static FixedPointVector3 Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new FixedPointVector3();
                {
                    instance.X = obj.GetSint32(1);
                }
                {
                    instance.Y = obj.GetSint32(2);
                }
                {
                    instance.Z = obj.GetSint32(3);
                }
                return instance;
            }
        }
    }
    
}
