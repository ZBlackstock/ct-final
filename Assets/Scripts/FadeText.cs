using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeText : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshPro text;
    [SerializeField] private float fadeRate = 0.01f;

    private void OnEnable()
    {
        if (text == null)
        {
            try
            {
                text = GetComponent<TMPro.TextMeshPro>();
            }
            catch
            {
                Debug.LogError("Didn't work.");
            }
        }
    }

    public void FadeTextUp()
    {
        StartCoroutine(FadeTextCoroutine());
    }

    public void SetTextAlpha(float alpha)
    {
        if (alpha <= 0 && alpha <= 1)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        }
        else
        {
            Debug.LogError("Text alpha (" + alpha + ") out of bounds");
        }
    }

    private IEnumerator FadeTextCoroutine()
    {
        float curAlpha = text.color.a;

        while (curAlpha < 1)
        {
            curAlpha += fadeRate;
            text.color = new Color(text.color.r, text.color.g, text.color.b, curAlpha);

            yield return new WaitForSeconds(Time.deltaTime);
        }

        curAlpha = 1;
        text.color = new Color(text.color.r, text.color.g, text.color.b, curAlpha);
    }
}
