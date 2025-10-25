using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Plays sound when player enters trigger
public class SoundTrigger : MonoBehaviour
{
    private AudioSource audioS;
    private bool played;

    [SerializeField] private float minVol = 0, startVolume = 0f, targetVolume = 0.8f;
    private float currentVolume;
    [SerializeField] private float fadeRate = 0.1f;
    [SerializeField] private bool playSoundOnAwake, fadeVolume = true, destroyOnExit, playOnce;

    private void Awake()
    {
        try
        {
            audioS = GetComponent<AudioSource>();
            audioS.playOnAwake = playSoundOnAwake;
            audioS.volume = startVolume;
            audioS.enabled = false;
        }
        catch
        {
            Debug.LogWarning("No AudioSource attached to SoundTrigger GameObject");
        }
    }

    private void Start()
    {
        currentVolume = startVolume;
        audioS.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && (!playOnce || playOnce && !played))
        {
            if (audioS.clip != null)
            {
                if (fadeVolume)
                {
                    CompareTargetAndCurrentVolume(targetVolume);
                }
                else
                {
                    audioS.volume = targetVolume;
                    audioS.Play();
                }

                if (!played)
                {
                    played = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (audioS.clip != null)
            {
                if (fadeVolume)
                {
                    CompareTargetAndCurrentVolume(minVol);
                }

                if (destroyOnExit)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void CompareTargetAndCurrentVolume(float targetVol)
    {
        StopAllCoroutines();

        if (targetVol > audioS.volume)
        {
            StartCoroutine(FadeClipVolume(true, targetVol));

            if (!playSoundOnAwake)
            {
                audioS.Play();
            }
        }
        else if (targetVol < audioS.volume)
        {
            StartCoroutine(FadeClipVolume(false, minVol));
        }
        else
        {
            Debug.LogWarning("Target volume is equal to current volume");
        }
    }

    private IEnumerator FadeClipVolume(bool fadeUp, float targetVol)
    {
        if (fadeUp)
        {
            while (targetVol > currentVolume)
            {
                yield return new WaitForSeconds(Time.deltaTime);

                currentVolume += fadeRate * Time.deltaTime;
                audioS.volume = currentVolume;
            }
        }
        else
        {
            while (targetVol < currentVolume)
            {
                yield return new WaitForSeconds(Time.deltaTime);

                currentVolume -= fadeRate * Time.deltaTime;
                audioS.volume = currentVolume;
            }
        }

        currentVolume = targetVol;
        audioS.volume = currentVolume;
    }
}
