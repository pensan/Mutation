using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameStateManager : MonoBehaviour
{

    public EvolutionController EvolutionController;

	void Awake()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        EvolutionController.CreatePopulation();
        EvolutionController.gameObject.SetActive(true);
    }
}
