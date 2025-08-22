using Core.Controller;
using Logic.Controller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GMWindow : MonoBehaviour
{
    public TMP_InputField gmContent;
    public Button sendBtn;

    private void Start()
    {
        sendBtn.onClick.AddListener(OnClickSend);
    }

    private void OnDestroy()
    {
        sendBtn.onClick.RemoveAllListeners();
    }

    private void OnClickSend()
    {
        if (string.IsNullOrEmpty(gmContent.text))
            LogModule.LogWarning("the GM command needs to be entered!");
        else
            ControllerManager.Instance.Get<GameControllder>().OnSendGMMsg(gmContent.text);
    }
}