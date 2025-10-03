using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup gameVolumeMixer;
    public List<AudioSource> audioS = new List<AudioSource>();
    public List<AudioSource> audioSUI = new List<AudioSource>();

    private float soundRandomiser;

    private void Start()
    {
        StartCoroutine(AudioClear());
        StartCoroutine(DestroyUISound());
    }
    private IEnumerator AudioClear()
    {
        while (true)
        {
            if (Time.timeScale != 0)
            {
                if (audioS.Count > 0)
                {
                    foreach (AudioSource audio in audioS.ToArray())
                    {
                        if (audio != null)
                        {
                            if (!audio.isPlaying)
                            {
                                audioS.Remove(audio);
                                Destroy(audio);
                            }
                        }
                    }

                }
            }
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
    private IEnumerator DestroyUISound()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1);

            if (audioSUI.Count > 0)
            {
                for (int i = 0; i < audioSUI.Count; i++)
                {
                    if (!audioSUI[i].isPlaying)
                    {
                        AudioSource temp = audioSUI[i];
                        audioSUI.RemoveAt(i);
                        Destroy(temp);
                    }
                }
            }
        }
    }
    public bool IsSoundPlaying(AudioClip clip)
    {
        foreach(AudioSource audio in audioS.ToArray())
        {
            if(audio.clip == clip)
            {
                return true;
            }
        }
        return false;
    }
    public void PlaySoundRandom(AudioClip[] audioClip, float volume, float pitchMin, float pitchMax)
    {
        soundRandomiser = Random.Range(0, audioClip.Length);

        AudioSource newAudioS = gameObject.AddComponent<AudioSource>();
        audioS.Add(newAudioS);

        newAudioS.volume = volume;
        newAudioS.pitch = Random.Range(pitchMin, pitchMax);
        newAudioS.clip = audioClip[(int)soundRandomiser];
        newAudioS.outputAudioMixerGroup = gameVolumeMixer;
        newAudioS.Play();
    }    
    public void PlaySound(AudioClip audioClip)
    {
        AudioSource newAudioS = gameObject.AddComponent<AudioSource>();
        audioS.Add(newAudioS);

        newAudioS.volume = 1;
        newAudioS.clip = audioClip;
        newAudioS.outputAudioMixerGroup = gameVolumeMixer;
        newAudioS.Play();
    }
    public void PlaySound(AudioClip audioClip, float volume)
    {
        AudioSource newAudioS = gameObject.AddComponent<AudioSource>();
        audioS.Add(newAudioS);

        newAudioS.volume = volume;
        newAudioS.clip = audioClip;
        newAudioS.outputAudioMixerGroup = gameVolumeMixer;
        newAudioS.Play();
    }
    public void PlaySound(AudioClip audioClip, float volume, float pitchMin, float pitchMax)
    {
        AudioSource newAudioS = gameObject.AddComponent<AudioSource>();
        audioS.Add(newAudioS);

        newAudioS.volume = volume;
        newAudioS.pitch = Random.Range(pitchMin, pitchMax);
        newAudioS.clip = audioClip;
        newAudioS.outputAudioMixerGroup = gameVolumeMixer;
        newAudioS.Play();
    }
    public void PlaySoundUI(AudioClip audioClip, float volume, float pitchMin, float pitchMax)
    {
        AudioSource newAudioS = gameObject.AddComponent<AudioSource>();
        audioSUI.Add(newAudioS);

        newAudioS.volume = volume;
        newAudioS.pitch = Random.Range(pitchMin, pitchMax);
        newAudioS.clip = audioClip;
        newAudioS.outputAudioMixerGroup = gameVolumeMixer;
        newAudioS.ignoreListenerPause = true;
        newAudioS.Play();
    }
    public void PlaySoundLoop(AudioClip audioClip, float volume, float pitchMin, float pitchMax)
    {
        AudioSource newAudioS = gameObject.AddComponent<AudioSource>();
        audioS.Add(newAudioS);

        newAudioS.volume = volume;
        newAudioS.loop = true;
        newAudioS.pitch = Random.Range(pitchMin, pitchMax);
        newAudioS.clip = audioClip;
        newAudioS.outputAudioMixerGroup = gameVolumeMixer;
        newAudioS.Play();
    }
    public void PlaySoundLoop(AudioClip audioClip, float volume)
    {
        AudioSource newAudioS = gameObject.AddComponent<AudioSource>();
        audioS.Add(newAudioS);

        newAudioS.volume = volume;
        newAudioS.loop = true;
        newAudioS.clip = audioClip;
        newAudioS.outputAudioMixerGroup = gameVolumeMixer;
        newAudioS.Play();
    }
    public void StopSound(AudioClip audio)
    {
        if (audioS.Count > 0)
        {
            for (int i = 0; i < audioS.Count; i++)
            {
                if (audioS[i].clip == audio)
                {
                    audioS[i].clip = null;
                    audioS[i].Stop();
                }
            }
        }
    }
    public void StopSound(AudioClip[] audio)
    {
        if (audioS.Count > 0)
        {
            for (int i = 0; i < audioS.Count; i++)
            {
                foreach (AudioClip clip in audio)
                {
                    if (audioS[i].clip == clip)
                    {
                        audioS[i].clip = null;
                        audioS[i].Stop();
                        break;
                    }
                }

            }
        }
    }
}
