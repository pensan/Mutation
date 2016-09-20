using UnityEngine;
using System.Collections;

public class Sensor : MonoBehaviour
{
    public LayerMask LayerToSense;

    public SpriteRenderer cross;

    private const float MAX_DIST = 10f;
    private const float MIN_DIST = 0.01f;

    public float Output
    {
        get;
        private set;
    }

	void Start ()
    {
        cross.gameObject.SetActive(true);
	}
	
	void FixedUpdate ()
    {
        Vector2 direction = cross.transform.position - this.transform.position;
        direction.Normalize();
        RaycastHit2D hit =  Physics2D.Raycast(this.transform.position, direction, MAX_DIST, LayerToSense);

        if (hit.collider == null)
            hit.distance = MAX_DIST;
        else if (hit.distance < MIN_DIST)
            hit.distance = MIN_DIST;

        this.Output = hit.distance / MAX_DIST;
        cross.transform.position = (Vector2) this.transform.position + direction * hit.distance;
	}
}
