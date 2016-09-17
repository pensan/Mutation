using UnityEngine;
using System.Collections;

public class Saw : MonoBehaviour
{
    public float rotationSpeed = 10;

	void FixedUpdate ()
    {
        this.transform.rotation *= Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, new Vector3(0, 0, 1));
	}
}
