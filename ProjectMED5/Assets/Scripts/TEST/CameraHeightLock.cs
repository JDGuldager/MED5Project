using UnityEngine;
using Unity.XR.CoreUtils; // Needed for XR Origin

public class CameraHeightLock : MonoBehaviour
{
    public Transform characterHead; // Reference to your character’s head position
    public XROrigin xrOrigin;       // Reference to the XR Origin GameObject

    void Update()
    {
        if (characterHead == null || xrOrigin == null) return;

        // Set the XR Origin position to match character's head on the Y axis only
        Vector3 fixedPosition = xrOrigin.transform.position;
        fixedPosition.y = characterHead.position.y; // Match Y position with the head

        // Apply the new fixed position to the XR Origin
        xrOrigin.transform.position = fixedPosition;
    }
}
