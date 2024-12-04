using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class ForearmRotationExercise : MonoBehaviour
{
    [Header("Other Scripts")]
    public CharacterDialogue CharacterDialogue;
    // Enum to select which arm(s) to train
    public enum ArmSelection { Both, Left, Right }
    [Header("Selected Arm")]
    public ArmSelection selectedArm = ArmSelection.Both; // Default to training both arms

    // Transform references for elbow and hand positions
    [Header("Transforms")]
    public Transform leftElbow;
    public Transform rightElbow;
    public Transform leftHand;
    public Transform rightHand;

    // UI text elements to display forearm rotation angles
    [Header("Text")]
    public TMPro.TextMeshProUGUI leftForearmAngleText;
    public TMPro.TextMeshProUGUI rightForearmAngleText;
    public TMPro.TextMeshProUGUI repetetionsText;

    [Header("Offsets")]
    public float specificAngleTolerance = 5f; // Range around target angle for specific sound
    public float anglesoundThreshold = 5f;    // Angle change threshold to play regular sound
    public float specificAngleThreshold = 45f; // Target angle for specific angle sound

    // Audio clips for regular angle sound and specific angle sound
    [Header("Sound Clips")]
    public AudioClip angleSound;
    public AudioClip specificAngleSound;
    public AudioClip exerciseCompleted;

    // Audio sources for stereo and ear-specific playback
    [Header("Audio Sources")]
    public AudioSource stereoAudioSource;
    public AudioSource leftEarAudioSource;
    public AudioSource rightEarAudioSource;

    // Track the last known angles to detect significant angle changes
    private float lastLeftAngle = 0f;
    private float lastRightAngle = 0f;

    // Repetetions
    [Header("Repetetions")]
    public int repetetionsCompletet = 0;
    public int repetetionAmount = 10;
    public int minReps = 1;
    public int maxReps = 20;

    // Flags to ensure specific angle sound is only played once per range entry
    private bool hasPlayedSpecificSoundLeft = false;
    private bool hasPlayedSpecificSoundRight = false;

    // Flags to track if the angle was below the specific threshold range
    private bool wasBelowThresholdLeft = true;
    private bool wasBelowThresholdRight = true;

    // Default colors for UI text, used for resetting after angle changes
    private Color defaultLeftTextColor;
    private Color defaultRightTextColor;

    // Activation flag for toggling exercise tracking
    private bool isActivated = false;

    // Controllers for haptic feedback
    [Header("Controllers")]
    public ActionBasedController leftController; 
    public ActionBasedController rightController;

    // Gameobjects 
    [Header("GameObjects")]
    public GameObject endExerciseButton;
    public GameObject exercisesButton;
    public GameObject exercisesReturnButton;

    void Start()
    {
        // Store default text colors to reset them later
        defaultLeftTextColor = leftForearmAngleText.color;
        defaultRightTextColor = rightForearmAngleText.color;

        // Hide UI text at the beginning until exercise is activated
        leftForearmAngleText.gameObject.SetActive(false);
        rightForearmAngleText.gameObject.SetActive(false);
    }

    void Update()
    {
        // If exercise is not activated, hide the text and skip further processing
        if (!isActivated)
        {
            leftForearmAngleText.gameObject.SetActive(false);
            rightForearmAngleText.gameObject.SetActive(false);
            repetetionsText.gameObject.SetActive(false);
            return;
        }

        // Show or hide UI text based on selected arm for training
        leftForearmAngleText.gameObject.SetActive(selectedArm == ArmSelection.Left || selectedArm == ArmSelection.Both);
        rightForearmAngleText.gameObject.SetActive(selectedArm == ArmSelection.Right || selectedArm == ArmSelection.Both);
        repetetionsText.gameObject.SetActive(selectedArm == ArmSelection.Right || selectedArm == ArmSelection.Left);

        // Process left arm if selected
        if (selectedArm == ArmSelection.Left || selectedArm == ArmSelection.Both)
        {
            float leftForearmAngle = CalculateForearmFanAngle(leftElbow, leftHand);
            UpdateUIAndPlaySound(leftForearmAngle, ref lastLeftAngle, true);
        }

        // Process right arm if selected
        if (selectedArm == ArmSelection.Right || selectedArm == ArmSelection.Both)
        {
            float rightForearmAngle = CalculateForearmFanAngle(rightElbow, rightHand);
            UpdateUIAndPlaySound(rightForearmAngle, ref lastRightAngle, false);
        }

        if(repetetionsCompletet == repetetionAmount)
        {
            ToggleActivation();
            EndExercise();
            CharacterDialogue.PlayDialogue(exerciseCompleted);
        }
    }

    // Calculate the angle of forearm rotation based on elbow and hand positions
    float CalculateForearmFanAngle(Transform elbow, Transform hand)
    {
        // Create a vector from elbow to hand and project it onto the horizontal plane
        Vector3 elbowToHand = hand.position - elbow.position;
        elbowToHand.y = 0; // Flatten to horizontal plane for angle calculation

        // Determine the angle relative to the forward direction
        float angle;

        if (hand == leftHand)
        {
            // For the left arm, calculate angle anti-clockwise
            angle = Vector3.SignedAngle(elbowToHand, Vector3.forward, Vector3.up);
        }
        else
        {
            // For the right arm, use clockwise direction
            angle = Vector3.SignedAngle(Vector3.forward, elbowToHand, Vector3.up);
        }

        // Convert angle to positive if it is negative
        return (angle < 0) ? 360 + angle : angle;
    }

    void UpdateUIAndPlaySound(float currentAngle, ref float lastAngle, bool isLeftArm)
    {
        repetetionsText.text = repetetionsCompletet + " / "+ repetetionAmount;
        // Only update text and play regular angle sound if angle change meets the threshold
        if (Mathf.Abs(currentAngle - lastAngle) >= anglesoundThreshold)
        {
            // Display the current angle in the UI text and update the color based on thresholds
            if (isLeftArm)
            {
                // Update left forearm UI text and color based on the angle range
                leftForearmAngleText.text = "Left Forearm Rotation: " + currentAngle.ToString("F0") + "°";
                leftForearmAngleText.color = (currentAngle > specificAngleThreshold + specificAngleTolerance && !(currentAngle >= 340f && currentAngle <= 360f))
                    ? Color.red
                    : Mathf.Abs(currentAngle - specificAngleThreshold) <= specificAngleTolerance
                        ? Color.green
                        : defaultLeftTextColor;

                // Play sound in the left ear
                leftEarAudioSource.PlayOneShot(angleSound);
            }
            else
            {
                // Update right forearm UI text and color based on the angle range
                rightForearmAngleText.text = "Right Forearm Rotation: " + currentAngle.ToString("F0") + "°";
                rightForearmAngleText.color = (currentAngle > specificAngleThreshold + specificAngleTolerance && !(currentAngle >= 340f && currentAngle <= 360f))
                    ? Color.red
                    : Mathf.Abs(currentAngle - specificAngleThreshold) <= specificAngleTolerance
                        ? Color.green
                        : defaultRightTextColor;

                // Play sound in the right ear
                rightEarAudioSource.PlayOneShot(angleSound);
            }

            // Update the last recorded angle
            lastAngle = currentAngle;
        }

        // Check if the angle is within the specific range to play a specific angle sound
        bool isWithinSpecificAngle = Mathf.Abs(currentAngle - specificAngleThreshold) <= specificAngleTolerance;

        if (isWithinSpecificAngle)
        {
            if (isLeftArm && !hasPlayedSpecificSoundLeft && wasBelowThresholdLeft)
            {
                stereoAudioSource.PlayOneShot(specificAngleSound);
                hasPlayedSpecificSoundLeft = true;
                repetetionsCompletet++;
            }
            else if (!isLeftArm && !hasPlayedSpecificSoundRight && wasBelowThresholdRight)
            {
                stereoAudioSource.PlayOneShot(specificAngleSound);
                hasPlayedSpecificSoundRight = true;
                repetetionsCompletet++;
            }
        }

        // Check if the angle is above the threshold and in the red range, but not between 340-360 degrees
        bool isInRedRange = currentAngle > specificAngleThreshold + specificAngleTolerance && !(currentAngle >= 340f && currentAngle <= 360f);

        // Trigger vibration if in the red range (excluding 340-360 degrees)
        if (isInRedRange)
        {
            // Calculate the angle difference from the red threshold
            float angleAboveRed = currentAngle - (specificAngleThreshold + specificAngleTolerance);

            // Calculate vibration intensity (min intensity 0.1, max intensity 1.0 at 10 degrees above the red threshold)
            float vibrationIntensity = Mathf.Clamp01(Mathf.Lerp(0.1f, 1f, Mathf.InverseLerp(0f, 10f, angleAboveRed)));

            // Apply vibration based on the current angle
            if (isLeftArm)
            {
                leftController.SendHapticImpulse(vibrationIntensity, 0.2f); // 0.2f is the duration of the vibration
            }
            else
            {
                rightController.SendHapticImpulse(vibrationIntensity, 0.2f);
            }
        }
        else
        {
            // Reset flags when moving out of the red range
            if (isLeftArm)
            {
                hasPlayedSpecificSoundLeft = false;
                wasBelowThresholdLeft = currentAngle < specificAngleThreshold - specificAngleTolerance;
            }
            else
            {
                hasPlayedSpecificSoundRight = false;
                wasBelowThresholdRight = currentAngle < specificAngleThreshold - specificAngleTolerance;
            }

            // Stop vibration when the user is out of the red range
            if (isLeftArm)
            {
                leftController.SendHapticImpulse(0, 0); // Stop vibration when out of range
            }
            else
            {
                rightController.SendHapticImpulse(0, 0); // Stop vibration when out of range
            }
        }
    }



    // Toggle activation of the exercise tracking
    public void ToggleActivation()
    {
        repetetionsCompletet = 0;
        isActivated = !isActivated;
    }

    // Set which arm(s) to train based on input parameter (0 = Both, 1 = Left, 2 = Right)
    public void SetSelectedArm(int armIndex) => selectedArm = (ArmSelection)armIndex;

    public void SetArmAngle(float userInputValue) 
    {
        specificAngleThreshold = userInputValue;
    }
    public void SetRepetetions(int repetetionAmountChosen)
    {
        repetetionAmount = repetetionAmountChosen;
    }

    public void RepsUp()
    {
        // Make sure the user doesn't go bellow minReps rep or above maxReps
        repetetionAmount = Mathf.Clamp(repetetionAmount + 1, minReps, maxReps);
    }
    public void RepsDown()
    {
        // Make sure the user doesn't go bellow 1 rep or above 20
        repetetionAmount = Mathf.Clamp(repetetionAmount - 1, minReps, maxReps);
    }
    public void EndExercise()
    {
        endExerciseButton.SetActive(false);
        exercisesButton.SetActive(true);
        exercisesReturnButton.SetActive(true);
    }
}
