using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using System.Collections.Generic;
using System;

public class ToggleObject : MonoBehaviour
{
    // Public GameObject to be assigned in the Inspector
    public GameObject targetObject;
    public float timeToDisableFor = 0.5f;
    [Range(0f, 1f)]
    public float vibrationIntensity;
    [Range(0f, 1f)]
    public float vibrationDuration;

    public ActionBasedController leftController; // Manually assigned Left XR Controller
    public ActionBasedController rightController; // Manually assigned Right XR Controller

    // Function to be called on an OnClick event
    public void ToggleVisibility()
    {
        // Start the coroutine to toggle the visibility
        StartCoroutine(DisableTemporarily());
        TriggerVibration(rightController);  // Example: Vibrate left controller
    }

    // Coroutine to disable and re-enable the GameObject
    private IEnumerator DisableTemporarily()
    {
        // Disable the GameObject
        targetObject.SetActive(false);

        // Wait for 0.5 seconds
        yield return new WaitForSeconds(timeToDisableFor);

        // Re-enable the GameObject
        targetObject.SetActive(true);
    }
    // Function to simulate vibration on a specific controller
    public void TriggerVibration(ActionBasedController controller)
    {
        if (controller != null && vibrationIntensity > 0f)
        {
            controller.SendHapticImpulse(vibrationIntensity, vibrationDuration);
        }
    }
}


