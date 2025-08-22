using UnityEngine;

namespace Logic.Home
{

    public enum LadderType
    {
        Vertical,
        Horizontal
    }

    public class Ladder : MonoBehaviour
    {
        public LadderType ladderType = LadderType.Vertical;
    }
}