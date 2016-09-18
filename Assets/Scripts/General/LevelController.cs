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
        CameraMovement camMovement = GameStateManager.Instance.CamMovement;
        camMovement.AllowUserInput = false;
        camMovement.FollowBestAgent = FollowBestAgent;
        if (FollowBestAgent)
            camMovement.SetCamPosInstant(StartPosition.position);
        else
            camMovement.SetCamPosInstant(Vector3.zero);


        ModifyDummyAgent();

        StartEvolution();
    }


    private void ModifyDummyAgent()
    {
        Agent DummyAgent = GameStateManager.Instance.DummyAgent;
        DummyAgent.StartPosition = StartPosition.position;
        if (LoadGenome)
            DummyAgent.GenomeCreationMethod = Agent.GenomeCreation.Load;
        else
            DummyAgent.GenomeCreationMethod = Agent.GenomeCreation.Random;
    }

    private void StartEvolution()
    {
        EvolutionController evoController = GameStateManager.Instance.EvolutionController;
        
        switch (EvoType)
        {
            case EvolutionType.Automatic:
                evoController.OnAllAgentsDied += evoController.AutoRepopulate;
                break;
            case EvolutionType.User:
                evoController.OnAllAgentsDied += ShowBreedMenu;
                break;
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
        DummyAgent.GenomeCreationMethod = Agent.GenomeCreation.None;
    }

    private void ShowBreedMenu()
    {
        GUIController.Instance.CurrentMenu = GUIController.Instance.BreedMenu;
    }
}
