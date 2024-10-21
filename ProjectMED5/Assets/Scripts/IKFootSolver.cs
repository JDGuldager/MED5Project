using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IKFootSolver handles the foot placement for an Inverse Kinematics (IK) system, ensuring that the foot follows terrain properly.
public class IKFootSolver : MonoBehaviour
{
    // Indicates if the foot is moving forward or sideways during the step.
    public bool isMovingForward;

    // The layer that represents the terrain (used for raycasting to detect where the foot should step).
    [SerializeField] LayerMask terrainLayer = default;

    // Reference to the body of the character (used for raycasting to determine step position).
    [SerializeField] Transform body = default;

    // Reference to the other foot's IK solver to coordinate foot movement (so both feet don't move at the same time).
    [SerializeField] IKFootSolver otherFoot = default;

    // Speed at which the foot moves during a step.
    [SerializeField] float speed = 4;

    // Distance the foot must be from its target before taking a new step.
    [SerializeField] float stepDistance = .2f;

    // The forward length of the step.
    [SerializeField] float stepLength = .2f;

    // The sideward length of the step (used when the character is moving sideways).
    [SerializeField] float sideStepLength = .1f;

    // Height of the foot during a step (how much the foot lifts off the ground).
    [SerializeField] float stepHeight = .3f;

    // Offset applied to the foot's position.
    [SerializeField] Vector3 footOffset = default;

    // Offset applied to the foot's rotation.
    public Vector3 footRotOffset;

    // Vertical offset to be applied to the foot position to prevent foot from sinking into the ground.
    public float footYPosOffset = 0.1f;

    // Starting Y offset for the raycast (how high the ray starts from).
    public float rayStartYOffset = 0;

    // Length of the raycast to detect the ground/terrain.
    public float rayLength = 1.5f;

    // Horizontal spacing of the foot from the center of the body.
    float footSpacing;

    // Position and normal vectors to store old, current, and new foot positions and ground normals.
    Vector3 oldPosition, currentPosition, newPosition;
    Vector3 oldNormal, currentNormal, newNormal;

    // Lerp value used to smoothly transition between steps.
    float lerp;

    // Called when the script starts; initializes positions, normals, and lerp value.
    private void Start()
    {
        // Calculate the horizontal spacing of the foot relative to the body.
        footSpacing = transform.localPosition.x;

        // Initialize foot positions to the current transform's position.
        currentPosition = newPosition = oldPosition = transform.position;

        // Initialize ground normals (orientation of the surface the foot is stepping on).
        currentNormal = newNormal = oldNormal = transform.up;

        // Set lerp to 1, meaning no stepping movement is happening at the start.
        lerp = 1;
    }

    // Update is called once per frame.
    void Update()
    {
        // Apply vertical offset to the current foot position and set the rotation.
        transform.position = currentPosition + Vector3.up * footYPosOffset;
        transform.localRotation = Quaternion.Euler(footRotOffset);

        // Create a ray starting from the body position, offset by foot spacing, and cast it downward to detect terrain.
        Ray ray = new Ray(body.position + (body.right * footSpacing) + Vector3.up * rayStartYOffset, Vector3.down);

        // Visualize the ray in the Unity Editor for debugging purposes.
        Debug.DrawRay(body.position + (body.right * footSpacing) + Vector3.up * rayStartYOffset, Vector3.down);

        // Perform a raycast to detect if the ray hits the terrain layer.
        if (Physics.Raycast(ray, out RaycastHit info, rayLength, terrainLayer.value))
        {
            // Check if the distance between the current and new foot positions exceeds the step distance,
            // the other foot is not moving, and the foot is not already stepping (lerp >= 1).
            if (Vector3.Distance(newPosition, info.point) > stepDistance && !otherFoot.IsMoving() && lerp >= 1)
            {
                // Begin a new step by resetting lerp to 0.
                lerp = 0;

                // Calculate the direction for the foot to move towards the new target position.
                Vector3 direction = Vector3.ProjectOnPlane(info.point - currentPosition, Vector3.up).normalized;

                // Calculate the angle between the body forward direction and the step direction to determine movement type.
                float angle = Vector3.Angle(body.forward, body.InverseTransformDirection(direction));

                // If the angle is less than 50 degrees or greater than 130 degrees, the foot is moving forward.
                isMovingForward = angle < 50 || angle > 130;

                // Set the new position and normal based on whether the foot is moving forward or sideways.
                if (isMovingForward)
                {
                    newPosition = info.point + direction * stepLength + footOffset;
                    newNormal = info.normal;
                }
                else
                {
                    newPosition = info.point + direction * sideStepLength + footOffset;
                    newNormal = info.normal;
                }
            }
        }

        // If the foot is in the process of stepping (lerp < 1), interpolate between old and new positions/normals.
        if (lerp < 1)
        {
            // Lerp the foot position between the old and new positions, adding sinusoidal height for a smooth step arc.
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            // Update the current foot position and normal based on the interpolation value.
            currentPosition = tempPosition;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);

            // Increment the lerp value based on time and speed, moving closer to 1 (step completed).
            lerp += Time.deltaTime * speed;
        }
        else
        {
            // Once the step is completed, set the old position and normal to the new ones for the next step.
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }

    // Draw a red sphere at the new foot position for debugging in the Unity Editor.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.1f);
    }

    // Returns true if the foot is currently moving (lerp < 1 indicates a step in progress).
    public bool IsMoving()
    {
        return lerp < 1;
    }
}
