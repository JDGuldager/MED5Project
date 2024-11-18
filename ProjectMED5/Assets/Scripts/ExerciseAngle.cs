using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseAngle : MonoBehaviour
{
    public Slider angleSlider1;
    public ShoulderAngles shoulderScript;
    public TMPro.TextMeshProUGUI angleText1;

    public Slider angleSlider2;
    public ForearmRotationExercise forearmScript;
    public TMPro.TextMeshProUGUI angleText2;

    private int a = 5;
    private int b = 45;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        shoulderScript.specificAngleThreshold = angleSlider1.value * a + b;
        angleText1.text = "Exercise Angle: " + shoulderScript.specificAngleThreshold;

        forearmScript.specificAngleThreshold = angleSlider2.value * a + b;
        angleText2.text = "Exercise Angle: " + forearmScript.specificAngleThreshold;
    }
  

}
