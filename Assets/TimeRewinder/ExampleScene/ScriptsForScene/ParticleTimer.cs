using UnityEngine;
using UnityEngine.UI;

public class ParticleTimer : MonoBehaviour
{
    [SerializeField] private Text timeText;
    [SerializeField] private GameObject particles;
    private readonly float timerDefault = 5;
    private RewindManager rewindManager;
    public float CurrentTimer { get; set; }

    private void Start()
    {
        CurrentTimer = timerDefault;
        rewindManager = FindObjectOfType<RewindManager>();
    }

    private void Update()
    {
        if (rewindManager.IsBeingRewinded) //Simple solution how to solve Update fighting with FixedUpdate in rewind
            return;


        CurrentTimer -= Time.deltaTime;
        timeText.text = "Time to disable/enable particles: " + CurrentTimer.ToString("0.0");
        if (CurrentTimer < 0)
        {
            particles.SetActive(!particles.activeSelf);
            CurrentTimer = timerDefault;
        }
    }

    public void SetText(float value)
    {
        timeText.text = "Time to disable/enable particles: " + value.ToString("0.0");
    }
}