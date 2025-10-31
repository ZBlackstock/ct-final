using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Displays NPC dialogue as multiple texts strings.
public class DialogueBox : MonoBehaviour
{
    private Container container;

    [SerializeField] private TMPro.TextMeshPro skipText;
    [SerializeField] private DisplayDialogueBox dialogueBoxController;
    [SerializeField] private FadeText nameTextFade, skipTextFade;
    [SerializeField] private float dialogueOpenSoundVolume = 0.75f;

    private List<TMPro.TextMeshPro> texts = new List<TMPro.TextMeshPro>();
    [HideInInspector] public Vector3 spawnTrans;
    private InputDetection input;
    private List<int> charCounts = new List<int>();
    private List<string> lineStores = new List<string>();
    private bool skipLine;
    private Animator anim;
    private float realTimeSinceStartup;

    [SerializeField] private AudioClip dialogueTyping;

    private void Awake()
    {
        container = FindFirstObjectByType<Container>();
        input = FindFirstObjectByType<InputDetection>();
        anim = transform.GetChild(0).GetComponent<Animator>();

        Transform dialogues = transform.GetChild(0).transform.GetChild(0);

        for (int i = 0; i < dialogues.childCount; i++)
        {
            try
            {
                TMPro.TextMeshPro curText = dialogues.GetChild(i).GetComponent<TMPro.TextMeshPro>();
                texts.Add(curText);
            }
            catch
            {
                Debug.LogError(gameObject.name + ": Could not add TextMeshPro to dialogue text list");
            }
        }

        for (int i = 0; i < texts.Count; i++)
        {
            int tempCharCount = texts[i].text.Length;
            string tempLineStore = texts[i].text;

            charCounts.Add(tempCharCount);
            lineStores.Add(tempLineStore);
        }

        if (spawnTrans == null)
        {
            Debug.LogWarning("Dialogue box spawnTrans is null");
        }

        gameObject.SetActive(false);
    }

    void Start()
    {
        transform.position = spawnTrans;
        nameTextFade.SetTextAlpha(0);
        skipTextFade.SetTextAlpha(0);
    }

    void Update()
    {
        realTimeSinceStartup = Time.realtimeSinceStartup;

        transform.position = spawnTrans;

        nextText();
        DetectInput();
    }

    private void DetectInput()
    {
        if (input.GetCurrentInput() == "keyboard")
        {
            skipText.text = "[E] Skip";
        }
        else if (input.GetCurrentInput() == "xbox")
        {
            skipText.text = "(Y) Skip";
        }
    }

    private bool nextText()
    {
        if (Input.GetButtonDown("Interact"))
        {
            container.sounds.PlaySound(container.sounds.UI_Dialogue_Skip, 1);
            skipLine = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator DisplayText()
    {
        container.sounds.PlaySound(container.sounds.UI_Vocal_NightmareDweller, dialogueOpenSoundVolume);

        foreach (TMPro.TextMeshPro text in texts)
        {
            text.text = "";
        }

        float timer = 0;
        while (timer < anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Base Layer")).length)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        nameTextFade.FadeTextUp();
        skipTextFade.FadeTextUp();


        for (int i = 0; i < texts.Count; i++) // Texts
        {
            container.sounds.PlaySoundLoop(container.sounds.UI_Dialogue_Typing, 0.8f, 0.975f, 1f);

            texts[i].text = "";
            char currentChar = ' ';

            for (int j = 0; j < charCounts[i]; j++) // Text string
            {
                currentChar = lineStores[i][j];
                texts[i].text += currentChar;

                if (skipLine && j > 0) // Skip line generation
                {
                    container.sounds.StopSound(container.sounds.UI_Dialogue_Typing);
                    texts[i].text = lineStores[i];
                    j = charCounts[i];
                }

                skipLine = false;

                float currentTimeSinceStartup = realTimeSinceStartup;
                yield return new WaitUntil(() => realTimeSinceStartup >= (currentTimeSinceStartup + 0.025f));
            }

            container.sounds.StopSound(container.sounds.UI_Dialogue_Typing);
            yield return new WaitUntil(() => nextText()); // Next text 
            texts[i].text = "";
        }

        StartCoroutine(DisableDialogueBox());
    }

    public void ResetDialogue()
    {
        container.sounds.StopSound(container.sounds.UI_Dialogue_Typing);

        StopCoroutine(DisplayText());
        nameTextFade.SetTextAlpha(0);
    }

    public void StartDialogue()
    {

        container.sounds.PlaySound(container.sounds.UI_Dialogue_Open, 1);
        foreach (TMPro.TextMeshPro text in texts)
        {
            text.enabled = true;
        }
        StartCoroutine(DisplayText());
    }

    public IEnumerator DisableDialogueBox()
    {
        nameTextFade.SetTextAlpha(0);
        skipTextFade.SetTextAlpha(0);
        foreach (TMPro.TextMeshPro text in texts)
        {
            text.enabled = false;
        }
        anim.SetTrigger("close");

        yield return new WaitForSecondsRealtime(0.05f);
        yield return new WaitForSecondsRealtime(anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Base Layer")).length - 0.05f);

        container.sounds.StopSound(container.sounds.UI_Dialogue_Typing);
        container.sounds.PlaySound(container.sounds.UI_Dialogue_Close, 1);

        container.playerController.SetUIOpen(false, false);
        container.playerController.SetDialogueOpen(false);

        gameObject.SetActive(false);
    }
}
