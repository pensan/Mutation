using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogInGame : MonoBehaviour
{
    public Text HeaderText;
    public Text ContentText;
    public Button OKButton;
	public Button NextButton;
    
    void Awake()
    {
		OKButton.onClick.AddListener(Hide);
		NextButton.onClick.AddListener(delegate() 
		{
			Time.timeScale = 1.0f;	
			LevelController.Instance.StopTimeoutTimer();
			if (GameStateManager.Instance.IsTraining)
			{
				GameStateManager.Instance.SaveCurrentNeuralNet();
			}
				GUIController.Instance.MainMenu.LevelIndex++;
				GameStateManager.Instance.EvolutionController.PopulationCount = 10;
				GameStateManager.Instance.EvolutionController.mutationAmount = 0.5f;
				GameStateManager.Instance.EvolutionController.mutationProb = 0.15f;

				GameStateManager.Instance.LoadSingleplayerLevel(GUIController.Instance.MainMenu.LevelIndex);

				this.gameObject.SetActive(false);
		});
    }

	public void Show(string header, string content, bool isNext = false)
    {
        this.gameObject.SetActive(true);

        HeaderText.text = header;
        ContentText.text = content;
		if (isNext) {
			NextButton.gameObject.SetActive (true);
			OKButton.gameObject.SetActive (false);
		} else {
			NextButton.gameObject.SetActive (false);
			OKButton.gameObject.SetActive (true);
		}
		Time.timeScale = 0.0f;
    }

	public void Hide()
	{
		Time.timeScale = 1.0f;
		this.gameObject.SetActive (false);
	}
		
}
