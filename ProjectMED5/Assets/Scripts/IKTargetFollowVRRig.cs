using UnityEngine;

// Serializable class that allows for mapping between VR tracking targets and Inverse Kinematics (IK) targets.
[System.Serializable]
public class VRMap
{
    // The VR tracking target 
    public Transform vrTarget;

    // The corresponding IK target that will follow the VR target's position and rotation.
    public Transform ikTarget;

    // Position offset to apply when mapping the VR target to the IK target.
    public Vector3 trackingPositionOffset;

    // Rotation offset to apply when mapping the VR target to the IK target.
    public Vector3 trackingRotationOffset;

    // Maps the VR target's position and rotation to the IK target, applying the position and rotation offsets.
    public void Map()
    {
        // Update the IK target's position based on the VR target's position and the applied offset.
        ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);

        // Update the IK target's rotation based on the VR target's rotation and the applied offset.
        ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

// Class that manages the IK targets for the VR rig, making sure they follow the VR controllers and head in the virtual world.
public class IKTargetFollowVRRig : MonoBehaviour
{
    // Smoothness factor to control how quickly the body rotation follows the head's rotation (ranges from 0 to 1).
    [Range(0, 1)]
    public float turnSmoothness = 0.1f;

    // VRMap for the head, containing the VR headset's tracking data and the corresponding IK target.
    public VRMap head;

    // VRMap for the left hand, containing the left VR controller's tracking data and the corresponding IK target.
    public VRMap leftHand;

    // VRMap for the right hand, containing the right VR controller's tracking data and the corresponding IK target.
    public VRMap rightHand;

    // Position offset between the head's position and the body position in the game world.
    public Vector3 headBodyPositionOffset;

    // Yaw (rotation around the vertical axis) offset between the head's orientation and the body's orientation.
    public float headBodyYawOffset;

    // LateUpdate is called once per frame after all Update functions have been called.
    // It's used here to ensure the VR rig's IK targets follow the VR devices smoothly.
    void LateUpdate()
    {
        // Preserve the current y-position of the body.
        Vector3 newPosition = head.ikTarget.position + headBodyPositionOffset;
        newPosition.y = transform.position.y;  // Lock y-axis movement

        // Set the body's position based on the new x and z values while keeping the y constant.
        transform.position = newPosition;

        // Get the yaw (rotation around the vertical axis) of the VR headset.
        float yaw = head.vrTarget.eulerAngles.y;

        // Smoothly rotate the body's orientation to match the VR headset's yaw, controlled by the turnSmoothness factor.
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(transform.eulerAngles.x, yaw, transform.eulerAngles.z),
            turnSmoothness
        );

        // Map the VR headset position/rotation to the corresponding IK target.
        head.Map();

        // Map the left-hand VR controller position/rotation to the corresponding IK target.
        leftHand.Map();

        // Map the right-hand VR controller position/rotation to the corresponding IK target.
        rightHand.Map();
    }

}
