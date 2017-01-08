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
					"Master your first obstacles by playing with the " +
					"\"Mutation Strength\" slider, that controls the " +
					"bacterias ability mutation strength. The higher " +
					"the value the bigger the impact on the abilites."
				);
				Debug.Log (tutStatus);
				WaitForDialog();
				tutStatus++;
			} else if (tutStatus == 1) {
				GUIController.Instance.DialogInGame.Show (
					"Tutorial",
					"Test\nTest"
				);
				WaitForDialog();
				Debug.Log (tutStatus);
				tutStatus++;
			} else if (tutStatus == 2) {
				GUIController.Instance.DialogInGame.Show (
					"Tutorial 2",
					"Test\nTest2"
				);
				WaitForDialog();
				Debug.Log (tutStatus);
				tutStatus++;
			}
		}
	}

	private IEnumerator WaitForDialog() {
		Debug.Log (GUIController.Instance.DialogInGame.isActiveAndEnabled);
		while (true) {
			Debug.Log (GUIController.Instance.DialogInGame.isActiveAndEnabled);
			yield return GUIController.Instance.DialogInGame.isActiveAndEnabled;
		}
	}
}