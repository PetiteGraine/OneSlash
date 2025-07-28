using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [Header("Audio mixer")]
    [SerializeField] private AudioMixer _myMixer;

    [Header("UI sliders")]
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    private void Awake()
    {
        setMusicVolume();
        setSFXVolume();
    }

    public void setMusicVolume()
    {
        float volume = _musicSlider.value;
        _myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
    }

    public void setSFXVolume()
    {
        float volume = _sfxSlider.value;
        _myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
    }
}