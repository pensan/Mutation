using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BreedUIController : MonoBehaviour
{

    public static BreedUIController Instance
    {
        get;
        private set;
    }

    public EvolutionController EvolutionController;
    public SelectedAgentsPanel SelectedAgents
    {
        get;
        private set;
    }

    public CameraMovement CamMovement;

    public GameObject BreedMenu;
    public Button AutoBreedButton;
    public Button ManualBreedButton;
    public Slider MutationAmountSlider;
    public Slider MutationProbSlider;

    private int runnerLayer;

    void Awake()
    {
        Instance = this;

        AutoBreedButton.onClick.AddListener(StartAutoBreed);
        ManualBreedButton.onClick.AddListener(StartManualBreed);
        SelectedAgents = GetComponentInChildren<SelectedAgentsPanel>();
        SelectedAgents.OnAgentAdded += SelectedAgentsChanged;
        SelectedAgents.OnAgentRemoved += SelectedAgentsChanged;

        BreedMenu.SetActive(false);

        runnerLayer = LayerMask.NameToLayer("Runner");
    }

    public void Show()
    {
        BreedMenu.SetActive(true);
        foreach (Agent agent in EvolutionController.Population)
        {
            Runner runner = (Runner)agent;
            runner.Selectable = true;
        }

        ManualBreedButton.interactable = SelectedAgents.AgentCount >= 2;

        CamMovement.AllowUserInput = true;
    }

    public void Hide()
    {
        BreedMenu.SetActive(false);
        CamMovement.AllowUserInput = false;
    }

    private void StartAutoBreed()
    {
        EvolutionController.AutoRepopulate(MutationProbSlider.value, MutationAmountSlider.value);
        Hide();
    }

    private void StartManualBreed()
    {
        EvolutionController.Repopulate(MutationProbSlider.value, MutationAmountSlider.value, SelectedAgents.GetSelectedAgents());
        SelectedAgents.Clear();
        Hide();
    }

    private void SelectedAgentsChanged()
    {
        ManualBreedButton.interactable = SelectedAgents.AgentCount >= 2;
    }

}
