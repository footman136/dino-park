// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Dinopark.Npc
{
    public partial class EggData
    {
        public class DiffHatchCommandStorage : IComponentCommandDiffStorage
            , IDiffCommandRequestStorage<Hatch.ReceivedRequest>
            , IDiffCommandResponseStorage<Hatch.ReceivedResponse>
        {
            private MessageList<Hatch.ReceivedRequest> requestStorage =
                new MessageList<Hatch.ReceivedRequest>();

            private MessageList<Hatch.ReceivedResponse> responseStorage =
                new MessageList<Hatch.ReceivedResponse>();

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
                return typeof(Hatch.ReceivedRequest);
            }

            public Type GetResponseType()
            {
                return typeof(Hatch.ReceivedResponse);
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

            public void AddRequest(Hatch.ReceivedRequest request)
            {
                requestStorage.Add(request);
            }

            public void AddResponse(Hatch.ReceivedResponse response)
            {
                responseStorage.Add(response);
            }

            public ReceivedMessagesSpan<Hatch.ReceivedRequest> GetRequests()
            {
                return new ReceivedMessagesSpan<Hatch.ReceivedRequest>(requestStorage);
            }

            public ReceivedMessagesSpan<Hatch.ReceivedRequest> GetRequests(EntityId targetEntityId)
            {
                if (!requestsSorted)
                {
                    requestStorage.Sort(requestComparer);
                    requestsSorted = true;
                }

                var (firstIndex, count) = requestStorage.GetEntityRange(targetEntityId);

                return new ReceivedMessagesSpan<Hatch.ReceivedRequest>(requestStorage, firstIndex, count);
            }

            public ReceivedMessagesSpan<Hatch.ReceivedResponse> GetResponses()
            {
                return new ReceivedMessagesSpan<Hatch.ReceivedResponse>(responseStorage);
            }

            public ReceivedMessagesSpan<Hatch.ReceivedResponse> GetResponse(long requestId)
            {
                if (!responsesSorted)
                {
                    responseStorage.Sort(responseComparer);
                    responsesSorted = true;
                }

                var responseIndex = responseStorage.GetResponseIndex(requestId);
                if (responseIndex < 0)
                {
                    return ReceivedMessagesSpan<Hatch.ReceivedResponse>.Empty();
                }

                return new ReceivedMessagesSpan<Hatch.ReceivedResponse>(responseStorage, responseIndex, 1);
            }

            private class RequestComparer : IComparer<Hatch.ReceivedRequest>
            {
                public int Compare(Hatch.ReceivedRequest x, Hatch.ReceivedRequest y)
                {
                    return x.EntityId.Id.CompareTo(y.EntityId.Id);
                }
            }

            private class ResponseComparer : IComparer<Hatch.ReceivedResponse>
            {
                public int Compare(Hatch.ReceivedResponse x, Hatch.ReceivedResponse y)
                {
                    return x.RequestId.CompareTo(y.RequestId);
                }
            }
        }


        public class HatchCommandsToSendStorage : ICommandSendStorage, IComponentCommandSendStorage
            , ICommandRequestSendStorage<Hatch.Request>
            , ICommandResponseSendStorage<Hatch.Response>
        {
            private MessageList<CommandRequestWithMetaData<Hatch.Request>> requestStorage =
                new MessageList<CommandRequestWithMetaData<Hatch.Request>>();

            private MessageList<Hatch.Response> responseStorage =
                new MessageList<Hatch.Response>();

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
                return typeof(Hatch.Request);
            }

            public Type GetResponseType()
            {
                return typeof(Hatch.Response);
            }

            public void Clear()
            {
                requestStorage.Clear();
                responseStorage.Clear();
            }

            public void AddRequest(Hatch.Request request, Entity entity, long requestId)
            {
                requestStorage.Add(new CommandRequestWithMetaData<Hatch.Request>(request, entity, requestId));
            }

            public void AddResponse(Hatch.Response response)
            {
                responseStorage.Add(response);
            }

            internal MessageList<CommandRequestWithMetaData<Hatch.Request>> GetRequests()
            {
                return requestStorage;
            }

            internal MessageList<Hatch.Response> GetResponses()
            {
                return responseStorage;
            }
        }

    }
}
