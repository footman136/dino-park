// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Dinopark.Fire
{
    public partial class Flammable
    {
        public class DiffIgniteCommandStorage : IComponentCommandDiffStorage
            , IDiffCommandRequestStorage<Ignite.ReceivedRequest>
            , IDiffCommandResponseStorage<Ignite.ReceivedResponse>
        {
            private MessageList<Ignite.ReceivedRequest> requestStorage =
                new MessageList<Ignite.ReceivedRequest>();

            private MessageList<Ignite.ReceivedResponse> responseStorage =
                new MessageList<Ignite.ReceivedResponse>();

            private readonly RequestComparer requestComparer = new RequestComparer();
            private readonly ResponseComparer responseComparer = new ResponseComparer();

            private bool requestsSorted;
            private bool responsesSorted;

            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 1;
            }

            public Type GetRequestType()
            {
                return typeof(Ignite.ReceivedRequest);
            }

            public Type GetResponseType()
            {
                return typeof(Ignite.ReceivedResponse);
            }

            public void Clear()
            {
                requestStorage.Clear();
                responseStorage.Clear();
                requestsSorted = false;
                responsesSorted = false;
            }

            public void RemoveRequests(long entityId)
            {
                requestStorage.RemoveAll(request => request.EntityId.Id == entityId);
            }

            public void AddRequest(Ignite.ReceivedRequest request)
            {
                requestStorage.Add(request);
            }

            public void AddResponse(Ignite.ReceivedResponse response)
            {
                responseStorage.Add(response);
            }

            public ReceivedMessagesSpan<Ignite.ReceivedRequest> GetRequests()
            {
                return new ReceivedMessagesSpan<Ignite.ReceivedRequest>(requestStorage);
            }

            public ReceivedMessagesSpan<Ignite.ReceivedRequest> GetRequests(EntityId targetEntityId)
            {
                if (!requestsSorted)
                {
                    requestStorage.Sort(requestComparer);
                    requestsSorted = true;
                }

                var (firstIndex, count) = requestStorage.GetEntityRange(targetEntityId);

                return new ReceivedMessagesSpan<Ignite.ReceivedRequest>(requestStorage, firstIndex, count);
            }

            public ReceivedMessagesSpan<Ignite.ReceivedResponse> GetResponses()
            {
                return new ReceivedMessagesSpan<Ignite.ReceivedResponse>(responseStorage);
            }

            public ReceivedMessagesSpan<Ignite.ReceivedResponse> GetResponse(long requestId)
            {
                if (!responsesSorted)
                {
                    responseStorage.Sort(responseComparer);
                    responsesSorted = true;
                }

                var responseIndex = responseStorage.GetResponseIndex(requestId);
                if (responseIndex < 0)
                {
                    return ReceivedMessagesSpan<Ignite.ReceivedResponse>.Empty();
                }

                return new ReceivedMessagesSpan<Ignite.ReceivedResponse>(responseStorage, responseIndex, 1);
            }

            private class RequestComparer : IComparer<Ignite.ReceivedRequest>
            {
                public int Compare(Ignite.ReceivedRequest x, Ignite.ReceivedRequest y)
                {
                    return x.EntityId.Id.CompareTo(y.EntityId.Id);
                }
            }

            private class ResponseComparer : IComparer<Ignite.ReceivedResponse>
            {
                public int Compare(Ignite.ReceivedResponse x, Ignite.ReceivedResponse y)
                {
                    return x.RequestId.CompareTo(y.RequestId);
                }
            }
        }

        public class DiffExtinguishCommandStorage : IComponentCommandDiffStorage
            , IDiffCommandRequestStorage<Extinguish.ReceivedRequest>
            , IDiffCommandResponseStorage<Extinguish.ReceivedResponse>
        {
            private MessageList<Extinguish.ReceivedRequest> requestStorage =
                new MessageList<Extinguish.ReceivedRequest>();

            private MessageList<Extinguish.ReceivedResponse> responseStorage =
                new MessageList<Extinguish.ReceivedResponse>();

            private readonly RequestComparer requestComparer = new RequestComparer();
            private readonly ResponseComparer responseComparer = new ResponseComparer();

            private bool requestsSorted;
            private bool responsesSorted;

            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 2;
            }

            public Type GetRequestType()
            {
                return typeof(Extinguish.ReceivedRequest);
            }

            public Type GetResponseType()
            {
                return typeof(Extinguish.ReceivedResponse);
            }

            public void Clear()
            {
                requestStorage.Clear();
                responseStorage.Clear();
                requestsSorted = false;
                responsesSorted = false;
            }

            public void RemoveRequests(long entityId)
            {
                requestStorage.RemoveAll(request => request.EntityId.Id == entityId);
            }

            public void AddRequest(Extinguish.ReceivedRequest request)
            {
                requestStorage.Add(request);
            }

            public void AddResponse(Extinguish.ReceivedResponse response)
            {
                responseStorage.Add(response);
            }

            public ReceivedMessagesSpan<Extinguish.ReceivedRequest> GetRequests()
            {
                return new ReceivedMessagesSpan<Extinguish.ReceivedRequest>(requestStorage);
            }

            public ReceivedMessagesSpan<Extinguish.ReceivedRequest> GetRequests(EntityId targetEntityId)
            {
                if (!requestsSorted)
                {
                    requestStorage.Sort(requestComparer);
                    requestsSorted = true;
                }

                var (firstIndex, count) = requestStorage.GetEntityRange(targetEntityId);

                return new ReceivedMessagesSpan<Extinguish.ReceivedRequest>(requestStorage, firstIndex, count);
            }

            public ReceivedMessagesSpan<Extinguish.ReceivedResponse> GetResponses()
            {
                return new ReceivedMessagesSpan<Extinguish.ReceivedResponse>(responseStorage);
            }

            public ReceivedMessagesSpan<Extinguish.ReceivedResponse> GetResponse(long requestId)
            {
                if (!responsesSorted)
                {
                    responseStorage.Sort(responseComparer);
                    responsesSorted = true;
                }

                var responseIndex = responseStorage.GetResponseIndex(requestId);
                if (responseIndex < 0)
                {
                    return ReceivedMessagesSpan<Extinguish.ReceivedResponse>.Empty();
                }

                return new ReceivedMessagesSpan<Extinguish.ReceivedResponse>(responseStorage, responseIndex, 1);
            }

            private class RequestComparer : IComparer<Extinguish.ReceivedRequest>
            {
                public int Compare(Extinguish.ReceivedRequest x, Extinguish.ReceivedRequest y)
                {
                    return x.EntityId.Id.CompareTo(y.EntityId.Id);
                }
            }

            private class ResponseComparer : IComparer<Extinguish.ReceivedResponse>
            {
                public int Compare(Extinguish.ReceivedResponse x, Extinguish.ReceivedResponse y)
                {
                    return x.RequestId.CompareTo(y.RequestId);
                }
            }
        }

        public class DiffSetCanBeIgnitedCommandStorage : IComponentCommandDiffStorage
            , IDiffCommandRequestStorage<SetCanBeIgnited.ReceivedRequest>
            , IDiffCommandResponseStorage<SetCanBeIgnited.ReceivedResponse>
        {
            private MessageList<SetCanBeIgnited.ReceivedRequest> requestStorage =
                new MessageList<SetCanBeIgnited.ReceivedRequest>();

            private MessageList<SetCanBeIgnited.ReceivedResponse> responseStorage =
                new MessageList<SetCanBeIgnited.ReceivedResponse>();

            private readonly RequestComparer requestComparer = new RequestComparer();
            private readonly ResponseComparer responseComparer = new ResponseComparer();

            private bool requestsSorted;
            private bool responsesSorted;

            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 3;
            }

            public Type GetRequestType()
            {
                return typeof(SetCanBeIgnited.ReceivedRequest);
            }

            public Type GetResponseType()
            {
                return typeof(SetCanBeIgnited.ReceivedResponse);
            }

            public void Clear()
            {
                requestStorage.Clear();
                responseStorage.Clear();
                requestsSorted = false;
                responsesSorted = false;
            }

            public void RemoveRequests(long entityId)
            {
                requestStorage.RemoveAll(request => request.EntityId.Id == entityId);
            }

            public void AddRequest(SetCanBeIgnited.ReceivedRequest request)
            {
                requestStorage.Add(request);
            }

            public void AddResponse(SetCanBeIgnited.ReceivedResponse response)
            {
                responseStorage.Add(response);
            }

            public ReceivedMessagesSpan<SetCanBeIgnited.ReceivedRequest> GetRequests()
            {
                return new ReceivedMessagesSpan<SetCanBeIgnited.ReceivedRequest>(requestStorage);
            }

            public ReceivedMessagesSpan<SetCanBeIgnited.ReceivedRequest> GetRequests(EntityId targetEntityId)
            {
                if (!requestsSorted)
                {
                    requestStorage.Sort(requestComparer);
                    requestsSorted = true;
                }

                var (firstIndex, count) = requestStorage.GetEntityRange(targetEntityId);

                return new ReceivedMessagesSpan<SetCanBeIgnited.ReceivedRequest>(requestStorage, firstIndex, count);
            }

            public ReceivedMessagesSpan<SetCanBeIgnited.ReceivedResponse> GetResponses()
            {
                return new ReceivedMessagesSpan<SetCanBeIgnited.ReceivedResponse>(responseStorage);
            }

            public ReceivedMessagesSpan<SetCanBeIgnited.ReceivedResponse> GetResponse(long requestId)
            {
                if (!responsesSorted)
                {
                    responseStorage.Sort(responseComparer);
                    responsesSorted = true;
                }

                var responseIndex = responseStorage.GetResponseIndex(requestId);
                if (responseIndex < 0)
                {
                    return ReceivedMessagesSpan<SetCanBeIgnited.ReceivedResponse>.Empty();
                }

                return new ReceivedMessagesSpan<SetCanBeIgnited.ReceivedResponse>(responseStorage, responseIndex, 1);
            }

            private class RequestComparer : IComparer<SetCanBeIgnited.ReceivedRequest>
            {
                public int Compare(SetCanBeIgnited.ReceivedRequest x, SetCanBeIgnited.ReceivedRequest y)
                {
                    return x.EntityId.Id.CompareTo(y.EntityId.Id);
                }
            }

            private class ResponseComparer : IComparer<SetCanBeIgnited.ReceivedResponse>
            {
                public int Compare(SetCanBeIgnited.ReceivedResponse x, SetCanBeIgnited.ReceivedResponse y)
                {
                    return x.RequestId.CompareTo(y.RequestId);
                }
            }
        }


        public class IgniteCommandsToSendStorage : ICommandSendStorage, IComponentCommandSendStorage
            , ICommandRequestSendStorage<Ignite.Request>
            , ICommandResponseSendStorage<Ignite.Response>
        {
            private MessageList<CommandRequestWithMetaData<Ignite.Request>> requestStorage =
                new MessageList<CommandRequestWithMetaData<Ignite.Request>>();

            private MessageList<Ignite.Response> responseStorage =
                new MessageList<Ignite.Response>();

            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 1;
            }

            public Type GetRequestType()
            {
                return typeof(Ignite.Request);
            }

            public Type GetResponseType()
            {
                return typeof(Ignite.Response);
            }

            public void Clear()
            {
                requestStorage.Clear();
                responseStorage.Clear();
            }

            public void AddRequest(Ignite.Request request, Entity entity, long requestId)
            {
                requestStorage.Add(new CommandRequestWithMetaData<Ignite.Request>(request, entity, requestId));
            }

            public void AddResponse(Ignite.Response response)
            {
                responseStorage.Add(response);
            }

            internal MessageList<CommandRequestWithMetaData<Ignite.Request>> GetRequests()
            {
                return requestStorage;
            }

            internal MessageList<Ignite.Response> GetResponses()
            {
                return responseStorage;
            }
        }

        public class ExtinguishCommandsToSendStorage : ICommandSendStorage, IComponentCommandSendStorage
            , ICommandRequestSendStorage<Extinguish.Request>
            , ICommandResponseSendStorage<Extinguish.Response>
        {
            private MessageList<CommandRequestWithMetaData<Extinguish.Request>> requestStorage =
                new MessageList<CommandRequestWithMetaData<Extinguish.Request>>();

            private MessageList<Extinguish.Response> responseStorage =
                new MessageList<Extinguish.Response>();

            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 2;
            }

            public Type GetRequestType()
            {
                return typeof(Extinguish.Request);
            }

            public Type GetResponseType()
            {
                return typeof(Extinguish.Response);
            }

            public void Clear()
            {
                requestStorage.Clear();
                responseStorage.Clear();
            }

            public void AddRequest(Extinguish.Request request, Entity entity, long requestId)
            {
                requestStorage.Add(new CommandRequestWithMetaData<Extinguish.Request>(request, entity, requestId));
            }

            public void AddResponse(Extinguish.Response response)
            {
                responseStorage.Add(response);
            }

            internal MessageList<CommandRequestWithMetaData<Extinguish.Request>> GetRequests()
            {
                return requestStorage;
            }

            internal MessageList<Extinguish.Response> GetResponses()
            {
                return responseStorage;
            }
        }

        public class SetCanBeIgnitedCommandsToSendStorage : ICommandSendStorage, IComponentCommandSendStorage
            , ICommandRequestSendStorage<SetCanBeIgnited.Request>
            , ICommandResponseSendStorage<SetCanBeIgnited.Response>
        {
            private MessageList<CommandRequestWithMetaData<SetCanBeIgnited.Request>> requestStorage =
                new MessageList<CommandRequestWithMetaData<SetCanBeIgnited.Request>>();

            private MessageList<SetCanBeIgnited.Response> responseStorage =
                new MessageList<SetCanBeIgnited.Response>();

            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 3;
            }

            public Type GetRequestType()
            {
                return typeof(SetCanBeIgnited.Request);
            }

            public Type GetResponseType()
            {
                return typeof(SetCanBeIgnited.Response);
            }

            public void Clear()
            {
                requestStorage.Clear();
                responseStorage.Clear();
            }

            public void AddRequest(SetCanBeIgnited.Request request, Entity entity, long requestId)
            {
                requestStorage.Add(new CommandRequestWithMetaData<SetCanBeIgnited.Request>(request, entity, requestId));
            }

            public void AddResponse(SetCanBeIgnited.Response response)
            {
                responseStorage.Add(response);
            }

            internal MessageList<CommandRequestWithMetaData<SetCanBeIgnited.Request>> GetRequests()
            {
                return requestStorage;
            }

            internal MessageList<SetCanBeIgnited.Response> GetResponses()
            {
                return responseStorage;
            }
        }

    }
}
