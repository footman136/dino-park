// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Improbable.Worker.CInterop;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;

namespace Dinopark.Npc
{
    public partial class DinoAiData
    {
        public partial class Attack
        {
            public struct Request : ICommandRequest
            {
                public EntityId TargetEntityId;
                public global::Dinopark.Npc.AttackRequest Payload;
                public uint? TimeoutMillis;
                public bool AllowShortCircuiting;
                public object Context;

                public Request(
                    EntityId targetEntityId,
                    global::Dinopark.Npc.AttackRequest request,
                    uint? timeoutMillis = null,
                    bool allowShortCircuiting = false,
                    object context = null)
                {
                    TargetEntityId = targetEntityId;
                    Payload = request;
                    TimeoutMillis = timeoutMillis;
                    AllowShortCircuiting = allowShortCircuiting;
                    Context = context;
                }
            }


            public readonly struct ReceivedRequest : IReceivedCommandRequest
            {
                public readonly EntityId EntityId;
                public readonly long RequestId;
                public readonly string CallerWorkerId;
                public readonly List<string> CallerAttributeSet;
                public readonly global::Dinopark.Npc.AttackRequest Payload;

                public ReceivedRequest(
                    EntityId entityId,
                    long requestId,
                    string callerWorkerId,
                    List<string> callerAttributeSet,
                    global::Dinopark.Npc.AttackRequest request)
                {
                    EntityId = entityId;
                    RequestId = requestId;
                    CallerWorkerId = callerWorkerId;
                    CallerAttributeSet = callerAttributeSet;
                    Payload = request;
                }

                long IReceivedCommandRequest.GetRequestId()
                {
                    return RequestId;
                }

                EntityId IReceivedEntityMessage.GetEntityId()
                {
                    return EntityId;
                }
            }

            /// <summary>
            ///     A Response will be considered a failure if FailureMessage is not null;
            /// </summary>
            // todo Should consider making this a union of a failure and a response
            // todo consider making this readonly
            public struct Response : ICommandResponse
            {
                public long RequestId;
                public global::Dinopark.Npc.AttackResponse? Payload;
                public string FailureMessage;

                public Response(long requestId, global::Dinopark.Npc.AttackResponse payload)
                {
                    RequestId = requestId;
                    Payload = payload;
                    FailureMessage = null;
                }

                public Response(long requestId, string failureMessage)
                {
                    RequestId = requestId;
                    Payload = null;
                    FailureMessage = failureMessage;
                }
            }

            public readonly struct ReceivedResponse : IReceivedCommandResponse
            {
                public readonly Unity.Entities.Entity SendingEntity;
                public readonly EntityId EntityId;
                public readonly string Message;
                public readonly StatusCode StatusCode;
                public readonly global::Dinopark.Npc.AttackResponse? ResponsePayload;
                public readonly global::Dinopark.Npc.AttackRequest RequestPayload;
                public readonly global::System.Object Context;
                public readonly long RequestId;

                public ReceivedResponse(
                    Unity.Entities.Entity sendingEntity,
                    EntityId entityId,
                    string message,
                    StatusCode statusCode,
                    global::Dinopark.Npc.AttackResponse? response,
                    global::Dinopark.Npc.AttackRequest request,
                    global::System.Object context,
                    long requestId)
                {
                    SendingEntity = sendingEntity;
                    EntityId = entityId;
                    Message = message;
                    StatusCode = statusCode;
                    ResponsePayload = response;
                    RequestPayload = request;
                    Context = context;
                    RequestId = requestId;
                }

                long IReceivedCommandResponse.GetRequestId()
                {
                    return RequestId;
                }
            }

            public readonly struct RawReceivedResponse : IRawReceivedCommandResponse
            {
                public readonly EntityId EntityId;
                public readonly string Message;
                public readonly StatusCode StatusCode;
                public readonly global::Dinopark.Npc.AttackResponse? ResponsePayload;
                public readonly long RequestId;

                public RawReceivedResponse(
                    EntityId entityId,
                    string message,
                    StatusCode statusCode,
                    global::Dinopark.Npc.AttackResponse? response,
                    long requestId)
                {
                    EntityId = entityId;
                    Message = message;
                    StatusCode = statusCode;
                    ResponsePayload = response;
                    RequestId = requestId;
                }
            }
        }
        public partial class Eat
        {
            public struct Request : ICommandRequest
            {
                public EntityId TargetEntityId;
                public global::Dinopark.Npc.EatRequest Payload;
                public uint? TimeoutMillis;
                public bool AllowShortCircuiting;
                public object Context;

