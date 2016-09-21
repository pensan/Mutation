using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MenuScreen
{
    public Button BreedButton;
    public Button ChallengeButton;
    public Button ExitButton;

    public int LevelIndex = 1;

    protected override void Awake()
    {
        base.Awake();
        BreedButton.onClick.AddListener(delegate ()
        {
            GameStateManager.Instance.LoadSingleplayerLevel(LevelIndex);
        });
        ChallengeButton.onClick.AddListener(delegate ()
        {
            GameStateManager.Instance.LoadMultiplayerLevel(LevelIndex);
        });
        ExitButton.onClick.AddListener(delegate ()
        {
            Application.Quit();
            Debug.Log("Quit");
        });
    }


	
}
