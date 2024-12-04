using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShoulderAngles : MonoBehaviour
{
    [Header("Other Scripts")]
    public CharacterDialogue CharacterDialogue;

    public enum ArmSelection { Both, Left, Right }
    [Header("Selected Arm")]
    public ArmSelection selectedArm = ArmSelection.Both; // Default to training both arms

    // Transforms for shoulders and hands
    [Header("Transforms")]
    public Transform leftShoulder;
    public Transform rightShoulder;
    public Transform leftHand;
    public Transform rightHand;

    // UI text elements to display arm elevation angles
    [Header("Text")]
    public TMPro.TextMeshProUGUI leftArmAngleText;
    public TMPro.TextMeshProUGUI rightArmAngleText;
    public TMPro.TextMeshProUGUI repetetionsText;

    // Calibration offset to set baseline 0° at rest and 180° when fully raised
    [Header("Offsets")]
    // Offset values to fine-tune shoulder position (if needed)
    public Vector3 leftShoulderOffset = Vector3.zero;
    public Vector3 rightShoulderOffset = Vector3.zero;
    public float calibrationOffset = 25f;
    public float anglesoundThreshold = 5f;             // Degrees to trigger the regular angle sound
    public float specificAngleTolerance;               // Tolerance range for triggering specific angle sound
    public float specificAngleThreshold = 90f;         // Angle at which specific sound should play

    // Audio clips for angle sound and specific angle threshold sound
    [Header("Sound Clips")]
    public AudioClip angleSound;
    public AudioClip specificAngleSound;
    public AudioClip exerciseCompleted;

    // Audio sources for stereo and separate ear playback
    [Header("Audio Sources")]
    public AudioSource stereoAudioSource;              // For both ears
    public AudioSource leftEarAudioSource;             // For left ear
    public AudioSource rightEarAudioSource;            // For right ear

    // Tracking the last angles for each arm
    private float lastLeftAngle = 0f;
    private float lastRightAngle = 0f;

    [Header("Repetetions")]
    public int repetetionsCompletet = 0;
    public int repetetionAmount = 10;
    public int minReps = 1;
    public int maxReps = 20;

    // Flags to prevent specific angle sound from playing repeatedly
    private bool hasPlayedSpecificSoundLeft = false;
    private bool hasPlayedSpecificSoundRight = false;

    // Flags to track if the arm has previously been below the specific angle threshold
    private bool wasBelowThresholdLeft = false;
    private bool wasBelowThresholdRight = false;

    // Default colors for UI text to reset after angle is reached
    private Color defaultLeftTextColor;
    private Color defaultRightTextColor;

    // Flag to activate/deactivate shoulder angle tracking
    private bool isActivated = false;

    [Header("Controllers")]
    public ActionBasedController leftController;
    public ActionBasedController rightController;

    [Header("GameObjects")]
    public GameObject endExerciseButton;
    public GameObject exercisesButton;
    public GameObject exercisesReturnButton;

    void Start()
    {
        // Store the default text colors for resetting later
        defaultLeftTextColor = leftArmAngleText.color;
        defaultRightTextColor = rightArmAngleText.color;

        // Set up audio sources for 3D sound playback on each side
        leftEarAudioSource.clip = angleSound;
        leftEarAudioSource.spatialBlend = 1.0f; // Full 3D sound
        rightEarAudioSource.clip = angleSound;
        rightEarAudioSource.spatialBlend = 1.0f; // Full 3D sound

        // Initially hide the UI text for arm angles
        leftArmAngleText.gameObject.SetActive(false);
        rightArmAngleText.gameObject.SetActive(false);
        repetetionsText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Check if tracking is active; if not, hide text and return early
        if (!isActivated)
        {
            leftArmAngleText.gameObject.SetActive(false);
            rightArmAngleText.gameObject.SetActive(false);
            repetetionsText.gameObject.SetActive(false);
            return;
        }
        else
        {
            repetetionsText.gameObject.SetActive(true);
        }
        // Show only the relevant arm text based on the selected arm for training
        leftArmAngleText.gameObject.SetActive(selectedArm == ArmSelection.Left || selectedArm == ArmSelection.Both);
        rightArmAngleText.gameObject.SetActive(selectedArm == ArmSelection.Right || selectedArm == ArmSelection.Both);

        // Process left arm if selected
        if (selectedArm == ArmSelection.Left || selectedArm == ArmSelection.Both)
        {
            float leftArmAngle = CalculateArmElevation(leftShoulder, leftHand, leftShoulderOffset);
            UpdateUIAndPlaySound(leftArmAngle, ref lastLeftAngle, true);
        }

        // Process right arm if selected
        if (selectedArm == ArmSelection.Right || selectedArm == ArmSelection.Both)
        {
            float rightArmAngle = CalculateArmElevation(rightShoulder, rightHand, rightShoulderOffset);
            UpdateUIAndPlaySound(rightArmAngle, ref lastRightAngle, false);
        }
        if (repetetionsCompletet == repetetionAmount)
        {
            ToggleActivation();
            EndExercise();
            CharacterDialogue.PlayDialogue(exerciseCompleted);

        }
    }

    // Calculates the angle of arm elevation based on shoulder and hand positions
    float CalculateArmElevation(Transform shoulder, Transform hand, Vector3 shoulderOffset)
    {
        // Adjust shoulder position with offset if needed
        Vector3 adjustedShoulderPosition = shoulder.position + shoulderOffset;

        // Create a vector from shoulder to hand
        Vector3 shoulderToHand = hand.position - adjustedShoulderPosition;

        // Downward vector representing a 0° baseline
        Vector3 down = -Vector3.up;

        // Calculate the angle between the downward vector and shoulder-to-hand vector
        float angle = Vector3.Angle(down, shoulderToHand);

        // Apply calibration offset and clamp result to 0-180°
        return Mathf.Clamp(angle - calibrationOffset, 0, 180);
    }

    // Updates the UI text, plays sounds, and triggers vibrations based on the current arm angle
    void UpdateUIAndPlaySound(float currentAngle, ref float lastAngle, bool isLeftArm)
    {
        repetetionsText.text = repetetionsCompletet + " / " + repetetionAmount;
        // Only update text and play sound if the angle has changed by the threshold amount
        if (Mathf.Abs(currentAngle - lastAngle) >= anglesoundThreshold)
        {
            // Update the UI text and color based on the thresholds
            if (isLeftArm)
            {
                // Update left arm angle text and color
                leftArmAngleText.text = "Left Arm Elevation: " + currentAngle.ToString("F0") + "°";
                leftArmAngleText.color = currentAngle > specificAngleThreshold + specificAngleTolerance
                    ? Color.red  // Above threshold + tolerance
                    : Mathf.Abs(currentAngle - specificAngleThreshold) <= specificAngleTolerance
                        ? Color.green  // Within threshold ± tolerance
                        : defaultLeftTextColor;  // Below threshold
            }
            else
            {
                // Update right arm angle text and color
                rightArmAngleText.text = "Right Arm Elevation: " + currentAngle.ToString("F0") + "°";
                rightArmAngleText.color = currentAngle > specificAngleThreshold + specificAngleTolerance
                    ? Color.red  // Above threshold + tolerance
                    : Mathf.Abs(currentAngle - specificAngleThreshold) <= specificAngleTolerance
                        ? Color.green  // Within threshold ± tolerance
                        : defaultRightTextColor;  // Below threshold
            }

            // Play the angle sound if angle change meets the threshold
            if (isLeftArm) leftEarAudioSource.PlayOneShot(angleSound);
            else rightEarAudioSource.PlayOneShot(angleSound);

            // Update last angle to current for next threshold check
            lastAngle = currentAngle;
        }

        // Play the specific angle sound only if within tolerance and entering the range from below
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

        // Check if the angle is above the threshold for triggering the red range vibration
        bool isInRedRange = currentAngle > specificAngleThreshold + specificAngleTolerance;

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
            // Reset flags and stop vibration when moving out of the red range
            if (isLeftArm)
            {
                hasPlayedSpecificSoundLeft = false;
                wasBelowThresholdLeft = currentAngle < specificAngleThreshold - specificAngleTolerance;
                leftController.SendHapticImpulse(0, 0); // Stop vibration
            }
            else
            {
                hasPlayedSpecificSoundRight = false;
                wasBelowThresholdRight = currentAngle < specificAngleThreshold - specificAngleTolerance;
                rightController.SendHapticImpulse(0, 0); // Stop vibration
            }
        }
    }



    // Toggles the activation of the angle tracking
    public void ToggleActivation()
    {
        repetetionsCompletet = 0;
        isActivated = !isActivated;
    }

    // Sets the selected arm based on a button click (0 = Both, 1 = Left, 2 = Right)
    public void SetSelectedArm(int armIndex) => selectedArm = (ArmSelection)armIndex;

    //Sets the exercise angle to a specific float
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
        // Make sure the user doesn't go bellow 1 rep or above 20
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
