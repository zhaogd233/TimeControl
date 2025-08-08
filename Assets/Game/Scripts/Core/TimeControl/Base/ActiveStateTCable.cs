using TVA;
using UnityEngine;

public class ActiveStateTCable : TCableBase<bool>
{
    public GameObject target;

    protected override void InitTCObj()
    {
        Initialized(TCManager.Instance.TrackTime, Time.fixedDeltaTime, TCManager.Instance.MaxRate, null);
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

    /// <summary>
    ///     当前可见再记录其他的，当前不可见就不记录或者回溯其他的
    /// </summary>
    /// <returns></returns>
    protected override bool CheckMainValid()
    {
        return target.activeInHierarchy;
    }
}