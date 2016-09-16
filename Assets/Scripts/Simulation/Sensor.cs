using UnityEngine;
using System.Collections;

public class Sensor : MonoBehaviour
{
    private int wallLayer;

    public SpriteRenderer cross;

    private const float MAX_DIST = 5f;

    public float Output
    {
        get;
        private set;
    }

	void Start ()
    {
        wallLayer = LayerMask.NameToLayer("Wall");
        cross.gameObject.SetActive(true);
	}
	
	void FixedUpdate ()
    {
        Vector2 direction = cross.transform.position - this.transform.position;
        direction.Normalize();
        RaycastHit2D hit =  Physics2D.Raycast(this.transform.position, direction, MAX_DIST, 1 << wallLayer);

        if (hit.collider == null)
            hit.distance = MAX_DIST;

        this.Output = hit.distance / MAX_DIST;
        cross.transform.position = (Vector2) this.transform.position + direction * hit.distance;
	}
}
