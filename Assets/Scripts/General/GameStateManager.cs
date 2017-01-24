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
    public CameraMovement CamMovement;
    public Agent DummyAgent;
    public NeuralNetwork CurrentNeuralNet
    {
        get;
        private set;
    }

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
        //HandleServerStatus(SyncCoroutine(NetworkManager.GetServerstatus()));
    }

    public static object SyncCoroutine(IEnumerator target)
    {
        object data = null;
        while (target.MoveNext())
        {
            data = target.Current;
        }

        return data;
    }

    private void HandleServerStatus(object data)
    {
        bool result = (bool)data;

        if (result)
        {
            Debug.Log("Successfully connected to server! Posting login!");
            HandleServerLogin(SyncCoroutine(NetworkManager.PostLogin()));
        }
        else
        {
            StartCoroutine(WaitedDialog("Server Error", "Couldn't connect to Server!\nPlayer try again at a later time."));
        }
    }

    private void HandleServerLogin(object serverData)
    {
        if (serverData == null)
            CurrentNeuralNet = null;
        else
        {
            try
            {
                CurrentNeuralNet = (NeuralNetwork)serverData;
                Debug.Log("Successfully loaded neuralNetwork from server");
            }
            catch (InvalidCastException)
            {
                bool status = (bool)serverData;
                if (status)
                    Debug.LogError("Server status was good but couldn't load neural network!");
                else
                    Debug.LogError("Connection error while trying to load neural network from server!");

                WaitedDialog("Server Error", "Failed to load your population from server...\nPlease try again at a later time.");
            }
        }
        
    }

    public void ResetNeuralNet()
    {
        Debug.Log("Resetting neural network...");
        CurrentNeuralNet = null;
    }

    private IEnumerator WaitedDialog(string header, string content, System.Action okAction = null)
    {
        yield return new WaitForEndOfFrame();
        GUIController.Instance.Dialog.Show(header, content, okAction);
    }

    public void SaveCurrentNeuralNet()
    {
        Debug.Log("Saving current best neural network.");
        Genome saveGenome = EvolutionController.AlphaGenome;
        if (saveGenome == null) saveGenome = EvolutionController.BestAgent.Genome;
        CurrentNeuralNet = saveGenome.NeuralNet.DeepCopy();
        StartCoroutine(NetworkManager.PostNeuralNet(saveGenome.NeuralNet));
    }

    public void LoadSingleplayerLevel(int index)
    {
        IsMultiplayer = false;
        IsTraining = true;
        GUIController.Instance.FadeOperation(0.5f, 
            delegate 
            {
                LoadLevel(index);
            }, 
            EnableAllAgents);
    }

    public void LoadMultiplayerLevel(int index, string opponentName = "")
    {
        GUIController.Instance.FadeOperation(0.8f, delegate () { LoadMultiplayer(index, opponentName); }, EnableAllAgents);
    }

    private void LoadMultiplayer(int index, string opponentName = "")
    {
        new CoroutineWithData(this, NetworkManager.GetOpponent(opponentName), delegate (object serverData)
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
        });
    }

    private IEnumerator LoadLevelCo(int index)
    {
        yield return SceneManager.LoadSceneAsync("Level_" + index, LoadSceneMode.Additive);
        CurLevel = SceneManager.GetSceneByName("Level_" + index);
		SceneManager.UnloadSceneAsync("MainMenu");

		UnloadCurrentLevel ();

        IsInLevel = true;

        GUIController.Instance.CurrentMenu = GUIController.Instance.IngameMenu;
    }

    private void LoadLevel(int index)
    {
<<<<<<< HEAD
		UnloadCurrentLevel ();
		if (SceneManager.GetSceneByName ("MainMenu").name != null) {
			SceneManager.UnloadSceneAsync ("MainMenu");
		}
		SceneManager.LoadScene("Level_" + index, LoadSceneMode.Additive);
		CurLevel = SceneManager.GetSceneByName("Level_" + index);
=======
        SceneManager.LoadScene("Level_" + index, LoadSceneMode.Additive);
		UnloadCurrentLevel();
        CurLevel = SceneManager.GetSceneByName("Level_" + index);
        SceneManager.UnloadScene("MainMenu");
>>>>>>> bb7090b5af4fb6b9899b6c0b0be6b23de2a54162

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
			SceneManager.UnloadSceneAsync(CurLevel);
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
        Debug.Log("Start Evolution");

        EvolutionController.CreatePopulation(DummyAgent);
        EvolutionController.gameObject.SetActive(true);

        if (IsMultiplayer)
        {
            Agent challengerAgent = DummyAgent.CreateInstance();
            challengerAgent.Genome = new Genome(challengerNetwork);
            MultiplayerEvoController.CreatePopulation(challengerAgent);
            MultiplayerEvoController.gameObject.SetActive(true);

            MultiplayerEvoController.ActivateAll(false);
        }

        EvolutionController.ActivateAll(false);
    }

    public void EnableAllAgents()
    {
        EvolutionController.ActivateAll(true);
        if (MultiplayerEvoController != null)
            MultiplayerEvoController.ActivateAll(true);
    }

}
