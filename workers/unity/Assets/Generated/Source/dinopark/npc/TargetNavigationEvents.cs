// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker;
using Unity.Entities;

namespace Dinopark.Npc
{
    public partial class TargetNavigation
    {
        public static class NavigationFinished
        {
            public readonly struct Event : IEvent
            {
                public readonly global::Dinopark.Npc.NavigationFinished Payload;

                public Event(global::Dinopark.Npc.NavigationFinished payload)
                {
                    Payload = payload;
                }
            }
        }


#if !DISABLE_REACTIVE_COMPONENTS
        public static class ReceivedEvents
        {
            public struct NavigationFinished : IComponentData
            {
                internal uint handle;

                public List<global::Dinopark.Npc.NavigationFinished> Events
                {
                    get => global::Dinopark.Npc.TargetNavigation.ReferenceTypeProviders.NavigationFinishedProvider.Get(handle);
                    internal set => global::Dinopark.Npc.TargetNavigation.ReferenceTypeProviders.NavigationFinishedProvider.Set(handle, value);
                }
            }

        }

        public static class EventSender
        {
            public struct NavigationFinished : IComponentData
            {
                internal uint handle;

                public List<global::Dinopark.Npc.NavigationFinished> Events
                {
                    get => global::Dinopark.Npc.TargetNavigation.ReferenceTypeProviders.NavigationFinishedProvider.Get(handle);
                    internal set => global::Dinopark.Npc.TargetNavigation.ReferenceTypeProviders.NavigationFinishedProvider.Set(handle, value);
                }
            }

        }
#endif
    }
}
