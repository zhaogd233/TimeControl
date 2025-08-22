using System.Collections.Generic;
using Core.Controller;
using Core.Data;
using Core.EventBus;
using Core.NetWork;
using Logic.UserData;
using ProtobufPacket;
using UnityEngine;

namespace Logic.Controller
{
    /// <summary>
    /// 处理登录相关的逻辑
    /// </summary>
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// 可以依赖多个data
        /// </summary>
        private LoginData loginData;
        
        protected override void InitProtocol()
        {
            RegisterPacketHandler<GC_SESSION>(OnReceiveSession);
            RegisterPacketHandler<GC_LOGIN_RET>(OnReceiveLogin);
            RegisterPacketHandler<GC_CREATE_ROLE_RET>(OnCreateRole);
            RegisterPacketHandler<GC_SELECT_ROLE_RET>(OnSelectRole);
        }

        protected override void InitListener()
        {
        }

        public override void OnInit()
        {
            base.OnInit();
            loginData = DataManager.Instance.Get<LoginData>();
            
        }

        #region 协议处理相关


        private void OnReceiveSession(GC_SESSION packet)
        {
            //TODO 访问权限越界
            NetworkLogic.SetCanSendPacket(true);
            NetworkLogic.Session = System.Text.Encoding.ASCII.GetBytes(packet.session.ToString());
            NetworkLogic.ExecConnectCB();
        }

        public void SendLoginMsg(string account)
        {
            CG_LOGIN_PAK packet = new CG_LOGIN_PAK();
            packet.data.accountname =account;
            packet.data.gameversion = (int)GameDefine.VERSION.GameVersion;//MainUILogic.Instance().m_GameVersion;
            packet.data.programversion = (int)GameDefine.VERSION.ProgramVersion;//MainUILogic.Instance().m_ProgramVersion;
            packet.data.maxpacketid = (int)MessageID.PACKETID_MAX;
            packet.data.forceenter = true; 

            packet.data.token = "";
            // packet.data.bimac = "Robot";
            packet.data.bios = (int)CG_LOGIN.BIOSTYPE.ANDROID;
            packet.data.logintype = (int)CG_LOGIN.LOGINTYPE.TEST;
            packet.data.deviceid = "000000";
            packet.SendPacket();
        }
        
        private void OnReceiveLogin(GC_LOGIN_RET packet)
        {
            loginData.loginRoleList.Clear();
            for (int i = 0; i < packet.roleguidlist.Count; i++)
            {
                loginData.loginRoleList.Add(new PlayerRoleData(packet.roleguidlist[i], 
                    packet.roleprofessionlist[i], 
                    packet.rolenamelist[i],
                    packet.rolelevellist[i],
                    packet.rapidvalidatecode));
            }
            loginData.lastRapidCode = packet.rapidvalidatecode;
            
            bool result = false;
            if (packet.result == ((int)GC_LOGIN_RET.RESULT.SUCCESS))
                result = true;
            else
            {
                Debug.LogError("login fail" + packet.result);
            }
            
            EventManager.Instance.RaiseEvent(new EventMessages.LoginEventMessage(){result = result});
        }

        public void SendCreateRoleMsg(string userName)
        {
            CG_CREATE_ROLE_PAK packet = new CG_CREATE_ROLE_PAK();
            packet.data.name = userName;
            packet.data.profession = 1; //temp
            packet.data.sex = 1;

            packet.data.defaultbodyvisual = 1;
            packet.data.defaultfacevisual = 1;
            packet.data.defaulthairvisual = 1;
            List<uint> aList = new List<uint>()
            {
                2159399503, 3274444435, 1311601300, 3464224136, 1386149029, 830574228, 2199126115, 3601953710,
                3481102963, 2246393977, 1338827739, 1169853563,
                3650583444, 2356928040, 1671227233, 2438507023, 3927568772, 1030161763, 53036064, 138543632, 30307228,
                3463683303, 98361345, 7, 1, 753008640,
                476061056, 1048672, 753146208, 753146208, 754476256, 0, 12514, 1596896, 1363296, 127184, 0, 0, 0, 0, 0,
                0, 0, 0, 754512000,
            };
            packet.data.NieRenValue = aList;
            //packet.data.activationcode = "1";
            packet.SendPacket();
        }

        private void OnCreateRole(GC_CREATE_ROLE_RET packet)
        {
            bool result = false;
            if (packet.createresult == ((int)GC_LOGIN_RET.RESULT.SUCCESS))
                result = true;
            else
            {
                Debug.LogError("login create fail" + packet.createresult);
            }
            EventManager.Instance.RaiseEvent(new EventMessages.CreateRoleEventMessage(){result = result});
        }
        private void OnSelectRole(GC_SELECT_ROLE_RET packet)
        {
            bool result = false;
            if (packet.result == ((int)GC_LOGIN_RET.RESULT.SUCCESS))
                result = true;
            else
            {
                Debug.LogError("login create fail" + packet.result);
            }
            EventManager.Instance.RaiseEvent(new EventMessages.SelecteRoleEventMessage(){result = result});
        }
        
        public void SendRandomRoleMsg()
        {
            if (!IsHaveRole())
                return;
            ulong selectGuid = loginData.loginRoleList[0].guid;
            int tempLevel = loginData.loginRoleList[0].level;
            for (int i = 0; i < loginData.loginRoleList.Count; ++i)
            {
                if(loginData.loginRoleList[i].level > tempLevel)
                {
                    tempLevel = loginData.loginRoleList[i].level;
                    selectGuid = loginData.loginRoleList[i].guid;
                }
            }
            CG_SELECT_ROLE_PAK packet = new CG_SELECT_ROLE_PAK();
            packet.data.roleguid = selectGuid;
            packet.SendPacket();
        }
        #endregion


        #region 功能相关

        public bool IsHaveRole()
        {
            return loginData.loginRoleList.Count > 0;
        }

        #endregion
    }
}