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

    public Button AutoBreedButton;
    public Button ManualBreedButton;
    public Slider MutationAmountSlider;
    public Slider MutationProbSlider;

    private int runnerLayer;

    void Awake()
    {
        AutoBreedButton.onClick.AddListener(StartAutoBreed);
        ManualBreedButton.onClick.AddListener(StartManualBreed);
        SelectedAgents = GetComponentInChildren<SelectedAgentsPanel>();
        SelectedAgents.OnAgentAdded += SelectedAgentsChanged;
        SelectedAgents.OnAgentRemoved += SelectedAgentsChanged;

        runnerLayer = LayerMask.NameToLayer("Runner");
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

        GameStateManager.Instance.CamMovement.AllowUserInput = true;
    }

    public override void Hide()
    {
        base.Hide();
        GameStateManager.Instance.CamMovement.AllowUserInput = false;
    }

    private void StartAutoBreed()
    {
        GameStateManager.Instance.EvolutionController.AutoRepopulate(MutationProbSlider.value, MutationAmountSlider.value);
        NewBreedStarted();
    }

    private void StartManualBreed()
    {
        GameStateManager.Instance.EvolutionController.Repopulate(MutationProbSlider.value, MutationAmountSlider.value, SelectedAgents.GetSelectedAgents());
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
