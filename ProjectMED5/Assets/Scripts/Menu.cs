using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Menu : MonoBehaviour
{
    public GameObject canvas;
    public InputActionProperty toggleCanvasAction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DisableCanvas()
    {
        canvas.SetActive(false);
    }

    public void ShowCanvas()
    {
        canvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (toggleCanvasAction.action.WasPressedThisFrame())
        {
            if (canvas != null)
            {
                if (canvas.activeSelf)
                {
                    DisableCanvas();
                }
                else
                {
                    ShowCanvas();
                }
            }
        }
    }
}

