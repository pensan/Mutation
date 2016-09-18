using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MenuScreen
{
    public Button BreedButton;
    public Button ChallengeButton;
    public Button ExitButton;

    public int LevelIndex = 1;

    void Awake()
    {
        BreedButton.onClick.AddListener(delegate ()
        {
            GameStateManager.Instance.LoadSingleplayerLevel(LevelIndex);
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
