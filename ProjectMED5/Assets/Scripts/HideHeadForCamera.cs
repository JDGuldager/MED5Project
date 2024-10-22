using UnityEngine;

public class HideHeadForCamera : MonoBehaviour
{
    public Renderer headRenderer;  // Reference to the head mesh renderer
    public Camera vrCamera;        // Reference to the VR camera

    void Start()
    {
        if (headRenderer != null)
        {
            // Disable the head mesh renderer to make the head invisible
            headRenderer.enabled = false;
        }
    }
}