using UnityEngine;

public class ResetPositionAfterSpecificAnimations : MonoBehaviour
{
    public Animator animator;
    public string firstAnimationLeftName;       // Name of the first animation left to check
    public string firstAnimationRightName;  // Name of the first animation right to check
    public string secondAnimationLeftName;      // Name of the second animation to check
    public string secondAnimationRightName;      // Name of the second animation right to check
    public string boolExercise1;            // Name of the first bool parameter to set to false
    public string boolExercise2;            // Name of the second bool parameter to set to false
    public string finishTriggerName = "AnimationFinished"; // Trigger to set when animation finishes

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool animationEnded = false;

    public ShoulderAngles shoulderAngles;
    public ForearmRotationExercise ForearmRotationExercise;
    public GameObject Fys;
    public ShoulderAngles shoulerAnglesScript;
    public ForearmRotationExercise forearmScript;

    void Start()
    {
        // Store the initial position and rotation
        originalPosition = Fys.transform.position;
        originalRotation = Fys.transform.rotation;

    }

    void Update()
    {
        /*
        // Get the current state information for the base layer (layer 0)
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Check if the first or second animation is playing and has finished
        if (stateInfo.normalizedTime >= 1f && !animator.IsInTransition(0))
        {
            // Check if the first left arm animation has finished
            if (stateInfo.IsName(firstAnimationLeftName) && !animationEnded)
            {
                ResetPosition();
                animationEnded = true;

                // Set the bool parameter for the first exercise to false and trigger shoulder activation
                if (!string.IsNullOrEmpty(boolExercise1))
                {
                    animator.SetBool(boolExercise1, false);
                    shoulderAngles.ToggleActivation();
                }

                // Set the trigger to indicate the animation is finished
                animator.SetTrigger(finishTriggerName);
            }
            // Check if the first animation right arm has finished
            else if (stateInfo.IsName(firstAnimationRightName) && !animationEnded)
            {
                ResetPosition();
                animationEnded = true;

                // Set the bool parameter for the first exercise right arm to false and trigger forearm rotation activation
                if (!string.IsNullOrEmpty(boolExercise2))
                {
                    animator.SetBool(boolExercise2, false);
                    ForearmRotationExercise.ToggleActivation();
                }

                // Set the trigger to indicate the animation is finished
                animator.SetTrigger(finishTriggerName);
            }

            // Check if the second animation left arm has finished
            else if (stateInfo.IsName(secondAnimationLeftName) && !animationEnded)
            {
                ResetPosition();
                animationEnded = true;

                // Set the bool parameter for the second exercise left arm to false and trigger forearm rotation activation
                if (!string.IsNullOrEmpty(boolExercise2))
                {
                    animator.SetBool(boolExercise2, false);
                    ForearmRotationExercise.ToggleActivation();
                }

                // Set the trigger to indicate the animation is finished
                animator.SetTrigger(finishTriggerName);
            }

            // Check if the second animation right arm has finished
            else if (stateInfo.IsName(secondAnimationRightName) && !animationEnded)
            {
                ResetPosition();
                animationEnded = true;

                // Set the bool parameter for the second exercise right arm to false and trigger forearm rotation activation
                if (!string.IsNullOrEmpty(boolExercise2))
                {
                    animator.SetBool(boolExercise2, false);
                    ForearmRotationExercise.ToggleActivation();
                }

                // Set the trigger to indicate the animation is finished
                animator.SetTrigger(finishTriggerName);
            }
        }
        else if (!stateInfo.IsName(firstAnimationLeftName) && !stateInfo.IsName(firstAnimationRightName))
        {
            // Reset the flag if we're not in either of the target animation states
            animationEnded = false;
        }*/
    }

    public void ToggleExer1()
    {
        shoulerAnglesScript.ToggleActivation();
        Debug.Log("Toggle");
    }

    public void ToggleExer2()
    {
        forearmScript.ToggleActivation();
        Debug.Log("Toggle");
    }

    private void ResetPosition()
    {
        // Reset the character to the original starting position and rotation
        Fys.transform.position = originalPosition;
        Fys.transform.rotation = originalRotation;
        Debug.Log("Position Reset");
    }
}
