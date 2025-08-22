using System;
using Core.Obj;
using Cysharp.Threading.Tasks;
using Logic.Home;
using UnityEngine;

namespace Logic.Obj
{
    public class CreatePlayerParams : IShowObjLogicParams
    {
        public Vector3 bornPos;
    }
    /// <summary>
    /// 基地移动的主角
    /// </summary>
    public class HomePlayerObjLogic : ObjDynamicLogic
    {
        SimpleJoystick joystick;
        private Vector3 joystickForward;
        private float moveSpeed = 3f;
        private float gravitySpeed = 0.3f;
        public float climbSpeed = 3f;
        private float rotationSpeed = 10f;
        CharacterController characterCtrl;
        private Vector3 playerVelocity;
        private bool isGrounded;
        private Vector3 moveStep;
        
        private bool onLadder = false;
        private Ladder  currentLadder;
        private Transform currentElevator;
        private Vector3 lastElevatorPos;
        private Vector3 beginPlayerPos;
        
        /// <summary>
        /// 坑，用的CharacterController，必须等它内部初始化之后，才能move,
        /// </summary>
        private bool canMove = false;
        public override void OnInit(Core.Obj.Obj obj)
        {
            base.OnInit(obj);
            characterCtrl = GetComponent<CharacterController>();
            joystick =  GameObject.Find("UI").transform.Find("JoyStick").GetComponent<SimpleJoystick>();
        }

        public override async void OnShow(IShowObjLogicParams userData)
        {
            base.OnShow(userData);

            CreatePlayerParams playerParams =  (CreatePlayerParams)userData;
            WorldPosition = playerParams.bornPos;
            beginPlayerPos = WorldPosition;
            
            await UniTask.Yield(); // 等待一帧

            // 从下一帧开始才允许 Move
            canMove = true;
            
            Camera.main.GetComponent<CameraController>().target = (this.transform);
            Camera.main.GetComponent<CameraController>().OnFreeViewEvent = OnHomeCameraViewChange;
        }

        public override void OnUpdate(float elapseSeconds)
        {
            base.OnUpdate(elapseSeconds);

            if (!canMove)
                return;
            
            if (onLadder && currentLadder != null && !characterCtrl.isGrounded)
                ClimbLadder(elapseSeconds);
            else
               MoveNormal(elapseSeconds);//摇杆移动&电梯
        }


        private void ClimbLadder(float elapseSeconds)
        {
            float h = joystick.Horizontal;
            float v = joystick.Vertical;
            // --- 如果在梯子上 ---
            Vector3 climbMove = Vector3.zero;
            if (currentLadder.ladderType == LadderType.Vertical)
            {
                climbMove = new Vector3(0, v * climbSpeed * elapseSeconds, 0);
            }
            else if (currentLadder.ladderType == LadderType.Horizontal)
            {
                climbMove = new Vector3(h * climbSpeed * elapseSeconds, 0, 0);
            }

            characterCtrl.Move(climbMove);
        }
        private void MoveNormal(float elapseSeconds)
        {
            float h = joystick.Horizontal;
            float v = joystick.Vertical;
          
            float movePower = joystick.Distance;
            // 计算世界方向（相机对齐）
            Vector3 camForward = Camera.main.transform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = Camera.main.transform.right;
            camRight.y = 0;
            camRight.Normalize();

            Vector3 moveDir = camForward * v + camRight * h;
            moveDir.Normalize();

            // 角色旋转朝向
            ObjTransform.forward = Vector3.Slerp(ObjTransform.forward, moveDir, elapseSeconds * rotationSpeed);

            // 地面移动
            Vector3 move = moveDir * moveSpeed * movePower * elapseSeconds;

            // 重力
            if (characterCtrl.isGrounded)
            {
                playerVelocity.y = 0;
            }
            else
            {
                playerVelocity.y += Physics.gravity.y * elapseSeconds * gravitySpeed;
            }

            move.y = playerVelocity.y;
            
            // 电梯逻辑 和普通移动一样，只不过多了y
            if (currentElevator != null)
            {
                move.y = 0;
                Vector3 delta = currentElevator.position - lastElevatorPos;
                move += delta;
                lastElevatorPos = currentElevator.position;
            }
           
            // 移动
           characterCtrl.Move(move);
        }
        #region 步行梯相关
 
        private void OnTriggerEnter(Collider other)
        {
            int otherLayer = 1 << other.gameObject.layer;
            if ((otherLayer & GameDefine.Define.LadderLayer) != 0)
            {
                onLadder = true;
                currentLadder = other.gameObject.GetComponent<Ladder>();
                // 停止重力或移动脚本
            }else if((otherLayer & GameDefine.Define.ElevatorLayer) != 0)
            {
                onLadder = true;
                currentElevator = other.transform;
                lastElevatorPos = currentElevator.position;
            }
            Debug.Log(other.gameObject.name);
        }

        private void OnTriggerExit(Collider other)
        {
            int otherLayer = 1 << other.gameObject.layer;
            if ((otherLayer & GameDefine.Define.LadderLayer) != 0)
            {
                onLadder = false;
                currentLadder = null;
            }else if((otherLayer & GameDefine.Define.ElevatorLayer) != 0)
            {
                onLadder = false;
                currentElevator = null;
            }
            // 停止重力或移动脚本
            Debug.Log("exit:"+other.gameObject.name);
        }
        #endregion

        private void LateUpdate()
        {
			if(Input.GetKey(KeyCode.R))
			Position = beginPlayerPos;
		}

        private void OnHomeCameraViewChange(bool isFreeView)
        {
            Debug.Log(isFreeView);
                joystick.gameObject.SetActive(!isFreeView);
        }
    }
}