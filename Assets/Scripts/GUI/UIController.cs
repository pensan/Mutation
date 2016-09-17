using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    public static UIController Instance
    {
        get;
        private set;
    }

    public GameObject BreedMenu;

    void Awake()
    {
        Instance = this;


        BreedMenu.SetActive(false);
    }

    public void StartRebreeding()
    {
        BreedMenu.SetActive(true);
    }
}
