// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;

namespace Dinopark.Plants
{
    public partial class Harvestable
    {
        public class HarvestDiffCommandDeserializer : ICommandDiffDeserializer
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
                var deserializedRequest = global::Dinopark.Plants.HarvestRequest.Serialization.Deserialize(op.Request.SchemaData.Value.GetObject());

                var request = new Harvest.ReceivedRequest(
                    new EntityId(op.EntityId),
                    op.RequestId,
                    op.CallerWorkerId,
                    op.CallerAttributeSet,
                    deserializedRequest);

                diff.AddCommandRequest(request, ComponentId, 1);
            }

            public void AddResponseToDiff(CommandResponseOp op, ViewDiff diff, CommandMetaDataAggregate commandMetaData)
            {
                global::Dinopark.Plants.HarvestResponse? rawResponse = null;
                if (op.StatusCode == StatusCode.Success)
                {
                    rawResponse = global::Dinopark.Plants.HarvestResponse.Serialization.Deserialize(op.Response.SchemaData.Value.GetObject());
                }

                var commandContext = commandMetaData.GetContext<global::Dinopark.Plants.HarvestRequest>(ComponentId, 1, op.RequestId);
                commandMetaData.MarkIdForRemoval(ComponentId, 1, op.RequestId);

                var response = new Harvest.ReceivedResponse(
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

        public class HarvestCommandSerializer : ICommandSerializer
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
                var storage = (HarvestCommandsToSendStorage) messages.GetCommandSendStorage(ComponentId, 1);

                var requests = storage.GetRequests();

                for (int i = 0; i < requests.Count; ++i)
                {
                    ref readonly var request = ref requests[i];
                    var context = new CommandContext<global::Dinopark.Plants.HarvestRequest>(request.SendingEntity, request.Request.Payload, request.Request.Context, request.RequestId);
                    commandMetaData.AddRequest<global::Dinopark.Plants.HarvestRequest>(ComponentId, 1, in context);

                    var schemaCommandRequest = new global::Improbable.Worker.CInterop.SchemaCommandRequest(ComponentId, 1);
                    global::Dinopark.Plants.HarvestRequest.Serialization.Serialize(request.Request.Payload, schemaCommandRequest.GetObject());
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
                    global::Dinopark.Plants.HarvestResponse.Serialization.Serialize(response.Payload.Value, schemaCommandResponse.GetObject());

                    var serializedResponse = new global::Improbable.Worker.CInterop.CommandResponse(schemaCommandResponse);

                    serializedMessages.AddResponse(serializedResponse, (uint) response.RequestId);
                }
            }
        }

    }
}
