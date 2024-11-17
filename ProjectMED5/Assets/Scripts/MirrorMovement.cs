using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMovement : MonoBehaviour
{
    public Transform playerTarget;
    public Transform mirror;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        //creates vector3 of local player position if it was a child of mirror
        Vector3 localPlayer = mirror.InverseTransformPoint(playerTarget.position);
        //invert posisition of player along z-axis
        transform.position = mirror.TransformPoint(new Vector3(localPlayer.x, localPlayer.y, -localPlayer.z));

        //Makes camera look at x inverted position
        Vector3 lookatmirror = mirror.TransformPoint(new Vector3(-localPlayer.x, localPlayer.y, localPlayer.z));
        transform.LookAt(lookatmirror);
    }
}
