// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Dinopark.Npc
{
    public partial class DinoAttrs
    {
        public const uint ComponentId = 1872;

        public struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            public uint ComponentId => 1872;

            // Bit masks for tracking which component properties were changed locally and need to be synced.
            // Each byte tracks 8 component properties.
            private byte dirtyBits0;

            public bool IsDataDirty()
            {
                var isDataDirty = false;
                isDataDirty |= (dirtyBits0 != 0x0);
                return isDataDirty;
            }

            /*
            The propertyIndex argument counts up from 0 in the order defined in your schema component.
            It is not the schema field number itself. For example:
            component MyComponent
            {
                id = 1337;
                bool val_a = 1;
                bool val_b = 3;
            }
            In that case, val_a corresponds to propertyIndex 0 and val_b corresponds to propertyIndex 1 in this method.
            This method throws an InvalidOperationException in case your component doesn't contain properties.
            */
            public bool IsDataDirty(int propertyIndex)
            {
                if (propertyIndex < 0 || propertyIndex >= 6)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 5]. " +
                        "Unless you are using custom component replication code, this is most likely caused by a code generation bug. " +
                        "Please contact SpatialOS support if you encounter this issue.");
                }

                // Retrieve the dirtyBits[0-n] field that tracks this property.
                var dirtyBitsByteIndex = propertyIndex / 8;
                switch (dirtyBitsByteIndex)
                {
                    case 0:
                        return (dirtyBits0 & (0x1 << propertyIndex % 8)) != 0x0;
                }

                return false;
            }

            // Like the IsDataDirty() method above, the propertyIndex arguments starts counting from 0.
            // This method throws an InvalidOperationException in case your component doesn't contain properties.
            public void MarkDataDirty(int propertyIndex)
            {
                if (propertyIndex < 0 || propertyIndex >= 6)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 5]. " +
                        "Unless you are using custom component replication code, this is most likely caused by a code generation bug. " +
                        "Please contact SpatialOS support if you encounter this issue.");
                }

                // Retrieve the dirtyBits[0-n] field that tracks this property.
                var dirtyBitsByteIndex = propertyIndex / 8;
                switch (dirtyBitsByteIndex)
                {
                    case 0:
                        dirtyBits0 |= (byte) (0x1 << propertyIndex % 8);
                        break;
                }
            }

            public void MarkDataClean()
            {
                dirtyBits0 = 0x0;
            }

            public Snapshot ToComponentSnapshot(global::Unity.Entities.World world)
            {
                var componentDataSchema = new ComponentData(new SchemaComponentData(1872));
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields());

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }

            private bool isDead;

            public bool IsDead
            {
                get => isDead;
                set
                {
                    MarkDataDirty(0);
                    this.isDead = value;
                }
            }

            private float currentFood;

            public float CurrentFood
            {
                get => currentFood;
                set
                {
                    MarkDataDirty(1);
                    this.currentFood = value;
                }
            }

            private float originalScent;

            public float OriginalScent
            {
                get => originalScent;
                set
                {
                    MarkDataDirty(2);
                    this.originalScent = value;
                }
            }

            private float originalAgression;

            public float OriginalAgression
            {
                get => originalAgression;
                set
                {
                    MarkDataDirty(3);
                    this.originalAgression = value;
                }
            }

            private int originalDominance;

            public int OriginalDominance
            {
                get => originalDominance;
                set
                {
                    MarkDataDirty(4);
                    this.originalDominance = value;
                }
            }

            private global::Improbable.Vector3f originPosotion;

            public global::Improbable.Vector3f OriginPosotion
            {
                get => originPosotion;
                set
                {
                    MarkDataDirty(5);
                    this.originPosotion = value;
                }
            }
        }

        public struct ComponentAuthority : ISharedComponentData, IEquatable<ComponentAuthority>
        {
            public bool HasAuthority;

            public ComponentAuthority(bool hasAuthority)
            {
                HasAuthority = hasAuthority;
            }

            // todo think about whether any of this is necessary
            // Unity does a bitwise equality check so this is just for users reading the struct
            public static readonly ComponentAuthority NotAuthoritative = new ComponentAuthority(false);
            public static readonly ComponentAuthority Authoritative = new ComponentAuthority(true);

            public bool Equals(ComponentAuthority other)
            {
                return this == other;
            }

            public override bool Equals(object obj)
            {
                return obj is ComponentAuthority auth && this == auth;
            }

            public override int GetHashCode()
            {
                return HasAuthority.GetHashCode();
            }

            public static bool operator ==(ComponentAuthority a, ComponentAuthority b)
            {
                return a.HasAuthority == b.HasAuthority;
            }

            public static bool operator !=(ComponentAuthority a, ComponentAuthority b)
            {
                return !(a == b);
            }
        }

        [global::System.Serializable]
        public struct Snapshot : ISpatialComponentSnapshot
        {
            public uint ComponentId => 1872;

            public bool IsDead;
            public float CurrentFood;
            public float OriginalScent;
            public float OriginalAgression;
            public int OriginalDominance;
            public global::Improbable.Vector3f OriginPosotion;

            public Snapshot(bool isDead, float currentFood, float originalScent, float originalAgression, int originalDominance, global::Improbable.Vector3f originPosotion)
            {
                IsDead = isDead;
                CurrentFood = currentFood;
                OriginalScent = originalScent;
                OriginalAgression = originalAgression;
                OriginalDominance = originalDominance;
                OriginPosotion = originPosotion;
            }
        }

        public static class Serialization
        {
            public static void SerializeComponent(global::Dinopark.Npc.DinoAttrs.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                {
                    obj.AddBool(1, component.IsDead);
                }
                {
                    obj.AddFloat(2, component.CurrentFood);
                }
                {
                    obj.AddFloat(4, component.OriginalScent);
                }
                {
                    obj.AddFloat(5, component.OriginalAgression);
                }
                {
                    obj.AddInt32(6, component.OriginalDominance);
                }
                {
                    global::Improbable.Vector3f.Serialization.Serialize(component.OriginPosotion, obj.AddObject(7));
                }
            }

            public static void SerializeUpdate(global::Dinopark.Npc.DinoAttrs.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDataDirty(0))
                    {
                        obj.AddBool(1, component.IsDead);
                    }

                }
                {
                    if (component.IsDataDirty(1))
                    {
                        obj.AddFloat(2, component.CurrentFood);
                    }

                }
                {
                    if (component.IsDataDirty(2))
                    {
                        obj.AddFloat(4, component.OriginalScent);
                    }

                }
                {
                    if (component.IsDataDirty(3))
                    {
                        obj.AddFloat(5, component.OriginalAgression);
                    }

                }
                {
                    if (component.IsDataDirty(4))
                    {
                        obj.AddInt32(6, component.OriginalDominance);
                    }

                }
                {
                    if (component.IsDataDirty(5))
                    {
                        global::Improbable.Vector3f.Serialization.Serialize(component.OriginPosotion, obj.AddObject(7));
                    }

                }
            }

            public static void SerializeUpdate(global::Dinopark.Npc.DinoAttrs.Update update, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (update.IsDead.HasValue)
                    {
                        var field = update.IsDead.Value;
                        obj.AddBool(1, field);
                    }
                }
                {
                    if (update.CurrentFood.HasValue)
                    {
                        var field = update.CurrentFood.Value;
                        obj.AddFloat(2, field);
                    }
                }
                {
                    if (update.OriginalScent.HasValue)
                    {
                        var field = update.OriginalScent.Value;
                        obj.AddFloat(4, field);
                    }
                }
                {
                    if (update.OriginalAgression.HasValue)
                    {
                        var field = update.OriginalAgression.Value;
                        obj.AddFloat(5, field);
                    }
                }
                {
                    if (update.OriginalDominance.HasValue)
                    {
                        var field = update.OriginalDominance.Value;
                        obj.AddInt32(6, field);
                    }
                }
                {
                    if (update.OriginPosotion.HasValue)
                    {
                        var field = update.OriginPosotion.Value;
                        global::Improbable.Vector3f.Serialization.Serialize(field, obj.AddObject(7));
                    }
                }
            }

            public static void SerializeSnapshot(global::Dinopark.Npc.DinoAttrs.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddBool(1, snapshot.IsDead);
                }
                {
                    obj.AddFloat(2, snapshot.CurrentFood);
                }
                {
                    obj.AddFloat(4, snapshot.OriginalScent);
                }
                {
                    obj.AddFloat(5, snapshot.OriginalAgression);
                }
                {
                    obj.AddInt32(6, snapshot.OriginalDominance);
                }
                {
                    global::Improbable.Vector3f.Serialization.Serialize(snapshot.OriginPosotion, obj.AddObject(7));
                }
            }

            public static global::Dinopark.Npc.DinoAttrs.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new global::Dinopark.Npc.DinoAttrs.Component();

                {
                    component.IsDead = obj.GetBool(1);
                }
                {
                    component.CurrentFood = obj.GetFloat(2);
                }
                {
                    component.OriginalScent = obj.GetFloat(4);
                }
                {
                    component.OriginalAgression = obj.GetFloat(5);
                }
                {
                    component.OriginalDominance = obj.GetInt32(6);
                }
                {
                    component.OriginPosotion = global::Improbable.Vector3f.Serialization.Deserialize(obj.GetObject(7));
                }
                return component;
            }

            public static global::Dinopark.Npc.DinoAttrs.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new global::Dinopark.Npc.DinoAttrs.Update();
                var obj = updateObj.GetFields();

                {
                    if (obj.GetBoolCount(1) == 1)
                    {
                        var value = obj.GetBool(1);
                        update.IsDead = new global::Improbable.Gdk.Core.Option<bool>(value);
                    }
                    
                }
                {
                    if (obj.GetFloatCount(2) == 1)
                    {
                        var value = obj.GetFloat(2);
                        update.CurrentFood = new global::Improbable.Gdk.Core.Option<float>(value);
                    }
                    
                }
                {
                    if (obj.GetFloatCount(4) == 1)
                    {
                        var value = obj.GetFloat(4);
                        update.OriginalScent = new global::Improbable.Gdk.Core.Option<float>(value);
                    }
                    
                }
                {
                    if (obj.GetFloatCount(5) == 1)
                    {
                        var value = obj.GetFloat(5);
                        update.OriginalAgression = new global::Improbable.Gdk.Core.Option<float>(value);
                    }
                    
                }
                {
                    if (obj.GetInt32Count(6) == 1)
                    {
                        var value = obj.GetInt32(6);
                        update.OriginalDominance = new global::Improbable.Gdk.Core.Option<int>(value);
                    }
                    
                }
                {
                    if (obj.GetObjectCount(7) == 1)
                    {
                        var value = global::Improbable.Vector3f.Serialization.Deserialize(obj.GetObject(7));
                        update.OriginPosotion = new global::Improbable.Gdk.Core.Option<global::Improbable.Vector3f>(value);
                    }
                    
                }
                return update;
            }

            public static global::Dinopark.Npc.DinoAttrs.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentData data)
            {
                var update = new global::Dinopark.Npc.DinoAttrs.Update();
                var obj = data.GetFields();

                {
                    var value = obj.GetBool(1);
                    update.IsDead = new global::Improbable.Gdk.Core.Option<bool>(value);
                    
                }
                {
                    var value = obj.GetFloat(2);
                    update.CurrentFood = new global::Improbable.Gdk.Core.Option<float>(value);
                    
                }
                {
                    var value = obj.GetFloat(4);
                    update.OriginalScent = new global::Improbable.Gdk.Core.Option<float>(value);
                    
                }
                {
                    var value = obj.GetFloat(5);
                    update.OriginalAgression = new global::Improbable.Gdk.Core.Option<float>(value);
                    
                }
                {
                    var value = obj.GetInt32(6);
                    update.OriginalDominance = new global::Improbable.Gdk.Core.Option<int>(value);
                    
                }
                {
                    var value = global::Improbable.Vector3f.Serialization.Deserialize(obj.GetObject(7));
                    update.OriginPosotion = new global::Improbable.Gdk.Core.Option<global::Improbable.Vector3f>(value);
                    
                }
                return update;
            }

            public static global::Dinopark.Npc.DinoAttrs.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var component = new global::Dinopark.Npc.DinoAttrs.Snapshot();

                {
                    component.IsDead = obj.GetBool(1);
                }

                {
                    component.CurrentFood = obj.GetFloat(2);
                }

                {
                    component.OriginalScent = obj.GetFloat(4);
                }

                {
                    component.OriginalAgression = obj.GetFloat(5);
                }

                {
                    component.OriginalDominance = obj.GetInt32(6);
                }

                {
                    component.OriginPosotion = global::Improbable.Vector3f.Serialization.Deserialize(obj.GetObject(7));
                }

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Dinopark.Npc.DinoAttrs.Component component)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetBoolCount(1) == 1)
                    {
                        var value = obj.GetBool(1);
                        component.IsDead = value;
                    }
                    
                }
                {
                    if (obj.GetFloatCount(2) == 1)
                    {
                        var value = obj.GetFloat(2);
                        component.CurrentFood = value;
                    }
                    
                }
                {
                    if (obj.GetFloatCount(4) == 1)
                    {
                        var value = obj.GetFloat(4);
                        component.OriginalScent = value;
                    }
                    
                }
                {
                    if (obj.GetFloatCount(5) == 1)
                    {
                        var value = obj.GetFloat(5);
                        component.OriginalAgression = value;
                    }
                    
                }
                {
                    if (obj.GetInt32Count(6) == 1)
                    {
                        var value = obj.GetInt32(6);
                        component.OriginalDominance = value;
                    }
                    
                }
                {
                    if (obj.GetObjectCount(7) == 1)
                    {
                        var value = global::Improbable.Vector3f.Serialization.Deserialize(obj.GetObject(7));
                        component.OriginPosotion = value;
                    }
                    
                }
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Dinopark.Npc.DinoAttrs.Snapshot snapshot)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetBoolCount(1) == 1)
                    {
                        var value = obj.GetBool(1);
                        snapshot.IsDead = value;
                    }
                    
                }
                {
                    if (obj.GetFloatCount(2) == 1)
                    {
                        var value = obj.GetFloat(2);
                        snapshot.CurrentFood = value;
                    }
                    
                }
                {
                    if (obj.GetFloatCount(4) == 1)
                    {
                        var value = obj.GetFloat(4);
                        snapshot.OriginalScent = value;
                    }
                    
                }
                {
                    if (obj.GetFloatCount(5) == 1)
                    {
                        var value = obj.GetFloat(5);
                        snapshot.OriginalAgression = value;
                    }
                    
                }
                {
                    if (obj.GetInt32Count(6) == 1)
                    {
                        var value = obj.GetInt32(6);
                        snapshot.OriginalDominance = value;
                    }
                    
                }
                {
                    if (obj.GetObjectCount(7) == 1)
                    {
                        var value = global::Improbable.Vector3f.Serialization.Deserialize(obj.GetObject(7));
                        snapshot.OriginPosotion = value;
                    }
                    
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

            public Option<bool> IsDead;
            public Option<float> CurrentFood;
            public Option<float> OriginalScent;
            public Option<float> OriginalAgression;
            public Option<int> OriginalDominance;
            public Option<global::Improbable.Vector3f> OriginPosotion;
        }

