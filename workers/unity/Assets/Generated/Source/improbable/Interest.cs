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
    public partial class Interest
    {
        public const uint ComponentId = 58;

        public struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            public uint ComponentId => 58;

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
                if (propertyIndex < 0 || propertyIndex >= 1)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 0]. " +
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
                if (propertyIndex < 0 || propertyIndex >= 1)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 0]. " +
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
                var componentDataSchema = new ComponentData(new SchemaComponentData(58));
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields());

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }

            internal uint componentInterestHandle;

            public global::System.Collections.Generic.Dictionary<uint,global::Improbable.ComponentInterest> ComponentInterest
            {
                get => Improbable.Interest.ReferenceTypeProviders.ComponentInterestProvider.Get(componentInterestHandle);
                set
                {
                    MarkDataDirty(0);
                    Improbable.Interest.ReferenceTypeProviders.ComponentInterestProvider.Set(componentInterestHandle, value);
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
            public uint ComponentId => 58;

            public global::System.Collections.Generic.Dictionary<uint,global::Improbable.ComponentInterest> ComponentInterest;

            public Snapshot(global::System.Collections.Generic.Dictionary<uint,global::Improbable.ComponentInterest> componentInterest)
            {
                ComponentInterest = componentInterest;
            }
        }

        public static class Serialization
        {
            public static void SerializeComponent(global::Improbable.Interest.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                {
                    foreach (var keyValuePair in component.ComponentInterest)
                    {
                        var mapObj = obj.AddObject(1);
                        mapObj.AddUint32(1, keyValuePair.Key);
                        global::Improbable.ComponentInterest.Serialization.Serialize(keyValuePair.Value, mapObj.AddObject(2));
                    }
                    
                }
            }

            public static void SerializeUpdate(global::Improbable.Interest.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDataDirty(0))
                    {
                        foreach (var keyValuePair in component.ComponentInterest)
                        {
                            var mapObj = obj.AddObject(1);
                            mapObj.AddUint32(1, keyValuePair.Key);
                            global::Improbable.ComponentInterest.Serialization.Serialize(keyValuePair.Value, mapObj.AddObject(2));
                        }
                        
                    }

                    if (component.ComponentInterest.Count == 0)
                        {
                            updateObj.AddClearedField(1);
                        }
                        
                }
            }

            public static void SerializeUpdate(global::Improbable.Interest.Update update, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (update.ComponentInterest.HasValue)
                    {
                        var field = update.ComponentInterest.Value;
                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(1);
                            mapObj.AddUint32(1, keyValuePair.Key);
                            global::Improbable.ComponentInterest.Serialization.Serialize(keyValuePair.Value, mapObj.AddObject(2));
                        }
                        
                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(1);
                        }
                        
                    }
                }
            }

            public static void SerializeSnapshot(global::Improbable.Interest.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    foreach (var keyValuePair in snapshot.ComponentInterest)
                {
                    var mapObj = obj.AddObject(1);
                    mapObj.AddUint32(1, keyValuePair.Key);
                    global::Improbable.ComponentInterest.Serialization.Serialize(keyValuePair.Value, mapObj.AddObject(2));
                }
                
                }
            }

            public static global::Improbable.Interest.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new global::Improbable.Interest.Component();

                component.componentInterestHandle = Improbable.Interest.ReferenceTypeProviders.ComponentInterestProvider.Allocate(world);
                {
                    component.ComponentInterest = new global::System.Collections.Generic.Dictionary<uint,global::Improbable.ComponentInterest>();
                    var map = component.ComponentInterest;
                    var mapSize = obj.GetObjectCount(1);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = global::Improbable.ComponentInterest.Serialization.Deserialize(mapObj.GetObject(2));
                        map.Add(key, value);
                    }
                    
                }
                return component;
            }

            public static global::Improbable.Interest.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new global::Improbable.Interest.Update();
                var obj = updateObj.GetFields();

                var clearedFields = updateObj.GetClearedFields();

                {
                    var mapSize = obj.GetObjectCount(1);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 1;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.ComponentInterest = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<uint,global::Improbable.ComponentInterest>>(new global::System.Collections.Generic.Dictionary<uint,global::Improbable.ComponentInterest>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = global::Improbable.ComponentInterest.Serialization.Deserialize(mapObj.GetObject(2));
                        update.ComponentInterest.Value.Add(key, value);
                    }
                    
                }
                return update;
            }

            public static global::Improbable.Interest.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentData data)
            {
                var update = new global::Improbable.Interest.Update();
                var obj = data.GetFields();

                {
                    var mapSize = obj.GetObjectCount(1);
                    update.ComponentInterest = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<uint,global::Improbable.ComponentInterest>>(new global::System.Collections.Generic.Dictionary<uint,global::Improbable.ComponentInterest>());
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = global::Improbable.ComponentInterest.Serialization.Deserialize(mapObj.GetObject(2));
                        update.ComponentInterest.Value.Add(key, value);
                    }
                    
                }
                return update;
            }

            public static global::Improbable.Interest.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var component = new global::Improbable.Interest.Snapshot();

                {
                    component.ComponentInterest = new global::System.Collections.Generic.Dictionary<uint,global::Improbable.ComponentInterest>();
                    var map = component.ComponentInterest;
                    var mapSize = obj.GetObjectCount(1);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = global::Improbable.ComponentInterest.Serialization.Deserialize(mapObj.GetObject(2));
                        map.Add(key, value);
                    }
                    
                }

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.Interest.Component component)
            {
                var obj = updateObj.GetFields();

                var clearedFields = updateObj.GetClearedFields();

                {
                    var mapSize = obj.GetObjectCount(1);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 1;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.ComponentInterest.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = global::Improbable.ComponentInterest.Serialization.Deserialize(mapObj.GetObject(2));
                        component.ComponentInterest.Add(key, value);
                    }
                    
                }
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.Interest.Snapshot snapshot)
            {
                var obj = updateObj.GetFields();

                var clearedFields = updateObj.GetClearedFields();

                {
                    var mapSize = obj.GetObjectCount(1);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 1;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.ComponentInterest.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = global::Improbable.ComponentInterest.Serialization.Deserialize(mapObj.GetObject(2));
                        snapshot.ComponentInterest.Add(key, value);
                    }
                    
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

            public Option<global::System.Collections.Generic.Dictionary<uint,global::Improbable.ComponentInterest>> ComponentInterest;
        }

#if !DISABLE_REACTIVE_COMPONENTS
        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => global::Improbable.Interest.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
#endif

        internal class InterestDynamic : IDynamicInvokable
        {
            public uint ComponentId => Interest.ComponentId;

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
                update.ComponentInterest = new Option<global::System.Collections.Generic.Dictionary<uint,global::Improbable.ComponentInterest>>(snapshot.ComponentInterest);
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
