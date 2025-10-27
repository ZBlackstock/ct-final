using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sets an chosen animator variable on a chosen animator to a chosen value
public class Trigger_SetAnimatorVariable : MonoBehaviour
{
    [SerializeField] private Animator anim; // Animator
    enum varTypes { trigger, boolean, integer, floatingPoint }
    [SerializeField] private varTypes varType;

    [SerializeField] private string varName = "variable"; // Get variable name via string
    [SerializeField] private float varValue_number; // For floats & integers
    [SerializeField] private bool varValue_boolean; // For triggers & bools


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            try
            {
                switch (varType) // Find type of variable to change
                {
                    case varTypes.trigger:
                        if (varValue_boolean == true) // Use boolean true = set trigger, false = reset trigger
                        {
                            anim.SetTrigger(varName);
                        }
                        else
                        {
                            anim.ResetTrigger(varName);
                        }
                        break;

                    case varTypes.boolean: // Set anim bool
                        anim.SetBool(varName, varValue_boolean);
                        break;

                    case varTypes.integer: // Parse float to int
                        anim.SetInteger(varName, (int)varValue_number);
                        break;

                    case varTypes.floatingPoint: // Set anim float
                        anim.SetFloat(varName, varValue_number);
                        break;
                }
            }
            catch
            {
                Debug.LogError("Setting animator variable unsuccessful");
            }
        }
    }
}
