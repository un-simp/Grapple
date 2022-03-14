using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRPlayerController : MonoBehaviour
{
    [SerializeField] private float acceleration = 1;
    [SerializeField] private float drag = 0.2f;
    [SerializeField] private int jumpForce = 400;
    [SerializeField] private int gravityMultiplier = 12;
    [SerializeField] private float minGroundRange = 0.1f;
    public float absMaxVel = 40;
    public float speed = 50;
    public float maxVelocity = 12;
    public float upthrust = 40;
    public float lowerVelocityThresholdMultiplier = 0.9f;
    public float currentVelocity { get; private set; }

    //On Starts
    [HideInInspector] public float maxVelocityOnStart;
    [SerializeField] private SteamVR_Action_Vector2 touchpad;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform orientation;

    [SerializeField] private Transform steamVRObjects;

    bool isGrounded;

    void Start()
    {
        maxVelocityOnStart = maxVelocity;
    }

    void LateUpdate()
    {
    }

    void FixedUpdate()
    {
        orientation.forward -= Vector3.up * orientation.forward.y;

        ClampVelocity();

        Movement(1, 1);
        CounterMovement();
    }

    void Movement(float hMultiplier, float vMultiplier)
    {
        acceleration = currentVelocity / maxVelocity;
        
        if (!isGrounded && currentVelocity >= maxVelocity - 0.5f)
            maxVelocity += Time.deltaTime * acceleration;
        else if (currentVelocity <= maxVelocity * lowerVelocityThresholdMultiplier) 
            maxVelocity -= Time.deltaTime * 10 * acceleration;

        maxVelocity = Mathf.Clamp(maxVelocity, maxVelocityOnStart, absMaxVel);
        var dir = (orientation.forward * touchpad.axis.y * vMultiplier + orientation.right * touchpad.axis.x * hMultiplier).normalized;
        rb.AddForce(dir * speed);
    }

    private void CounterMovement()
    {
        Vector3 vel = rb.velocity;
        vel.y = 0;
        rb.AddForce(-vel * drag);
    }

    private void ClampVelocity()
    {
        //New Clamping Code

        var noUpVec = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (noUpVec.magnitude > maxVelocity)
        {
            noUpVec *= 0.97f;
            noUpVec = Vector3.ClampMagnitude(noUpVec, maxVelocity);
            noUpVec.y = rb.velocity.y;
            rb.velocity = noUpVec;
        }

        currentVelocity = rb.velocity.magnitude;
    }

    void OnCollisionStay(Collision col)
    {
        if(col.GetContact(0).normal == Vector3.up) //Is Floor
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision col)
    {
        if(isGrounded) //Is Floor
        {
            isGrounded = false;
        }
    }
}
