using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResetNNButton : MonoBehaviour
{
    private Button buttonComponent;

	void Awake()
    {
        buttonComponent = GetComponent<Button>();
        buttonComponent.onClick.AddListener(GameStateManager.Instance.ResetNeuralNet);
    }
}
