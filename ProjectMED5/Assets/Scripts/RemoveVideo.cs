using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class RemoveVideo : MonoBehaviour
{
    public ActionBasedController rightController;
    public ActionBasedController leftController;
    public InputActionProperty actionRight;
    public InputActionProperty actionLeft;
    public float actionValueRight = 0;
    public float actionValueLeft = 0;
    public Vector2 stick;

    private void Update()
    {
        if (gameObject.tag == "Stick")
        {
            stick = actionRight.action.ReadValue<Vector2>();
        }
        else
        {
            actionValueRight = actionRight.action.ReadValue<float>();
            actionValueLeft = actionLeft.action.ReadValue<float>();
        }
       
        if (rightController != null)
        {
            if (actionValueRight == 1 || stick != Vector2.zero)
            {
                gameObject.SetActive(false);
            }
        }
        if (leftController != null)
        {
            if (actionValueLeft == 1 || stick != Vector2.zero)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
