using UnityEngine;
using System.Collections;

public class Dragable : MonoBehaviour
{

    public event System.Action OnDrag;

	void OnMouseDrag()
    {
        if (OnDrag != null)
            OnDrag();
    }
}
