using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class APMStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _apmText;
    private float _apm = 0f;
    private List<float> _actionTimestamps = new List<float>();

    public void ActionPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _actionTimestamps.Add(Time.time);
        }
    }

    private void Update()
    {
        float currentTime = Time.time;
        _actionTimestamps.RemoveAll(t => t < currentTime - 60f);

        float timeSpan = Mathf.Min(currentTime, 60f);
        _apm = _actionTimestamps.Count / timeSpan * 60f;

        _apmText.text = $"APM: {_apm:F0}";
    }
}
