using Core.Controller;
using Core.Obj;
using GameDefine;
using Logic.Obj;
using UnityEngine;

namespace Logic.Controller
{
    public class HomeController : ControllerBase
    {
        protected override void InitProtocol()
        {
        }

        protected override void InitListener()
        {
        }

        public override void OnInit()
        {
            base.OnInit();
            
            //测试创建 根据服务器返回的数据创建
            CreatePlayerParams param = new CreatePlayerParams();
            param.bornPos = new Vector3(13.36f, 8.19f, 144.5f);
            ObjManager.Instance.ShowObj<HomePlayerObjLogic>(1,"Player",ObjType.HomePlayer,param);
        }
    }
}