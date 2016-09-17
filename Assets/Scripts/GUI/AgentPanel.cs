using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AgentPanel : MonoBehaviour
{
    public Text nameText;
    public Image agentImage;

    private Agent agent;
    public Agent Agent
    {
        get { return agent; }
        set
        {
            agent = value;
            nameText.text = agent.name;
        }
    }

	

}
