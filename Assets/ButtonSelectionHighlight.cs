using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelectionHighlight : MonoBehaviour
{
    private RectTransform selectedButtonTrans;
    private GameObject selectedButton;
    [SerializeField] private float swordOffset;
    [SerializeField] private RectTransform[] swords = new RectTransform[2];

    private void Start()
    {
        
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != selectedButton &&
            EventSystem.current.currentSelectedGameObject != null)
        {
            selectedButton = EventSystem.current.currentSelectedGameObject;
            selectedButtonTrans = selectedButton.GetComponent<RectTransform>();
            SetSelectionPosition(selectedButtonTrans.position, selectedButtonTrans.rect.width);
        }

        if(EventSystem.current.currentSelectedGameObject == null && selectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(selectedButton); 
        }
    }

    private void SetSelectionPosition(Vector2 pos, float width)
    {
        swords[0].position = new Vector2(pos.x - (width / 2) - swordOffset, pos.y);
        swords[1].position = new Vector2(pos.x + (width / 2) + swordOffset, pos.y);
    }
}
