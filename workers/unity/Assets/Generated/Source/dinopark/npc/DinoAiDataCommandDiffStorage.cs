// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Dinopark.Npc
{
    public partial class DinoAiData
    {
        public class DiffAttackCommandStorage : IComponentCommandDiffStorage
            , IDiffCommandRequestStorage<Attack.ReceivedRequest>
            , IDiffCommandResponseStorage<Attack.ReceivedResponse>
        {
            private MessageList<Attack.ReceivedRequest> requestStorage =
                new MessageList<Attack.ReceivedRequest>();

            private MessageList<Attack.ReceivedResponse> responseStorage =
                new MessageList<Attack.ReceivedResponse>();

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
                return typeof(Attack.ReceivedRequest);
            }

            public Type GetResponseType()
            {
                return typeof(Attack.ReceivedResponse);
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

            public void AddRequest(Attack.ReceivedRequest request)
            {
                requestStorage.Add(request);
            }

            public void AddResponse(Attack.ReceivedResponse response)
            {
                responseStorage.Add(response);
            }

            public ReceivedMessagesSpan<Attack.ReceivedRequest> GetRequests()
            {
                return new ReceivedMessagesSpan<Attack.ReceivedRequest>(requestStorage);
            }

            public ReceivedMessagesSpan<Attack.ReceivedRequest> GetRequests(EntityId targetEntityId)
            {
                if (!requestsSorted)
                {
                    requestStorage.Sort(requestComparer);
                    requestsSorted = true;
                }

                var (firstIndex, count) = requestStorage.GetEntityRange(targetEntityId);

                return new ReceivedMessagesSpan<Attack.ReceivedRequest>(requestStorage, firstIndex, count);
            }

            public ReceivedMessagesSpan<Attack.ReceivedResponse> GetResponses()
            {
                return new ReceivedMessagesSpan<Attack.ReceivedResponse>(responseStorage);
            }

            public ReceivedMessagesSpan<Attack.ReceivedResponse> GetResponse(long requestId)
            {
                if (!responsesSorted)
                {
                    responseStorage.Sort(responseComparer);
                    responsesSorted = true;
                }

                var responseIndex = responseStorage.GetResponseIndex(requestId);
                if (responseIndex < 0)
                {
                    return ReceivedMessagesSpan<Attack.ReceivedResponse>.Empty();
                }

                return new ReceivedMessagesSpan<Attack.ReceivedResponse>(responseStorage, responseIndex, 1);
            }

            private class RequestComparer : IComparer<Attack.ReceivedRequest>
            {
                public int Compare(Attack.ReceivedRequest x, Attack.ReceivedRequest y)
                {
                    return x.EntityId.Id.CompareTo(y.EntityId.Id);
                }
            }

            private class ResponseComparer : IComparer<Attack.ReceivedResponse>
            {
                public int Compare(Attack.ReceivedResponse x, Attack.ReceivedResponse y)
                {
                    return x.RequestId.CompareTo(y.RequestId);
                }
            }
        }

        public class DiffEatCommandStorage : IComponentCommandDiffStorage
            , IDiffCommandRequestStorage<Eat.ReceivedRequest>
            , IDiffCommandResponseStorage<Eat.ReceivedResponse>
        {
            private MessageList<Eat.ReceivedRequest> requestStorage =
                new MessageList<Eat.ReceivedRequest>();

            private MessageList<Eat.ReceivedResponse> responseStorage =
                new MessageList<Eat.ReceivedResponse>();

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
                return typeof(Eat.ReceivedRequest);
            }

            public Type GetResponseType()
            {
                return typeof(Eat.ReceivedResponse);
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

            public void AddRequest(Eat.ReceivedRequest request)
            {
                requestStorage.Add(request);
            }

            public void AddResponse(Eat.ReceivedResponse response)
            {
                responseStorage.Add(response);
            }

            public ReceivedMessagesSpan<Eat.ReceivedRequest> GetRequests()
            {
                return new ReceivedMessagesSpan<Eat.ReceivedRequest>(requestStorage);
            }

            public ReceivedMessagesSpan<Eat.ReceivedRequest> GetRequests(EntityId targetEntityId)
            {
                if (!requestsSorted)
                {
                    requestStorage.Sort(requestComparer);
                    requestsSorted = true;
                }

                var (firstIndex, count) = requestStorage.GetEntityRange(targetEntityId);

                return new ReceivedMessagesSpan<Eat.ReceivedRequest>(requestStorage, firstIndex, count);
            }

            public ReceivedMessagesSpan<Eat.ReceivedResponse> GetResponses()
            {
                return new ReceivedMessagesSpan<Eat.ReceivedResponse>(responseStorage);
            }

            public ReceivedMessagesSpan<Eat.ReceivedResponse> GetResponse(long requestId)
            {
                if (!responsesSorted)
                {
                    responseStorage.Sort(responseComparer);
                    responsesSorted = true;
                }

                var responseIndex = responseStorage.GetResponseIndex(requestId);
                if (responseIndex < 0)
                {
                    return ReceivedMessagesSpan<Eat.ReceivedResponse>.Empty();
                }

                return new ReceivedMessagesSpan<Eat.ReceivedResponse>(responseStorage, responseIndex, 1);
            }

            private class RequestComparer : IComparer<Eat.ReceivedRequest>
            {
                public int Compare(Eat.ReceivedRequest x, Eat.ReceivedRequest y)
                {
                    return x.EntityId.Id.CompareTo(y.EntityId.Id);
                }
            }

            private class ResponseComparer : IComparer<Eat.ReceivedResponse>
            {
                public int Compare(Eat.ReceivedResponse x, Eat.ReceivedResponse y)
                {
                    return x.RequestId.CompareTo(y.RequestId);
                }
            }
        }


        public class AttackCommandsToSendStorage : ICommandSendStorage, IComponentCommandSendStorage
            , ICommandRequestSendStorage<Attack.Request>
            , ICommandResponseSendStorage<Attack.Response>
        {
            private MessageList<CommandRequestWithMetaData<Attack.Request>> requestStorage =
                new MessageList<CommandRequestWithMetaData<Attack.Request>>();

            private MessageList<Attack.Response> responseStorage =
                new MessageList<Attack.Response>();

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
                return typeof(Attack.Request);
            }

            public Type GetResponseType()
            {
                return typeof(Attack.Response);
            }

            public void Clear()
            {
                requestStorage.Clear();
                responseStorage.Clear();
            }

            public void AddRequest(Attack.Request request, Entity entity, long requestId)
            {
                requestStorage.Add(new CommandRequestWithMetaData<Attack.Request>(request, entity, requestId));
            }

            public void AddResponse(Attack.Response response)
            {
                responseStorage.Add(response);
            }

            internal MessageList<CommandRequestWithMetaData<Attack.Request>> GetRequests()
            {
                return requestStorage;
            }

            internal MessageList<Attack.Response> GetResponses()
            {
                return responseStorage;
            }
        }

        public class EatCommandsToSendStorage : ICommandSendStorage, IComponentCommandSendStorage
            , ICommandRequestSendStorage<Eat.Request>
            , ICommandResponseSendStorage<Eat.Response>
        {
            private MessageList<CommandRequestWithMetaData<Eat.Request>> requestStorage =
                new MessageList<CommandRequestWithMetaData<Eat.Request>>();

            private MessageList<Eat.Response> responseStorage =
                new MessageList<Eat.Response>();

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
                return typeof(Eat.Request);
            }

            public Type GetResponseType()
            {
                return typeof(Eat.Response);
            }

            public void Clear()
            {
                requestStorage.Clear();
                responseStorage.Clear();
            }

            public void AddRequest(Eat.Request request, Entity entity, long requestId)
            {
                requestStorage.Add(new CommandRequestWithMetaData<Eat.Request>(request, entity, requestId));
            }

            public void AddResponse(Eat.Response response)
            {
                responseStorage.Add(response);
            }

            internal MessageList<CommandRequestWithMetaData<Eat.Request>> GetRequests()
            {
                return requestStorage;
            }

            internal MessageList<Eat.Response> GetResponses()
            {
                return responseStorage;
            }
        }

    }
}
