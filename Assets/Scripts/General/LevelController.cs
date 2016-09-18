using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour
{

    public enum EvolutionType
    {
        Automatic, User
    }

    public EvolutionType EvoType = EvolutionType.User;

    public Transform StartPosition;
    public bool LoadGenome = true;
    public bool FollowBestAgent = true;

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
        EvolutionController evoController = GameStateManager.Instance.EvolutionController;

        if (!GameStateManager.Instance.IsMultiplayer)
        {
            switch (EvoType)
            {
                case EvolutionType.Automatic:
                    evoController.OnAllAgentsDied += evoController.AutoRepopulate;
                    break;
                case EvolutionType.User:
                    evoController.OnAllAgentsDied += ShowBreedMenu;
                    break;
            }
        }
        
        
        GameStateManager.Instance.StartEvolution();
    }

    void OnDestroy()
    {
        CleanUpEvolutionController();

        CleanUpDummyAgent();
    }

    private void CleanUpEvolutionController()
    {
        EvolutionController evoController = GameStateManager.Instance.EvolutionController;

        switch (EvoType)
        {
            case EvolutionType.Automatic:
                evoController.OnAllAgentsDied -= evoController.AutoRepopulate;
                break;
            case EvolutionType.User:
                evoController.OnAllAgentsDied -= ShowBreedMenu;
                break;
        }
    }

    private void CleanUpDummyAgent()
    {
        Agent DummyAgent = GameStateManager.Instance.DummyAgent;
        DummyAgent.StartPosition = Vector3.zero;
    }

    private void ShowBreedMenu()
    {
        GUIController.Instance.CurrentMenu = GUIController.Instance.BreedMenu;
    }
}
