// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;

namespace Dinopark.Npc
{
    public partial class DinoAiData
    {
        public class AttackDiffCommandDeserializer : ICommandDiffDeserializer
        {
            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 1;
            }

            public void AddRequestToDiff(CommandRequestOp op, ViewDiff diff)
            {
                var deserializedRequest = global::Dinopark.Npc.AttackRequest.Serialization.Deserialize(op.Request.SchemaData.Value.GetObject());

                var request = new Attack.ReceivedRequest(
                    new EntityId(op.EntityId),
                    op.RequestId,
                    op.CallerWorkerId,
                    op.CallerAttributeSet,
                    deserializedRequest);

                diff.AddCommandRequest(request, ComponentId, 1);
            }

            public void AddResponseToDiff(CommandResponseOp op, ViewDiff diff, CommandMetaDataAggregate commandMetaData)
            {
                global::Dinopark.Npc.AttackResponse? rawResponse = null;
                if (op.StatusCode == StatusCode.Success)
                {
                    rawResponse = global::Dinopark.Npc.AttackResponse.Serialization.Deserialize(op.Response.SchemaData.Value.GetObject());
                }

                var commandContext = commandMetaData.GetContext<global::Dinopark.Npc.AttackRequest>(ComponentId, 1, op.RequestId);
                commandMetaData.MarkIdForRemoval(ComponentId, 1, op.RequestId);

                var response = new Attack.ReceivedResponse(
                    commandContext.SendingEntity,
                    new EntityId(op.EntityId),
                    op.Message,
                    op.StatusCode,
                    rawResponse,
                    commandContext.Request,
                    commandContext.Context,
                    commandContext.RequestId);

                diff.AddCommandResponse(response, ComponentId, 1);
            }
        }

        public class AttackCommandSerializer : ICommandSerializer
        {
            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 1;
            }

            public void Serialize(MessagesToSend messages, SerializedMessagesToSend serializedMessages, CommandMetaData commandMetaData)
            {
                var storage = (AttackCommandsToSendStorage) messages.GetCommandSendStorage(ComponentId, 1);

                var requests = storage.GetRequests();

                for (int i = 0; i < requests.Count; ++i)
                {
                    ref readonly var request = ref requests[i];
                    var context = new CommandContext<global::Dinopark.Npc.AttackRequest>(request.SendingEntity, request.Request.Payload, request.Request.Context, request.RequestId);
                    commandMetaData.AddRequest<global::Dinopark.Npc.AttackRequest>(ComponentId, 1, in context);

                    var schemaCommandRequest = new global::Improbable.Worker.CInterop.SchemaCommandRequest(ComponentId, 1);
                    global::Dinopark.Npc.AttackRequest.Serialization.Serialize(request.Request.Payload, schemaCommandRequest.GetObject());
                    var serializedRequest = new global::Improbable.Worker.CInterop.CommandRequest(schemaCommandRequest);

                    serializedMessages.AddRequest(serializedRequest, 1,
                        request.Request.TargetEntityId.Id, request.Request.TimeoutMillis, request.RequestId);
                }

                var responses = storage.GetResponses();
                for (int i = 0; i < responses.Count; ++i)
                {
                    ref readonly var response = ref responses[i];
                    if (response.FailureMessage != null)
                    {
                        // Send a command failure if the string is non-null.

                        serializedMessages.AddFailure(response.FailureMessage, (uint) response.RequestId);
                        continue;
                    }

                    var schemaCommandResponse = new global::Improbable.Worker.CInterop.SchemaCommandResponse(ComponentId, 1);
                    global::Dinopark.Npc.AttackResponse.Serialization.Serialize(response.Payload.Value, schemaCommandResponse.GetObject());

                    var serializedResponse = new global::Improbable.Worker.CInterop.CommandResponse(schemaCommandResponse);

                    serializedMessages.AddResponse(serializedResponse, (uint) response.RequestId);
                }
            }
        }

        public class EatDiffCommandDeserializer : ICommandDiffDeserializer
        {
            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 2;
            }

            public void AddRequestToDiff(CommandRequestOp op, ViewDiff diff)
            {
                var deserializedRequest = global::Dinopark.Npc.EatRequest.Serialization.Deserialize(op.Request.SchemaData.Value.GetObject());

                var request = new Eat.ReceivedRequest(
                    new EntityId(op.EntityId),
                    op.RequestId,
                    op.CallerWorkerId,
                    op.CallerAttributeSet,
                    deserializedRequest);

                diff.AddCommandRequest(request, ComponentId, 2);
            }

            public void AddResponseToDiff(CommandResponseOp op, ViewDiff diff, CommandMetaDataAggregate commandMetaData)
            {
                global::Dinopark.Npc.EatResponse? rawResponse = null;
                if (op.StatusCode == StatusCode.Success)
                {
                    rawResponse = global::Dinopark.Npc.EatResponse.Serialization.Deserialize(op.Response.SchemaData.Value.GetObject());
                }

                var commandContext = commandMetaData.GetContext<global::Dinopark.Npc.EatRequest>(ComponentId, 2, op.RequestId);
                commandMetaData.MarkIdForRemoval(ComponentId, 2, op.RequestId);

                var response = new Eat.ReceivedResponse(
                    commandContext.SendingEntity,
                    new EntityId(op.EntityId),
                    op.Message,
                    op.StatusCode,
                    rawResponse,
                    commandContext.Request,
                    commandContext.Context,
                    commandContext.RequestId);

                diff.AddCommandResponse(response, ComponentId, 2);
            }
        }

        public class EatCommandSerializer : ICommandSerializer
        {
            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 2;
            }

            public void Serialize(MessagesToSend messages, SerializedMessagesToSend serializedMessages, CommandMetaData commandMetaData)
            {
                var storage = (EatCommandsToSendStorage) messages.GetCommandSendStorage(ComponentId, 2);

                var requests = storage.GetRequests();

                for (int i = 0; i < requests.Count; ++i)
                {
                    ref readonly var request = ref requests[i];
                    var context = new CommandContext<global::Dinopark.Npc.EatRequest>(request.SendingEntity, request.Request.Payload, request.Request.Context, request.RequestId);
                    commandMetaData.AddRequest<global::Dinopark.Npc.EatRequest>(ComponentId, 2, in context);

                    var schemaCommandRequest = new global::Improbable.Worker.CInterop.SchemaCommandRequest(ComponentId, 2);
                    global::Dinopark.Npc.EatRequest.Serialization.Serialize(request.Request.Payload, schemaCommandRequest.GetObject());
                    var serializedRequest = new global::Improbable.Worker.CInterop.CommandRequest(schemaCommandRequest);

                    serializedMessages.AddRequest(serializedRequest, 2,
                        request.Request.TargetEntityId.Id, request.Request.TimeoutMillis, request.RequestId);
                }

                var responses = storage.GetResponses();
                for (int i = 0; i < responses.Count; ++i)
                {
                    ref readonly var response = ref responses[i];
                    if (response.FailureMessage != null)
                    {
                        // Send a command failure if the string is non-null.

                        serializedMessages.AddFailure(response.FailureMessage, (uint) response.RequestId);
                        continue;
                    }

                    var schemaCommandResponse = new global::Improbable.Worker.CInterop.SchemaCommandResponse(ComponentId, 2);
                    global::Dinopark.Npc.EatResponse.Serialization.Serialize(response.Payload.Value, schemaCommandResponse.GetObject());

                    var serializedResponse = new global::Improbable.Worker.CInterop.CommandResponse(schemaCommandResponse);

                    serializedMessages.AddResponse(serializedResponse, (uint) response.RequestId);
                }
            }
        }

    }
}
