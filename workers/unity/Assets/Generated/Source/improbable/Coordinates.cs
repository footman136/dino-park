// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable
{
    
    [global::System.Serializable]
    public struct Coordinates
    {
        public double X;
        public double Y;
        public double Z;
    
        public Coordinates(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    
        public static readonly Coordinates Zero = new Coordinates(0, 0, 0);
        
        public static Coordinates operator -(Coordinates v1, Coordinates v2)
        {
            return new Coordinates(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        
        public static Coordinates operator +(Coordinates a, Coordinates b)
        {
            return new Coordinates(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        
        public static Coordinates operator *(Coordinates coords, float f)
        {
            return new Coordinates(coords.X * f, coords.Y * f, coords.Z * f);
        }
        
        public static Coordinates operator *(Coordinates coords, double d)
        {
            return new Coordinates(coords.X * d, coords.Y * d, coords.Z * d);
        }
        
        public static Coordinates operator /(Coordinates coords, float f)
        {
            return new Coordinates(coords.X / f, coords.Y / f, coords.Z / f);
        }
        
        public static Coordinates operator /(Coordinates coords, double d)
        {
            return new Coordinates(coords.X / d, coords.Y / d, coords.Z / d);
        }
        
        public static bool operator ==(Coordinates a, Coordinates b) => a.Equals(b);
        public static bool operator !=(Coordinates a, Coordinates b) => !a.Equals(b);
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
        
            return obj is Coordinates other && Equals(other);
        }
        
        public bool Equals(Coordinates other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }
        
        /// <summary>
        ///     Returns the string representation of the Coordinates.
        /// </summary>
        public override string ToString()
        {
            return "Coordinates(" + X + ", " + Y + ", " + Z + ")";
        }
        
        /// <summary>
        ///     Converts to a Unity Vector3.
        /// </summary>
        public Vector3 ToUnityVector()
        {
            return new Vector3((float) X, (float) Y, (float) Z);
        }
        
        /// <summary>
        ///     Converts to a Spatial Vector3d.
        /// </summary>
        public Vector3d ToSpatialVector3d()
        {
            return new Vector3d(X, Y, Z);
        }
    
        public static class Serialization
        {
            public static void Serialize(Coordinates instance, global::Improbable.Worker.CInterop.SchemaObject obj)
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
    
            public static Coordinates Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new Coordinates();
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
