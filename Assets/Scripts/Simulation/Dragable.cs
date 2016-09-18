using UnityEngine;
using System.Collections;

public class Dragable : MonoBehaviour
{

    public float DragThreshold = 50;
    public event System.Action OnDrag;

    private bool dragging = false;
    private Vector3 startPosition;

	void OnMouseDown()
    {
        startPosition = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        if (!dragging && (Input.mousePosition - startPosition).magnitude >= DragThreshold)
        {
            dragging = true;
        }

        if (dragging && OnDrag != null)
                OnDrag();
    }

    void OnMouseUp()
    {
        dragging = false;
    }
}
