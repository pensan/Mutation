using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public int CamZ = -10;
    public Agent Target;

    public EvolutionController EvolutionController;

    public float CamSpeed = 5f;

    public float UserInputSpeed = 5f;

    public bool AllowUserInput;

    void Start()
    {
        EvolutionController.OnBestChanged += delegate (Agent bestGenome)
        {
            this.Target = bestGenome;
        };
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
    }

    public void SetCamPosInstant(Vector3 camPos)
    {
        camPos.z = CamZ;
        this.transform.position = camPos;
    }

    private void CheckUserInput()
    {
        float horizontalInput, verticalInput;

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        targetCamPos += new Vector3(horizontalInput * UserInputSpeed * Time.deltaTime, verticalInput * UserInputSpeed * Time.deltaTime);
    }
}
