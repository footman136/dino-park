// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Restricted
{
    public partial class Worker
    {
        public const uint ComponentId = 60;

        public struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            public uint ComponentId => 60;

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
                if (propertyIndex < 0 || propertyIndex >= 3)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 2]. " +
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
                if (propertyIndex < 0 || propertyIndex >= 3)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 2]. " +
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
                var componentDataSchema = new ComponentData(new SchemaComponentData(60));
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields());

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }

            internal uint workerIdHandle;

            public string WorkerId
            {
                get => Improbable.Restricted.Worker.ReferenceTypeProviders.WorkerIdProvider.Get(workerIdHandle);
                set
                {
                    MarkDataDirty(0);
                    Improbable.Restricted.Worker.ReferenceTypeProviders.WorkerIdProvider.Set(workerIdHandle, value);
                }
            }

            internal uint workerTypeHandle;

            public string WorkerType
            {
                get => Improbable.Restricted.Worker.ReferenceTypeProviders.WorkerTypeProvider.Get(workerTypeHandle);
                set
                {
                    MarkDataDirty(1);
                    Improbable.Restricted.Worker.ReferenceTypeProviders.WorkerTypeProvider.Set(workerTypeHandle, value);
                }
            }

            private global::Improbable.Restricted.Connection connection;

            public global::Improbable.Restricted.Connection Connection
            {
                get => connection;
                set
                {
                    MarkDataDirty(2);
                    this.connection = value;
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
            public uint ComponentId => 60;

            public string WorkerId;
            public string WorkerType;
            public global::Improbable.Restricted.Connection Connection;

            public Snapshot(string workerId, string workerType, global::Improbable.Restricted.Connection connection)
            {
                WorkerId = workerId;
                WorkerType = workerType;
                Connection = connection;
            }
        }

        public static class Serialization
        {
            public static void SerializeComponent(global::Improbable.Restricted.Worker.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                {
                    obj.AddString(1, component.WorkerId);
                }
                {
                    obj.AddString(2, component.WorkerType);
                }
                {
                    global::Improbable.Restricted.Connection.Serialization.Serialize(component.Connection, obj.AddObject(3));
                }
            }

            public static void SerializeUpdate(global::Improbable.Restricted.Worker.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDataDirty(0))
                    {
                        obj.AddString(1, component.WorkerId);
                    }

                }
                {
                    if (component.IsDataDirty(1))
                    {
                        obj.AddString(2, component.WorkerType);
                    }

                }
                {
                    if (component.IsDataDirty(2))
                    {
                        global::Improbable.Restricted.Connection.Serialization.Serialize(component.Connection, obj.AddObject(3));
                    }

                }
            }

            public static void SerializeUpdate(global::Improbable.Restricted.Worker.Update update, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (update.WorkerId.HasValue)
                    {
                        var field = update.WorkerId.Value;
                        obj.AddString(1, field);
                    }
                }
                {
                    if (update.WorkerType.HasValue)
                    {
                        var field = update.WorkerType.Value;
                        obj.AddString(2, field);
                    }
                }
                {
                    if (update.Connection.HasValue)
                    {
                        var field = update.Connection.Value;
                        global::Improbable.Restricted.Connection.Serialization.Serialize(field, obj.AddObject(3));
                    }
                }
            }

            public static void SerializeSnapshot(global::Improbable.Restricted.Worker.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddString(1, snapshot.WorkerId);
                }
                {
                    obj.AddString(2, snapshot.WorkerType);
                }
                {
                    global::Improbable.Restricted.Connection.Serialization.Serialize(snapshot.Connection, obj.AddObject(3));
                }
            }

            public static global::Improbable.Restricted.Worker.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new global::Improbable.Restricted.Worker.Component();

                component.workerIdHandle = Improbable.Restricted.Worker.ReferenceTypeProviders.WorkerIdProvider.Allocate(world);
                {
                    component.WorkerId = obj.GetString(1);
                }
                component.workerTypeHandle = Improbable.Restricted.Worker.ReferenceTypeProviders.WorkerTypeProvider.Allocate(world);
                {
                    component.WorkerType = obj.GetString(2);
                }
                {
                    component.Connection = global::Improbable.Restricted.Connection.Serialization.Deserialize(obj.GetObject(3));
                }
                return component;
            }

            public static global::Improbable.Restricted.Worker.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new global::Improbable.Restricted.Worker.Update();
                var obj = updateObj.GetFields();

                {
                    if (obj.GetStringCount(1) == 1)
                    {
                        var value = obj.GetString(1);
                        update.WorkerId = new global::Improbable.Gdk.Core.Option<string>(value);
                    }
                    
                }
                {
                    if (obj.GetStringCount(2) == 1)
                    {
                        var value = obj.GetString(2);
                        update.WorkerType = new global::Improbable.Gdk.Core.Option<string>(value);
                    }
                    
                }
                {
                    if (obj.GetObjectCount(3) == 1)
                    {
                        var value = global::Improbable.Restricted.Connection.Serialization.Deserialize(obj.GetObject(3));
                        update.Connection = new global::Improbable.Gdk.Core.Option<global::Improbable.Restricted.Connection>(value);
                    }
                    
                }
                return update;
            }

            public static global::Improbable.Restricted.Worker.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentData data)
            {
                var update = new global::Improbable.Restricted.Worker.Update();
                var obj = data.GetFields();

                {
                    var value = obj.GetString(1);
                    update.WorkerId = new global::Improbable.Gdk.Core.Option<string>(value);
                    
                }
                {
                    var value = obj.GetString(2);
                    update.WorkerType = new global::Improbable.Gdk.Core.Option<string>(value);
                    
                }
                {
                    var value = global::Improbable.Restricted.Connection.Serialization.Deserialize(obj.GetObject(3));
                    update.Connection = new global::Improbable.Gdk.Core.Option<global::Improbable.Restricted.Connection>(value);
                    
                }
                return update;
            }

            public static global::Improbable.Restricted.Worker.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var component = new global::Improbable.Restricted.Worker.Snapshot();

                {
                    component.WorkerId = obj.GetString(1);
                }

                {
                    component.WorkerType = obj.GetString(2);
                }

                {
                    component.Connection = global::Improbable.Restricted.Connection.Serialization.Deserialize(obj.GetObject(3));
                }

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.Restricted.Worker.Component component)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetStringCount(1) == 1)
                    {
                        var value = obj.GetString(1);
                        component.WorkerId = value;
                    }
                    
                }
                {
                    if (obj.GetStringCount(2) == 1)
                    {
                        var value = obj.GetString(2);
                        component.WorkerType = value;
                    }
                    
                }
                {
                    if (obj.GetObjectCount(3) == 1)
                    {
                        var value = global::Improbable.Restricted.Connection.Serialization.Deserialize(obj.GetObject(3));
                        component.Connection = value;
                    }
                    
                }
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.Restricted.Worker.Snapshot snapshot)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetStringCount(1) == 1)
                    {
                        var value = obj.GetString(1);
                        snapshot.WorkerId = value;
                    }
                    
                }
                {
                    if (obj.GetStringCount(2) == 1)
                    {
                        var value = obj.GetString(2);
                        snapshot.WorkerType = value;
                    }
                    
                }
                {
                    if (obj.GetObjectCount(3) == 1)
                    {
                        var value = global::Improbable.Restricted.Connection.Serialization.Deserialize(obj.GetObject(3));
                        snapshot.Connection = value;
                    }
                    
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

            public Option<string> WorkerId;
            public Option<string> WorkerType;
            public Option<global::Improbable.Restricted.Connection> Connection;
        }

#if !DISABLE_REACTIVE_COMPONENTS
        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => global::Improbable.Restricted.Worker.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
#endif

        internal class WorkerDynamic : IDynamicInvokable
        {
            public uint ComponentId => Worker.ComponentId;

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
                update.WorkerId = new Option<string>(snapshot.WorkerId);
                update.WorkerType = new Option<string>(snapshot.WorkerType);
                update.Connection = new Option<global::Improbable.Restricted.Connection>(snapshot.Connection);
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
