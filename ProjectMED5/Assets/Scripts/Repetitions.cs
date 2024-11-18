using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repetitions : MonoBehaviour
{
    public ShoulderAngles ShoulderScript;
    public TMPro.TextMeshProUGUI repsText1;

    public ForearmRotationExercise forearmScript;
    public TMPro.TextMeshProUGUI repsText2;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        repsText1.text = "Repetitions: ";
        repsText2.text = "Repetitions: " + forearmScript.repetetionAmount;
    }

    
}
