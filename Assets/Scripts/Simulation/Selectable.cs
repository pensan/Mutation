using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour
{

    private bool selected;
    public bool Selected
    {
        get { return selected; }
        private set
        {
            if (selected != value)
            {
                selected = value;
                if (OnSelectChanged != null)
                    OnSelectChanged(selected);
            }
        }
    }

    public event System.Action<bool> OnSelectChanged;

    public void Select(bool selected)
    {
        this.Selected = selected;
    }

    void OnMouseDown()
    {
        if (isActiveAndEnabled)
            Selected = !Selected;
    }
}
