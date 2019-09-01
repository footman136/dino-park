// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Dinopark.Player
{
    
    [global::System.Serializable]
    public struct LayEggResponse
    {
        public bool Result;
        public int ErrorCode;
    
        public LayEggResponse(bool result, int errorCode)
        {
            Result = result;
            ErrorCode = errorCode;
        }
        public static class Serialization
        {
            public static void Serialize(LayEggResponse instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddBool(1, instance.Result);
                }
                {
                    obj.AddInt32(2, instance.ErrorCode);
                }
            }
    
            public static LayEggResponse Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new LayEggResponse();
                {
                    instance.Result = obj.GetBool(1);
                }
                {
                    instance.ErrorCode = obj.GetInt32(2);
                }
                return instance;
            }
        }
    }
    
}
