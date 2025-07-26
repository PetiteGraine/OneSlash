using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _apmStats;
    private bool _isSettingsMenuOpen = false;
    private bool _isAPMStatsDisplay = false;

    public void ToggleSettingsMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isSettingsMenuOpen = !_isSettingsMenuOpen;
            _settingsMenu.SetActive(_isSettingsMenuOpen);
        }
    }

    public void ToggleAPMStats()
    {
        _isAPMStatsDisplay = !_isAPMStatsDisplay;
        _apmStats.SetActive(_isAPMStatsDisplay);
    }
}
