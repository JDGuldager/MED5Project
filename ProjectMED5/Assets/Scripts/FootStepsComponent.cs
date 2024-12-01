using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepsComponent : MonoBehaviour
{
    public AudioClip leftFootAudio;
    public AudioClip rightFootAudio;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FootStepEvent(string whichFoot)
    {
        if (whichFoot == "L")
        {
            audioSource.PlayOneShot(leftFootAudio);
        }
        else if (whichFoot == "R")
        {
            audioSource.PlayOneShot(rightFootAudio);
        }
    }
}