#if !DISABLE_REACTIVE_COMPONENTS
        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => global::Dinopark.Npc.DinoAttrs.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
#endif

        internal class DinoAttrsDynamic : IDynamicInvokable
        {
            public uint ComponentId => DinoAttrs.ComponentId;

            private static Component DeserializeData(ComponentData data, World world)
            {
                var schemaDataOpt = data.SchemaData;
                if (!schemaDataOpt.HasValue)
                {
                    throw new ArgumentException($"Can not deserialize an empty {nameof(ComponentData)}");
                }

                return Serialization.Deserialize(schemaDataOpt.Value.GetFields(), world);
            }

            private static Update DeserializeUpdate(ComponentUpdate update, World world)
            {
                var schemaDataOpt = update.SchemaData;
                if (!schemaDataOpt.HasValue)
                {
                    throw new ArgumentException($"Can not deserialize an empty {nameof(ComponentUpdate)}");
                }

                return Serialization.DeserializeUpdate(schemaDataOpt.Value);
            }

            private static Snapshot DeserializeSnapshot(ComponentData snapshot)
            {
                var schemaDataOpt = snapshot.SchemaData;
                if (!schemaDataOpt.HasValue)
                {
                    throw new ArgumentException($"Can not deserialize an empty {nameof(ComponentData)}");
                }

                return Serialization.DeserializeSnapshot(schemaDataOpt.Value.GetFields());
            }

            private static void SerializeSnapshot(Snapshot snapshot, ComponentData data)
            {
                var schemaDataOpt = data.SchemaData;
                if (!schemaDataOpt.HasValue)
                {
                    throw new ArgumentException($"Can not serialise an empty {nameof(ComponentData)}");
                }

                Serialization.SerializeSnapshot(snapshot, data.SchemaData.Value.GetFields());
            }

            private static Update SnapshotToUpdate(in Snapshot snapshot)
            {
                var update = new Update();
                update.IsDead = new Option<bool>(snapshot.IsDead);
                update.CurrentFood = new Option<float>(snapshot.CurrentFood);
                update.OriginalScent = new Option<float>(snapshot.OriginalScent);
                update.OriginalAgression = new Option<float>(snapshot.OriginalAgression);
                update.OriginalDominance = new Option<int>(snapshot.OriginalDominance);
                update.OriginPosotion = new Option<global::Improbable.Vector3f>(snapshot.OriginPosotion);
                return update;
            }

            public void InvokeHandler(Dynamic.IHandler handler)
            {
                handler.Accept<Component, Update>(ComponentId, DeserializeData, DeserializeUpdate);
            }

            public void InvokeSnapshotHandler(DynamicSnapshot.ISnapshotHandler handler)
            {
                handler.Accept<Snapshot>(ComponentId, DeserializeSnapshot, SerializeSnapshot);
            }

            public void InvokeConvertHandler(DynamicConverter.IConverterHandler handler)
            {
                handler.Accept<Snapshot, Update>(ComponentId, SnapshotToUpdate);
            }
        }
    }
}
