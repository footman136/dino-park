// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker;
using Unity.Entities;

namespace Dinopark.Core
{
    public partial class TransformComponent
    {
        public static class TeleportEvent
        {
            public readonly struct Event : IEvent
            {
                public readonly global::Dinopark.Core.TeleportEvent Payload;

                public Event(global::Dinopark.Core.TeleportEvent payload)
                {
                    Payload = payload;
                }
            }
        }


#if !DISABLE_REACTIVE_COMPONENTS
        public static class ReceivedEvents
        {
            public struct TeleportEvent : IComponentData
            {
                internal uint handle;

                public List<global::Dinopark.Core.TeleportEvent> Events
                {
                    get => global::Dinopark.Core.TransformComponent.ReferenceTypeProviders.TeleportEventProvider.Get(handle);
                    internal set => global::Dinopark.Core.TransformComponent.ReferenceTypeProviders.TeleportEventProvider.Set(handle, value);
                }
            }

        }

        public static class EventSender
        {
            public struct TeleportEvent : IComponentData
            {
                internal uint handle;

                public List<global::Dinopark.Core.TeleportEvent> Events
                {
                    get => global::Dinopark.Core.TransformComponent.ReferenceTypeProviders.TeleportEventProvider.Get(handle);
                    internal set => global::Dinopark.Core.TransformComponent.ReferenceTypeProviders.TeleportEventProvider.Set(handle, value);
                }
            }

        }
#endif
    }
}
