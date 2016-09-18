using UnityEngine;
using System.Collections;

public class MenuScreen : MonoBehaviour
{

    public virtual void Show()
    {
        this.gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
