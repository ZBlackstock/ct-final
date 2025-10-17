using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Menu_Settings : MonoBehaviour
{
    [SerializeField] private GameObject primaryMenu;
    [SerializeField] private GameObject firstSelectedButton;
    private ButtonSelectionHighlight buttonHighlighter;
    [SerializeField] private Slider[] sliders;
    [SerializeField] private Button[] buttons;

    [Header("Music Volume")]
    public AudioMixer mixer_Music;
    [SerializeField] private string exposedMixerParameter_Music;
    [SerializeField] private string playerPrefName_Music;
    [SerializeField] private Slider slider_Music;
    [SerializeField] private float defaultSliderValue_Music;

    [Header("Gameplay Volume")]
    public AudioMixer mixer_Gameplay;
    [SerializeField] private string exposedMixerParameter_Gameplay;
    [SerializeField] private string playerPrefName_Gameplay;
    [SerializeField] private Slider slider_Gameplay;
    [SerializeField] private float defaultSliderValue_Gameplay;
    [SerializeField] private AudioClip buttonHighlight, buttonSelect;
    private SoundManager sound;
    private bool awake;

    private void Awake()
    {
        awake = true;
        buttonHighlighter = FindFirstObjectByType<ButtonSelectionHighlight>();
        sound = FindFirstObjectByType<SoundManager>();
    }

    private void Start()
    {
        Initialise();
        awake = false;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //Check if scene is starting, otherwise buttonHighlighter will get confused
        if (!awake)
        {
            buttonHighlighter.SetEventSystemCurrentButton(firstSelectedButton);
            Initialise();
        }
    }

    private void Initialise()
    {        
        // Music Volume
        if (PlayerPrefs.GetFloat(playerPrefName_Music) == 0)
        {
            PlayerPrefs.SetFloat(playerPrefName_Music, defaultSliderValue_Music); // If playerPref not assigned, assign it as default value
        }
        slider_Music.value = GetVolumeValue_Music();
        SetLevel_Music(PlayerPrefs.GetFloat(playerPrefName_Music));

        // Gameplay Volume
        if (PlayerPrefs.GetFloat(playerPrefName_Gameplay) == 0)
        {
            PlayerPrefs.SetFloat(playerPrefName_Gameplay, defaultSliderValue_Gameplay);
        }
        slider_Gameplay.value = GetVolumeValue_Gameplay();
        SetLevel_Gameplay(PlayerPrefs.GetFloat(playerPrefName_Gameplay));
    }

    //Set Mixer level (used by slider UI and once on scene start)
    public void SetLevel_Music(float value)
    {
        mixer_Music.SetFloat(exposedMixerParameter_Music, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(playerPrefName_Music, value);
    }

    public void SetLevel_Gameplay(float value)
    {
        mixer_Gameplay.SetFloat(exposedMixerParameter_Gameplay, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(playerPrefName_Gameplay, value);
    }

    public float GetVolumeValue_Music()
    {
        float currenVol = PlayerPrefs.GetFloat(playerPrefName_Music);
        return currenVol;
    }

    public float GetVolumeValue_Gameplay()
    {
        float currenVol = PlayerPrefs.GetFloat(playerPrefName_Gameplay);
        return currenVol;
    }

    public void btn_Back()
    {
        primaryMenu.SetActive(true);
        buttonHighlighter.SetEventSystemCurrentButton(buttonHighlighter.firstSelectedButton);
        sound.PlaySound(buttonSelect);

        gameObject.SetActive(false);
    }

    public void PlaySound_ButtonHighlight()
    {
        // Check if scene starting, otherwise highlight sound is heard on start
        if (!awake)
        {
            sound.PlaySound(buttonHighlight);
        }
    }
}
