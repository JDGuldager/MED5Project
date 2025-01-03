using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightScale : MonoBehaviour
{
    public Transform userCharacter;
    public float userHeight = 174;
    public float characterHeight = 174;
    public float newScale;
    public TMPro.TextMeshProUGUI userHeightText;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        newScale = userHeight / characterHeight;
        Vector3 scaleFactor = new Vector3(newScale, newScale, newScale);
        transform.localScale = scaleFactor;
        userHeightText.text = "User Height: " + userHeight;
    }
    public void HeightUp()
    {
        userHeight++;
    }
    public void HeightDown()
    {
        userHeight--;
    }
}
