using UnityEngine;
using UnityEngine.Audio;

namespace MaxHelpers
{
    public class AudioManager : StaticInstance<AudioManager>
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource soundsSource;
        [SerializeField] private AudioMixer audioMixer;
        
        public void PlayMusic(AudioClip clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }

        public void PlaySound(AudioClip clip, Vector3 pos, float vol = 1)
        {
            soundsSource.transform.position = pos;
            PlaySound(clip, vol);
        }
        
        public void PlaySound(AudioClip clip, float vol = 1) => soundsSource.PlayOneShot(clip, vol);

        public void SetVolume(string mixerName, float volume) => audioMixer.SetFloat(mixerName, Mathf.Log10(volume) * 20);
    }
}