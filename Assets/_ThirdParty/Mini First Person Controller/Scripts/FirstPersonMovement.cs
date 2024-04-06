using System.Collections.Generic;
using UnityEngine;

// David: Many of the variables below in this (modified) 3rd party script are marked as [HideInInspector]
//        since I want to control them from Player.cs script programatically
//        instead of defining them via Inspector (so to prevent unplanned errors).

public class FirstPersonMovement : MonoBehaviour
{
    [HideInInspector]
    public float speed = 5;

    [HideInInspector]
    public float runSpeed = 9;

    [Header("Running")]
    [HideInInspector]
    public bool canRun = false;
    public bool IsRunning { get; private set; }

    [HideInInspector]
    public KeyCode runningKey = KeyCode.LeftShift;
    new Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    public bool IsDashingForward { get; set; }

    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity;

        if (IsDashingForward)
        {
            targetVelocity = new Vector2(0, targetMovingSpeed);
        }
        else
        {
            targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);
        }

        // Apply movement.
        rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
    }
}