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
        BackToMain.onClick.AddListener(GameStateManager.Instance.LoadMainMenu);
    }
	
}
