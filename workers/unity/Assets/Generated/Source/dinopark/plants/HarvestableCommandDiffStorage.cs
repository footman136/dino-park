// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Dinopark.Plants
{
    public partial class Harvestable
    {
        public class DiffHarvestCommandStorage : IComponentCommandDiffStorage
            , IDiffCommandRequestStorage<Harvest.ReceivedRequest>
            , IDiffCommandResponseStorage<Harvest.ReceivedResponse>
        {
            private MessageList<Harvest.ReceivedRequest> requestStorage =
                new MessageList<Harvest.ReceivedRequest>();

            private MessageList<Harvest.ReceivedResponse> responseStorage =
                new MessageList<Harvest.ReceivedResponse>();

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
                return typeof(Harvest.ReceivedRequest);
            }

            public Type GetResponseType()
            {
                return typeof(Harvest.ReceivedResponse);
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

            public void AddRequest(Harvest.ReceivedRequest request)
            {
                requestStorage.Add(request);
            }

            public void AddResponse(Harvest.ReceivedResponse response)
            {
                responseStorage.Add(response);
            }

            public ReceivedMessagesSpan<Harvest.ReceivedRequest> GetRequests()
            {
                return new ReceivedMessagesSpan<Harvest.ReceivedRequest>(requestStorage);
            }

            public ReceivedMessagesSpan<Harvest.ReceivedRequest> GetRequests(EntityId targetEntityId)
            {
                if (!requestsSorted)
                {
                    requestStorage.Sort(requestComparer);
                    requestsSorted = true;
                }

                var (firstIndex, count) = requestStorage.GetEntityRange(targetEntityId);

                return new ReceivedMessagesSpan<Harvest.ReceivedRequest>(requestStorage, firstIndex, count);
            }

            public ReceivedMessagesSpan<Harvest.ReceivedResponse> GetResponses()
            {
                return new ReceivedMessagesSpan<Harvest.ReceivedResponse>(responseStorage);
            }

            public ReceivedMessagesSpan<Harvest.ReceivedResponse> GetResponse(long requestId)
            {
                if (!responsesSorted)
                {
                    responseStorage.Sort(responseComparer);
                    responsesSorted = true;
                }

                var responseIndex = responseStorage.GetResponseIndex(requestId);
                if (responseIndex < 0)
                {
                    return ReceivedMessagesSpan<Harvest.ReceivedResponse>.Empty();
                }

                return new ReceivedMessagesSpan<Harvest.ReceivedResponse>(responseStorage, responseIndex, 1);
            }

            private class RequestComparer : IComparer<Harvest.ReceivedRequest>
            {
                public int Compare(Harvest.ReceivedRequest x, Harvest.ReceivedRequest y)
                {
                    return x.EntityId.Id.CompareTo(y.EntityId.Id);
                }
            }

            private class ResponseComparer : IComparer<Harvest.ReceivedResponse>
            {
                public int Compare(Harvest.ReceivedResponse x, Harvest.ReceivedResponse y)
                {
                    return x.RequestId.CompareTo(y.RequestId);
                }
            }
        }


        public class HarvestCommandsToSendStorage : ICommandSendStorage, IComponentCommandSendStorage
            , ICommandRequestSendStorage<Harvest.Request>
            , ICommandResponseSendStorage<Harvest.Response>
        {
            private MessageList<CommandRequestWithMetaData<Harvest.Request>> requestStorage =
                new MessageList<CommandRequestWithMetaData<Harvest.Request>>();

            private MessageList<Harvest.Response> responseStorage =
                new MessageList<Harvest.Response>();

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
                return typeof(Harvest.Request);
            }

            public Type GetResponseType()
            {
                return typeof(Harvest.Response);
            }

            public void Clear()
            {
                requestStorage.Clear();
                responseStorage.Clear();
            }

            public void AddRequest(Harvest.Request request, Entity entity, long requestId)
            {
                requestStorage.Add(new CommandRequestWithMetaData<Harvest.Request>(request, entity, requestId));
            }

            public void AddResponse(Harvest.Response response)
            {
                responseStorage.Add(response);
            }

            internal MessageList<CommandRequestWithMetaData<Harvest.Request>> GetRequests()
            {
                return requestStorage;
            }

            internal MessageList<Harvest.Response> GetResponses()
            {
                return responseStorage;
            }
        }

    }
}
