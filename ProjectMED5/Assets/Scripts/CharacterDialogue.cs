using System.Collections;
using UnityEngine;

public class CharacterDialogue : MonoBehaviour
{
    // Public float to control the delay before the character speaks at the start
    public float dialogueDelay = 3f;

    // Audio clip to play at the beginning of the game
    public AudioClip startupDialogueClip;

    // Reference to the AudioSource component
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component (make sure it’s attached to the same GameObject)
        audioSource = GetComponent<AudioSource>();

        // Check if AudioSource is set
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found! Please attach an AudioSource component.");
            return;
        }

        // Play the startup dialogue after a delay if a clip is assigned
        if (startupDialogueClip != null)
        {
            StartCoroutine(StartDialogueAfterDelay(startupDialogueClip));
        }
    }

    // Coroutine to wait and then play the audio with a delay
    private IEnumerator StartDialogueAfterDelay(AudioClip clip)
    {
        // Wait for the specified number of seconds
        yield return new WaitForSeconds(dialogueDelay);

        // Play the dialogue audio if a clip was provided
        PlayDialogue(clip);
    }

    // Public method to play the specified audio clip on demand (for OnClick events)
    public void PlayDialogue(AudioClip clip)
    {
        // Check if an audio clip is provided
        if (clip == null)
        {
            Debug.LogWarning("No audio clip provided! Please assign an audio clip.");
            return;
        }

        // Stop any currently playing audio
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Play the provided audio clip
        audioSource.PlayOneShot(clip);
    }

    // Public method to stop the audio on demand (for OnClick events)
    public void StopDialogue()
    {
        // Stop the audio if it's playing
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
