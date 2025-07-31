using System.Collections;
using TVA;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
///     Example how to rewind time with slider Input
/// </summary>
public class RewindBySlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Slider slider;
    [SerializeField] private AudioSource rewindSound;

    [SerializeField] private Button restartTracking;

    [SerializeField] private Button pauseTracking;
    [SerializeField] private Button resumeTracking;

    [SerializeField] private Button rewindPause;
    [SerializeField] private Button rewindResume;
    private int howManyFingersTouching;
    private bool isRewindPaused;

    private Animator sliderAnimator;

    private void Start()
    {
        sliderAnimator = slider.GetComponent<Animator>();
        SetNormalSpeed();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        howManyFingersTouching++;

        if (howManyFingersTouching == 1)
            OnSliderDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        howManyFingersTouching--;

        if (howManyFingersTouching == 0)
            OnSliderUp();
    }

    public void OnSliderUp()
    {
        if (slider.interactable)
        {
            if (!isRewindPaused)
            {
                RewindManager.Instance.StopRewindTimeBySeconds();
                TCManager.Instance.StopRewindTimeBySeconds(); //After rewind is done, correctly stop it
                RestoreSliderAnimation();

                if (RewindManager.Instance.TrackingEnabled)
                    SetNormalSpeed();
            }

            rewindSound.Stop();
        }
    }

    public void OnSliderDown()
    {
        if (slider.interactable)
        {
            if (!isRewindPaused)
                RewindManager.Instance.StartRewindTimeBySeconds(-slider.value *
                                                                RewindManager.Instance
                                                                    .HowManySecondsToTrack); //Start rewind preview (note that slider have negative values, that is why it is passed with minus sign)                                               

            SliderAnimationPause();
            rewindSound.Play();

            TCManager.Instance.StartRewindTimeBySeconds(-slider.value *
                                                        RewindManager.Instance
                                                            .HowManySecondsToTrack); //Start rewind in TCManager
        }
    }

    private void SetNormalSpeed()
    {
        sliderAnimator.speed = 1 / RewindManager.Instance.HowManySecondsToTrack;
    }

    public void OnSliderUpdate(float value)
    {
        TCManager.Instance.SetTimeSecondsInRewind(-value *
                                                  RewindManager.Instance
                                                      .HowManySecondsToTrack); //If slider value changes, change rewind preview state (note that slider have negative values, that is why it is passed with minus sign) 
        RewindManager.Instance.SetTimeSecondsInRewind(-value *
                                                      RewindManager.Instance
                                                          .HowManySecondsToTrack); //If slider value changes, change rewind preview state (note that slider have negative values, that is why it is passed with minus sign) 
    }

    public void SliderAnimationPause() //When rewinding slider animator is paused
    {
        sliderAnimator.speed = 0;
    }

    public void RestoreSliderAnimation() //Slider restoration after user releases it, it will snap back to correct value
    {
        var animationTimeStartFrom = slider.value - slider.minValue;
        sliderAnimator.Play("AutoResizeAnim", 0, animationTimeStartFrom);
        StartCoroutine(ResetSliderValue());
    }

    //Cause slider animator is in fixed update
    private IEnumerator ResetSliderValue()
    {
        yield return new WaitForFixedUpdate();
        slider.value = 0;
    }

    #region Additional controls

    public void RestartTracking()
    {
        slider.value = slider.minValue;
        RewindManager.Instance.RestartTracking();
        RestoreSliderAnimation();
        SetNormalSpeed();

        isRewindPaused = false;

        pauseTracking.interactable = true;
        resumeTracking.interactable = false;

        rewindPause.interactable = true;
        rewindResume.interactable = false;
    }

    public void PauseTracking()
    {
        RewindManager.Instance.TrackingEnabled = false;
        SliderAnimationPause();

        pauseTracking.interactable = false;
        resumeTracking.interactable = true;
    }

    public void ResumeTracking()
    {
        RewindManager.Instance.TrackingEnabled = true;

        if (!isRewindPaused)
            SetNormalSpeed();

        pauseTracking.interactable = true;
        resumeTracking.interactable = false;
    }

    public void PauseRewind()
    {
        RewindManager.Instance.StartRewindTimeBySeconds(0);
        isRewindPaused = true;
        SliderAnimationPause();
        rewindResume.interactable = true;
        rewindPause.interactable = false;
    }

    public void ResumeRewind()
    {
        RewindManager.Instance.StopRewindTimeBySeconds();
        isRewindPaused = false;

        RestoreSliderAnimation();

        if (RewindManager.Instance.TrackingEnabled)
            SetNormalSpeed();

        rewindResume.interactable = false;
        rewindPause.interactable = true;
    }

    #endregion
}