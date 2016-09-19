using UnityEngine;
using System.Collections;

public class Dragable : MonoBehaviour
{

    public float DragThreshold = 20;
    public event System.Action OnDrag;

    private bool dragging = false;
    private Vector3 startPosition;

	void OnMouseDown()
    {
        if (this.isActiveAndEnabled)
            startPosition = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        if (this.isActiveAndEnabled)
        {
            if (!dragging && (Input.mousePosition - startPosition).magnitude >= DragThreshold)
            {
                dragging = true;
            }

            if (dragging && OnDrag != null)
                OnDrag();
        }  
    }

    void OnMouseUp()
    {
        dragging = false;
    }
}
