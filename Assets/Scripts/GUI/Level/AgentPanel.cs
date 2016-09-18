using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AgentPanel : MonoBehaviour
{
    public InputField NameText;
    public Text GenerationText;
    public Image AgentImage;

    void Start()
    {
        NameText.onEndEdit.AddListener(EditAgentName);
    }

    private Runner agent;
    public Runner Agent
    {
        get { return agent; }
        set
        {
            agent = value;
            NameText.text = agent.FirstName;
            GenerationText.text = agent.GenerationName;
        }
    }

	private void EditAgentName(string name)
    {
        this.Agent.FirstName = name;
    }

}
