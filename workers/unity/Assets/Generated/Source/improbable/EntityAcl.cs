// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable
{
    public partial class EntityAcl
    {
        public const uint ComponentId = 50;

        public struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            public uint ComponentId => 50;

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
                if (propertyIndex < 0 || propertyIndex >= 2)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 1]. " +
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
                if (propertyIndex < 0 || propertyIndex >= 2)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 1]. " +
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
                var componentDataSchema = new ComponentData(new SchemaComponentData(50));
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields());

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }

            internal uint readAclHandle;

            public global::Improbable.WorkerRequirementSet ReadAcl
            {
                get => Improbable.EntityAcl.ReferenceTypeProviders.ReadAclProvider.Get(readAclHandle);
                set
                {
                    MarkDataDirty(0);
                    Improbable.EntityAcl.ReferenceTypeProviders.ReadAclProvider.Set(readAclHandle, value);
                }
            }

            internal uint componentWriteAclHandle;

            public global::System.Collections.Generic.Dictionary<uint,global::Improbable.WorkerRequirementSet> ComponentWriteAcl
            {
                get => Improbable.EntityAcl.ReferenceTypeProviders.ComponentWriteAclProvider.Get(componentWriteAclHandle);
                set
                {
                    MarkDataDirty(1);
                    Improbable.EntityAcl.ReferenceTypeProviders.ComponentWriteAclProvider.Set(componentWriteAclHandle, value);
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
            public uint ComponentId => 50;

            public global::Improbable.WorkerRequirementSet ReadAcl;
            public global::System.Collections.Generic.Dictionary<uint,global::Improbable.WorkerRequirementSet> ComponentWriteAcl;

            public Snapshot(global::Improbable.WorkerRequirementSet readAcl, global::System.Collections.Generic.Dictionary<uint,global::Improbable.WorkerRequirementSet> componentWriteAcl)
            {
                ReadAcl = readAcl;
                ComponentWriteAcl = componentWriteAcl;
            }
        }

        public static class Serialization
        {
            public static void SerializeComponent(global::Improbable.EntityAcl.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                {
                    global::Improbable.WorkerRequirementSet.Serialization.Serialize(component.ReadAcl, obj.AddObject(1));
                }
                {
                    foreach (var keyValuePair in component.ComponentWriteAcl)
                    {
                        var mapObj = obj.AddObject(2);
                        mapObj.AddUint32(1, keyValuePair.Key);
                        global::Improbable.WorkerRequirementSet.Serialization.Serialize(keyValuePair.Value, mapObj.AddObject(2));
                    }
                    
                }
            }

            public static void SerializeUpdate(global::Improbable.EntityAcl.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDataDirty(0))
                    {
                        global::Improbable.WorkerRequirementSet.Serialization.Serialize(component.ReadAcl, obj.AddObject(1));
                    }

                    
                }
                {
                    if (component.IsDataDirty(1))
                    {
                        foreach (var keyValuePair in component.ComponentWriteAcl)
                        {
                            var mapObj = obj.AddObject(2);
                            mapObj.AddUint32(1, keyValuePair.Key);
                            global::Improbable.WorkerRequirementSet.Serialization.Serialize(keyValuePair.Value, mapObj.AddObject(2));
                        }
                        
                    }

                    if (component.ComponentWriteAcl.Count == 0)
                        {
                            updateObj.AddClearedField(2);
                        }
                        
                }
            }

            public static void SerializeUpdate(global::Improbable.EntityAcl.Update update, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (update.ReadAcl.HasValue)
                    {
                        var field = update.ReadAcl.Value;
                        global::Improbable.WorkerRequirementSet.Serialization.Serialize(field, obj.AddObject(1));
                        
                    }
                }
                {
                    if (update.ComponentWriteAcl.HasValue)
                    {
                        var field = update.ComponentWriteAcl.Value;
                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(2);
                            mapObj.AddUint32(1, keyValuePair.Key);
                            global::Improbable.WorkerRequirementSet.Serialization.Serialize(keyValuePair.Value, mapObj.AddObject(2));
                        }
                        
                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(2);
                        }
                        
                    }
                }
            }

            public static void SerializeSnapshot(global::Improbable.EntityAcl.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    global::Improbable.WorkerRequirementSet.Serialization.Serialize(snapshot.ReadAcl, obj.AddObject(1));
                }
                {
                    foreach (var keyValuePair in snapshot.ComponentWriteAcl)
                {
                    var mapObj = obj.AddObject(2);
                    mapObj.AddUint32(1, keyValuePair.Key);
                    global::Improbable.WorkerRequirementSet.Serialization.Serialize(keyValuePair.Value, mapObj.AddObject(2));
                }
                
                }
            }

            public static global::Improbable.EntityAcl.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new global::Improbable.EntityAcl.Component();

                component.readAclHandle = Improbable.EntityAcl.ReferenceTypeProviders.ReadAclProvider.Allocate(world);
                {
                    component.ReadAcl = global::Improbable.WorkerRequirementSet.Serialization.Deserialize(obj.GetObject(1));
                }
                component.componentWriteAclHandle = Improbable.EntityAcl.ReferenceTypeProviders.ComponentWriteAclProvider.Allocate(world);
                {
                    component.ComponentWriteAcl = new global::System.Collections.Generic.Dictionary<uint,global::Improbable.WorkerRequirementSet>();
                    var map = component.ComponentWriteAcl;
                    var mapSize = obj.GetObjectCount(2);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = global::Improbable.WorkerRequirementSet.Serialization.Deserialize(mapObj.GetObject(2));
                        map.Add(key, value);
                    }
                    
                }
                return component;
            }

            public static global::Improbable.EntityAcl.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new global::Improbable.EntityAcl.Update();
                var obj = updateObj.GetFields();

                var clearedFields = updateObj.GetClearedFields();

                {
                    if (obj.GetObjectCount(1) == 1)
                    {
                        var value = global::Improbable.WorkerRequirementSet.Serialization.Deserialize(obj.GetObject(1));
                        update.ReadAcl = new global::Improbable.Gdk.Core.Option<global::Improbable.WorkerRequirementSet>(value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(2);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 2;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.ComponentWriteAcl = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<uint,global::Improbable.WorkerRequirementSet>>(new global::System.Collections.Generic.Dictionary<uint,global::Improbable.WorkerRequirementSet>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = global::Improbable.WorkerRequirementSet.Serialization.Deserialize(mapObj.GetObject(2));
                        update.ComponentWriteAcl.Value.Add(key, value);
                    }
                    
                }
                return update;
            }

            public static global::Improbable.EntityAcl.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentData data)
            {
                var update = new global::Improbable.EntityAcl.Update();
                var obj = data.GetFields();

                {
                    var value = global::Improbable.WorkerRequirementSet.Serialization.Deserialize(obj.GetObject(1));
                    update.ReadAcl = new global::Improbable.Gdk.Core.Option<global::Improbable.WorkerRequirementSet>(value);
                    
                }
                {
                    var mapSize = obj.GetObjectCount(2);
                    update.ComponentWriteAcl = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<uint,global::Improbable.WorkerRequirementSet>>(new global::System.Collections.Generic.Dictionary<uint,global::Improbable.WorkerRequirementSet>());
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = global::Improbable.WorkerRequirementSet.Serialization.Deserialize(mapObj.GetObject(2));
                        update.ComponentWriteAcl.Value.Add(key, value);
                    }
                    
                }
                return update;
            }

            public static global::Improbable.EntityAcl.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var component = new global::Improbable.EntityAcl.Snapshot();

                {
                    component.ReadAcl = global::Improbable.WorkerRequirementSet.Serialization.Deserialize(obj.GetObject(1));
                }

                {
                    component.ComponentWriteAcl = new global::System.Collections.Generic.Dictionary<uint,global::Improbable.WorkerRequirementSet>();
                    var map = component.ComponentWriteAcl;
                    var mapSize = obj.GetObjectCount(2);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = global::Improbable.WorkerRequirementSet.Serialization.Deserialize(mapObj.GetObject(2));
                        map.Add(key, value);
                    }
                    
                }

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.EntityAcl.Component component)
            {
                var obj = updateObj.GetFields();

                var clearedFields = updateObj.GetClearedFields();

                {
                    if (obj.GetObjectCount(1) == 1)
                    {
                        var value = global::Improbable.WorkerRequirementSet.Serialization.Deserialize(obj.GetObject(1));
                        component.ReadAcl = value;
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(2);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 2;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.ComponentWriteAcl.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = global::Improbable.WorkerRequirementSet.Serialization.Deserialize(mapObj.GetObject(2));
                        component.ComponentWriteAcl.Add(key, value);
                    }
                    
                }
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.EntityAcl.Snapshot snapshot)
            {
                var obj = updateObj.GetFields();

                var clearedFields = updateObj.GetClearedFields();

                {
                    if (obj.GetObjectCount(1) == 1)
                    {
                        var value = global::Improbable.WorkerRequirementSet.Serialization.Deserialize(obj.GetObject(1));
                        snapshot.ReadAcl = value;
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(2);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 2;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.ComponentWriteAcl.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = global::Improbable.WorkerRequirementSet.Serialization.Deserialize(mapObj.GetObject(2));
                        snapshot.ComponentWriteAcl.Add(key, value);
                    }
                    
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

            public Option<global::Improbable.WorkerRequirementSet> ReadAcl;
            public Option<global::System.Collections.Generic.Dictionary<uint,global::Improbable.WorkerRequirementSet>> ComponentWriteAcl;
        }

#if !DISABLE_REACTIVE_COMPONENTS
        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => global::Improbable.EntityAcl.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
#endif

        internal class EntityAclDynamic : IDynamicInvokable
        {
            public uint ComponentId => EntityAcl.ComponentId;

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
                update.ReadAcl = new Option<global::Improbable.WorkerRequirementSet>(snapshot.ReadAcl);
                update.ComponentWriteAcl = new Option<global::System.Collections.Generic.Dictionary<uint,global::Improbable.WorkerRequirementSet>>(snapshot.ComponentWriteAcl);
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
