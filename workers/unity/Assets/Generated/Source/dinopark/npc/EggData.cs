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
    public partial class EggData
    {
        public const uint ComponentId = 1873;

        public struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            public uint ComponentId => 1873;

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
                var componentDataSchema = new ComponentData(new SchemaComponentData(1873));
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields());

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }

            private global::Dinopark.Npc.EggTypeEnum eggType;

            public global::Dinopark.Npc.EggTypeEnum EggType
            {
                get => eggType;
                set
                {
                    MarkDataDirty(0);
                    this.eggType = value;
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

            private global::Dinopark.Npc.EggStateEnum currentState;

            public global::Dinopark.Npc.EggStateEnum CurrentState
            {
                get => currentState;
                set
                {
                    MarkDataDirty(2);
                    this.currentState = value;
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
            public uint ComponentId => 1873;

            public global::Dinopark.Npc.EggTypeEnum EggType;
            public float CurrentFood;
            public global::Dinopark.Npc.EggStateEnum CurrentState;

            public Snapshot(global::Dinopark.Npc.EggTypeEnum eggType, float currentFood, global::Dinopark.Npc.EggStateEnum currentState)
            {
                EggType = eggType;
                CurrentFood = currentFood;
                CurrentState = currentState;
            }
        }

        public static class Serialization
        {
            public static void SerializeComponent(global::Dinopark.Npc.EggData.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                {
                    obj.AddEnum(1, (uint) component.EggType);
                }
                {
                    obj.AddFloat(2, component.CurrentFood);
                }
                {
                    obj.AddEnum(3, (uint) component.CurrentState);
                }
            }

            public static void SerializeUpdate(global::Dinopark.Npc.EggData.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDataDirty(0))
                    {
                        obj.AddEnum(1, (uint) component.EggType);
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
                        obj.AddEnum(3, (uint) component.CurrentState);
                    }

                }
            }

            public static void SerializeUpdate(global::Dinopark.Npc.EggData.Update update, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (update.EggType.HasValue)
                    {
                        var field = update.EggType.Value;
                        obj.AddEnum(1, (uint) field);
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
                    if (update.CurrentState.HasValue)
                    {
                        var field = update.CurrentState.Value;
                        obj.AddEnum(3, (uint) field);
                    }
                }
            }

            public static void SerializeSnapshot(global::Dinopark.Npc.EggData.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddEnum(1, (uint) snapshot.EggType);
                }
                {
                    obj.AddFloat(2, snapshot.CurrentFood);
                }
                {
                    obj.AddEnum(3, (uint) snapshot.CurrentState);
                }
            }

            public static global::Dinopark.Npc.EggData.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new global::Dinopark.Npc.EggData.Component();

                {
                    component.EggType = (global::Dinopark.Npc.EggTypeEnum) obj.GetEnum(1);
                }
                {
                    component.CurrentFood = obj.GetFloat(2);
                }
                {
                    component.CurrentState = (global::Dinopark.Npc.EggStateEnum) obj.GetEnum(3);
                }
                return component;
            }

            public static global::Dinopark.Npc.EggData.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new global::Dinopark.Npc.EggData.Update();
                var obj = updateObj.GetFields();

                {
                    if (obj.GetEnumCount(1) == 1)
                    {
                        var value = (global::Dinopark.Npc.EggTypeEnum) obj.GetEnum(1);
                        update.EggType = new global::Improbable.Gdk.Core.Option<global::Dinopark.Npc.EggTypeEnum>(value);
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
                    if (obj.GetEnumCount(3) == 1)
                    {
                        var value = (global::Dinopark.Npc.EggStateEnum) obj.GetEnum(3);
                        update.CurrentState = new global::Improbable.Gdk.Core.Option<global::Dinopark.Npc.EggStateEnum>(value);
                    }
                    
                }
                return update;
            }

            public static global::Dinopark.Npc.EggData.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentData data)
            {
                var update = new global::Dinopark.Npc.EggData.Update();
                var obj = data.GetFields();

                {
                    var value = (global::Dinopark.Npc.EggTypeEnum) obj.GetEnum(1);
                    update.EggType = new global::Improbable.Gdk.Core.Option<global::Dinopark.Npc.EggTypeEnum>(value);
                    
                }
                {
                    var value = obj.GetFloat(2);
                    update.CurrentFood = new global::Improbable.Gdk.Core.Option<float>(value);
                    
                }
                {
                    var value = (global::Dinopark.Npc.EggStateEnum) obj.GetEnum(3);
                    update.CurrentState = new global::Improbable.Gdk.Core.Option<global::Dinopark.Npc.EggStateEnum>(value);
                    
                }
                return update;
            }

            public static global::Dinopark.Npc.EggData.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var component = new global::Dinopark.Npc.EggData.Snapshot();

                {
                    component.EggType = (global::Dinopark.Npc.EggTypeEnum) obj.GetEnum(1);
                }

                {
                    component.CurrentFood = obj.GetFloat(2);
                }

                {
                    component.CurrentState = (global::Dinopark.Npc.EggStateEnum) obj.GetEnum(3);
                }

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Dinopark.Npc.EggData.Component component)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetEnumCount(1) == 1)
                    {
                        var value = (global::Dinopark.Npc.EggTypeEnum) obj.GetEnum(1);
                        component.EggType = value;
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
                    if (obj.GetEnumCount(3) == 1)
                    {
                        var value = (global::Dinopark.Npc.EggStateEnum) obj.GetEnum(3);
                        component.CurrentState = value;
                    }
                    
                }
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Dinopark.Npc.EggData.Snapshot snapshot)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetEnumCount(1) == 1)
                    {
                        var value = (global::Dinopark.Npc.EggTypeEnum) obj.GetEnum(1);
                        snapshot.EggType = value;
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
                    if (obj.GetEnumCount(3) == 1)
                    {
                        var value = (global::Dinopark.Npc.EggStateEnum) obj.GetEnum(3);
                        snapshot.CurrentState = value;
                    }
                    
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

            public Option<global::Dinopark.Npc.EggTypeEnum> EggType;
            public Option<float> CurrentFood;
            public Option<global::Dinopark.Npc.EggStateEnum> CurrentState;
        }

#if !DISABLE_REACTIVE_COMPONENTS
        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => global::Dinopark.Npc.EggData.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
#endif

        internal class EggDataDynamic : IDynamicInvokable
        {
            public uint ComponentId => EggData.ComponentId;

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
                update.EggType = new Option<global::Dinopark.Npc.EggTypeEnum>(snapshot.EggType);
                update.CurrentFood = new Option<float>(snapshot.CurrentFood);
                update.CurrentState = new Option<global::Dinopark.Npc.EggStateEnum>(snapshot.CurrentState);
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
