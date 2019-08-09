// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Improbable.Gdk.Core;

namespace Dinopark.Npc
{
    public partial class EggData
    {
        public class HatchCommandMetaDataStorage : ICommandMetaDataStorage, ICommandPayloadStorage<global::Dinopark.Npc.HatchRequest>
        {
            private readonly Dictionary<long, CommandContext<global::Dinopark.Npc.HatchRequest>> requestIdToRequest =
                new Dictionary<long, CommandContext<global::Dinopark.Npc.HatchRequest>>();

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

            public void AddRequest(in CommandContext<global::Dinopark.Npc.HatchRequest> context)
            {
                requestIdToRequest[context.RequestId] = context;
            }

            public CommandContext<global::Dinopark.Npc.HatchRequest> GetPayload(uint internalRequestId)
            {
                var id = internalRequestIdToRequestId[internalRequestId];
                return requestIdToRequest[id];
            }
        }

    }
}
