// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable
{
    
    [global::System.Serializable]
    public struct Vector3d
    {
        public double X;
        public double Y;
        public double Z;
    
        public Vector3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    
        /// <summary>
        ///     A Vector3d with 0 in each dimension.
        /// </summary>
        public static readonly Vector3d Zero = new Vector3d(0, 0, 0);
        
        /// <summary>
        ///     Override of the multiplication operator. Used for multiplying the Vector3d by a float scalar.
        /// </summary>
        public static Vector3d operator *(Vector3d vector3d, double scalar)
        {
            return new Vector3d(vector3d.X * scalar, vector3d.Y * scalar, vector3d.Z * scalar);
        }
        
        /// <summary>
        ///     Override of the multiplication operator. Used for multiplying the Vector3d by a float scalar.
        /// </summary>
        public static Vector3d operator *(double scalar, Vector3d vector3d)
        {
            return new Vector3d(vector3d.X * scalar, vector3d.Y * scalar, vector3d.Z * scalar);
        }
        
        /// <summary>
        ///     Override of the division operator. Used for dividing the Vector3d by a float scalar.
        /// </summary>
        public static Vector3d operator /(Vector3d vector3d, double scalar)
        {
            return new Vector3d(vector3d.X / scalar, vector3d.Y / scalar, vector3d.Z / scalar);
        }
        
        /// <summary>
        ///     Override of the addition operator. Used for adding two Vector3s.
        /// </summary>
        public static Vector3d operator +(Vector3d vector3d, Vector3d addvector3d)
        {
            return new Vector3d(vector3d.X + addvector3d.X, vector3d.Y + addvector3d.Y, vector3d.Z + addvector3d.Z);
        }
        
        /// <summary>
        ///     Override of the subtraction operator. Used for subtracting one Vector3d from another.
        /// </summary>
        public static Vector3d operator -(Vector3d vector3d, Vector3d subtractVector3d)
        {
            return new Vector3d(vector3d.X - subtractVector3d.X, vector3d.Y - subtractVector3d.Y, vector3d.Z - subtractVector3d.Z);
        }
        
        public static bool operator ==(Vector3d a, Vector3d b) => a.Equals(b);
        public static bool operator !=(Vector3d a, Vector3d b) => !a.Equals(b);
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
        
            return obj is Vector3d other && Equals(other);
        }
        
        public bool Equals(Vector3d other)
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
        ///     Returns the string representation of the Vector3d.
        /// </summary>
        public override string ToString()
        {
            return "Vector3d(" + X + ", " + Y + ", " + Z + ")";
        }
        
        /// <summary>
        ///     Converts the Vector3d to a Unity Vector3.
        /// </summary>
        public Vector3 ToUnityVector()
        {
            return new Vector3((float) X, (float) Y, (float) Z);
        }
    
        public static class Serialization
        {
            public static void Serialize(Vector3d instance, global::Improbable.Worker.CInterop.SchemaObject obj)
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
    
            public static Vector3d Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new Vector3d();
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
