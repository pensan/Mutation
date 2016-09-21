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
        Instance = this;

        IsInLevel = false;
        IsTraining = false;
        IsMultiplayer = false;

        SceneManager.LoadScene("GUI", LoadSceneMode.Additive);
        LoadMainMenu();
    }

    public void LoadSingleplayerLevel(int index)
    {
        IsMultiplayer = false;
        IsTraining = true;

        GUIController.Instance.FadeOperation(0.5f, delegate { LoadLevel(index); }, EnableAllAgents);
    }

    public void LoadMultiplayerLevel(int index, string opponentName = "")
    {
        /*StartCoroutine(GUIController.Instance.FadeAndWaitForCo(new CoroutineWithData(this, NetworkManager.GetOpponent(opponentName), delegate (object serverData) 
        {
            challengerNetwork = serverData as NeuralNetwork;
            if (challengerNetwork == null)
            {
                Debug.LogError("Failed to start multiplayer: challengerNetwork was null!");
            }
            else
            {
                IsMultiplayer = true;
                IsTraining = false;

                MultiplayerEvoController = Instantiate(EvolutionController);

                LoadLevel(index);
            }
        })));*/
    }

    private IEnumerator LoadLevelCo(int index)
    {
        yield return SceneManager.LoadSceneAsync("Level_" + index, LoadSceneMode.Additive);
        CurLevel = SceneManager.GetSceneByName("Level_" + index);
        SceneManager.UnloadScene("MainMenu");

        IsInLevel = true;

        GUIController.Instance.CurrentMenu = GUIController.Instance.IngameMenu;
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
        if (GUIController.Instance == null)
        {
            LoadMain();
            StartCoroutine(EnableAllCo());
        }
        else
        {
            GUIController.Instance.FadeOperation(0.8f, LoadMain, EnableAllAgents);
        }
    }

    private void LoadMain()
    {
        UnloadCurrentLevel();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        if (GUIController.Instance != null)
            GUIController.Instance.CurrentMenu = GUIController.Instance.MainMenu;
    }

    private IEnumerator EnableAllCo()
    {
        yield return new WaitForEndOfFrame(); //Need to wait 1 frame for level controller to initialize
        EnableAllAgents();
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

        Debug.Log("Start Evolution");

        if (IsMultiplayer)
        {
            Agent challengerAgent = DummyAgent.CreateInstance();
            challengerAgent.Genome = new Genome(challengerNetwork);
            MultiplayerEvoController.CreatePopulation(challengerAgent);
            MultiplayerEvoController.gameObject.SetActive(true);
        }

        EvolutionController.ActivateAll(false);
    }

    public void EnableAllAgents()
    {
        EvolutionController.ActivateAll(true);
    }

}
