using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoulderAngles : MonoBehaviour
{
    public Transform leftShoulder; // Set this to the left shoulder or body center for the left arm
    public Transform rightShoulder; // Set this to the right shoulder or body center for the right arm
    public Transform leftHand; // Set this to the left hand (VR controller or hand position)
    public Transform rightHand; // Set this to the right hand (VR controller or hand position)

    public TMPro.TextMeshProUGUI leftArmAngleText; // UI text for left arm angle
    public TMPro.TextMeshProUGUI rightArmAngleText; // UI text for right arm angle

    public Vector3 leftShoulderOffset = Vector3.zero; // Offset for left shoulder adjustment
    public Vector3 rightShoulderOffset = Vector3.zero; // Offset for right shoulder adjustment

    // Calibration offset to ensure 0° at rest and 180° when fully raised
    public float calibrationOffset = 25f; // This can be tuned based on initial observations

    public AudioClip angleSound; // Sound to play for every 2 degrees
    public AudioClip specificAngleSound; // Sound to play at a specific angle
    public float specificAngleTolerance;
    public float specificAngleThreshold = 90f; // The angle at which to play the specific sound

    public AudioSource stereoAudioSource; // AudioSource for the left ear
    public AudioSource leftEarAudioSource; // AudioSource for the left ear
    public AudioSource rightEarAudioSource; // AudioSource for the right ear

    private float lastLeftAngle = 0f; // Last angle for the left arm
    private float lastRightAngle = 0f; // Last angle for the right arm

    private bool hasPlayedSpecificSoundLeft = false;  // To track left arm sound
    private bool hasPlayedSpecificSoundRight = false; // To track right arm sound

    private Color defaultLeftTextColor;
    private Color defaultRightTextColor;

    void Start()
    {
        // Store default text colors
        defaultLeftTextColor = leftArmAngleText.color;
        defaultRightTextColor = rightArmAngleText.color;

        // Set up AudioSources
        leftEarAudioSource.clip = angleSound;
        leftEarAudioSource.spatialBlend = 1.0f; // 3D sound
        rightEarAudioSource.clip = angleSound;
        rightEarAudioSource.spatialBlend = 1.0f; // 3D sound
    }

    void Update()
    {
        // Calculate the angles for both arms
        float leftArmAngle = CalculateArmElevation(leftShoulder, leftHand, leftShoulderOffset);
        float rightArmAngle = CalculateArmElevation(rightShoulder, rightHand, rightShoulderOffset);
        // Reset flags if the angle moves away from the threshold
        if (leftArmAngle > specificAngleThreshold + specificAngleTolerance ||
            leftArmAngle < specificAngleThreshold - specificAngleTolerance)
        {
            hasPlayedSpecificSoundLeft = false; // Reset for left arm
            leftArmAngleText.color = defaultLeftTextColor; // Reset text color to default
        }
        else
        {
            leftArmAngleText.color = Color.green; // Set text color to green if angle is correct
        }

        if (rightArmAngle > specificAngleThreshold + specificAngleTolerance ||
            rightArmAngle < specificAngleThreshold - specificAngleTolerance)
        {
            hasPlayedSpecificSoundRight = false; // Reset for right arm
            rightArmAngleText.color = defaultRightTextColor; // Reset text color to default
        }
        else
        {
            rightArmAngleText.color = Color.green; // Set text color to green if angle is correct
        }

        // Update the UI with the calibrated arm angles
        leftArmAngleText.text = "Left Arm Elevation: " + leftArmAngle.ToString("F0") + "°";
        rightArmAngleText.text = "Right Arm Elevation: " + rightArmAngle.ToString("F0") + "°";

        // Call the sound play method for both arms
        PlaySoundBasedOnAngle(leftArmAngle, ref lastLeftAngle, true);
        PlaySoundBasedOnAngle(rightArmAngle, ref lastRightAngle, false);
    }

    float CalculateArmElevation(Transform shoulder, Transform hand, Vector3 shoulderOffset)
    {
        // Apply the offset to the shoulder position
        Vector3 adjustedShoulderPosition = shoulder.position + shoulderOffset;

        // Vector from adjusted shoulder to hand
        Vector3 shoulderToHand = hand.position - adjustedShoulderPosition;

        // Use Vector pointing downwards (instead of up)
        Vector3 down = -Vector3.up;

        // Calculate the angle between the shoulder-to-hand vector and the downward vector
        float angle = Vector3.Angle(down, shoulderToHand);

        // Apply the calibration offset
        angle = Mathf.Clamp(angle - calibrationOffset, 0, 180); // Clamps the angle to ensure it's between 0 and 180

        return angle;
    }
    void PlaySoundBasedOnAngle(float currentAngle, ref float lastAngle, bool isLeftArm)
    {
        // Check for every 2 degrees
        if (Mathf.Abs(currentAngle - lastAngle) >= 2f)
        {
            if (isLeftArm)
            {
                Debug.Log("Playing sound for left arm at angle: " + currentAngle);
                leftEarAudioSource.PlayOneShot(angleSound); // Play sound in left ear
            }
            else
            {
                Debug.Log("Playing sound for right arm at angle: " + currentAngle);
                rightEarAudioSource.PlayOneShot(angleSound); // Play sound in right ear
            }
            lastAngle = currentAngle; // Update the last angle to the current angle
        }

        // Check for specific angle with tolerance
        if (Mathf.Abs(currentAngle - specificAngleThreshold) <= specificAngleTolerance)
        {
            if (isLeftArm && !hasPlayedSpecificSoundLeft)
            {
                Debug.Log("Playing specific angle sound for left arm at angle: " + currentAngle);
                stereoAudioSource.PlayOneShot(specificAngleSound); // Play in left ear
                hasPlayedSpecificSoundLeft = true; // Mark as played
            }
            else if (!isLeftArm && !hasPlayedSpecificSoundRight)
            {
                Debug.Log("Playing specific angle sound for right arm at angle: " + currentAngle);
                stereoAudioSource.PlayOneShot(specificAngleSound); // Play in right ear
                hasPlayedSpecificSoundRight = true; // Mark as played
            }
        }
    }


}
