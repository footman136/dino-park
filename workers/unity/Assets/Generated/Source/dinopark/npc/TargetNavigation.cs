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
    public partial class TargetNavigation
    {
        public const uint ComponentId = 1800;

        public struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            public uint ComponentId => 1800;

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
                if (propertyIndex < 0 || propertyIndex >= 4)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 3]. " +
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
                if (propertyIndex < 0 || propertyIndex >= 4)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 3]. " +
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
                var componentDataSchema = new ComponentData(new SchemaComponentData(1800));
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields());

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }

            private global::Dinopark.Npc.NavigationState navigationState;

            public global::Dinopark.Npc.NavigationState NavigationState
            {
                get => navigationState;
                set
                {
                    MarkDataDirty(0);
                    this.navigationState = value;
                }
            }

            private global::Improbable.Vector3f targetPosition;

            public global::Improbable.Vector3f TargetPosition
            {
                get => targetPosition;
                set
                {
                    MarkDataDirty(1);
                    this.targetPosition = value;
                }
            }

            private global::Improbable.Gdk.Core.EntityId targetEntityId;

            public global::Improbable.Gdk.Core.EntityId TargetEntityId
            {
                get => targetEntityId;
                set
                {
                    MarkDataDirty(2);
                    this.targetEntityId = value;
                }
            }

            private float interactionSqrDistance;

            public float InteractionSqrDistance
            {
                get => interactionSqrDistance;
                set
                {
                    MarkDataDirty(3);
                    this.interactionSqrDistance = value;
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
            public uint ComponentId => 1800;

            public global::Dinopark.Npc.NavigationState NavigationState;
            public global::Improbable.Vector3f TargetPosition;
            public global::Improbable.Gdk.Core.EntityId TargetEntityId;
            public float InteractionSqrDistance;

            public Snapshot(global::Dinopark.Npc.NavigationState navigationState, global::Improbable.Vector3f targetPosition, global::Improbable.Gdk.Core.EntityId targetEntityId, float interactionSqrDistance)
            {
                NavigationState = navigationState;
                TargetPosition = targetPosition;
                TargetEntityId = targetEntityId;
                InteractionSqrDistance = interactionSqrDistance;
            }
        }

        public static class Serialization
        {
            public static void SerializeComponent(global::Dinopark.Npc.TargetNavigation.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                {
                    obj.AddEnum(1, (uint) component.NavigationState);
                }
                {
                    global::Improbable.Vector3f.Serialization.Serialize(component.TargetPosition, obj.AddObject(2));
                }
                {
                    obj.AddEntityId(3, component.TargetEntityId);
                }
                {
                    obj.AddFloat(4, component.InteractionSqrDistance);
                }
            }

            public static void SerializeUpdate(global::Dinopark.Npc.TargetNavigation.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDataDirty(0))
                    {
                        obj.AddEnum(1, (uint) component.NavigationState);
                    }

                }
                {
                    if (component.IsDataDirty(1))
                    {
                        global::Improbable.Vector3f.Serialization.Serialize(component.TargetPosition, obj.AddObject(2));
                    }

                }
                {
                    if (component.IsDataDirty(2))
                    {
                        obj.AddEntityId(3, component.TargetEntityId);
                    }

                }
                {
                    if (component.IsDataDirty(3))
                    {
                        obj.AddFloat(4, component.InteractionSqrDistance);
                    }

                }
            }

            public static void SerializeUpdate(global::Dinopark.Npc.TargetNavigation.Update update, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (update.NavigationState.HasValue)
                    {
                        var field = update.NavigationState.Value;
                        obj.AddEnum(1, (uint) field);
                    }
                }
                {
                    if (update.TargetPosition.HasValue)
                    {
                        var field = update.TargetPosition.Value;
                        global::Improbable.Vector3f.Serialization.Serialize(field, obj.AddObject(2));
                    }
                }
                {
                    if (update.TargetEntityId.HasValue)
                    {
                        var field = update.TargetEntityId.Value;
                        obj.AddEntityId(3, field);
                    }
                }
                {
                    if (update.InteractionSqrDistance.HasValue)
                    {
                        var field = update.InteractionSqrDistance.Value;
                        obj.AddFloat(4, field);
                    }
                }
            }

            public static void SerializeSnapshot(global::Dinopark.Npc.TargetNavigation.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddEnum(1, (uint) snapshot.NavigationState);
                }
                {
                    global::Improbable.Vector3f.Serialization.Serialize(snapshot.TargetPosition, obj.AddObject(2));
                }
                {
                    obj.AddEntityId(3, snapshot.TargetEntityId);
                }
                {
                    obj.AddFloat(4, snapshot.InteractionSqrDistance);
                }
            }

            public static global::Dinopark.Npc.TargetNavigation.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new global::Dinopark.Npc.TargetNavigation.Component();

                {
                    component.NavigationState = (global::Dinopark.Npc.NavigationState) obj.GetEnum(1);
                }
                {
                    component.TargetPosition = global::Improbable.Vector3f.Serialization.Deserialize(obj.GetObject(2));
                }
                {
                    component.TargetEntityId = obj.GetEntityIdStruct(3);
                }
                {
                    component.InteractionSqrDistance = obj.GetFloat(4);
                }
                return component;
            }

            public static global::Dinopark.Npc.TargetNavigation.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new global::Dinopark.Npc.TargetNavigation.Update();
                var obj = updateObj.GetFields();

                {
                    if (obj.GetEnumCount(1) == 1)
                    {
                        var value = (global::Dinopark.Npc.NavigationState) obj.GetEnum(1);
                        update.NavigationState = new global::Improbable.Gdk.Core.Option<global::Dinopark.Npc.NavigationState>(value);
                    }
                    
                }
                {
                    if (obj.GetObjectCount(2) == 1)
                    {
                        var value = global::Improbable.Vector3f.Serialization.Deserialize(obj.GetObject(2));
                        update.TargetPosition = new global::Improbable.Gdk.Core.Option<global::Improbable.Vector3f>(value);
                    }
                    
                }
                {
                    if (obj.GetEntityIdCount(3) == 1)
                    {
                        var value = obj.GetEntityIdStruct(3);
                        update.TargetEntityId = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.Core.EntityId>(value);
                    }
                    
                }
                {
                    if (obj.GetFloatCount(4) == 1)
                    {
                        var value = obj.GetFloat(4);
                        update.InteractionSqrDistance = new global::Improbable.Gdk.Core.Option<float>(value);
                    }
                    
                }
                return update;
            }

            public static global::Dinopark.Npc.TargetNavigation.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentData data)
            {
                var update = new global::Dinopark.Npc.TargetNavigation.Update();
                var obj = data.GetFields();

                {
                    var value = (global::Dinopark.Npc.NavigationState) obj.GetEnum(1);
                    update.NavigationState = new global::Improbable.Gdk.Core.Option<global::Dinopark.Npc.NavigationState>(value);
                    
                }
                {
                    var value = global::Improbable.Vector3f.Serialization.Deserialize(obj.GetObject(2));
                    update.TargetPosition = new global::Improbable.Gdk.Core.Option<global::Improbable.Vector3f>(value);
                    
                }
                {
                    var value = obj.GetEntityIdStruct(3);
                    update.TargetEntityId = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.Core.EntityId>(value);
                    
                }
                {
                    var value = obj.GetFloat(4);
                    update.InteractionSqrDistance = new global::Improbable.Gdk.Core.Option<float>(value);
                    
                }
                return update;
            }

            public static global::Dinopark.Npc.TargetNavigation.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var component = new global::Dinopark.Npc.TargetNavigation.Snapshot();

                {
                    component.NavigationState = (global::Dinopark.Npc.NavigationState) obj.GetEnum(1);
                }

                {
                    component.TargetPosition = global::Improbable.Vector3f.Serialization.Deserialize(obj.GetObject(2));
                }

                {
                    component.TargetEntityId = obj.GetEntityIdStruct(3);
                }

                {
                    component.InteractionSqrDistance = obj.GetFloat(4);
                }

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Dinopark.Npc.TargetNavigation.Component component)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetEnumCount(1) == 1)
                    {
                        var value = (global::Dinopark.Npc.NavigationState) obj.GetEnum(1);
                        component.NavigationState = value;
                    }
                    
                }
                {
                    if (obj.GetObjectCount(2) == 1)
                    {
                        var value = global::Improbable.Vector3f.Serialization.Deserialize(obj.GetObject(2));
                        component.TargetPosition = value;
                    }
                    
                }
                {
                    if (obj.GetEntityIdCount(3) == 1)
                    {
                        var value = obj.GetEntityIdStruct(3);
                        component.TargetEntityId = value;
                    }
                    
                }
                {
                    if (obj.GetFloatCount(4) == 1)
                    {
                        var value = obj.GetFloat(4);
                        component.InteractionSqrDistance = value;
                    }
                    
                }
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Dinopark.Npc.TargetNavigation.Snapshot snapshot)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetEnumCount(1) == 1)
                    {
                        var value = (global::Dinopark.Npc.NavigationState) obj.GetEnum(1);
                        snapshot.NavigationState = value;
                    }
                    
                }
                {
                    if (obj.GetObjectCount(2) == 1)
                    {
                        var value = global::Improbable.Vector3f.Serialization.Deserialize(obj.GetObject(2));
                        snapshot.TargetPosition = value;
                    }
                    
                }
                {
                    if (obj.GetEntityIdCount(3) == 1)
                    {
                        var value = obj.GetEntityIdStruct(3);
                        snapshot.TargetEntityId = value;
                    }
                    
                }
                {
                    if (obj.GetFloatCount(4) == 1)
                    {
                        var value = obj.GetFloat(4);
                        snapshot.InteractionSqrDistance = value;
                    }
                    
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

            public Option<global::Dinopark.Npc.NavigationState> NavigationState;
            public Option<global::Improbable.Vector3f> TargetPosition;
            public Option<global::Improbable.Gdk.Core.EntityId> TargetEntityId;
            public Option<float> InteractionSqrDistance;
        }

#if !DISABLE_REACTIVE_COMPONENTS
        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => global::Dinopark.Npc.TargetNavigation.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
#endif

        internal class TargetNavigationDynamic : IDynamicInvokable
        {
            public uint ComponentId => TargetNavigation.ComponentId;

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
                update.NavigationState = new Option<global::Dinopark.Npc.NavigationState>(snapshot.NavigationState);
                update.TargetPosition = new Option<global::Improbable.Vector3f>(snapshot.TargetPosition);
                update.TargetEntityId = new Option<global::Improbable.Gdk.Core.EntityId>(snapshot.TargetEntityId);
                update.InteractionSqrDistance = new Option<float>(snapshot.InteractionSqrDistance);
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
