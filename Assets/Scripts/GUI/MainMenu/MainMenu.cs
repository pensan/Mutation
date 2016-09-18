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
            GameStateManager.Instance.LoadLevel(1);
        });
        ExitButton.onClick.AddListener(delegate ()
        {
            Application.Quit();
            Debug.Log("Quit");
        });
    }


	
}
