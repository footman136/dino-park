// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;

namespace Dinopark.Life
{
    public partial class Health
    {
        internal static class ReferenceTypeProviders
        {
#if !DISABLE_REACTIVE_COMPONENTS
            public static class UpdatesProvider 
            {
                private static readonly Dictionary<uint, List<global::Dinopark.Life.Health.Update>> Storage = new Dictionary<uint, List<global::Dinopark.Life.Health.Update>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Dinopark.Life.Health.Update>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Dinopark.Life.Health.Update> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Dinopark.Life.Health.Update> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
                    }
            
                    Storage[handle] = value;
                }
            
                public static void Free(uint handle)
                {
                    Storage.Remove(handle);
                    WorldMapping.Remove(handle);
                }
            
                public static void CleanDataInWorld(global::Unity.Entities.World world)
                {
                    var handles = WorldMapping.Where(pair => pair.Value == world).Select(pair => pair.Key).ToList();
            
                    foreach (var handle in handles)
                    {
                        Free(handle);
                    }
                }
            
                private static uint GetNextHandle() 
                {
                    nextHandle++;
                    
                    while (Storage.ContainsKey(nextHandle))
                    {
                        nextHandle++;
                    }
            
                    return nextHandle;
                }
            }
            
#endif

        }
    }
}
