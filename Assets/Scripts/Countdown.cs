using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    [Header("Timer settings")]
    [SerializeField] private float _totalTime = 60f;
    private bool _isCountdownTimerOn = false;

    [Header("UI elements")]
    [SerializeField] private Slider _timerSlider;
    private float _countdownTime;
    [SerializeField] private TextMeshProUGUI _timerText;

    [Header("Internal state")]
    private TimeSpan _timePlaying;

    private void Start()
    {
        _timerText.text = _totalTime.ToString("F2");
    }

    public void ResetTimer()
    {
        _countdownTime = _totalTime;
        _timerText.text = _countdownTime.ToString("F2");
        _timerSlider.value = _countdownTime;
    }

    public float GetRemainingTime()
    {
        return _totalTime - _countdownTime;
    }

    public void BeginTimer()
    {
        _isCountdownTimerOn = true;
        StartCoroutine(UpdateTimer());
    }

    public void StopTimer()
    {
        _isCountdownTimerOn = false;
        StopCoroutine(UpdateTimer());
    }

    private void EndTimer()
    {
        _isCountdownTimerOn = false;
        GetScripts.GameControllerScript.GetComponent<GameController>().GameOver();
        StopCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (_isCountdownTimerOn)
        {
            _countdownTime -= Time.deltaTime;
            _timePlaying = TimeSpan.FromSeconds(_countdownTime);
            string timePlayingStr = _timePlaying.ToString(@"ss\.ff");
            _timerText.text = timePlayingStr;
            _timerSlider.value = _countdownTime;
            if (_countdownTime <= 0)
            {
                EndTimer();
            }
            yield return null;
        }
    }
}