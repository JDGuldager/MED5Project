using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class RemoveVideo : MonoBehaviour
{
    public ActionBasedController rightController;
    public InputActionProperty action;
    public float actionValue = 0;
    public Vector2 stick;

    private void Update()
    {
        if (gameObject.tag == "Stick")
        {
            stick = action.action.ReadValue<Vector2>();
        }
        else
        {
            actionValue = action.action.ReadValue<float>();
        }
       
        if (rightController != null)
        {
            if (actionValue == 1 || stick != Vector2.zero)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
