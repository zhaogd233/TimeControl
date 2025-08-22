using System.Collections.Generic;
using Core.Data;
using Core.EventBus;
using Core.NetWork;
using ProtobufPacket;
using EventBus;
using UnityEditorInternal;
using UnityEngine;

namespace Logic.UserData
{
  
        
    public class PlayerRoleData
    {
        public PlayerRoleData()
        {
            guid = 0xFFFFFFFFFFFFFFFFul; 
            type = 0;
            name = "";
            level = 0;
            lastRapidCode = -1;
        }
        public PlayerRoleData(ulong _guid, int _type, string _name, int _level, int _lastRapidCode)
        {
            guid = _guid;
            type = _type;
            name = _name;
            level= _level;
            lastRapidCode = _lastRapidCode;
        }
        public ulong guid;
        public int type;
        public string name;
        public int level;
        public int lastRapidCode;
    }
    
    public class LoginData : DataBase
    {
        public List<PlayerRoleData> loginRoleList = new List<PlayerRoleData>();
        public int lastRapidCode = -1;
      
        protected override void InitData()
        {
        }

        protected override void ClearData()
        {
        }
    }
}