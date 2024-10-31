using System.Collections;
using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    // Public GameObject to be assigned in the Inspector
    public GameObject targetObject;
    public float timeToDisableFor = 0.5f;

    // Function to be called on an OnClick event
    public void ToggleVisibility()
    {
        // Start the coroutine to toggle the visibility
        StartCoroutine(DisableTemporarily());
    }

    // Coroutine to disable and re-enable the GameObject
    private IEnumerator DisableTemporarily()
    {
        // Disable the GameObject
        targetObject.SetActive(false);

        // Wait for 0.5 seconds
        yield return new WaitForSeconds(timeToDisableFor);

        // Re-enable the GameObject
        targetObject.SetActive(true);
    }
}
