using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class IngameMenu : MenuScreen
{
    public Button KillSwitch;
    public Button BackToMain;

    void Awake()
    {
        KillSwitch.onClick.AddListener(GameStateManager.Instance.EvolutionController.KillAll);
        BackToMain.onClick.AddListener(delegate() 
        {
            if (GameStateManager.Instance.IsTraining)
            {
                // @SAMUEL: I've added this null check as a quickfix, since it threw null ref errors when going back to main menu.
                if (GameStateManager.Instance.EvolutionController.AlphaGenome != null)
                {
                    NeuralNetwork saveNet = GameStateManager.Instance.EvolutionController.AlphaGenome.NeuralNet;
                    if (saveNet == null) saveNet = GameStateManager.Instance.EvolutionController.BestAgent.Genome.NeuralNet;
                    Serializer.SaveNetwork(saveNet);
                    GameStateManager.Instance.NetworkManager.SaveNeuralNet(GameStateManager.Instance.EvolutionController.AlphaGenome.NeuralNet);
                }
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
