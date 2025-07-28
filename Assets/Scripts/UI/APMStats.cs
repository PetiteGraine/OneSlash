using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;

public class APMStats : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] private TextMeshProUGUI _apmText;

    [Header("APM calculation")]
    private const float WindowDuration = 60f;
    private readonly List<float> _actionTimestamps = new();
    private float _apm = 0f;
    private float _lastDisplayedApm = -1f;

    public void ActionPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _actionTimestamps.Add(Time.time);
        }
    }

    private void Update()
    {
        float now = Time.time;
        _actionTimestamps.RemoveAll(t => t < now - WindowDuration);

        float effectiveSpan = Mathf.Min(now, WindowDuration);

        _apm = _actionTimestamps.Count / effectiveSpan * 60f;

        if (Mathf.FloorToInt(_apm) != Mathf.FloorToInt(_lastDisplayedApm))
        {
            _apmText.text = $"APM: {_apm:F0}";
            _lastDisplayedApm = _apm;
        }
    }
}
