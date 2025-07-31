using TVA;
using UnityEngine;

public class ActiveStateTCable : TCableBase<bool>
{
    protected override void InitTCObj()
    {
        Initialized(TCManager.Instance.TrackTime, Time.fixedDeltaTime);
    }

    protected override bool GetCurTrackData(float rate)
    {
        return gameObject.activeInHierarchy;
    }

    protected override void RewindAction(bool curValue)
    {
        if(gameObject.activeInHierarchy != curValue)
            gameObject.SetActive(curValue);
    }

    protected override void DestoryCompelety()
    {
    }
}
