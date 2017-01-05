using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class IngameMenu : MenuScreen
{
    public Button KillSwitch;
    public Button BackToMain;
	private int tutStatus = 0;

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
		InTutorial();
        GUIController.Instance.IngameMenuParameters.Show();

        KillSwitch.gameObject.SetActive(!GameStateManager.Instance.IsMultiplayer);
    }

	private void InTutorial() {
		if (tutStatus == 0) {
			GUIController.Instance.DialogInGame.Show(
				"Level 1",
				"Test"
			);
			tutStatus++;
		}

	}

    public override void Hide()
    {
        base.Hide();

        GUIController.Instance.IngameMenuParameters.Hide();
    }
}
