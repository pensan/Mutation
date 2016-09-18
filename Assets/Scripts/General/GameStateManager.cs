using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class GameStateManager : MonoBehaviour
{

    public EvolutionController EvolutionController;
    public EvolutionController MultiplayerEvoController
    {
        get;
        private set;
    }
    public NetworkManager NetworkManager;
    public CameraMovement CamMovement;
    public Agent DummyAgent;

    private NeuralNetwork challengerNetwork;

    public static GameStateManager Instance
    {
        get;
        private set;
    }

    public Scene CurLevel
    {
        get;
        private set;
    }
    public bool IsInLevel
    {
        get;
        private set;
    }

    public bool IsTraining
    {
        get;
        private set;
    }

    public bool IsMultiplayer
    {
        get;
        private set;
    }

    void Awake()
    {
        SceneManager.LoadScene("GUI", LoadSceneMode.Additive);
        LoadMainMenu();

        Instance = this;

        IsInLevel = false;
        IsTraining = true;
        IsMultiplayer = false;
    }

    public void LoadSingleplayerLevel(int index)
    {
        IsMultiplayer = false;
        IsTraining = true;

        LoadLevel(index);
    }

    public void LoadMultiplayerLevel(int index, string opponentName = "")
    {
        challengerNetwork = NetworkManager.GetChallenger(opponentName);
        if (challengerNetwork != null)
        {
            IsMultiplayer = true;
            IsTraining = false;

            MultiplayerEvoController = Instantiate(EvolutionController);

            LoadLevel(index);
        }
        else
        {
            //TODO: Error message
        }

    }

    private void LoadLevel(int index)
    {
        SceneManager.LoadScene("Level_" + index, LoadSceneMode.Additive);
        CurLevel = SceneManager.GetSceneByName("Level_" + index);
        SceneManager.UnloadScene("MainMenu");

        IsInLevel = true;

        GUIController.Instance.CurrentMenu = GUIController.Instance.IngameMenu;
    }

    public void LoadMainMenu()
    {
        UnloadCurrentLevel();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        if (GUIController.Instance != null)
            GUIController.Instance.CurrentMenu = GUIController.Instance.MainMenu;
    }

    private void UnloadCurrentLevel()
    {
        if (IsInLevel)
        {
            SceneManager.UnloadScene(CurLevel);
            IsInLevel = false;
        }

        if (IsMultiplayer)
        {
            UnloadMultiplayer();
        }

        IsTraining = false;
    }

    private void UnloadMultiplayer()
    {
        IsMultiplayer = false;

        if (MultiplayerEvoController != null)
        {
            MultiplayerEvoController.KillEvolution();
            Destroy(MultiplayerEvoController);
        }

        MultiplayerEvoController = null;
    }

    public void StartEvolution()
    {
        EvolutionController.CreatePopulation(DummyAgent);
        EvolutionController.gameObject.SetActive(true);

        if (IsMultiplayer)
        {
            Agent challengerAgent = DummyAgent.CreateInstance();
            challengerAgent.Genome = new Genome(challengerNetwork);
            MultiplayerEvoController.CreatePopulation(challengerAgent);
            MultiplayerEvoController.gameObject.SetActive(true);
        }
    }

}
