using TVA;
using UnityEngine;

public class ActiveStateTCable : TCableBase<bool>
{
    public GameObject target;
    protected override void InitTCObj()
    {
        Initialized(TCManager.Instance.TrackTime, Time.fixedDeltaTime);
    }

    protected override bool GetCurTrackData(float rate)
    {
        return target.activeInHierarchy;
    }

    protected override void RewindAction(bool curValue)
    {
        if (target.activeInHierarchy != curValue)
            target.SetActive(curValue);
    }

    protected override void FinishRewindAction(bool rewindValue)
    {
    }


    protected override void DestoryCompelety()
    {
    }
}