using UnityEngine;
using System.Collections;

public class RunnerMovement : MonoBehaviour
{
    public enum InputValues
    {
        Horizontal = 0, JumpForce = 1
    }


    private const int MAX_SPEED = 25;
    private const int MAX_JUMP = 35;


    public bool UseUserInput;

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
        CurInput = new double[2];
        rigidBodyComponent = GetComponent<Rigidbody2D>();
    }

	void FixedUpdate ()
    {
	    if (UseUserInput)
            CheckForInput();

        CapInput();

        ApplyInput();


	}

    private void CheckForInput()
    {
        CurInput[(int) InputValues.Horizontal] = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
            CurInput[(int)InputValues.JumpForce] = 1;
        else
            CurInput[(int)InputValues.JumpForce] = 0;
    }


    private void CapInput()
    {
        if (CurInput[(int)InputValues.Horizontal] > 1)
            CurInput[(int)InputValues.Horizontal] = 1;
        else if (CurInput[(int)InputValues.Horizontal] < -1)
            CurInput[(int)InputValues.Horizontal] = -1;

        if (CurInput[(int)InputValues.JumpForce] > 1)
            CurInput[(int)InputValues.JumpForce] = 1;
        else if (CurInput[(int)InputValues.JumpForce] < 0)
            CurInput[(int)InputValues.JumpForce] = 0;

    }

    private void ApplyInput()
    {
        SpeedPerc = (float)CurInput[(int)InputValues.Horizontal];

        if (rigidBodyComponent.velocity.y == 0 || !jumping)
        {
            if (CurInput[(int)InputValues.JumpForce] > 0)
                jumping = true;
            else if (rigidBodyComponent.velocity.y == 0)
                jumping = false;
               
            rigidBodyComponent.velocity = new Vector2(SpeedPerc * MAX_SPEED, rigidBodyComponent.velocity.y + (float)CurInput[(int)InputValues.JumpForce] * MAX_JUMP);
        }
    }
}
