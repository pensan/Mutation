using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class IngameMenu : MenuScreen
{
    public Button KillSwitch;
    public Button BackToMain;

    protected override void Awake()
    {
        base.Awake();

        KillSwitch.onClick.AddListener(GameStateManager.Instance.EvolutionController.KillAll);
        BackToMain.onClick.AddListener(delegate() 
        {
            LevelController.Instance.StopTimeoutTimer();
            if (GameStateManager.Instance.IsTraining)
            {
                Genome saveGenome = GameStateManager.Instance.EvolutionController.AlphaGenome;
                if (saveGenome == null) saveGenome = GameStateManager.Instance.EvolutionController.BestAgent.Genome;
                Serializer.SaveNetwork(saveGenome.NeuralNet);
                GameStateManager.Instance.NetworkManager.SaveNeuralNet(saveGenome.NeuralNet);
            }
            GameStateManager.Instance.LoadMainMenu();
        });

    }

    public override void Show()
    {
        base.Show();

        GUIController.Instance.IngameMenuParameters.Show();

        KillSwitch.gameObject.SetActive(!GameStateManager.Instance.IsMultiplayer);
    }

    public override void Hide()
    {
        base.Hide();

        GUIController.Instance.IngameMenuParameters.Hide();
    }
}
