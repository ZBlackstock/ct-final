using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetSpriteMaskSprite : MonoBehaviour
{
    private SpriteRenderer sr;
    private SpriteMask sm;

    private void Awake()
    {
        try
        {
            sr = GetComponent<SpriteRenderer>();
            sm = GetComponent<SpriteMask>();
        }
        catch
        {
            Debug.LogError("Could not set spritemask and/or sprite renderer");
        }
    }

    void Update()
    {
        if(sr != null && sm != null)
        {
            if(sr.sprite != null && sm.sprite != sr.sprite)
            {
                sm.sprite = sr.sprite;
            }
        }
    }
}
