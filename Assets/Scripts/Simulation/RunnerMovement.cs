using UnityEngine;
using System.Collections;

public class RunnerMovement : MonoBehaviour
{
    public enum InputValues
    {
        Horizontal = 0, JumpForce = 1
    }


    private const int MAX_SPEED = 25;
    private const int MAX_JUMP = 60;

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

    public Rigidbody2D RigidBodyComponent
    {
        get;
        private set;
    }
    private Runner parent;
    private bool jumping;

    void Awake()
    {
        CurInput = new double[2];
        this.parent = GetComponent<Runner>();
        RigidBodyComponent = GetComponent<Rigidbody2D>();
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

        if (jumping)
        {
            if (CurInput[(int)InputValues.Horizontal] >= 0)
                horizontalSpeed = System.Math.Max(RigidBodyComponent.velocity.x, (float)CurInput[(int)InputValues.Horizontal] * curMaxSpeed);
            else
                horizontalSpeed = System.Math.Min(RigidBodyComponent.velocity.x, (float)CurInput[(int)InputValues.Horizontal] * curMaxSpeed);
        }
        else
        {
            horizontalSpeed = (float)CurInput[(int)InputValues.Horizontal] * curMaxSpeed;
        }
        


        verticalSpeed = RigidBodyComponent.velocity.y;
        if (!jumping && CurInput[(int) InputValues.JumpForce] > 0)
        {
            verticalSpeed = (float) CurInput[(int)InputValues.JumpForce] * MAX_JUMP;
            jumping = true;
        }

        RigidBodyComponent.velocity = new Vector2(horizontalSpeed, verticalSpeed);
    }


    public void Reset()
    {
        this.RigidBodyComponent.velocity = Vector2.zero;
        this.RigidBodyComponent.isKinematic = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (jumping)
            {
                foreach (ContactPoint2D point in collision.contacts)
                {
                    if (Vector2.Dot(Vector2.up, point.normal) >= System.Math.PI / 6)
                    {
                        jumping = false;
                        return;
                    }
                }
            }
        }
    }
}
