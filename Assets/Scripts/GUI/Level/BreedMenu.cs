using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BreedMenu : MenuScreen
{

    public SelectedAgentsPanel SelectedAgents
    {
        get;
        private set;
    }

    public Button ManualBreedButton;

    protected override void Awake()
    {
        base.Awake();

        ManualBreedButton.onClick.AddListener(StartManualBreed);
        SelectedAgents = GetComponentInChildren<SelectedAgentsPanel>();
        SelectedAgents.OnAgentAdded += SelectedAgentsChanged;
        SelectedAgents.OnAgentRemoved += SelectedAgentsChanged;
    }

    public override void Show()
    {
        base.Show();

        foreach (Agent agent in GameStateManager.Instance.EvolutionController.Population)
        {
            Runner runner = (Runner)agent;
            runner.Selectable = true;
        }

        ManualBreedButton.interactable = SelectedAgents.AgentCount >= 2;

        GUIController.Instance.IngameMenuParameters.Show();

        GameStateManager.Instance.CamMovement.AllowUserInput = true;
    }

    public override void Hide()
    {
        base.Hide();
        GameStateManager.Instance.CamMovement.AllowUserInput = false;

        if (LevelController.Instance != null && LevelController.Instance.EvoType != LevelController.EvolutionType.Automatic)
            GUIController.Instance.IngameMenuParameters.Hide();
    }

    private void StartManualBreed()
    {
        GameStateManager.Instance.EvolutionController.Repopulate(SelectedAgents.GetSelectedAgents());
        NewBreedStarted();
    }

    private void NewBreedStarted()
    {
        GUIController.Instance.CurrentMenu = GUIController.Instance.IngameMenu;
        SelectedAgents.Clear();
        GameStateManager.Instance.CamMovement.SetCamPosInstant(GameStateManager.Instance.DummyAgent.StartPosition);
    }

    private void SelectedAgentsChanged()
    {
        ManualBreedButton.interactable = SelectedAgents.AgentCount >= 2;
    }

}
