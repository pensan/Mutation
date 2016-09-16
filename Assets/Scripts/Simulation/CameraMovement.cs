using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public int CamZ = -10;
    public Agent Target;
    public EvolutionController EvolutionController;

    public float CamSpeed = 5f;

    void Start()
    {
        EvolutionController.OnBestChanged += delegate (Agent bestGenome)
        {
            this.Target = bestGenome;
        };
    }

	void FixedUpdate ()
    {

        Vector3 targetCamPos = Target.transform.position;
        targetCamPos.z = CamZ;

        this.transform.position = Vector3.Lerp(this.transform.position, targetCamPos, CamSpeed * Time.deltaTime);
	}
}
