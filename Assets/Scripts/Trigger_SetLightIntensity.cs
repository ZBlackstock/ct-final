using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Trigger_SetLightIntensity : MonoBehaviour
{
    [SerializeField] private Light2D _light;
    [SerializeField] private float targetIntensity;
    [SerializeField] private float rate = 0.2f;

    [Header("Exit Intensity")]
    [SerializeField] private bool setIntensityOnExit;
    [SerializeField] private float triggerExitIntensity;
    [SerializeField] private float triggerExitRate = 0.2f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _light != null)
        {
            if (_light.intensity != targetIntensity)
            {
                StopAllCoroutines();
                StartCoroutine(FadeLightToTarget(targetIntensity, rate));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _light != null && setIntensityOnExit)
        {
            if (_light.intensity != triggerExitIntensity)
            {
                StopAllCoroutines();
                StartCoroutine(FadeLightToTarget(triggerExitIntensity, triggerExitRate));
            }
        }
    }

    private IEnumerator FadeLightToTarget(float target, float fadeRate)
    {
        if (_light.intensity > target)
        {
            while (_light.intensity > target)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                _light.intensity -= fadeRate;
            }
        }
        else
        {
            while (_light.intensity < target)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                _light.intensity += fadeRate;
            }
        }

        _light.intensity = target;
    }


}
