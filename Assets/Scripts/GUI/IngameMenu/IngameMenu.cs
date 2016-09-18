using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IngameMenu : MenuScreen
{
    public LevelController levelController;
    public Button KillSwitch;
    public Button BackToMain;
    public Toggle AutoBreedToggle;

    void Awake()
    {
        KillSwitch.onClick.AddListener(GameStateManager.Instance.EvolutionController.KillAll);
        BackToMain.onClick.AddListener(delegate() 
        {
            if (GameStateManager.Instance.IsTraining)
            {
                Serializer.SaveNetwork(GameStateManager.Instance.EvolutionController.AlphaGenome.NeuralNet);
                GameStateManager.Instance.NetworkManager.SaveNeuralNet(GameStateManager.Instance.EvolutionController.AlphaGenome.NeuralNet);
            }
            GameStateManager.Instance.LoadMainMenu();
        });

        AutoBreedToggle.onValueChanged.AddListener(AutoBreedToggleChanged);
    }

    public override void Show()
    {
        base.Show();

        KillSwitch.gameObject.SetActive(!GameStateManager.Instance.IsMultiplayer);
        AutoBreedToggle.gameObject.SetActive(!GameStateManager.Instance.IsMultiplayer);
    }


    public void AutoBreedToggleChanged(bool v)
    {
        LevelController.Instance.EvoType = v ? LevelController.EvolutionType.Automatic : LevelController.EvolutionType.User;
    }
}
