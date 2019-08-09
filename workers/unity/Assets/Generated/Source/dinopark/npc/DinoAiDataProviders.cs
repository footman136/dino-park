// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;

namespace Dinopark.Npc
{
    public partial class DinoAiData
    {
        internal static class ReferenceTypeProviders
        {
#if !DISABLE_REACTIVE_COMPONENTS
            public static class UpdatesProvider 
            {
                private static readonly Dictionary<uint, List<global::Dinopark.Npc.DinoAiData.Update>> Storage = new Dictionary<uint, List<global::Dinopark.Npc.DinoAiData.Update>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Dinopark.Npc.DinoAiData.Update>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Dinopark.Npc.DinoAiData.Update> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Dinopark.Npc.DinoAiData.Update> value)
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

            public static class AttackSenderProvider 
            {
                private static readonly Dictionary<uint, List<global::Dinopark.Npc.DinoAiData.Attack.Request>> Storage = new Dictionary<uint, List<global::Dinopark.Npc.DinoAiData.Attack.Request>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Dinopark.Npc.DinoAiData.Attack.Request>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Dinopark.Npc.DinoAiData.Attack.Request> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"AttackSenderProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Dinopark.Npc.DinoAiData.Attack.Request> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"AttackSenderProvider does not contain handle {handle}");
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
            
            public static class AttackRequestsProvider 
            {
                private static readonly Dictionary<uint, List<global::Dinopark.Npc.DinoAiData.Attack.ReceivedRequest>> Storage = new Dictionary<uint, List<global::Dinopark.Npc.DinoAiData.Attack.ReceivedRequest>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Dinopark.Npc.DinoAiData.Attack.ReceivedRequest>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Dinopark.Npc.DinoAiData.Attack.ReceivedRequest> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"AttackRequestsProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Dinopark.Npc.DinoAiData.Attack.ReceivedRequest> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"AttackRequestsProvider does not contain handle {handle}");
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
            
            public static class AttackResponderProvider 
            {
                private static readonly Dictionary<uint, List<global::Dinopark.Npc.DinoAiData.Attack.Response>> Storage = new Dictionary<uint, List<global::Dinopark.Npc.DinoAiData.Attack.Response>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Dinopark.Npc.DinoAiData.Attack.Response>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Dinopark.Npc.DinoAiData.Attack.Response> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"AttackResponderProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Dinopark.Npc.DinoAiData.Attack.Response> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"AttackResponderProvider does not contain handle {handle}");
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
            
            public static class AttackResponsesProvider 
            {
                private static readonly Dictionary<uint, List<global::Dinopark.Npc.DinoAiData.Attack.ReceivedResponse>> Storage = new Dictionary<uint, List<global::Dinopark.Npc.DinoAiData.Attack.ReceivedResponse>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Dinopark.Npc.DinoAiData.Attack.ReceivedResponse>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Dinopark.Npc.DinoAiData.Attack.ReceivedResponse> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"AttackResponsesProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Dinopark.Npc.DinoAiData.Attack.ReceivedResponse> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"AttackResponsesProvider does not contain handle {handle}");
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
            

            public static class EatSenderProvider 
            {
                private static readonly Dictionary<uint, List<global::Dinopark.Npc.DinoAiData.Eat.Request>> Storage = new Dictionary<uint, List<global::Dinopark.Npc.DinoAiData.Eat.Request>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Dinopark.Npc.DinoAiData.Eat.Request>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Dinopark.Npc.DinoAiData.Eat.Request> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"EatSenderProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Dinopark.Npc.DinoAiData.Eat.Request> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"EatSenderProvider does not contain handle {handle}");
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
            
            public static class EatRequestsProvider 
            {
                private static readonly Dictionary<uint, List<global::Dinopark.Npc.DinoAiData.Eat.ReceivedRequest>> Storage = new Dictionary<uint, List<global::Dinopark.Npc.DinoAiData.Eat.ReceivedRequest>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Dinopark.Npc.DinoAiData.Eat.ReceivedRequest>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Dinopark.Npc.DinoAiData.Eat.ReceivedRequest> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"EatRequestsProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Dinopark.Npc.DinoAiData.Eat.ReceivedRequest> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"EatRequestsProvider does not contain handle {handle}");
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
            
            public static class EatResponderProvider 
            {
                private static readonly Dictionary<uint, List<global::Dinopark.Npc.DinoAiData.Eat.Response>> Storage = new Dictionary<uint, List<global::Dinopark.Npc.DinoAiData.Eat.Response>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Dinopark.Npc.DinoAiData.Eat.Response>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Dinopark.Npc.DinoAiData.Eat.Response> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"EatResponderProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Dinopark.Npc.DinoAiData.Eat.Response> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"EatResponderProvider does not contain handle {handle}");
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
            
            public static class EatResponsesProvider 
            {
                private static readonly Dictionary<uint, List<global::Dinopark.Npc.DinoAiData.Eat.ReceivedResponse>> Storage = new Dictionary<uint, List<global::Dinopark.Npc.DinoAiData.Eat.ReceivedResponse>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Dinopark.Npc.DinoAiData.Eat.ReceivedResponse>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Dinopark.Npc.DinoAiData.Eat.ReceivedResponse> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"EatResponsesProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Dinopark.Npc.DinoAiData.Eat.ReceivedResponse> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"EatResponsesProvider does not contain handle {handle}");
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
            

        }
    }
}
