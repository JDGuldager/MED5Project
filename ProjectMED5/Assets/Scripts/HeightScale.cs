using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightScale : MonoBehaviour
{
    public Transform userCharacter;
    public float userHeight = 174;
    public float characterHeight = 174;
    public float newScale;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        newScale = userHeight / characterHeight;
        Vector3 scaleFactor = new Vector3(newScale, newScale, newScale);
        transform.localScale = scaleFactor;
    }
}
