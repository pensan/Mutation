using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MenuScreen
{
    public Button BreedButton;
	public Button TutorialButton;
    public Button ChallengeButton;
    public Button ExitButton;

    public int LevelIndex = 1;

    protected override void Awake()
    {
        base.Awake();
		TutorialButton.onClick.AddListener(delegate ()
        {
			GameStateManager.Instance.EvolutionController.PopulationCount = 4;
			GameStateManager.Instance.EvolutionController.mutationAmount = 1.0f;
			GameStateManager.Instance.EvolutionController.mutationProb = 1.0f;
            GameStateManager.Instance.LoadSingleplayerLevel(1);
        });
		BreedButton.onClick.AddListener(delegate ()
		{
			GameStateManager.Instance.EvolutionController.PopulationCount = 10;
			GameStateManager.Instance.EvolutionController.mutationAmount = 0.5f;
			GameStateManager.Instance.EvolutionController.mutationProb = 0.15f;
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
