using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float _totalTime = 60f;
    private bool _isCountdownTimerOn = false;

    [Header("UI Elements")]
    private float _countdownTime;
    [SerializeField] private TextMeshProUGUI _timerText;

    private TimeSpan _timePlaying;
    private GameObject _gameplayController;

    private void Start()
    {
        _timerText.text = "Time : " + _countdownTime.ToString("F2");
        _gameplayController = GameObject.FindGameObjectWithTag("GameController");
    }

    public void BeginTimer()
    {
        _countdownTime = _totalTime;
        _timerText.text = "Time : " + _countdownTime.ToString("F2");
        _isCountdownTimerOn = true;
        StartCoroutine(UpdateTimer());
    }

    private void EndTimer()
    {
        _isCountdownTimerOn = false;
        _gameplayController.GetComponent<GameController>().GameOver();
    }

    private IEnumerator UpdateTimer()
    {
        while (_isCountdownTimerOn)
        {
            _countdownTime -= Time.deltaTime;
            _timePlaying = TimeSpan.FromSeconds(_countdownTime);
            string timePlayingStr = "Time: " + _timePlaying.ToString(@"ss\.ff");
            _timerText.text = timePlayingStr;
            if (_countdownTime <= 0)
            {
                EndTimer();
            }
            yield return null;
        }
    }
}