using UnityEngine;
using System.Collections;

public class MenuScreen : MonoBehaviour
{
    public bool IsShown
    {
        get;
        private set;
    }


    private CanvasGroup canvasGroup;

    protected virtual void Awake()
    {
        this.canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetInteractable(bool interactable)
    {
        if (canvasGroup != null)
            canvasGroup.interactable = interactable;
    }


    public virtual void Show()
    {
        IsShown = true;
        SetInteractable(true);
        this.gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        IsShown = false;
        this.gameObject.SetActive(false);
    }
}
