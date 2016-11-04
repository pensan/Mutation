
#region INCLUDES
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
#endregion

[System.Serializable]
public class AgentPanel : MonoBehaviour
{
	public List<Sprite> portraitsBody1;
	public List<Sprite> portraitsBody2;
	public Image testImage;
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
			AgentImage.sprite = Instantiate(portraitsBody1[getRandomImage(agent.Appearance.body.name)]);
			AgentImage.color = agent.Appearance.body.color;
        }
    }

	private void EditAgentName(string name)
    {
        this.Agent.FirstName = name;
    }

	public int getRandomImage(string bodyType) {
		if (bodyType == "Body") {
			System.Random r = new System.Random ();
			return r.Next (0, portraitsBody1.Count);
		} else if (bodyType == "Body2") {
			System.Random r = new System.Random ();
			return r.Next (0, portraitsBody2.Count);
		} else {
			return 0;
		}
	}	
}
