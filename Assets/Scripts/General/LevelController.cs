using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

    public enum EvolutionType
    {
        Automatic, User
    }

    public EvolutionType EvoType = EvolutionType.User;

    public Transform StartPosition;
    public bool LoadGenome = true;
    public bool FollowBestAgent = true;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ModifyDummyAgent();

        StartEvolution();

        SetCamera();
    }

    private void SetCamera()
    {
        CameraMovement camMovement = GameStateManager.Instance.CamMovement;
        camMovement.AllowUserInput = false;
        camMovement.FollowBestAgent = FollowBestAgent;
        if (FollowBestAgent)
            camMovement.SetCamPosInstant(StartPosition.position);
    }

    private void ModifyDummyAgent()
    {
        Agent DummyAgent = GameStateManager.Instance.DummyAgent;
        DummyAgent.StartPosition = StartPosition.position;
        if (!DummyAgent.IsInitialized)
            DummyAgent.Init();

        if (LoadGenome)
            DummyAgent.LoadCurrentGenome();
        else
            DummyAgent.RandomizeGenome();
    }

    private void StartEvolution()
    {
        GameStateManager.Instance.EvolutionController.OnAllAgentsDied += OnAllAgentsDiedHandler;
        GameStateManager.Instance.StartEvolution();
    }

    void OnDestroy()
    {
        CleanUpEvolutionController();

        CleanUpDummyAgent();
    }

    private void CleanUpEvolutionController()
    {
        GameStateManager.Instance.EvolutionController.OnAllAgentsDied -= OnAllAgentsDiedHandler;
    }

    private void CleanUpDummyAgent()
    {
        Agent DummyAgent = GameStateManager.Instance.DummyAgent;
        DummyAgent.StartPosition = Vector3.zero;
    }

    private void OnAllAgentsDiedHandler()
    {
        switch (EvoType)
        {
            case EvolutionType.Automatic:
                GameStateManager.Instance.EvolutionController.AutoRepopulate();
                break;
            case EvolutionType.User:
                GUIController.Instance.CurrentMenu = GUIController.Instance.BreedMenu;
                break;
        }
    }

}