                public Request(
                    EntityId targetEntityId,
                    global::Dinopark.Npc.EatRequest request,
                    uint? timeoutMillis = null,
                    bool allowShortCircuiting = false,
                    object context = null)
                {
                    TargetEntityId = targetEntityId;
                    Payload = request;
                    TimeoutMillis = timeoutMillis;
                    AllowShortCircuiting = allowShortCircuiting;
                    Context = context;
                }
            }


            public readonly struct ReceivedRequest : IReceivedCommandRequest
            {
                public readonly EntityId EntityId;
                public readonly long RequestId;
                public readonly string CallerWorkerId;
                public readonly List<string> CallerAttributeSet;
                public readonly global::Dinopark.Npc.EatRequest Payload;

                public ReceivedRequest(
                    EntityId entityId,
                    long requestId,
                    string callerWorkerId,
                    List<string> callerAttributeSet,
                    global::Dinopark.Npc.EatRequest request)
                {
                    EntityId = entityId;
                    RequestId = requestId;
                    CallerWorkerId = callerWorkerId;
                    CallerAttributeSet = callerAttributeSet;
                    Payload = request;
                }

                long IReceivedCommandRequest.GetRequestId()
                {
                    return RequestId;
                }

                EntityId IReceivedEntityMessage.GetEntityId()
                {
                    return EntityId;
                }
            }

            /// <summary>
            ///     A Response will be considered a failure if FailureMessage is not null;
            /// </summary>
            // todo Should consider making this a union of a failure and a response
            // todo consider making this readonly
            public struct Response : ICommandResponse
            {
                public long RequestId;
                public global::Dinopark.Npc.EatResponse? Payload;
                public string FailureMessage;

                public Response(long requestId, global::Dinopark.Npc.EatResponse payload)
                {
                    RequestId = requestId;
                    Payload = payload;
                    FailureMessage = null;
                }

                public Response(long requestId, string failureMessage)
                {
                    RequestId = requestId;
                    Payload = null;
                    FailureMessage = failureMessage;
                }
            }

            public readonly struct ReceivedResponse : IReceivedCommandResponse
            {
                public readonly Unity.Entities.Entity SendingEntity;
                public readonly EntityId EntityId;
                public readonly string Message;
                public readonly StatusCode StatusCode;
                public readonly global::Dinopark.Npc.EatResponse? ResponsePayload;
                public readonly global::Dinopark.Npc.EatRequest RequestPayload;
                public readonly global::System.Object Context;
                public readonly long RequestId;

                public ReceivedResponse(
                    Unity.Entities.Entity sendingEntity,
                    EntityId entityId,
                    string message,
                    StatusCode statusCode,
                    global::Dinopark.Npc.EatResponse? response,
                    global::Dinopark.Npc.EatRequest request,
                    global::System.Object context,
                    long requestId)
                {
                    SendingEntity = sendingEntity;
                    EntityId = entityId;
                    Message = message;
                    StatusCode = statusCode;
                    ResponsePayload = response;
                    RequestPayload = request;
                    Context = context;
                    RequestId = requestId;
                }

                long IReceivedCommandResponse.GetRequestId()
                {
                    return RequestId;
                }
            }

            public readonly struct RawReceivedResponse : IRawReceivedCommandResponse
            {
                public readonly EntityId EntityId;
                public readonly string Message;
                public readonly StatusCode StatusCode;
                public readonly global::Dinopark.Npc.EatResponse? ResponsePayload;
                public readonly long RequestId;

                public RawReceivedResponse(
                    EntityId entityId,
                    string message,
                    StatusCode statusCode,
                    global::Dinopark.Npc.EatResponse? response,
                    long requestId)
                {
                    EntityId = entityId;
                    Message = message;
                    StatusCode = statusCode;
                    ResponsePayload = response;
                    RequestId = requestId;
                }
            }
        }
    }
}
