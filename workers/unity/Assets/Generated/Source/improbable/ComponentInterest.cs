// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable
{
    
    [global::System.Serializable]
    public struct ComponentInterest
    {
        public global::System.Collections.Generic.List<global::Improbable.ComponentInterest.Query> Queries;
    
        public ComponentInterest(global::System.Collections.Generic.List<global::Improbable.ComponentInterest.Query> queries)
        {
            Queries = queries;
        }
        public static class Serialization
        {
            public static void Serialize(ComponentInterest instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    foreach (var value in instance.Queries)
                    {
                        global::Improbable.ComponentInterest.Query.Serialization.Serialize(value, obj.AddObject(1));
                    }
                    
                }
            }
    
            public static ComponentInterest Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new ComponentInterest();
                {
                    instance.Queries = new global::System.Collections.Generic.List<global::Improbable.ComponentInterest.Query>();
                    var list = instance.Queries;
                    var listLength = obj.GetObjectCount(1);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(global::Improbable.ComponentInterest.Query.Serialization.Deserialize(obj.IndexObject(1, (uint) i)));
                    }
                    
                }
                return instance;
            }
        }
    
        
        [global::System.Serializable]
        public struct Query
        {
            public global::Improbable.ComponentInterest.QueryConstraint Constraint;
            public bool? FullSnapshotResult;
            public global::System.Collections.Generic.List<uint> ResultComponentId;
            public float? Frequency;
        
            public Query(global::Improbable.ComponentInterest.QueryConstraint constraint, bool? fullSnapshotResult, global::System.Collections.Generic.List<uint> resultComponentId, float? frequency)
            {
                Constraint = constraint;
                FullSnapshotResult = fullSnapshotResult;
                ResultComponentId = resultComponentId;
                Frequency = frequency;
            }
            public static class Serialization
            {
                public static void Serialize(Query instance, global::Improbable.Worker.CInterop.SchemaObject obj)
                {
                    {
                        global::Improbable.ComponentInterest.QueryConstraint.Serialization.Serialize(instance.Constraint, obj.AddObject(1));
                    }
                    {
                        if (instance.FullSnapshotResult.HasValue)
                        {
                            obj.AddBool(2, instance.FullSnapshotResult.Value);
                        }
                        
                    }
                    {
                        foreach (var value in instance.ResultComponentId)
                        {
                            obj.AddUint32(3, value);
                        }
                        
                    }
                    {
                        if (instance.Frequency.HasValue)
                        {
                            obj.AddFloat(4, instance.Frequency.Value);
                        }
                        
                    }
                }
        
                public static Query Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
                {
                    var instance = new Query();
                    {
                        instance.Constraint = global::Improbable.ComponentInterest.QueryConstraint.Serialization.Deserialize(obj.GetObject(1));
                    }
                    {
                        if (obj.GetBoolCount(2) == 1)
                        {
                            instance.FullSnapshotResult = new bool?(obj.GetBool(2));
                        }
                        
                    }
                    {
                        instance.ResultComponentId = new global::System.Collections.Generic.List<uint>();
                        var list = instance.ResultComponentId;
                        var listLength = obj.GetUint32Count(3);
                        for (var i = 0; i < listLength; i++)
                        {
                            list.Add(obj.IndexUint32(3, (uint) i));
                        }
                        
                    }
                    {
                        if (obj.GetFloatCount(4) == 1)
                        {
                            instance.Frequency = new float?(obj.GetFloat(4));
                        }
                        
                    }
                    return instance;
                }
            }
        }
        
    
        
        [global::System.Serializable]
        public struct QueryConstraint
        {
            public global::Improbable.ComponentInterest.SphereConstraint? SphereConstraint;
            public global::Improbable.ComponentInterest.CylinderConstraint? CylinderConstraint;
            public global::Improbable.ComponentInterest.BoxConstraint? BoxConstraint;
            public global::Improbable.ComponentInterest.RelativeSphereConstraint? RelativeSphereConstraint;
            public global::Improbable.ComponentInterest.RelativeCylinderConstraint? RelativeCylinderConstraint;
            public global::Improbable.ComponentInterest.RelativeBoxConstraint? RelativeBoxConstraint;
            public long? EntityIdConstraint;
            public uint? ComponentConstraint;
            public global::System.Collections.Generic.List<global::Improbable.ComponentInterest.QueryConstraint> AndConstraint;
            public global::System.Collections.Generic.List<global::Improbable.ComponentInterest.QueryConstraint> OrConstraint;
        
            public QueryConstraint(global::Improbable.ComponentInterest.SphereConstraint? sphereConstraint, global::Improbable.ComponentInterest.CylinderConstraint? cylinderConstraint, global::Improbable.ComponentInterest.BoxConstraint? boxConstraint, global::Improbable.ComponentInterest.RelativeSphereConstraint? relativeSphereConstraint, global::Improbable.ComponentInterest.RelativeCylinderConstraint? relativeCylinderConstraint, global::Improbable.ComponentInterest.RelativeBoxConstraint? relativeBoxConstraint, long? entityIdConstraint, uint? componentConstraint, global::System.Collections.Generic.List<global::Improbable.ComponentInterest.QueryConstraint> andConstraint, global::System.Collections.Generic.List<global::Improbable.ComponentInterest.QueryConstraint> orConstraint)
            {
                SphereConstraint = sphereConstraint;
                CylinderConstraint = cylinderConstraint;
                BoxConstraint = boxConstraint;
                RelativeSphereConstraint = relativeSphereConstraint;
                RelativeCylinderConstraint = relativeCylinderConstraint;
                RelativeBoxConstraint = relativeBoxConstraint;
                EntityIdConstraint = entityIdConstraint;
                ComponentConstraint = componentConstraint;
                AndConstraint = andConstraint;
                OrConstraint = orConstraint;
            }
            public static class Serialization
            {
                public static void Serialize(QueryConstraint instance, global::Improbable.Worker.CInterop.SchemaObject obj)
                {
                    {
                        if (instance.SphereConstraint.HasValue)
                        {
                            global::Improbable.ComponentInterest.SphereConstraint.Serialization.Serialize(instance.SphereConstraint.Value, obj.AddObject(1));
                        }
                        
                    }
                    {
                        if (instance.CylinderConstraint.HasValue)
                        {
                            global::Improbable.ComponentInterest.CylinderConstraint.Serialization.Serialize(instance.CylinderConstraint.Value, obj.AddObject(2));
                        }
                        
                    }
                    {
                        if (instance.BoxConstraint.HasValue)
                        {
                            global::Improbable.ComponentInterest.BoxConstraint.Serialization.Serialize(instance.BoxConstraint.Value, obj.AddObject(3));
                        }
                        
                    }
                    {
                        if (instance.RelativeSphereConstraint.HasValue)
                        {
                            global::Improbable.ComponentInterest.RelativeSphereConstraint.Serialization.Serialize(instance.RelativeSphereConstraint.Value, obj.AddObject(4));
                        }
                        
                    }
                    {
                        if (instance.RelativeCylinderConstraint.HasValue)
                        {
                            global::Improbable.ComponentInterest.RelativeCylinderConstraint.Serialization.Serialize(instance.RelativeCylinderConstraint.Value, obj.AddObject(5));
                        }
                        
                    }
                    {
                        if (instance.RelativeBoxConstraint.HasValue)
                        {
                            global::Improbable.ComponentInterest.RelativeBoxConstraint.Serialization.Serialize(instance.RelativeBoxConstraint.Value, obj.AddObject(6));
                        }
                        
                    }
                    {
                        if (instance.EntityIdConstraint.HasValue)
                        {
                            obj.AddInt64(7, instance.EntityIdConstraint.Value);
                        }
                        
                    }
                    {
                        if (instance.ComponentConstraint.HasValue)
                        {
                            obj.AddUint32(8, instance.ComponentConstraint.Value);
                        }
                        
                    }
                    {
                        foreach (var value in instance.AndConstraint)
                        {
                            global::Improbable.ComponentInterest.QueryConstraint.Serialization.Serialize(value, obj.AddObject(9));
                        }
                        
                    }
                    {
                        foreach (var value in instance.OrConstraint)
                        {
                            global::Improbable.ComponentInterest.QueryConstraint.Serialization.Serialize(value, obj.AddObject(10));
                        }
                        
                    }
                }
        
                public static QueryConstraint Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
                {
                    var instance = new QueryConstraint();
                    {
                        if (obj.GetObjectCount(1) == 1)
                        {
                            instance.SphereConstraint = new global::Improbable.ComponentInterest.SphereConstraint?(global::Improbable.ComponentInterest.SphereConstraint.Serialization.Deserialize(obj.GetObject(1)));
                        }
                        
                    }
                    {
                        if (obj.GetObjectCount(2) == 1)
                        {
                            instance.CylinderConstraint = new global::Improbable.ComponentInterest.CylinderConstraint?(global::Improbable.ComponentInterest.CylinderConstraint.Serialization.Deserialize(obj.GetObject(2)));
                        }
                        
                    }
                    {
                        if (obj.GetObjectCount(3) == 1)
                        {
                            instance.BoxConstraint = new global::Improbable.ComponentInterest.BoxConstraint?(global::Improbable.ComponentInterest.BoxConstraint.Serialization.Deserialize(obj.GetObject(3)));
                        }
                        
                    }
                    {
                        if (obj.GetObjectCount(4) == 1)
                        {
                            instance.RelativeSphereConstraint = new global::Improbable.ComponentInterest.RelativeSphereConstraint?(global::Improbable.ComponentInterest.RelativeSphereConstraint.Serialization.Deserialize(obj.GetObject(4)));
                        }
                        
                    }
                    {
                        if (obj.GetObjectCount(5) == 1)
                        {
                            instance.RelativeCylinderConstraint = new global::Improbable.ComponentInterest.RelativeCylinderConstraint?(global::Improbable.ComponentInterest.RelativeCylinderConstraint.Serialization.Deserialize(obj.GetObject(5)));
                        }
                        
                    }
                    {
                        if (obj.GetObjectCount(6) == 1)
                        {
                            instance.RelativeBoxConstraint = new global::Improbable.ComponentInterest.RelativeBoxConstraint?(global::Improbable.ComponentInterest.RelativeBoxConstraint.Serialization.Deserialize(obj.GetObject(6)));
                        }
                        
                    }
                    {
                        if (obj.GetInt64Count(7) == 1)
                        {
                            instance.EntityIdConstraint = new long?(obj.GetInt64(7));
                        }
                        
                    }
                    {
                        if (obj.GetUint32Count(8) == 1)
                        {
                            instance.ComponentConstraint = new uint?(obj.GetUint32(8));
                        }
                        
                    }
                    {
                        instance.AndConstraint = new global::System.Collections.Generic.List<global::Improbable.ComponentInterest.QueryConstraint>();
                        var list = instance.AndConstraint;
                        var listLength = obj.GetObjectCount(9);
                        for (var i = 0; i < listLength; i++)
                        {
                            list.Add(global::Improbable.ComponentInterest.QueryConstraint.Serialization.Deserialize(obj.IndexObject(9, (uint) i)));
                        }
                        
                    }
                    {
                        instance.OrConstraint = new global::System.Collections.Generic.List<global::Improbable.ComponentInterest.QueryConstraint>();
                        var list = instance.OrConstraint;
                        var listLength = obj.GetObjectCount(10);
                        for (var i = 0; i < listLength; i++)
                        {
                            list.Add(global::Improbable.ComponentInterest.QueryConstraint.Serialization.Deserialize(obj.IndexObject(10, (uint) i)));
                        }
                        
                    }
                    return instance;
                }
            }
        }
        
    
        
        [global::System.Serializable]
        public struct SphereConstraint
        {
            public global::Improbable.Coordinates Center;
            public double Radius;
        
            public SphereConstraint(global::Improbable.Coordinates center, double radius)
            {
                Center = center;
                Radius = radius;
            }
            public static class Serialization
            {
                public static void Serialize(SphereConstraint instance, global::Improbable.Worker.CInterop.SchemaObject obj)
                {
                    {
                        global::Improbable.Coordinates.Serialization.Serialize(instance.Center, obj.AddObject(1));
                    }
                    {
                        obj.AddDouble(2, instance.Radius);
                    }
                }
        
                public static SphereConstraint Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
                {
                    var instance = new SphereConstraint();
                    {
                        instance.Center = global::Improbable.Coordinates.Serialization.Deserialize(obj.GetObject(1));
                    }
                    {
                        instance.Radius = obj.GetDouble(2);
                    }
                    return instance;
                }
            }
        }
        
    
        
        [global::System.Serializable]
        public struct CylinderConstraint
        {
            public global::Improbable.Coordinates Center;
            public double Radius;
        
            public CylinderConstraint(global::Improbable.Coordinates center, double radius)
            {
                Center = center;
                Radius = radius;
            }
            public static class Serialization
            {
                public static void Serialize(CylinderConstraint instance, global::Improbable.Worker.CInterop.SchemaObject obj)
                {
                    {
                        global::Improbable.Coordinates.Serialization.Serialize(instance.Center, obj.AddObject(1));
                    }
                    {
                        obj.AddDouble(2, instance.Radius);
                    }
                }
        
                public static CylinderConstraint Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
                {
                    var instance = new CylinderConstraint();
                    {
                        instance.Center = global::Improbable.Coordinates.Serialization.Deserialize(obj.GetObject(1));
                    }
                    {
                        instance.Radius = obj.GetDouble(2);
                    }
                    return instance;
                }
            }
        }
        
    
        
        [global::System.Serializable]
        public struct BoxConstraint
        {
            public global::Improbable.Coordinates Center;
            public global::Improbable.EdgeLength EdgeLength;
        
            public BoxConstraint(global::Improbable.Coordinates center, global::Improbable.EdgeLength edgeLength)
            {
                Center = center;
                EdgeLength = edgeLength;
            }
            public static class Serialization
            {
                public static void Serialize(BoxConstraint instance, global::Improbable.Worker.CInterop.SchemaObject obj)
                {
                    {
                        global::Improbable.Coordinates.Serialization.Serialize(instance.Center, obj.AddObject(1));
                    }
                    {
                        global::Improbable.EdgeLength.Serialization.Serialize(instance.EdgeLength, obj.AddObject(2));
                    }
                }
        
                public static BoxConstraint Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
                {
                    var instance = new BoxConstraint();
                    {
                        instance.Center = global::Improbable.Coordinates.Serialization.Deserialize(obj.GetObject(1));
                    }
                    {
                        instance.EdgeLength = global::Improbable.EdgeLength.Serialization.Deserialize(obj.GetObject(2));
                    }
                    return instance;
                }
            }
        }
        
    
        
        [global::System.Serializable]
        public struct RelativeSphereConstraint
        {
            public double Radius;
        
            public RelativeSphereConstraint(double radius)
            {
                Radius = radius;
            }
            public static class Serialization
            {
                public static void Serialize(RelativeSphereConstraint instance, global::Improbable.Worker.CInterop.SchemaObject obj)
                {
                    {
                        obj.AddDouble(1, instance.Radius);
                    }
                }
        
                public static RelativeSphereConstraint Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
                {
                    var instance = new RelativeSphereConstraint();
                    {
                        instance.Radius = obj.GetDouble(1);
                    }
                    return instance;
                }
            }
        }
        
    
        
        [global::System.Serializable]
        public struct RelativeCylinderConstraint
        {
            public double Radius;
        
            public RelativeCylinderConstraint(double radius)
            {
                Radius = radius;
            }
            public static class Serialization
            {
                public static void Serialize(RelativeCylinderConstraint instance, global::Improbable.Worker.CInterop.SchemaObject obj)
                {
                    {
                        obj.AddDouble(1, instance.Radius);
                    }
                }
        
                public static RelativeCylinderConstraint Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
                {
                    var instance = new RelativeCylinderConstraint();
                    {
                        instance.Radius = obj.GetDouble(1);
                    }
                    return instance;
                }
            }
        }
        
    
        
        [global::System.Serializable]
        public struct RelativeBoxConstraint
        {
            public global::Improbable.EdgeLength EdgeLength;
        
            public RelativeBoxConstraint(global::Improbable.EdgeLength edgeLength)
            {
                EdgeLength = edgeLength;
            }
            public static class Serialization
            {
                public static void Serialize(RelativeBoxConstraint instance, global::Improbable.Worker.CInterop.SchemaObject obj)
                {
                    {
                        global::Improbable.EdgeLength.Serialization.Serialize(instance.EdgeLength, obj.AddObject(1));
                    }
                }
        
                public static RelativeBoxConstraint Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
                {
                    var instance = new RelativeBoxConstraint();
                    {
                        instance.EdgeLength = global::Improbable.EdgeLength.Serialization.Deserialize(obj.GetObject(1));
                    }
                    return instance;
                }
            }
        }
        
    }
    
}
