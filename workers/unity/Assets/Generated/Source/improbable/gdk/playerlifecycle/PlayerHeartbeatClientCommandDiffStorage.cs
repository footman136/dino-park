// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    public partial class PlayerHeartbeatClient
    {
        public class DiffPlayerHeartbeatCommandStorage : IComponentCommandDiffStorage
            , IDiffCommandRequestStorage<PlayerHeartbeat.ReceivedRequest>
            , IDiffCommandResponseStorage<PlayerHeartbeat.ReceivedResponse>
        {
            private MessageList<PlayerHeartbeat.ReceivedRequest> requestStorage =
                new MessageList<PlayerHeartbeat.ReceivedRequest>();

            private MessageList<PlayerHeartbeat.ReceivedResponse> responseStorage =
                new MessageList<PlayerHeartbeat.ReceivedResponse>();

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
                return typeof(PlayerHeartbeat.ReceivedRequest);
            }

            public Type GetResponseType()
            {
                return typeof(PlayerHeartbeat.ReceivedResponse);
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

            public void AddRequest(PlayerHeartbeat.ReceivedRequest request)
            {
                requestStorage.Add(request);
            }

            public void AddResponse(PlayerHeartbeat.ReceivedResponse response)
            {
                responseStorage.Add(response);
            }

            public ReceivedMessagesSpan<PlayerHeartbeat.ReceivedRequest> GetRequests()
            {
                return new ReceivedMessagesSpan<PlayerHeartbeat.ReceivedRequest>(requestStorage);
            }

            public ReceivedMessagesSpan<PlayerHeartbeat.ReceivedRequest> GetRequests(EntityId targetEntityId)
            {
                if (!requestsSorted)
                {
                    requestStorage.Sort(requestComparer);
                    requestsSorted = true;
                }

                var (firstIndex, count) = requestStorage.GetEntityRange(targetEntityId);

                return new ReceivedMessagesSpan<PlayerHeartbeat.ReceivedRequest>(requestStorage, firstIndex, count);
            }

            public ReceivedMessagesSpan<PlayerHeartbeat.ReceivedResponse> GetResponses()
            {
                return new ReceivedMessagesSpan<PlayerHeartbeat.ReceivedResponse>(responseStorage);
            }

            public ReceivedMessagesSpan<PlayerHeartbeat.ReceivedResponse> GetResponse(long requestId)
            {
                if (!responsesSorted)
                {
                    responseStorage.Sort(responseComparer);
                    responsesSorted = true;
                }

                var responseIndex = responseStorage.GetResponseIndex(requestId);
                if (responseIndex < 0)
                {
                    return ReceivedMessagesSpan<PlayerHeartbeat.ReceivedResponse>.Empty();
                }

                return new ReceivedMessagesSpan<PlayerHeartbeat.ReceivedResponse>(responseStorage, responseIndex, 1);
            }

            private class RequestComparer : IComparer<PlayerHeartbeat.ReceivedRequest>
            {
                public int Compare(PlayerHeartbeat.ReceivedRequest x, PlayerHeartbeat.ReceivedRequest y)
                {
                    return x.EntityId.Id.CompareTo(y.EntityId.Id);
                }
            }

            private class ResponseComparer : IComparer<PlayerHeartbeat.ReceivedResponse>
            {
                public int Compare(PlayerHeartbeat.ReceivedResponse x, PlayerHeartbeat.ReceivedResponse y)
                {
                    return x.RequestId.CompareTo(y.RequestId);
                }
            }
        }


        public class PlayerHeartbeatCommandsToSendStorage : ICommandSendStorage, IComponentCommandSendStorage
            , ICommandRequestSendStorage<PlayerHeartbeat.Request>
            , ICommandResponseSendStorage<PlayerHeartbeat.Response>
        {
            private MessageList<CommandRequestWithMetaData<PlayerHeartbeat.Request>> requestStorage =
                new MessageList<CommandRequestWithMetaData<PlayerHeartbeat.Request>>();

            private MessageList<PlayerHeartbeat.Response> responseStorage =
                new MessageList<PlayerHeartbeat.Response>();

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
                return typeof(PlayerHeartbeat.Request);
            }

            public Type GetResponseType()
            {
                return typeof(PlayerHeartbeat.Response);
            }

            public void Clear()
            {
                requestStorage.Clear();
                responseStorage.Clear();
            }

            public void AddRequest(PlayerHeartbeat.Request request, Entity entity, long requestId)
            {
                requestStorage.Add(new CommandRequestWithMetaData<PlayerHeartbeat.Request>(request, entity, requestId));
            }

            public void AddResponse(PlayerHeartbeat.Response response)
            {
                responseStorage.Add(response);
            }

            internal MessageList<CommandRequestWithMetaData<PlayerHeartbeat.Request>> GetRequests()
            {
                return requestStorage;
            }

            internal MessageList<PlayerHeartbeat.Response> GetResponses()
            {
                return responseStorage;
            }
        }

    }
}
