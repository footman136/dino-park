// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Improbable.Gdk.Core;

namespace Dinopark.Fire
{
    public partial class Flammable
    {
        public class IgniteCommandMetaDataStorage : ICommandMetaDataStorage, ICommandPayloadStorage<global::Dinopark.Core.Nothing>
        {
            private readonly Dictionary<long, CommandContext<global::Dinopark.Core.Nothing>> requestIdToRequest =
                new Dictionary<long, CommandContext<global::Dinopark.Core.Nothing>>();

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

            public void AddRequest(in CommandContext<global::Dinopark.Core.Nothing> context)
            {
                requestIdToRequest[context.RequestId] = context;
            }

            public CommandContext<global::Dinopark.Core.Nothing> GetPayload(uint internalRequestId)
            {
                var id = internalRequestIdToRequestId[internalRequestId];
                return requestIdToRequest[id];
            }
        }

        public class ExtinguishCommandMetaDataStorage : ICommandMetaDataStorage, ICommandPayloadStorage<global::Dinopark.Fire.ExtinguishRequest>
        {
            private readonly Dictionary<long, CommandContext<global::Dinopark.Fire.ExtinguishRequest>> requestIdToRequest =
                new Dictionary<long, CommandContext<global::Dinopark.Fire.ExtinguishRequest>>();

            private readonly Dictionary<uint, long> internalRequestIdToRequestId = new Dictionary<uint, long>();

            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 2;
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

            public void AddRequest(in CommandContext<global::Dinopark.Fire.ExtinguishRequest> context)
            {
                requestIdToRequest[context.RequestId] = context;
            }

            public CommandContext<global::Dinopark.Fire.ExtinguishRequest> GetPayload(uint internalRequestId)
            {
                var id = internalRequestIdToRequestId[internalRequestId];
                return requestIdToRequest[id];
            }
        }

        public class SetCanBeIgnitedCommandMetaDataStorage : ICommandMetaDataStorage, ICommandPayloadStorage<global::Dinopark.Fire.SetCanBeIgnitedRequest>
        {
            private readonly Dictionary<long, CommandContext<global::Dinopark.Fire.SetCanBeIgnitedRequest>> requestIdToRequest =
                new Dictionary<long, CommandContext<global::Dinopark.Fire.SetCanBeIgnitedRequest>>();

            private readonly Dictionary<uint, long> internalRequestIdToRequestId = new Dictionary<uint, long>();

            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 3;
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

            public void AddRequest(in CommandContext<global::Dinopark.Fire.SetCanBeIgnitedRequest> context)
            {
                requestIdToRequest[context.RequestId] = context;
            }

            public CommandContext<global::Dinopark.Fire.SetCanBeIgnitedRequest> GetPayload(uint internalRequestId)
            {
                var id = internalRequestIdToRequestId[internalRequestId];
                return requestIdToRequest[id];
            }
        }

    }
}
