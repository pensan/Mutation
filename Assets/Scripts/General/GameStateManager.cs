using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class GameStateManager : MonoBehaviour
{

    public EvolutionController EvolutionController;
    public CameraMovement CamMovement;
    public Agent DummyAgent;

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

    void Awake()
    {
        SceneManager.LoadScene("GUI", LoadSceneMode.Additive);
        LoadMainMenu();

        Instance = this;
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene("Level_" + index, LoadSceneMode.Additive);
        SceneManager.UnloadScene("MainMenu");
        GUIController.Instance.CurrentMenu = GUIController.Instance.IngameMenu;

        CurLevel = SceneManager.GetSceneByName("Level_" + index);
        IsInLevel = true;
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
    }

    public void StartEvolution()
    {
        EvolutionController.CreatePopulation(DummyAgent);
        EvolutionController.gameObject.SetActive(true);
    }

}
