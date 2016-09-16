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

    public double[] CurInput
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
        horizontalInput = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            jumpForce = 1;
        }
    }

    private void ApplyInput()
    {
        SpeedPerc = (float)horizontalInput;

        Vector3 movement = Vector3.zero;
        movement.x += SpeedPerc * SPEED * Time.deltaTime;

        this.transform.position += movement;

        Debug.Log("Speed: " + SpeedPerc);
        Debug.Log("Move: " + movement);

    }
}
