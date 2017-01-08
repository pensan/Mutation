using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class IngameMenu : MenuScreen
{
    public Button KillSwitch;
    public Button BackToMain;
	public int tutStatus = 0;

    protected override void Awake()
    {
        base.Awake();

        KillSwitch.onClick.AddListener(GameStateManager.Instance.EvolutionController.KillAll);
        BackToMain.onClick.AddListener(delegate() 
        {
            LevelController.Instance.StopTimeoutTimer();
            if (GameStateManager.Instance.IsTraining)
            {
                GameStateManager.Instance.SaveCurrentNeuralNet();
            }
            GameStateManager.Instance.LoadMainMenu();
        });
    }

    public override void Show()
    {
        base.Show();
		GUIController.Instance.DialogInGame.Hide();
		GUIController.Instance.IngameMenuParameters.Show();
		InTutorial();
        KillSwitch.gameObject.SetActive(!GameStateManager.Instance.IsMultiplayer);
    }

	public override void Hide()
	{
		base.Hide();
		GUIController.Instance.IngameMenuParameters.Hide();
	}

	public void InTutorial() {
		if (GUIController.Instance.MainMenu.LevelIndex == 1) {
			if (tutStatus == 0) {
				GUIController.Instance.DialogInGame.Show (
					"Tutorial",
					"Welcome to Mutate & Escape!\n" +
					"Master your first obstacles by playing with the\n" +
					"\"Mutation Strength\" slider, that controls the " +
					"mutation strength of the bacterias abilities. The higher " +
					"the value the bigger the impact on their abilites."
				);
				StartCoroutine(WaitForDialog());
			} else if (tutStatus == 1) {
				GUIController.Instance.DialogInGame.Show (
					"Tutorial",
					"When your bacteria died, choose 2 or more of them " +
					"to breed them to a new and stronger type!\n" +
					"Select, drag & drop the bacteria with your mouse " +
					"and move the camera with the WASD or arrow keys."
				);
				StartCoroutine(WaitForDialog());
			} else if (tutStatus == 2) {
				GUIController.Instance.DialogInGame.Show (
					"Tutorial",
					"Control the percentage of mutated bacteria with the\n" +
					"\"Mutation Amount\" slider.\n" +
					"Use these sliders and choose your bacteria wisely to " +
					"create the ultimate breed!"
				);
				StartCoroutine(WaitForDialog());
			}
		}
		if (GUIController.Instance.MainMenu.LevelIndex == 2) {
			if (tutStatus == 3) {
				GUIController.Instance.DialogInGame.Show (
					"Level 1",
					"In this level, your breed will face its " +
					"first bigger challenges. Teach them to survive " +
					"and avoid the obstacles through selective breeding " +
					"and clever mutating!\n" +
					"Good luck!"
				);
				StartCoroutine(WaitForDialog());
			}
		}
	}

	private IEnumerator WaitForDialog() {
		while (GUIController.Instance.DialogInGame.isActiveAndEnabled) {
			yield return null;
		}
		tutStatus++;
		InTutorial();
	}
}