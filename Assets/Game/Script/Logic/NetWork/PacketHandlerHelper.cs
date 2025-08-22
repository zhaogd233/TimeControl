using System;
using System.Collections.Generic;
using Core.NetWork;

namespace Network
{
    public class PacketHandlerHelper
    {
        private readonly List<Type> packetHandlers = new List<Type>();
        
        public void RegisterPackerHander<T>(Action<T> handler) where T : IGCPacket
        {
            if(NetManager.Instance.RegisterPacketHandler(handler))
                packetHandlers.Add(typeof(T));
        }

        public void UnRegisterAllHandlers()
        {
            for (int i = 0; i < packetHandlers.Count; i++)
            {
                NetManager.Instance.UnRegisterPacketHandler(packetHandlers[i]);
            }
        }
    }
}