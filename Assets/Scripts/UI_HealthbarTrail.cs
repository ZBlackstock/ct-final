using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Trails behind healthbar when damage is taken
public class UI_HealthbarTrail : MonoBehaviour
{
    [SerializeField] private float currentWidth;
    [SerializeField] private RectTransform healthFillTrans;
    private RectTransform trans;
    private float healthFillWidth;
    [SerializeField] private float trailSpeed = 1f;

    private void Awake()
    {
        trans = GetComponent<RectTransform>();
    }

    void Update()
    {
        healthFillWidth = healthFillTrans.rect.size.x;
        currentWidth = Mathf.Lerp(currentWidth, healthFillWidth, trailSpeed * Time.deltaTime);
        trans.sizeDelta = new Vector2(currentWidth, trans.rect.size.y);
    }
}
