using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] private GameObject[] _proModeElements;
    [SerializeField] private GameObject[] _normalModeElements;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _apmStats;

    [Header("State flags")]
    private bool _isSettingsMenuOpen = false;
    private bool _isAPMStatsDisplay = false;

    public void ToggleSettingsMenuWithInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isSettingsMenuOpen = !_isSettingsMenuOpen;
            _settingsMenu.SetActive(_isSettingsMenuOpen);
        }
    }

    public void ToggleSettingsMenu()
    {
        _isSettingsMenuOpen = !_isSettingsMenuOpen;
        _settingsMenu.SetActive(_isSettingsMenuOpen);
    }

    public void OpenSettingsMenu()
    {
        _isSettingsMenuOpen = true;
        _settingsMenu.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        _isSettingsMenuOpen = false;
        _settingsMenu.SetActive(false);
    }

    public void ToggleAPMStats()
    {
        _isAPMStatsDisplay = !_isAPMStatsDisplay;
        _apmStats.SetActive(_isAPMStatsDisplay);
    }

    public void ToggleProMode()
    {
        GameController.ProModeEnabled = !GameController.ProModeEnabled;
        foreach (var element in _proModeElements)
        {
            element.SetActive(GameController.ProModeEnabled);
        }
        foreach (var element in _normalModeElements)
        {
            element.SetActive(!GameController.ProModeEnabled);
        }
    }
}
