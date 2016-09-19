using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIController : MonoBehaviour
{
    public List<MenuScreen> MenuScreens
    {
        get;
        private set;
    }

    public MainMenu MainMenu
    {
        get;
        private set;
    }
    public BreedMenu BreedMenu
    {
        get;
        private set;
    }
    public IngameMenu IngameMenu
    {
        get;
        private set;
    }

    public IngameMenuParameters IngameMenuParameters
    {
        get;
        private set;
    }

    public static GUIController Instance
    {
        get;
        private set;
    }

    private MenuScreen currentMenu;
    public MenuScreen CurrentMenu
    {
        get { return currentMenu; }
        set
        {
            if (currentMenu != null)
                currentMenu.Hide();

            currentMenu = value;

            if(currentMenu != null)
                currentMenu.Show();
        }
    }

	void Awake ()
    {
        Instance = this;
        MainMenu = GetComponentInChildren<MainMenu>(true);
        BreedMenu = GetComponentInChildren<BreedMenu>(true);
        IngameMenu = GetComponentInChildren<IngameMenu>(true);
        IngameMenuParameters = GetComponentInChildren<IngameMenuParameters>(true);


        MenuScreens = new List<MenuScreen>();
        MenuScreens.Add(MainMenu);
        MenuScreens.Add(BreedMenu);
        MenuScreens.Add(IngameMenu);
        MenuScreens.Add(IngameMenuParameters);

        foreach (MenuScreen menu in MenuScreens)
            menu.Hide();

        CurrentMenu = MainMenu;
	}
	

}
