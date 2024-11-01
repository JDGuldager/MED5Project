using UnityEngine;

public class ResetPositionAfterSpecificAnimations : MonoBehaviour
{
    public Animator animator;
    public string firstAnimationName;       // Name of the first animation to check
    public string secondAnimationName;      // Name of the second animation to check
    public string boolExercise1;            // Name of the first bool parameter to set to false
    public string boolExercise2;            // Name of the second bool parameter to set to false
    public string finishTriggerName = "AnimationFinished"; // Trigger to set when animation finishes

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool animationEnded = false;

    public ShoulderAngles shoulderAngles;
    public ForearmRotationExercise ForearmRotationExercise;

    void Start()
    {
        // Store the initial position and rotation
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        // Get the current state information for the base layer (layer 0)
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Check if the first or second animation is playing and has finished
        if (stateInfo.normalizedTime >= 1f && !animator.IsInTransition(0))
        {
            // Check if the first animation has finished
            if (stateInfo.IsName(firstAnimationName) && !animationEnded)
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
            // Check if the second animation has finished
            else if (stateInfo.IsName(secondAnimationName) && !animationEnded)
            {
                ResetPosition();
                animationEnded = true;

                // Set the bool parameter for the second exercise to false and trigger forearm rotation activation
                if (!string.IsNullOrEmpty(boolExercise2))
                {
                    animator.SetBool(boolExercise2, false);
                    ForearmRotationExercise.ToggleActivation();
                }

                // Set the trigger to indicate the animation is finished
                animator.SetTrigger(finishTriggerName);
            }
        }
        else if (!stateInfo.IsName(firstAnimationName) && !stateInfo.IsName(secondAnimationName))
        {
            // Reset the flag if we're not in either of the target animation states
            animationEnded = false;
        }
    }

    private void ResetPosition()
    {
        // Reset the character to the original starting position and rotation
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }
}
