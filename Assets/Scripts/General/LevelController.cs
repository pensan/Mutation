using UnityEngine;
using System.Collections;
using System;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

    public enum EvolutionType
    {
        Automatic, User
    }

    public EvolutionType EvoType = EvolutionType.User;

    public Transform StartPosition;
    public RectTransform CameraBounds;
    public bool LoadGenome = true;
    public bool FollowBestAgent = true;

    public float autoTimeoutTime = -1.0f;

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
        camMovement.Bounds = CameraBounds;
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
        StopCoroutine("TimeoutCo");
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

    internal void StartTimeoutTimer()
    {
        if (autoTimeoutTime > 0.0f)
        {
            StopCoroutine("TimeoutCo");
            StartCoroutine("TimeoutCo");
        }
    }

    private IEnumerator TimeoutCo()
    {
        float timer = 0.0f;
        while (timer < autoTimeoutTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        GameStateManager.Instance.EvolutionController.KillAll();
    }
}
