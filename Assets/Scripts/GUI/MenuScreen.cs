using UnityEngine;
using System.Collections;

public class MenuScreen : MonoBehaviour
{

    public bool IsShown
    {
        get;
        private set;
    }


    public virtual void Show()
    {
        IsShown = true;
        this.gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        IsShown = false;
        this.gameObject.SetActive(false);
    }
}
