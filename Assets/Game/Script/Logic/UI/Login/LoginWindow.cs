using Core.FSM;
using Logic.FSM;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     TODO
///     ui模块可见 GF_x 框架
/// </summary>
public class LoginWindow : MonoBehaviour
{
    public TMP_InputField ip, port, userAccount;

    public Button loginBtn;

    // Start is called before the first frame update
    private void Start()
    {
        loginBtn.onClick.AddListener(OnClickLogin);
    }

    private void OnDestroy()
    {
        loginBtn.onClick.RemoveAllListeners();
    }

    // Update is called once per frame
    private void OnClickLogin()
    {
        var connectServer = new Fsm_ConnectServer();
        connectServer.ip = ip.text;
        connectServer.port = int.Parse(port.text);
        connectServer.userAccount = userAccount.text;

        FsmManager.Instance.ChangeState(connectServer);
    }
}