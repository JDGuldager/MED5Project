using System.Collections;
using UnityEngine;

public class CharacterDialogue : MonoBehaviour
{
    // Public float to control the delay before the character speaks
    public float dialogueDelay = 3f;

    // Audio clip that the character will play
    public AudioClip dialogueClip;

    // Reference to the AudioSource component
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component (make sure it’s attached to the same GameObject)
        audioSource = GetComponent<AudioSource>();

        // Check if AudioSource and AudioClip are set
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found! Please attach an AudioSource component.");
            return;
        }

        if (dialogueClip == null)
        {
            Debug.LogError("No audio clip assigned! Please assign an audio clip.");
            return;
        }

        // Start the coroutine to trigger dialogue after a delay
        StartCoroutine(StartDialogueAfterDelay());
    }

    // Coroutine to wait and then play the audio
    private IEnumerator StartDialogueAfterDelay()
    {
        // Wait for the specified number of seconds
        yield return new WaitForSeconds(dialogueDelay);

        // Play the dialogue audio
        TriggerDialogue();
    }

    // Method to handle the dialogue action
    private void TriggerDialogue()
    {
        // Play the assigned audio clip
        audioSource.PlayOneShot(dialogueClip);
    }
}
