using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForearmRotationExercise : MonoBehaviour
{
    public enum ArmSelection { Both, Left, Right }
    public ArmSelection selectedArm = ArmSelection.Both;

    public Transform leftElbow; // Reference to the left elbow
    public Transform rightElbow; // Reference to the right elbow
    public Transform leftHand; // Reference to the left hand
    public Transform rightHand; // Reference to the right hand

    public TMPro.TextMeshProUGUI leftForearmAngleText; // UI text for left forearm angle
    public TMPro.TextMeshProUGUI rightForearmAngleText; // UI text for right forearm angle

    public AudioClip angleSound; // Sound for every 2 degrees
    public AudioClip specificAngleSound; // Sound at a specific angle
    public float specificAngleTolerance = 5f; // Tolerance range for specific angle sound
    public float specificAngleThreshold = 45f; // Target angle for specific sound

    public AudioSource stereoAudioSource; // Audio source for both ears
    public AudioSource leftEarAudioSource; // Audio source for left ear
    public AudioSource rightEarAudioSource; // Audio source for right ear

    private float lastLeftAngle = 0f; // Last angle for left forearm
    private float lastRightAngle = 0f; // Last angle for right forearm

    private bool hasPlayedSpecificSoundLeft = false; // Flag to check if specific sound has been played for left arm
    private bool hasPlayedSpecificSoundRight = false; // Flag to check if specific sound has been played for right arm

    private Color defaultLeftTextColor; // Default color for left arm text
    private Color defaultRightTextColor; // Default color for right arm text

    private bool isActivated = false; // Exercise activation toggle

    void Start()
    {
        // Store default text colors for reset
        defaultLeftTextColor = leftForearmAngleText.color;
        defaultRightTextColor = rightForearmAngleText.color;

        // Hide UI text initially
        leftForearmAngleText.gameObject.SetActive(false);
        rightForearmAngleText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Only update if exercise is active
        if (!isActivated)
        {
            leftForearmAngleText.gameObject.SetActive(false);
            rightForearmAngleText.gameObject.SetActive(false);
            return;
        }

        // Set UI visibility based on selected arm
        leftForearmAngleText.gameObject.SetActive(selectedArm == ArmSelection.Left || selectedArm == ArmSelection.Both);
        rightForearmAngleText.gameObject.SetActive(selectedArm == ArmSelection.Right || selectedArm == ArmSelection.Both);

        // Calculate and display angles if relevant
        if (selectedArm == ArmSelection.Left || selectedArm == ArmSelection.Both)
        {
            float leftForearmAngle = CalculateForearmFanAngle(leftElbow, leftHand);
            Debug.Log("Left Forearm Angle: " + leftForearmAngle); // Debug log
            UpdateUIAndPlaySound(leftForearmAngle, ref lastLeftAngle, true);
        }

        if (selectedArm == ArmSelection.Right || selectedArm == ArmSelection.Both)
        {
            float rightForearmAngle = CalculateForearmFanAngle(rightElbow, rightHand);
            Debug.Log("Right Forearm Angle: " + rightForearmAngle); // Debug log
            UpdateUIAndPlaySound(rightForearmAngle, ref lastRightAngle, false);
        }
    }

    // Calculate the fan angle based solely on the elbow and hand positions
    float CalculateForearmFanAngle(Transform elbow, Transform hand)
    {
        // Vector from elbow to hand
        Vector3 elbowToHand = hand.position - elbow.position;

        // Project the vector onto the horizontal plane by zeroing the y-component
        elbowToHand.y = 0;

        // Log the vector for debugging
        Debug.Log("Elbow to Hand: " + elbowToHand);

        // Calculate the angle relative to the forward direction (0 degrees)
        float angle;

        // Check if we are calculating for the left or right arm
        if (hand == leftHand)
        {
            // For left arm, we want the angle to increase anti-clockwise
            angle = Vector3.SignedAngle(elbowToHand, Vector3.forward, Vector3.up); // Reverse arguments for anti-clockwise
        }
        else
        {
            // For right arm, keep the positive rotation as clockwise
            angle = Vector3.SignedAngle(Vector3.forward, elbowToHand, Vector3.up);
        }

        // Normalize angle to 0-360 degrees
        angle = (angle < 0) ? 360 + angle : angle; // Ensure angle is positive
        return angle; // Return the absolute value for a consistent positive angle
    }


    // Update UI text and sound based on calculated angle
    void UpdateUIAndPlaySound(float currentAngle, ref float lastAngle, bool isLeftArm)
    {
        // Update text and color
        if (isLeftArm)
        {
            leftForearmAngleText.text = "Left Forearm Rotation: " + currentAngle.ToString("F0") + "°";
            leftForearmAngleText.color = Mathf.Abs(currentAngle - specificAngleThreshold) <= specificAngleTolerance
                ? Color.green
                : defaultLeftTextColor;
        }
        else
        {
            rightForearmAngleText.text = "Right Forearm Rotation: " + currentAngle.ToString("F0") + "°";
            rightForearmAngleText.color = Mathf.Abs(currentAngle - specificAngleThreshold) <= specificAngleTolerance
                ? Color.green
                : defaultRightTextColor;
        }

        // Play sound every 2 degrees
        if (Mathf.Abs(currentAngle - lastAngle) >= 2f)
        {
            if (isLeftArm) leftEarAudioSource.PlayOneShot(angleSound);
            else rightEarAudioSource.PlayOneShot(angleSound);

            lastAngle = currentAngle;
        }

        // Play specific angle sound within tolerance range
        bool isWithinSpecificAngle = Mathf.Abs(currentAngle - specificAngleThreshold) <= specificAngleTolerance;

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
            // Reset flags when moving out of the specific angle range
            if (isLeftArm) hasPlayedSpecificSoundLeft = false;
            else hasPlayedSpecificSoundRight = false;
        }
    }

    // Toggle exercise activation
    public void ToggleActivation()
    {
        isActivated = !isActivated;
    }

    // Select which arm to train
    public void SetSelectedArm(int armIndex)
    {
        selectedArm = (ArmSelection)armIndex;
    }
}
