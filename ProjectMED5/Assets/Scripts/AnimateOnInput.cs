using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Serializable class representing a mapping between an input action and an animation property.
[System.Serializable]
public class AnimationInput
{
    // The name of the property in the Animator that will be controlled
    public string animationPropertyName;

    // The input action that will control the animation property
    public InputActionProperty action;
}

// This class animates an object based on input actions from the Unity Input System.
public class AnimateOnInput : MonoBehaviour
{
    // A list of mappings between input actions and animation properties.
    public List<AnimationInput> animationInputs;

    // Reference to the Animator component that controls the animations.
    public Animator animator;

    // Update is called once per frame.
    // This method processes input actions and updates the corresponding animation properties.
    void Update()
    {
        // Loop through each item in the animationInputs list.
        foreach (var item in animationInputs)
        {
            // Read the current value of the input action (as a float).
            float actionValue = item.action.action.ReadValue<float>();

            // Set the float value of the corresponding animation property in the Animator.
            animator.SetFloat(item.animationPropertyName, actionValue);
        }
    }
}
