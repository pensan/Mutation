using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class IngameMenuParameters : MenuScreen
{
    public Slider MutationProbSlider;
    public Slider MutationAmountSlider;

    public Text MutationProbSliderValueText;
    public Text MutationAmountSliderValueText;

    public Toggle AutoBreedToggle;
    public GameObject timeoutPanel;
    public Slider timeoutSlider;
    public Text timeoutSliderTitle;

    void Awake()
    {
        MutationProbSlider.onValueChanged.AddListener(MutationPropSliderChangedHandler);
        MutationAmountSlider.onValueChanged.AddListener(MutatioAmountSliderChangedHandler);

        AutoBreedToggle.onValueChanged.AddListener(AutoBreedToggleChanged);
        timeoutSlider.onValueChanged.AddListener(TimeoutSliderChanged);
    }

    void Start()
    {
        AutoBreedToggleChanged(false);
        MutationPropSliderChangedHandler(MutationProbSlider.value);
        MutatioAmountSliderChangedHandler(MutationAmountSlider.value);

    }

    private void TimeoutSliderChanged(float time)
    {
        timeoutSliderTitle.text = string.Format("Rebreed after: {0} seconds", time.ToString("0"));

        LevelController.Instance.autoTimeoutTime = time;
    }

    public override void Show()
    {
        base.Show();

        AutoBreedToggle.gameObject.SetActive(!GameStateManager.Instance.IsMultiplayer);
    }

    public void AutoBreedToggleChanged(bool v)
    {
        LevelController.Instance.EvoType = v ? LevelController.EvolutionType.Automatic : LevelController.EvolutionType.User;

        if (v == true)
        {
            TimeoutSliderChanged(timeoutSlider.value);
            LevelController.Instance.StartTimeoutTimer();

            GameStateManager.Instance.EvolutionController.AutoRepopulate();
            GUIController.Instance.CurrentMenu = GUIController.Instance.IngameMenu;
            GameStateManager.Instance.CamMovement.SetCamPosInstant(GameStateManager.Instance.DummyAgent.StartPosition);
        }

        timeoutPanel.SetActive(v);
    }

    private void MutationPropSliderChangedHandler(float v)
    {
        GameStateManager.Instance.EvolutionController.mutationProb = v;
        MutationProbSliderValueText.text = string.Format("Mutation Probability: {0}%", (v * 100).ToString("0"));
    }

    private void MutatioAmountSliderChangedHandler(float v)
    {
        GameStateManager.Instance.EvolutionController.mutationAmount = v;
        MutationAmountSliderValueText.text = string.Format("Mutation Strength: {0}", v.ToString("0.##"));
    }
}
