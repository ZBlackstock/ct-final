using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetEditorName_Sound : MonoBehaviour
{
    private string newName = "";
    private AudioSource AudioS;

    void Update()
    {
        if (!Application.isPlaying && gameObject.name != newName)
        {
            if (AudioS == null)
            {
                AudioS = GetComponent<AudioSource>();
            }

            gameObject.name = "Sound_" + AudioS.clip.name;
        }
    }
}
