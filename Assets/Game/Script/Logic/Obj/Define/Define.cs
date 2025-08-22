using UnityEngine;

namespace GameDefine
{
    public enum ObjType
    {
        Default,
        Player,
        Effect,
        Item,
        HomePlayer,
        TCActor, // 可时间控制的实体对象
        TCNPC, // 时间控制副本的npc
    }
    
    public class Define
    {
        public static readonly  LayerMask LadderLayer = LayerMask.GetMask("Ladder");
        public static readonly  LayerMask ElevatorLayer = LayerMask.GetMask("Elevator");
    }
}