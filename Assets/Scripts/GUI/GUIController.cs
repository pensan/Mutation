using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GUIController : MonoBehaviour
{
    public Animator FadeBackground;
    private Image fadeImage; 

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

    public Dialog Dialog
    {
        get;
        private set;
    }

	public DialogInGame DialogInGame
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
        fadeImage = FadeBackground.GetComponent<Image>();

        MainMenu = GetComponentInChildren<MainMenu>(true);
        BreedMenu = GetComponentInChildren<BreedMenu>(true);
        IngameMenu = GetComponentInChildren<IngameMenu>(true);
        IngameMenuParameters = GetComponentInChildren<IngameMenuParameters>(true);

        Dialog = GetComponentInChildren<Dialog>(true);
		DialogInGame = GetComponentInChildren<DialogInGame>(true);

		DialogInGame.Hide ();
        Dialog.Hide();  //Just making sure

        MenuScreens = new List<MenuScreen>();
        MenuScreens.Add(MainMenu);
        MenuScreens.Add(BreedMenu);
        MenuScreens.Add(IngameMenu);
        MenuScreens.Add(IngameMenuParameters);

        foreach (MenuScreen menu in MenuScreens)
            menu.Hide();

        CurrentMenu = MainMenu;
	}

    public void Fade(bool fade)
    {
        if (fade)
        {
            FadeBackground.SetTrigger("FadeIn");
            CurrentMenu.SetInteractable(false);
        }
        else
        {
            FadeBackground.SetTrigger("FadeOut");
        }
    }

    public void FadeOperation(float minimumWait = 0.8f, System.Action duringFade = null, System.Action afterFade = null)
    {
        StartCoroutine(FadeOperationCo(minimumWait, duringFade, afterFade));
    }

    private IEnumerator FadeOperationCo(float minimumWait, System.Action duringFade, System.Action afterFade)
    {
        float waitTime = 0;
        Fade(true);
        yield return new WaitForEndOfFrame();

        while (fadeImage.color.a != 1)
        {
            waitTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (duringFade != null)
            duringFade();

        waitTime += Time.deltaTime;

        if (waitTime < minimumWait)
            yield return new WaitForSeconds(minimumWait - waitTime);

        Fade(false);

        while (fadeImage.color.a != 0)
            yield return new WaitForEndOfFrame();

        if (afterFade != null)
            afterFade();
    }

    public IEnumerator FadeAndWait(float waitTime = 0.8f)
    {
        Fade(true);
        yield return new WaitForSeconds(waitTime);
        Fade(false);
    }
}
