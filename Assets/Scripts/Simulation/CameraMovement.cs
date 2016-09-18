using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public int CamZ = -10;
    public Agent Target;

    public EvolutionController EvolutionController;
    public float CamSpeed = 5f;

    public float UserInputSpeed = 50f;

    public bool AllowUserInput;

    private bool followBestAgent = false;
    public bool FollowBestAgent
    {
        get
        {
            return followBestAgent;
        }
        set
        {
            followBestAgent = value;
            if (value)
            {
                EvolutionController.OnBestChanged += SetTarget;
            }
            else
            {
                EvolutionController.OnBestChanged -= SetTarget;
                SetCamPosInstant(new Vector3(0, 0, CamZ));
            }
        }
    }

    private Rect ScreenBounds;
    private RectTransform bounds;
    public RectTransform Bounds
    {
        get { return bounds; }
        set
        {
            bounds = value;
            ScreenBounds = new Rect(bounds.position, Camera.main.WorldToScreenPoint(new Vector2(bounds.rect.width, bounds.rect.height)));
        }
    }


    private void SetTarget(Agent bestGenome)
    {
        this.Target = bestGenome;
    }

    private Vector3 targetCamPos;
	void FixedUpdate ()
    {
        if (AllowUserInput)
            CheckUserInput();
        else if (Target != null)
            targetCamPos = Target.transform.position;

        targetCamPos.z = CamZ;
        this.transform.position = Vector3.Lerp(this.transform.position, targetCamPos, CamSpeed * Time.deltaTime);

        /*if (Bounds != null)
        {
            float rightDiff = (this.transform.position.x + Screen.width / 2) - (ScreenBounds.position.x + ScreenBounds.width / 2);
            float leftDiff = (this.transform.position.x - Screen.width / 2) + (ScreenBounds.position.x - ScreenBounds.width / 2);
            float upDiff = (this.transform.position.y + Screen.height / 2) - (ScreenBounds.position.y + ScreenBounds.height / 2);
            float downDiff = (this.transform.position.y - Screen.height / 2) + (ScreenBounds.position.y - ScreenBounds.height / 2);

            if (rightDiff > 0)
                this.transform.position = new Vector3(this.transform.position.x - rightDiff, this.transform.position.y, this.transform.position.z);
            else if (leftDiff < 0)
                this.transform.position = new Vector3(this.transform.position.x - leftDiff, this.transform.position.y, this.transform.position.z);

            if (upDiff > 0)
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - upDiff, this.transform.position.z);
            else if (downDiff < 0)
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - downDiff, this.transform.position.z);
        }*/
    }

    public void SetCamPosInstant(Vector3 camPos)
    {
        camPos.z = CamZ;
        this.transform.position = camPos;
        targetCamPos = this.transform.position;
    }

    private void CheckUserInput()
    {
        float horizontalInput, verticalInput;

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        targetCamPos += new Vector3(horizontalInput * UserInputSpeed * Time.deltaTime, verticalInput * UserInputSpeed * Time.deltaTime);
    }
}
