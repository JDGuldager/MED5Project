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

    private Playlist currentlyPlayingPlaylist = null; //to track the active playlist

    public void ToggleMusic(Playlist playlist)
    {
        //check if this playlist is already playing
        if (currentlyPlayingPlaylist == playlist)
        {
            audioSource.Stop();
            playlist.button.image.sprite = playlist.defaultSprite;
            currentlyPlayingPlaylist = null;
        }
        else
        {
            if (currentlyPlayingPlaylist != null)
            {
                {
                    currentlyPlayingPlaylist.button.image.sprite = currentlyPlayingPlaylist.defaultSprite;
                }

                // Play a random song from the new playlist
                if (playlist.songs.Count > 0)
                {
                    int randomIndex = Random.Range(0, playlist.songs.Count);
                    audioSource.clip = playlist.songs[randomIndex];
                    audioSource.Play();

                    // Update UI
                    playlist.button.image.sprite = playlist.playingSprite;
                    currentlyPlayingPlaylist = playlist;
                }
            }
        }

        void Start()
        {
            // Assign button click listeners
            foreach (Playlist playlist in playlists)
            {
                Playlist capturedPlaylist = playlist; // Capture the variable to avoid closure issues
                playlist.button.onClick.AddListener(() => ToggleMusic(capturedPlaylist));
            }
        }
    }
}
