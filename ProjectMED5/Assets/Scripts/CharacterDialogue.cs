using System.Collections;
using UnityEngine;

public class CharacterDialogue : MonoBehaviour
{
    // Audio clip to play at the beginning of the game
    public AudioClip startupDialogueClip;

    // Reference to the AudioSource component
    private AudioSource audioSource;

    public GameObject walkTutorial;
    public GameObject rotateTutorial;

    private int count = 0;

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
    }
    private void Update()
    {
        // Play if there is a clip and the user has completet both tutorials for moving
        if (startupDialogueClip != null && !walkTutorial.activeSelf && !rotateTutorial.activeSelf && count == 0)
        {
            count++;
            Debug.Log("Play Clip");
            PlayDialogue(startupDialogueClip);
        }
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
