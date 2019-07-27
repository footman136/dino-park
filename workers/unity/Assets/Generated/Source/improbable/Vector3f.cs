// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable
{
    
    [global::System.Serializable]
    public struct Vector3f
    {
        public float X;
        public float Y;
        public float Z;
    
        public Vector3f(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    
        /// <summary>
        ///     A Vector3f with 0f in each dimension.
        /// </summary>
        public static readonly Vector3f Zero = new Vector3f(0f, 0f, 0f);
        
        /// <summary>
        ///     Override of the multiplication operator. Used for multiplying the Vector3f by a float scalar.
        /// </summary>
        public static Vector3f operator *(Vector3f vector3f, float scalar)
        {
            return new Vector3f(vector3f.X * scalar, vector3f.Y * scalar, vector3f.Z * scalar);
        }
        
        /// <summary>
        ///     Override of the multiplication operator. Used for multiplying the Vector3f by a float scalar.
        /// </summary>
        public static Vector3f operator *(float scalar, Vector3f vector3f)
        {
            return new Vector3f(vector3f.X * scalar, vector3f.Y * scalar, vector3f.Z * scalar);
        }
        
        /// <summary>
        ///     Override of the division operator. Used for dividing the Vector3f by a float scalar.
        /// </summary>
        public static Vector3f operator /(Vector3f vector3f, float scalar)
        {
            return new Vector3f(vector3f.X / scalar, vector3f.Y / scalar, vector3f.Z / scalar);
        }
        
        /// <summary>
        ///     Override of the addition operator. Used for adding two Vector3s.
        /// </summary>
        public static Vector3f operator +(Vector3f vector3f, Vector3f addVector3f)
        {
            return new Vector3f(vector3f.X + addVector3f.X, vector3f.Y + addVector3f.Y, vector3f.Z + addVector3f.Z);
        }
        
        /// <summary>
        ///     Override of the subtraction operator. Used for subtracting one Vector3f from another.
        /// </summary>
        public static Vector3f operator -(Vector3f vector3f, Vector3f subtractVector3f)
        {
            return new Vector3f(vector3f.X - subtractVector3f.X, vector3f.Y - subtractVector3f.Y, vector3f.Z - subtractVector3f.Z);
        }
        
        public static bool operator ==(Vector3f a, Vector3f b) => a.Equals(b);
        public static bool operator !=(Vector3f a, Vector3f b) => !a.Equals(b);
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
        
            return obj is Vector3f other && Equals(other);
        }
        
        public bool Equals(Vector3f other)
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
        ///     Returns the string representation of the Vector3f.
        /// </summary>
        public override string ToString()
        {
            return "Vector3f(" + X + ", " + Y + ", " + Z + ")";
        }
        
        /// <summary>
        ///     Converts a Unity vector to a Spatial Vector3f.
        /// </summary>
        public static Vector3f FromUnityVector(Vector3 unityVector) 
        {
            return new Vector3f(unityVector.x, unityVector.y, unityVector.z);
        }
        
        /// <summary>
        ///     Converts the Vector3f to a Unity Vector3.
        /// </summary>
        public Vector3 ToUnityVector()
        {
            return new Vector3(X, Y, Z);
        }
    
        public static class Serialization
        {
            public static void Serialize(Vector3f instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddFloat(1, instance.X);
                }
                {
                    obj.AddFloat(2, instance.Y);
                }
                {
                    obj.AddFloat(3, instance.Z);
                }
            }
    
            public static Vector3f Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new Vector3f();
                {
                    instance.X = obj.GetFloat(1);
                }
                {
                    instance.Y = obj.GetFloat(2);
                }
                {
                    instance.Z = obj.GetFloat(3);
                }
                return instance;
            }
        }
    }
    
}
