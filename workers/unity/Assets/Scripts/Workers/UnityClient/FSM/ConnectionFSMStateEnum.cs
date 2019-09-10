using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionFSMStateEnum
{
    [global::System.Serializable]
    public enum StateEnum : uint
    {
        NONE = 0,
        LOGIN = 1,
        CONNECTING = 2,
        CONNECTED = 3,
        PLAYING = 4,
        RESULT = 5,
        DISCONNECTED= 6,
        COUNT = 7,
    }
}
