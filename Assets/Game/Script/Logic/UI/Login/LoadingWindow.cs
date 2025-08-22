using System;
using System.Collections;
using System.Collections.Generic;
using Core.EventBus;
using TMPro;
using UnityEngine;

public class LoadingWindow : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.SubscribeEvent<EventMessages.ChangeLoadingTips>(setLoadingText);
    }

    // Update is called once per frame
    public void setLoadingText(ref EventMessages.ChangeLoadingTips eventMsg)
    {
        loadingText.text = eventMsg.tips;
    }

    private void OnDestroy()
    {
        EventManager.Instance.UnSubscribeEvent<EventMessages.ChangeLoadingTips>(setLoadingText);
    }
}
