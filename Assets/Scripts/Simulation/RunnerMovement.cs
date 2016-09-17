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
        float horizontalSpeed, verticalSpeed;

        SpeedPerc = (float)CurInput[(int)InputValues.Horizontal];

        float curMaxSpeed = MAX_SPEED;
        if (jumping)
        {
            curMaxSpeed /= 2;
        }

        if (CurInput[(int)InputValues.Horizontal] >= 0)
            horizontalSpeed = System.Math.Max(rigidBodyComponent.velocity.x, (float)CurInput[(int)InputValues.Horizontal] * curMaxSpeed);
        else
            horizontalSpeed = System.Math.Min(rigidBodyComponent.velocity.x, (float)CurInput[(int)InputValues.Horizontal] * curMaxSpeed);


        verticalSpeed = rigidBodyComponent.velocity.y;
        if (!jumping && CurInput[(int) InputValues.JumpForce] > 0)
        {
            verticalSpeed = (float) CurInput[(int)InputValues.JumpForce] * MAX_JUMP;
            jumping = true;
        }

        rigidBodyComponent.velocity = new Vector2(horizontalSpeed, verticalSpeed);
    }


    public void Reset()
    {
        this.rigidBodyComponent.velocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (jumping)
        {
            
            foreach (ContactPoint2D point in collision.contacts)
            {
                if (Vector2.Dot(Vector2.up, point.normal) >= System.Math.PI / 4)
                {
                    jumping = false;
                    return;
                }
            }
        }
    }
}
