// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable
{
    
    [global::System.Serializable]
    public struct EdgeLength
    {
        public double X;
        public double Y;
        public double Z;
    
        public EdgeLength(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public static class Serialization
        {
            public static void Serialize(EdgeLength instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddDouble(1, instance.X);
                }
                {
                    obj.AddDouble(2, instance.Y);
                }
                {
                    obj.AddDouble(3, instance.Z);
                }
            }
    
            public static EdgeLength Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new EdgeLength();
                {
                    instance.X = obj.GetDouble(1);
                }
                {
                    instance.Y = obj.GetDouble(2);
                }
                {
                    instance.Z = obj.GetDouble(3);
                }
                return instance;
            }
        }
    }
    
}
