using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monnit.Mine;
using BaseApplication;
using System.Net;

public static class HoloMineServer
{
#if NET_4_6 && !NET_STANDARD_2_0 || UNITY_EDITOR || UNITY_STANDALONE
    public static MineServer CreateMineServer(eMineListenerProtocol pro, string ip, int port)
    {
        return new MineServer(pro, IPAddress.Parse(ip), port);
    }
#endif
}
