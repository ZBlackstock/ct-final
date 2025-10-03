using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class PostProcessingManager : MonoBehaviour
{
    public VolumeProfile profile;

    private void Awake()
    {
        SetDepthOfField(1);
    }

    public void SetDepthOfField(float targetFocalLength)
    {
        if (profile)
        {
            DepthOfField depthField;

            if (profile.TryGet(out depthField))
            {
                depthField.focalLength.value = targetFocalLength;
            }
        }
    }

    public void SetVignette(bool enable)
    {
        if (profile)
        {
            Vignette vignette;

            if (profile.TryGet(out vignette))
            {
                vignette.active = enable;   
            }
        }
    }
}
