// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    public partial class PlayerCreator
    {
        public class DiffCreatePlayerCommandStorage : IComponentCommandDiffStorage
            , IDiffCommandRequestStorage<CreatePlayer.ReceivedRequest>
            , IDiffCommandResponseStorage<CreatePlayer.ReceivedResponse>
        {
            private MessageList<CreatePlayer.ReceivedRequest> requestStorage =
                new MessageList<CreatePlayer.ReceivedRequest>();

            private MessageList<CreatePlayer.ReceivedResponse> responseStorage =
                new MessageList<CreatePlayer.ReceivedResponse>();

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
                return typeof(CreatePlayer.ReceivedRequest);
            }

            public Type GetResponseType()
            {
                return typeof(CreatePlayer.ReceivedResponse);
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

            public void AddRequest(CreatePlayer.ReceivedRequest request)
            {
                requestStorage.Add(request);
            }

            public void AddResponse(CreatePlayer.ReceivedResponse response)
            {
                responseStorage.Add(response);
            }

            public ReceivedMessagesSpan<CreatePlayer.ReceivedRequest> GetRequests()
            {
                return new ReceivedMessagesSpan<CreatePlayer.ReceivedRequest>(requestStorage);
            }

            public ReceivedMessagesSpan<CreatePlayer.ReceivedRequest> GetRequests(EntityId targetEntityId)
            {
                if (!requestsSorted)
                {
                    requestStorage.Sort(requestComparer);
                    requestsSorted = true;
                }

                var (firstIndex, count) = requestStorage.GetEntityRange(targetEntityId);

                return new ReceivedMessagesSpan<CreatePlayer.ReceivedRequest>(requestStorage, firstIndex, count);
            }

            public ReceivedMessagesSpan<CreatePlayer.ReceivedResponse> GetResponses()
            {
                return new ReceivedMessagesSpan<CreatePlayer.ReceivedResponse>(responseStorage);
            }

            public ReceivedMessagesSpan<CreatePlayer.ReceivedResponse> GetResponse(long requestId)
            {
                if (!responsesSorted)
                {
                    responseStorage.Sort(responseComparer);
                    responsesSorted = true;
                }

                var responseIndex = responseStorage.GetResponseIndex(requestId);
                if (responseIndex < 0)
                {
                    return ReceivedMessagesSpan<CreatePlayer.ReceivedResponse>.Empty();
                }

                return new ReceivedMessagesSpan<CreatePlayer.ReceivedResponse>(responseStorage, responseIndex, 1);
            }

            private class RequestComparer : IComparer<CreatePlayer.ReceivedRequest>
            {
                public int Compare(CreatePlayer.ReceivedRequest x, CreatePlayer.ReceivedRequest y)
                {
                    return x.EntityId.Id.CompareTo(y.EntityId.Id);
                }
            }

            private class ResponseComparer : IComparer<CreatePlayer.ReceivedResponse>
            {
                public int Compare(CreatePlayer.ReceivedResponse x, CreatePlayer.ReceivedResponse y)
                {
                    return x.RequestId.CompareTo(y.RequestId);
                }
            }
        }


        public class CreatePlayerCommandsToSendStorage : ICommandSendStorage, IComponentCommandSendStorage
            , ICommandRequestSendStorage<CreatePlayer.Request>
            , ICommandResponseSendStorage<CreatePlayer.Response>
        {
            private MessageList<CommandRequestWithMetaData<CreatePlayer.Request>> requestStorage =
                new MessageList<CommandRequestWithMetaData<CreatePlayer.Request>>();

            private MessageList<CreatePlayer.Response> responseStorage =
                new MessageList<CreatePlayer.Response>();

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
                return typeof(CreatePlayer.Request);
            }

            public Type GetResponseType()
            {
                return typeof(CreatePlayer.Response);
            }

            public void Clear()
            {
                requestStorage.Clear();
                responseStorage.Clear();
            }

            public void AddRequest(CreatePlayer.Request request, Entity entity, long requestId)
            {
                requestStorage.Add(new CommandRequestWithMetaData<CreatePlayer.Request>(request, entity, requestId));
            }

            public void AddResponse(CreatePlayer.Response response)
            {
                responseStorage.Add(response);
            }

            internal MessageList<CommandRequestWithMetaData<CreatePlayer.Request>> GetRequests()
            {
                return requestStorage;
            }

            internal MessageList<CreatePlayer.Response> GetResponses()
            {
                return responseStorage;
            }
        }

    }
}
