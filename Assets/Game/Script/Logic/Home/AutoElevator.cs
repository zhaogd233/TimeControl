using System.Collections.Generic;
using UnityEngine;

namespace Logic.Home
{
    [RequireComponent(typeof(Collider))]
    public class AutoElevator : MonoBehaviour
    {
        [Header("Elevator Settings")]
        public List<Transform> floors;       // 所有楼层
        public float speed = 2f;             // 移动速度
        public float waitTime = 2f;          // 停靠时间
        public bool autoRun = true;          // 是否自动循环运行

        private int currentFloor = 0;        // 当前所在层
        private int targetFloor = 0;         // 目标层
        private int direction = 1;           // 自动运行方向：1 向上，-1 向下
        private float timer;

        private enum ElevatorState
        {
            Moving,
            Waiting
        }

        private ElevatorState state;

        void Start()
        {
            if (floors.Count == 0) return;
            transform.position = floors[0].position;
            state = ElevatorState.Waiting;
            timer = waitTime;
        }

        void Update()
        {
            if (floors.Count == 0) return;

            switch (state)
            {
                case ElevatorState.Moving:
                    MoveElevator();
                    break;
                case ElevatorState.Waiting:
                    timer -= Time.deltaTime;
                    if (timer <= 0)
                    {
                        if (autoRun)
                        {
                            SetNextFloorAuto();
                        }
                        if (currentFloor != targetFloor)
                        {
                            state = ElevatorState.Moving;
                        }
                    }
                    break;
            }
        }

        void MoveElevator()
        {
            Vector3 targetPos = floors[targetFloor].position;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                currentFloor = targetFloor;
                state = ElevatorState.Waiting;
                timer = waitTime;
                // 可在这里触发开门动画或提示音
            }
        }

        /// <summary>
        /// 玩家或系统呼叫电梯到指定层
        /// </summary>
        public void CallElevator(int floorIndex)
        {
            if (floorIndex < 0 || floorIndex >= floors.Count) return;
            targetFloor = floorIndex;
            if (state == ElevatorState.Waiting && currentFloor != targetFloor)
            {
                state = ElevatorState.Moving;
            }
        }

        /// <summary>
        /// 自动运行，选择下一个层
        /// </summary>
        private void SetNextFloorAuto()
        {
            targetFloor = currentFloor + direction;
            if (targetFloor >= floors.Count)
            {
                targetFloor = floors.Count - 2;
                direction = -1;
            }
            else if (targetFloor < 0)
            {
                targetFloor = 1;
                direction = 1;
            }
        }
    }
}
