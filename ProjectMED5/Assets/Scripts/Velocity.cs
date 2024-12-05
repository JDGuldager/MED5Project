using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class Velocity : MonoBehaviour
{
    public ActionBasedController leftController;
    public InputActionProperty actionLeft;
    public float forward;
    public float sideways;
    public Rigidbody rigidBody;
    public Animator animator;
    public Vector2 stick;

    private Vector3 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        stick = actionLeft.action.ReadValue<Vector2>();
        forward = stick.y;
        sideways = stick.x;

        lastPos = transform.position;

        animator.SetFloat("Forward", forward);
        animator.SetFloat("Sideways", sideways);
    }
}
