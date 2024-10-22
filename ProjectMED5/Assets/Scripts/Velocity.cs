using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Velocity : MonoBehaviour
{
    public float speed;
    public Rigidbody rigidBody;
    public Animator animator;

    private Vector3 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        speed = Vector3.Distance(transform.position, lastPos) / Time.deltaTime;

        lastPos = transform.position;

        animator.SetFloat("Forward", speed);
    }
}
