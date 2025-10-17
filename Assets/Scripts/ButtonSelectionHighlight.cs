using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelectionHighlight : MonoBehaviour
{
    private RectTransform selectedButtonTrans;
    [SerializeField] private GameObject selectedButton;
    public GameObject firstSelectedButton;
    [SerializeField] private float swordOffset;
    [SerializeField] private RectTransform[] swords = new RectTransform[2];
    [SerializeField] private bool rapidAppear;
    [SerializeField] private AudioClip buttonHighlight;
    private SoundManager sound;

    private void Awake()
    {
        sound = FindFirstObjectByType<SoundManager>();
    }

    private void OnEnable()
    {
        try
        {
            foreach (RectTransform sword in swords)
            {
                sword.GetComponent<Animator>().SetBool("rapidAppear", rapidAppear);
            }
            selectedButton = firstSelectedButton;
            SetHighlighterPosition(selectedButton);
            SetSelectionPosition(selectedButtonTrans.position, selectedButtonTrans.rect.width);
        }
        catch
        {
        }

    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != selectedButton &&
            EventSystem.current.currentSelectedGameObject != null)
        {
            selectedButton = EventSystem.current.currentSelectedGameObject;
            SetHighlighterPosition(selectedButton);
            PlaySound_ButtonHighlight();
        }

        if (EventSystem.current.currentSelectedGameObject == null && selectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(selectedButton);
        }
    }

    private void PlaySound_ButtonHighlight()
    {
        sound.PlaySound(buttonHighlight);
    }

    private void SetHighlighterPosition(GameObject button)
    {
        selectedButtonTrans = button.GetComponent<RectTransform>();
        SetSelectionPosition(selectedButtonTrans.position, selectedButtonTrans.rect.width);
    }
    private void SetSelectionPosition(Vector2 pos, float width)
    {
        swords[0].position = new Vector2(pos.x - (width / 2) - swordOffset, pos.y);
        swords[1].position = new Vector2(pos.x + (width / 2) + swordOffset, pos.y);
    }

    public void SetEventSystemCurrentButton(GameObject current)
    {
        EventSystem.current.SetSelectedGameObject(current);
    }
}
