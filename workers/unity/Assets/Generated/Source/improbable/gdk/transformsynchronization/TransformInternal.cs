// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public partial class TransformInternal
    {
        public const uint ComponentId = 11000;

        public struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            public uint ComponentId => 11000;

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
                if (propertyIndex < 0 || propertyIndex >= 5)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 4]. " +
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
                if (propertyIndex < 0 || propertyIndex >= 5)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 4]. " +
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
                var componentDataSchema = new ComponentData(new SchemaComponentData(11000));
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields());

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }

            private global::Improbable.Gdk.TransformSynchronization.FixedPointVector3 location;

            public global::Improbable.Gdk.TransformSynchronization.FixedPointVector3 Location
            {
                get => location;
                set
                {
                    MarkDataDirty(0);
                    this.location = value;
                }
            }

            private global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion rotation;

            public global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion Rotation
            {
                get => rotation;
                set
                {
                    MarkDataDirty(1);
                    this.rotation = value;
                }
            }

            private global::Improbable.Gdk.TransformSynchronization.FixedPointVector3 velocity;

            public global::Improbable.Gdk.TransformSynchronization.FixedPointVector3 Velocity
            {
                get => velocity;
                set
                {
                    MarkDataDirty(2);
                    this.velocity = value;
                }
            }

            private uint physicsTick;

            public uint PhysicsTick
            {
                get => physicsTick;
                set
                {
                    MarkDataDirty(3);
                    this.physicsTick = value;
                }
            }

            private float ticksPerSecond;

            public float TicksPerSecond
            {
                get => ticksPerSecond;
                set
                {
                    MarkDataDirty(4);
                    this.ticksPerSecond = value;
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
            public uint ComponentId => 11000;

            public global::Improbable.Gdk.TransformSynchronization.FixedPointVector3 Location;
            public global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion Rotation;
            public global::Improbable.Gdk.TransformSynchronization.FixedPointVector3 Velocity;
            public uint PhysicsTick;
            public float TicksPerSecond;

            public Snapshot(global::Improbable.Gdk.TransformSynchronization.FixedPointVector3 location, global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion rotation, global::Improbable.Gdk.TransformSynchronization.FixedPointVector3 velocity, uint physicsTick, float ticksPerSecond)
            {
                Location = location;
                Rotation = rotation;
                Velocity = velocity;
                PhysicsTick = physicsTick;
                TicksPerSecond = ticksPerSecond;
            }
        }

        public static class Serialization
        {
            public static void SerializeComponent(global::Improbable.Gdk.TransformSynchronization.TransformInternal.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                {
                    global::Improbable.Gdk.TransformSynchronization.FixedPointVector3.Serialization.Serialize(component.Location, obj.AddObject(1));
                }
                {
                    global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion.Serialization.Serialize(component.Rotation, obj.AddObject(2));
                }
                {
                    global::Improbable.Gdk.TransformSynchronization.FixedPointVector3.Serialization.Serialize(component.Velocity, obj.AddObject(3));
                }
                {
                    obj.AddUint32(4, component.PhysicsTick);
                }
                {
                    obj.AddFloat(5, component.TicksPerSecond);
                }
            }

            public static void SerializeUpdate(global::Improbable.Gdk.TransformSynchronization.TransformInternal.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDataDirty(0))
                    {
                        global::Improbable.Gdk.TransformSynchronization.FixedPointVector3.Serialization.Serialize(component.Location, obj.AddObject(1));
                    }

                }
                {
                    if (component.IsDataDirty(1))
                    {
                        global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion.Serialization.Serialize(component.Rotation, obj.AddObject(2));
                    }

                }
                {
                    if (component.IsDataDirty(2))
                    {
                        global::Improbable.Gdk.TransformSynchronization.FixedPointVector3.Serialization.Serialize(component.Velocity, obj.AddObject(3));
                    }

                }
                {
                    if (component.IsDataDirty(3))
                    {
                        obj.AddUint32(4, component.PhysicsTick);
                    }

                }
                {
                    if (component.IsDataDirty(4))
                    {
                        obj.AddFloat(5, component.TicksPerSecond);
                    }

                }
            }

            public static void SerializeUpdate(global::Improbable.Gdk.TransformSynchronization.TransformInternal.Update update, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (update.Location.HasValue)
                    {
                        var field = update.Location.Value;
                        global::Improbable.Gdk.TransformSynchronization.FixedPointVector3.Serialization.Serialize(field, obj.AddObject(1));
                    }
                }
                {
                    if (update.Rotation.HasValue)
                    {
                        var field = update.Rotation.Value;
                        global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion.Serialization.Serialize(field, obj.AddObject(2));
                    }
                }
                {
                    if (update.Velocity.HasValue)
                    {
                        var field = update.Velocity.Value;
                        global::Improbable.Gdk.TransformSynchronization.FixedPointVector3.Serialization.Serialize(field, obj.AddObject(3));
                    }
                }
                {
                    if (update.PhysicsTick.HasValue)
                    {
                        var field = update.PhysicsTick.Value;
                        obj.AddUint32(4, field);
                    }
                }
                {
                    if (update.TicksPerSecond.HasValue)
                    {
                        var field = update.TicksPerSecond.Value;
                        obj.AddFloat(5, field);
                    }
                }
            }

            public static void SerializeSnapshot(global::Improbable.Gdk.TransformSynchronization.TransformInternal.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    global::Improbable.Gdk.TransformSynchronization.FixedPointVector3.Serialization.Serialize(snapshot.Location, obj.AddObject(1));
                }
                {
                    global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion.Serialization.Serialize(snapshot.Rotation, obj.AddObject(2));
                }
                {
                    global::Improbable.Gdk.TransformSynchronization.FixedPointVector3.Serialization.Serialize(snapshot.Velocity, obj.AddObject(3));
                }
                {
                    obj.AddUint32(4, snapshot.PhysicsTick);
                }
                {
                    obj.AddFloat(5, snapshot.TicksPerSecond);
                }
            }

            public static global::Improbable.Gdk.TransformSynchronization.TransformInternal.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new global::Improbable.Gdk.TransformSynchronization.TransformInternal.Component();

                {
                    component.Location = global::Improbable.Gdk.TransformSynchronization.FixedPointVector3.Serialization.Deserialize(obj.GetObject(1));
                }
                {
                    component.Rotation = global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion.Serialization.Deserialize(obj.GetObject(2));
                }
                {
                    component.Velocity = global::Improbable.Gdk.TransformSynchronization.FixedPointVector3.Serialization.Deserialize(obj.GetObject(3));
                }
                {
                    component.PhysicsTick = obj.GetUint32(4);
                }
                {
                    component.TicksPerSecond = obj.GetFloat(5);
                }
                return component;
            }

            public static global::Improbable.Gdk.TransformSynchronization.TransformInternal.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new global::Improbable.Gdk.TransformSynchronization.TransformInternal.Update();
                var obj = updateObj.GetFields();

                {
                    if (obj.GetObjectCount(1) == 1)
                    {
                        var value = global::Improbable.Gdk.TransformSynchronization.FixedPointVector3.Serialization.Deserialize(obj.GetObject(1));
                        update.Location = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.TransformSynchronization.FixedPointVector3>(value);
                    }
                    
                }
                {
                    if (obj.GetObjectCount(2) == 1)
                    {
                        var value = global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion.Serialization.Deserialize(obj.GetObject(2));
                        update.Rotation = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion>(value);
                    }
                    
                }
                {
                    if (obj.GetObjectCount(3) == 1)
                    {
                        var value = global::Improbable.Gdk.TransformSynchronization.FixedPointVector3.Serialization.Deserialize(obj.GetObject(3));
                        update.Velocity = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.TransformSynchronization.FixedPointVector3>(value);
                    }
                    
                }
                {
                    if (obj.GetUint32Count(4) == 1)
                    {
                        var value = obj.GetUint32(4);
                        update.PhysicsTick = new global::Improbable.Gdk.Core.Option<uint>(value);
                    }
                    
                }
                {
                    if (obj.GetFloatCount(5) == 1)
                    {
                        var value = obj.GetFloat(5);
                        update.TicksPerSecond = new global::Improbable.Gdk.Core.Option<float>(value);
                    }
                    
                }
                return update;
            }

            public static global::Improbable.Gdk.TransformSynchronization.TransformInternal.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentData data)
            {
                var update = new global::Improbable.Gdk.TransformSynchronization.TransformInternal.Update();
                var obj = data.GetFields();

                {
                    var value = global::Improbable.Gdk.TransformSynchronization.FixedPointVector3.Serialization.Deserialize(obj.GetObject(1));
                    update.Location = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.TransformSynchronization.FixedPointVector3>(value);
                    
                }
                {
                    var value = global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion.Serialization.Deserialize(obj.GetObject(2));
                    update.Rotation = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion>(value);
                    
                }
                {
                    var value = global::Improbable.Gdk.TransformSynchronization.FixedPointVector3.Serialization.Deserialize(obj.GetObject(3));
                    update.Velocity = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.TransformSynchronization.FixedPointVector3>(value);
                    
                }
                {
                    var value = obj.GetUint32(4);
                    update.PhysicsTick = new global::Improbable.Gdk.Core.Option<uint>(value);
                    
                }
                {
                    var value = obj.GetFloat(5);
                    update.TicksPerSecond = new global::Improbable.Gdk.Core.Option<float>(value);
                    
                }
                return update;
            }

            public static global::Improbable.Gdk.TransformSynchronization.TransformInternal.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var component = new global::Improbable.Gdk.TransformSynchronization.TransformInternal.Snapshot();

                {
                    component.Location = global::Improbable.Gdk.TransformSynchronization.FixedPointVector3.Serialization.Deserialize(obj.GetObject(1));
                }

                {
                    component.Rotation = global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion.Serialization.Deserialize(obj.GetObject(2));
                }

                {
                    component.Velocity = global::Improbable.Gdk.TransformSynchronization.FixedPointVector3.Serialization.Deserialize(obj.GetObject(3));
                }

                {
                    component.PhysicsTick = obj.GetUint32(4);
                }

                {
                    component.TicksPerSecond = obj.GetFloat(5);
                }

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.Gdk.TransformSynchronization.TransformInternal.Component component)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetObjectCount(1) == 1)
                    {
                        var value = global::Improbable.Gdk.TransformSynchronization.FixedPointVector3.Serialization.Deserialize(obj.GetObject(1));
                        component.Location = value;
                    }
                    
                }
                {
                    if (obj.GetObjectCount(2) == 1)
                    {
                        var value = global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion.Serialization.Deserialize(obj.GetObject(2));
                        component.Rotation = value;
                    }
                    
                }
                {
                    if (obj.GetObjectCount(3) == 1)
                    {
                        var value = global::Improbable.Gdk.TransformSynchronization.FixedPointVector3.Serialization.Deserialize(obj.GetObject(3));
                        component.Velocity = value;
                    }
                    
                }
                {
                    if (obj.GetUint32Count(4) == 1)
                    {
                        var value = obj.GetUint32(4);
                        component.PhysicsTick = value;
                    }
                    
                }
                {
                    if (obj.GetFloatCount(5) == 1)
                    {
                        var value = obj.GetFloat(5);
                        component.TicksPerSecond = value;
                    }
                    
                }
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.Gdk.TransformSynchronization.TransformInternal.Snapshot snapshot)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetObjectCount(1) == 1)
                    {
                        var value = global::Improbable.Gdk.TransformSynchronization.FixedPointVector3.Serialization.Deserialize(obj.GetObject(1));
                        snapshot.Location = value;
                    }
                    
                }
                {
                    if (obj.GetObjectCount(2) == 1)
                    {
                        var value = global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion.Serialization.Deserialize(obj.GetObject(2));
                        snapshot.Rotation = value;
                    }
                    
                }
                {
                    if (obj.GetObjectCount(3) == 1)
                    {
                        var value = global::Improbable.Gdk.TransformSynchronization.FixedPointVector3.Serialization.Deserialize(obj.GetObject(3));
                        snapshot.Velocity = value;
                    }
                    
                }
                {
                    if (obj.GetUint32Count(4) == 1)
                    {
                        var value = obj.GetUint32(4);
                        snapshot.PhysicsTick = value;
                    }
                    
                }
                {
                    if (obj.GetFloatCount(5) == 1)
                    {
                        var value = obj.GetFloat(5);
                        snapshot.TicksPerSecond = value;
                    }
                    
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

            public Option<global::Improbable.Gdk.TransformSynchronization.FixedPointVector3> Location;
            public Option<global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion> Rotation;
            public Option<global::Improbable.Gdk.TransformSynchronization.FixedPointVector3> Velocity;
            public Option<uint> PhysicsTick;
            public Option<float> TicksPerSecond;
        }

#if !DISABLE_REACTIVE_COMPONENTS
        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => global::Improbable.Gdk.TransformSynchronization.TransformInternal.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
#endif

        internal class TransformInternalDynamic : IDynamicInvokable
        {
            public uint ComponentId => TransformInternal.ComponentId;

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
                update.Location = new Option<global::Improbable.Gdk.TransformSynchronization.FixedPointVector3>(snapshot.Location);
                update.Rotation = new Option<global::Improbable.Gdk.TransformSynchronization.CompressedQuaternion>(snapshot.Rotation);
                update.Velocity = new Option<global::Improbable.Gdk.TransformSynchronization.FixedPointVector3>(snapshot.Velocity);
                update.PhysicsTick = new Option<uint>(snapshot.PhysicsTick);
                update.TicksPerSecond = new Option<float>(snapshot.TicksPerSecond);
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
