using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetEditorName_ChildSprites : MonoBehaviour
{
    private string newName = "";

    void Update()
    {
        if (!Application.isPlaying && !ChildNamesAreSet())
        {
            GameObject curChild = null;
            SpriteRenderer curChildSR = null;

            for (int i = 0; i < transform.childCount; i++)
            {
                curChild = transform.GetChild(i).gameObject;
                if (curChild.GetComponent<SpriteRenderer>())
                {
                    curChildSR = curChild.GetComponent<SpriteRenderer>();

                    if (curChild.name != curChildSR.sprite.name)
                    {
                        curChild.name = curChildSR.sprite.name;
                    }
                }
            }
        }
    }

    private bool ChildNamesAreSet()
    {
        GameObject curChild = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            curChild = transform.GetChild(i).gameObject;
            if (curChild.GetComponent<SpriteRenderer>())
            {
                if (curChild.name != curChild.GetComponent<SpriteRenderer>().sprite.name)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
