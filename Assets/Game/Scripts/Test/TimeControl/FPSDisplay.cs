using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    // 刷新间隔（秒）
    public float updateInterval = 0.5f;

    public bool HUDEnabled = true;

    private float accumulated; // 累积帧率
    private float fps; // 当前FPS
    private int frames; // 累积帧数
    private float timeLeft; // 剩余时间

    private void Start()
    {
        timeLeft = updateInterval;
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        accumulated += Time.timeScale / Time.deltaTime;
        ++frames;

        // 到了刷新间隔
        if (timeLeft <= 0.0)
        {
            fps = accumulated / frames;
            timeLeft = updateInterval;
            accumulated = 0f;
            frames = 0;
        }
    }

    private void OnGUI()
    {
        var style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.white;

        // 根据FPS变色
        if (fps < 30)
            style.normal.textColor = Color.red;
        else if (fps < 60)
            style.normal.textColor = Color.yellow;
        else
            style.normal.textColor = Color.green;

        GUI.Label(new Rect(10, 10, 200, 40), $"FPS: {fps:F1}", style);

        // 按钮大小 & 位置
        var buttonRect = new Rect(10, 50, 100, 40);

        // 按钮文字
        var buttonText = HUDEnabled ? "HUD:ON" : "HUD:OFF";

        // 点击切换状态
        if (GUI.Button(buttonRect, buttonText)) HUDEnabled = !HUDEnabled;
    }
}