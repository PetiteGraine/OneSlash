using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundEffectSource;

    [Header("Audio clips")]
    public AudioClip Music;
    public List<AudioClip> EnemyAttacks = new List<AudioClip>();
    public List<AudioClip> Dashs = new List<AudioClip>();
    public AudioClip SlashA;
    public AudioClip SlashB;
    public AudioClip Death;

    private void Start()
    {
        _musicSource.clip = Music;
        _musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        _soundEffectSource.PlayOneShot(clip);
    }
}