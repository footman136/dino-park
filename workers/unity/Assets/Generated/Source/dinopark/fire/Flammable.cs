// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Dinopark.Fire
{
    public partial class Flammable
    {
        public const uint ComponentId = 1400;

        public struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            public uint ComponentId => 1400;

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
                var componentDataSchema = new ComponentData(new SchemaComponentData(1400));
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields());

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }

            private bool isOnFire;

            public bool IsOnFire
            {
                get => isOnFire;
                set
                {
                    MarkDataDirty(0);
                    this.isOnFire = value;
                }
            }

            private bool canBeIgnited;

            public bool CanBeIgnited
            {
                get => canBeIgnited;
                set
                {
                    MarkDataDirty(1);
                    this.canBeIgnited = value;
                }
            }

            private global::Dinopark.Fire.FireEffectType effectType;

            public global::Dinopark.Fire.FireEffectType EffectType
            {
                get => effectType;
                set
                {
                    MarkDataDirty(2);
                    this.effectType = value;
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
            public uint ComponentId => 1400;

            public bool IsOnFire;
            public bool CanBeIgnited;
            public global::Dinopark.Fire.FireEffectType EffectType;

            public Snapshot(bool isOnFire, bool canBeIgnited, global::Dinopark.Fire.FireEffectType effectType)
            {
                IsOnFire = isOnFire;
                CanBeIgnited = canBeIgnited;
                EffectType = effectType;
            }
        }

        public static class Serialization
        {
            public static void SerializeComponent(global::Dinopark.Fire.Flammable.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                {
                    obj.AddBool(1, component.IsOnFire);
                }
                {
                    obj.AddBool(2, component.CanBeIgnited);
                }
                {
                    obj.AddEnum(3, (uint) component.EffectType);
                }
            }

            public static void SerializeUpdate(global::Dinopark.Fire.Flammable.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDataDirty(0))
                    {
                        obj.AddBool(1, component.IsOnFire);
                    }

                }
                {
                    if (component.IsDataDirty(1))
                    {
                        obj.AddBool(2, component.CanBeIgnited);
                    }

                }
                {
                    if (component.IsDataDirty(2))
                    {
                        obj.AddEnum(3, (uint) component.EffectType);
                    }

                }
            }

            public static void SerializeUpdate(global::Dinopark.Fire.Flammable.Update update, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (update.IsOnFire.HasValue)
                    {
                        var field = update.IsOnFire.Value;
                        obj.AddBool(1, field);
                    }
                }
                {
                    if (update.CanBeIgnited.HasValue)
                    {
                        var field = update.CanBeIgnited.Value;
                        obj.AddBool(2, field);
                    }
                }
                {
                    if (update.EffectType.HasValue)
                    {
                        var field = update.EffectType.Value;
                        obj.AddEnum(3, (uint) field);
                    }
                }
            }

            public static void SerializeSnapshot(global::Dinopark.Fire.Flammable.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddBool(1, snapshot.IsOnFire);
                }
                {
                    obj.AddBool(2, snapshot.CanBeIgnited);
                }
                {
                    obj.AddEnum(3, (uint) snapshot.EffectType);
                }
            }

            public static global::Dinopark.Fire.Flammable.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new global::Dinopark.Fire.Flammable.Component();

                {
                    component.IsOnFire = obj.GetBool(1);
                }
                {
                    component.CanBeIgnited = obj.GetBool(2);
                }
                {
                    component.EffectType = (global::Dinopark.Fire.FireEffectType) obj.GetEnum(3);
                }
                return component;
            }

            public static global::Dinopark.Fire.Flammable.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new global::Dinopark.Fire.Flammable.Update();
                var obj = updateObj.GetFields();

                {
                    if (obj.GetBoolCount(1) == 1)
                    {
                        var value = obj.GetBool(1);
                        update.IsOnFire = new global::Improbable.Gdk.Core.Option<bool>(value);
                    }
                    
                }
                {
                    if (obj.GetBoolCount(2) == 1)
                    {
                        var value = obj.GetBool(2);
                        update.CanBeIgnited = new global::Improbable.Gdk.Core.Option<bool>(value);
                    }
                    
                }
                {
                    if (obj.GetEnumCount(3) == 1)
                    {
                        var value = (global::Dinopark.Fire.FireEffectType) obj.GetEnum(3);
                        update.EffectType = new global::Improbable.Gdk.Core.Option<global::Dinopark.Fire.FireEffectType>(value);
                    }
                    
                }
                return update;
            }

            public static global::Dinopark.Fire.Flammable.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentData data)
            {
                var update = new global::Dinopark.Fire.Flammable.Update();
                var obj = data.GetFields();

                {
                    var value = obj.GetBool(1);
                    update.IsOnFire = new global::Improbable.Gdk.Core.Option<bool>(value);
                    
                }
                {
                    var value = obj.GetBool(2);
                    update.CanBeIgnited = new global::Improbable.Gdk.Core.Option<bool>(value);
                    
                }
                {
                    var value = (global::Dinopark.Fire.FireEffectType) obj.GetEnum(3);
                    update.EffectType = new global::Improbable.Gdk.Core.Option<global::Dinopark.Fire.FireEffectType>(value);
                    
                }
                return update;
            }

            public static global::Dinopark.Fire.Flammable.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var component = new global::Dinopark.Fire.Flammable.Snapshot();

                {
                    component.IsOnFire = obj.GetBool(1);
                }

                {
                    component.CanBeIgnited = obj.GetBool(2);
                }

                {
                    component.EffectType = (global::Dinopark.Fire.FireEffectType) obj.GetEnum(3);
                }

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Dinopark.Fire.Flammable.Component component)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetBoolCount(1) == 1)
                    {
                        var value = obj.GetBool(1);
                        component.IsOnFire = value;
                    }
                    
                }
                {
                    if (obj.GetBoolCount(2) == 1)
                    {
                        var value = obj.GetBool(2);
                        component.CanBeIgnited = value;
                    }
                    
                }
                {
                    if (obj.GetEnumCount(3) == 1)
                    {
                        var value = (global::Dinopark.Fire.FireEffectType) obj.GetEnum(3);
                        component.EffectType = value;
                    }
                    
                }
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Dinopark.Fire.Flammable.Snapshot snapshot)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetBoolCount(1) == 1)
                    {
                        var value = obj.GetBool(1);
                        snapshot.IsOnFire = value;
                    }
                    
                }
                {
                    if (obj.GetBoolCount(2) == 1)
                    {
                        var value = obj.GetBool(2);
                        snapshot.CanBeIgnited = value;
                    }
                    
                }
                {
                    if (obj.GetEnumCount(3) == 1)
                    {
                        var value = (global::Dinopark.Fire.FireEffectType) obj.GetEnum(3);
                        snapshot.EffectType = value;
                    }
                    
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

            public Option<bool> IsOnFire;
            public Option<bool> CanBeIgnited;
            public Option<global::Dinopark.Fire.FireEffectType> EffectType;
        }

#if !DISABLE_REACTIVE_COMPONENTS
        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => global::Dinopark.Fire.Flammable.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
#endif

        internal class FlammableDynamic : IDynamicInvokable
        {
            public uint ComponentId => Flammable.ComponentId;

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
                update.IsOnFire = new Option<bool>(snapshot.IsOnFire);
                update.CanBeIgnited = new Option<bool>(snapshot.CanBeIgnited);
                update.EffectType = new Option<global::Dinopark.Fire.FireEffectType>(snapshot.EffectType);
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
