using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMusic : MonoBehaviour
{
    [System.Serializable]
    public class Playlist
    {
        public string name;
        public List<AudioClip> songs; //list of songs in the playlist
        public Button button;
        public Sprite defaultSprite;
        public Sprite playingSprite;

    }

    public List<Playlist> playlists; //list of playlists
    public AudioSource audioSource;
    private int amount;
    private Playlist currentlyPlayingPlaylist = null; //to track the active playlist

    void Start()
    {
        foreach (Playlist playlist in playlists)
        {
            Playlist capturedPlaylist = playlist; // Capture the variable to avoid closure issues
            Debug.Log("Assigning listener for playlist: " + capturedPlaylist.name);
            playlist.button.onClick.RemoveAllListeners();
            playlist.button.onClick.AddListener(() => ToggleMusic(capturedPlaylist));
        }
    }

    public void ToggleMusic(Playlist playlist)
    {
        Debug.Log("ToggleMusic called for playlist: " + playlist.name);

        // Stop the currently playing playlist if it matches the clicked one
        if (currentlyPlayingPlaylist == playlist)
        {
            Debug.Log("Stopping currently playing playlist: " + playlist.name);
            audioSource.Stop();
            playlist.button.image.sprite = playlist.defaultSprite;
            currentlyPlayingPlaylist = null;
            return;
        }

        // Stop the currently playing playlist if it's different
        if (currentlyPlayingPlaylist != null)
        {
            Debug.Log("Stopping another playlist: " + currentlyPlayingPlaylist.name);
            audioSource.Stop();
            currentlyPlayingPlaylist.button.image.sprite = currentlyPlayingPlaylist.defaultSprite;
        }

        // Start playing the new playlist
        if (playlist.songs.Count > 0)
        {
            Debug.Log("Playing a new playlist: " + playlist.name);
            int randomIndex = Random.Range(0, playlist.songs.Count);
            audioSource.clip = playlist.songs[randomIndex];
            audioSource.Play();

            // Update UI
            playlist.button.image.sprite = playlist.playingSprite;

            // Update the currently playing playlist
            currentlyPlayingPlaylist = playlist;
            return;
        }
        else
        {
            Debug.LogWarning("Selected playlist has no songs!");
        }
    }

}
