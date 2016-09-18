using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MenuScreen
{
    public Button BreedButton;
    public Button ChallengeButton;
    public Button ExitButton;


    void Awake()
    {
        BreedButton.onClick.AddListener(delegate ()
        {
            GameStateManager.Instance.LoadSingleplayerLevel(1);
        });
        ChallengeButton.onClick.AddListener(delegate ()
        {
            GameStateManager.Instance.LoadMultiplayerLevel(1);
        });
        ExitButton.onClick.AddListener(delegate ()
        {
            Application.Quit();
            Debug.Log("Quit");
        });
    }


	
}
