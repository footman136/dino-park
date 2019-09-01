// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Dinopark.Player
{
    public partial class PlayerAttrs
    {
        public class DiffLayEggCommandStorage : IComponentCommandDiffStorage
            , IDiffCommandRequestStorage<LayEgg.ReceivedRequest>
            , IDiffCommandResponseStorage<LayEgg.ReceivedResponse>
        {
            private MessageList<LayEgg.ReceivedRequest> requestStorage =
                new MessageList<LayEgg.ReceivedRequest>();

            private MessageList<LayEgg.ReceivedResponse> responseStorage =
                new MessageList<LayEgg.ReceivedResponse>();

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
                return typeof(LayEgg.ReceivedRequest);
            }

            public Type GetResponseType()
            {
                return typeof(LayEgg.ReceivedResponse);
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

            public void AddRequest(LayEgg.ReceivedRequest request)
            {
                requestStorage.Add(request);
            }

            public void AddResponse(LayEgg.ReceivedResponse response)
            {
                responseStorage.Add(response);
            }

            public ReceivedMessagesSpan<LayEgg.ReceivedRequest> GetRequests()
            {
                return new ReceivedMessagesSpan<LayEgg.ReceivedRequest>(requestStorage);
            }

            public ReceivedMessagesSpan<LayEgg.ReceivedRequest> GetRequests(EntityId targetEntityId)
            {
                if (!requestsSorted)
                {
                    requestStorage.Sort(requestComparer);
                    requestsSorted = true;
                }

                var (firstIndex, count) = requestStorage.GetEntityRange(targetEntityId);

                return new ReceivedMessagesSpan<LayEgg.ReceivedRequest>(requestStorage, firstIndex, count);
            }

            public ReceivedMessagesSpan<LayEgg.ReceivedResponse> GetResponses()
            {
                return new ReceivedMessagesSpan<LayEgg.ReceivedResponse>(responseStorage);
            }

            public ReceivedMessagesSpan<LayEgg.ReceivedResponse> GetResponse(long requestId)
            {
                if (!responsesSorted)
                {
                    responseStorage.Sort(responseComparer);
                    responsesSorted = true;
                }

                var responseIndex = responseStorage.GetResponseIndex(requestId);
                if (responseIndex < 0)
                {
                    return ReceivedMessagesSpan<LayEgg.ReceivedResponse>.Empty();
                }

                return new ReceivedMessagesSpan<LayEgg.ReceivedResponse>(responseStorage, responseIndex, 1);
            }

            private class RequestComparer : IComparer<LayEgg.ReceivedRequest>
            {
                public int Compare(LayEgg.ReceivedRequest x, LayEgg.ReceivedRequest y)
                {
                    return x.EntityId.Id.CompareTo(y.EntityId.Id);
                }
            }

            private class ResponseComparer : IComparer<LayEgg.ReceivedResponse>
            {
                public int Compare(LayEgg.ReceivedResponse x, LayEgg.ReceivedResponse y)
                {
                    return x.RequestId.CompareTo(y.RequestId);
                }
            }
        }


        public class LayEggCommandsToSendStorage : ICommandSendStorage, IComponentCommandSendStorage
            , ICommandRequestSendStorage<LayEgg.Request>
            , ICommandResponseSendStorage<LayEgg.Response>
        {
            private MessageList<CommandRequestWithMetaData<LayEgg.Request>> requestStorage =
                new MessageList<CommandRequestWithMetaData<LayEgg.Request>>();

            private MessageList<LayEgg.Response> responseStorage =
                new MessageList<LayEgg.Response>();

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
                return typeof(LayEgg.Request);
            }

            public Type GetResponseType()
            {
                return typeof(LayEgg.Response);
            }

            public void Clear()
            {
                requestStorage.Clear();
                responseStorage.Clear();
            }

            public void AddRequest(LayEgg.Request request, Entity entity, long requestId)
            {
                requestStorage.Add(new CommandRequestWithMetaData<LayEgg.Request>(request, entity, requestId));
            }

            public void AddResponse(LayEgg.Response response)
            {
                responseStorage.Add(response);
            }

            internal MessageList<CommandRequestWithMetaData<LayEgg.Request>> GetRequests()
            {
                return requestStorage;
            }

            internal MessageList<LayEgg.Response> GetResponses()
            {
                return responseStorage;
            }
        }

    }
}
