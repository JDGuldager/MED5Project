using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoulderAngles : MonoBehaviour
{
    public enum ArmSelection { Both, Left, Right }
    public ArmSelection selectedArm = ArmSelection.Both; // Default to training both arms

    // Transforms for shoulders and hands
    public Transform leftShoulder;
    public Transform rightShoulder;
    public Transform leftHand;
    public Transform rightHand;

    // UI text elements to display arm elevation angles
    public TMPro.TextMeshProUGUI leftArmAngleText;
    public TMPro.TextMeshProUGUI rightArmAngleText;

    // Offset values to fine-tune shoulder position (if needed)
    public Vector3 leftShoulderOffset = Vector3.zero;
    public Vector3 rightShoulderOffset = Vector3.zero;

    // Calibration offset to set baseline 0° at rest and 180° when fully raised
    public float calibrationOffset = 25f;

    // Audio clips for angle sound and specific angle threshold sound
    public AudioClip angleSound;
    public AudioClip specificAngleSound;
    public float specificAngleTolerance; // Tolerance range for triggering specific angle sound
    public float specificAngleThreshold = 90f; // Angle at which specific sound should play

    // Audio sources for stereo and separate ear playback
    public AudioSource stereoAudioSource; // For both ears
    public AudioSource leftEarAudioSource; // For left ear
    public AudioSource rightEarAudioSource; // For right ear

    // Tracking the last angles for each arm
    private float lastLeftAngle = 0f;
    private float lastRightAngle = 0f;

    // Flags to prevent specific angle sound from playing repeatedly
    private bool hasPlayedSpecificSoundLeft = false;
    private bool hasPlayedSpecificSoundRight = false;

    // Default colors for UI text to reset after angle is reached
    private Color defaultLeftTextColor;
    private Color defaultRightTextColor;

    // Flag to activate/deactivate shoulder angle tracking
    private bool isActivated = false;

    void Start()
    {
        // Store the default text colors for resetting later
        defaultLeftTextColor = leftArmAngleText.color;
        defaultRightTextColor = rightArmAngleText.color;

        // Set up audio sources for 3D sound playback
        leftEarAudioSource.clip = angleSound;
        leftEarAudioSource.spatialBlend = 1.0f; // Full 3D sound
        rightEarAudioSource.clip = angleSound;
        rightEarAudioSource.spatialBlend = 1.0f; // Full 3D sound

        // Hide UI text initially
        leftArmAngleText.gameObject.SetActive(false);
        rightArmAngleText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Check if tracking is active; if not, exit early
        if (!isActivated)
        {
            leftArmAngleText.gameObject.SetActive(false);
            rightArmAngleText.gameObject.SetActive(false);
            return;
        }

        // Only show relevant arm text based on the selected arm for training
        leftArmAngleText.gameObject.SetActive(selectedArm == ArmSelection.Left || selectedArm == ArmSelection.Both);
        rightArmAngleText.gameObject.SetActive(selectedArm == ArmSelection.Right || selectedArm == ArmSelection.Both);

        // Process the left arm if selected for training
        if (selectedArm == ArmSelection.Left || selectedArm == ArmSelection.Both)
        {
            // Calculate the left arm angle and update UI and sounds
            float leftArmAngle = CalculateArmElevation(leftShoulder, leftHand, leftShoulderOffset);
            UpdateUIAndPlaySound(leftArmAngle, ref lastLeftAngle, true);
        }

        // Process the right arm if selected for training
        if (selectedArm == ArmSelection.Right || selectedArm == ArmSelection.Both)
        {
            // Calculate the right arm angle and update UI and sounds
            float rightArmAngle = CalculateArmElevation(rightShoulder, rightHand, rightShoulderOffset);
            UpdateUIAndPlaySound(rightArmAngle, ref lastRightAngle, false);
        }
    }

    // Calculate the angle of arm elevation based on shoulder and hand position
    float CalculateArmElevation(Transform shoulder, Transform hand, Vector3 shoulderOffset)
    {
        // Adjust shoulder position with offset if needed
        Vector3 adjustedShoulderPosition = shoulder.position + shoulderOffset;

        // Create a vector from shoulder to hand
        Vector3 shoulderToHand = hand.position - adjustedShoulderPosition;

        // Downward vector (representing 0° baseline)
        Vector3 down = -Vector3.up;

        // Calculate the angle between the downward vector and shoulder-to-hand vector
        float angle = Vector3.Angle(down, shoulderToHand);

        // Apply calibration offset and clamp to 0-180°
        return Mathf.Clamp(angle - calibrationOffset, 0, 180);
    }

    // Update UI text and play sounds based on current arm angle
    void UpdateUIAndPlaySound(float currentAngle, ref float lastAngle, bool isLeftArm)
    {
        // Update UI text and color based on angle threshold
        if (isLeftArm)
        {
            // Display left arm angle in text UI
            leftArmAngleText.text = "Left Arm Elevation: " + currentAngle.ToString("F0") + "°";

            // Set text color to green if angle is within specific range; otherwise, reset to default
            leftArmAngleText.color = Mathf.Abs(currentAngle - specificAngleThreshold) <= specificAngleTolerance
                ? Color.green
                : defaultLeftTextColor;
        }
        else
        {
            // Display right arm angle in text UI
            rightArmAngleText.text = "Right Arm Elevation: " + currentAngle.ToString("F0") + "°";

            // Set text color to green if angle is within specific range; otherwise, reset to default
            rightArmAngleText.color = Mathf.Abs(currentAngle - specificAngleThreshold) <= specificAngleTolerance
                ? Color.green
                : defaultRightTextColor;
        }

        // Check if angle has changed by 2 degrees to play sound
        if (Mathf.Abs(currentAngle - lastAngle) >= 2f)
        {
            // Play the angle sound in left or right ear based on which arm is moving
            if (isLeftArm) leftEarAudioSource.PlayOneShot(angleSound);
            else rightEarAudioSource.PlayOneShot(angleSound);

            // Update last angle to current
            lastAngle = currentAngle;
        }

        // Check if the arm is within the specific angle threshold
        bool isWithinSpecificAngle = Mathf.Abs(currentAngle - specificAngleThreshold) <= specificAngleTolerance;

        // Play the specific angle sound if angle is within threshold and hasn't played yet
        if (isWithinSpecificAngle)
        {
            if (isLeftArm && !hasPlayedSpecificSoundLeft)
            {
                stereoAudioSource.PlayOneShot(specificAngleSound);
                hasPlayedSpecificSoundLeft = true;
            }
            else if (!isLeftArm && !hasPlayedSpecificSoundRight)
            {
                stereoAudioSource.PlayOneShot(specificAngleSound);
                hasPlayedSpecificSoundRight = true;
            }
        }
        else
        {
            // Reset the flags when moving out of the specific angle range
            if (isLeftArm) hasPlayedSpecificSoundLeft = false;
            else hasPlayedSpecificSoundRight = false;
        }
    }

    // Method to toggle activation of the script
    public void ToggleActivation() => isActivated = !isActivated;

    // Method to set the selected arm based on button click (0 = Both, 1 = Left, 2 = Right)
    public void SetSelectedArm(int armIndex) => selectedArm = (ArmSelection)armIndex;
}
