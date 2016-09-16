using UnityEngine;
using System.Collections;

public class RunnerMovement : MonoBehaviour
{
    private const int MAX_SPEED = 25;
    private const int MAX_JUMP = 40;


    public bool UseUserInput
    {
        get;
        set;
    }

    public float SpeedPerc
    {
        get;
        private set;
    }

    public double[] CurInput
    {
        get;
        set;
    }

    private Rigidbody2D rigidBodyComponent;
    private bool jumping;

    void Awake()
    {
        rigidBodyComponent = GetComponent<Rigidbody2D>();
    }

    private double horizontalInput, jumpForce;
	void FixedUpdate ()
    {
	    if (UseUserInput)
            CheckForInput();

        ApplyInput();


	}

    private void CheckForInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
            jumpForce = 1;
        else
            jumpForce = 0;
    }

    private void ApplyInput()
    {
        SpeedPerc = (float)horizontalInput;

        if (rigidBodyComponent.velocity.y == 0 || !jumping)
        {
            if (jumpForce > 0)
                jumping = true;
            else if (rigidBodyComponent.velocity.y == 0)
                jumping = false;

            rigidBodyComponent.velocity = new Vector2(SpeedPerc * MAX_SPEED, rigidBodyComponent.velocity.y + (float)jumpForce * MAX_JUMP);
        }
    }
}
