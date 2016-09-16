using UnityEngine;
using System.Collections;

public class RunnerMovement : MonoBehaviour
{
    private const int SPEED = 30;


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

    public double[] Input
    {
        get;
        set;
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
        horizontalInput = UnityEngine.Input.GetAxis("Horizontal");

        if (UnityEngine.Input.GetButtonDown("Space"))
        {
            jumpForce = 1;
        }
    }

    private void ApplyInput()
    {
        Vector3 movement = Vector3.zero;
        movement.x += SpeedPerc * SPEED;

        this.transform.position += movement;


    }
}
