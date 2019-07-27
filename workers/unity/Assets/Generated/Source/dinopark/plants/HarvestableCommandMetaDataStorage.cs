// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Improbable.Gdk.Core;

namespace Dinopark.Plants
{
    public partial class Harvestable
    {
        public class HarvestCommandMetaDataStorage : ICommandMetaDataStorage, ICommandPayloadStorage<global::Dinopark.Plants.HarvestRequest>
        {
            private readonly Dictionary<long, CommandContext<global::Dinopark.Plants.HarvestRequest>> requestIdToRequest =
                new Dictionary<long, CommandContext<global::Dinopark.Plants.HarvestRequest>>();

            private readonly Dictionary<uint, long> internalRequestIdToRequestId = new Dictionary<uint, long>();

            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 1;
            }

            public void RemoveMetaData(uint internalRequestId)
            {
                var requestId = internalRequestIdToRequestId[internalRequestId];
                internalRequestIdToRequestId.Remove(internalRequestId);
                requestIdToRequest.Remove(requestId);
            }

            public void SetInternalRequestId(uint internalRequestId, long requestId)
            {
                internalRequestIdToRequestId.Add(internalRequestId, requestId);
            }

            public void AddRequest(in CommandContext<global::Dinopark.Plants.HarvestRequest> context)
            {
                requestIdToRequest[context.RequestId] = context;
            }

            public CommandContext<global::Dinopark.Plants.HarvestRequest> GetPayload(uint internalRequestId)
            {
                var id = internalRequestIdToRequestId[internalRequestId];
                return requestIdToRequest[id];
            }
        }

    }
}
