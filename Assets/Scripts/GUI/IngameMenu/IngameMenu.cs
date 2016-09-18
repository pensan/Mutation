using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
                Serializer.SaveNetwork(GameStateManager.Instance.EvolutionController.AlphaGenome.NeuralNet);
            GameStateManager.Instance.LoadMainMenu();
        });
    }

    public override void Show()
    {
        base.Show();

        KillSwitch.gameObject.SetActive(!GameStateManager.Instance.IsMultiplayer);
    }

}
