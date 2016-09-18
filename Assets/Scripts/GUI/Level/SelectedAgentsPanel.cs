using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectedAgentsPanel : MonoBehaviour
{
    public Transform ContentPanel;
    public AgentPanel DummyAgent;

    private LinkedList<AgentPanel> agentPool;

    public int AgentCount
    {
        get { return agentPool.Count; }
    }

    public event System.Action OnAgentAdded;
    public event System.Action OnAgentRemoved;

    void Awake ()
    {
        agentPool = new LinkedList<AgentPanel>();
        DummyAgent.gameObject.SetActive(false);
    }

    public void AddAgent(Agent newAgent)
    {
        AgentPanel copy = Instantiate(DummyAgent);
        copy.transform.SetParent(ContentPanel, true);
        copy.Agent = newAgent;
        agentPool.AddLast(copy);
        copy.gameObject.SetActive(true);

        if (OnAgentAdded != null)
            OnAgentAdded();
    }

    public void RemoveAgent(Agent agent)
    {
        LinkedListNode<AgentPanel> curPanel = agentPool.First;
        while (curPanel != null)
        {
            if (curPanel.Value.Agent == agent)
            {
                agentPool.Remove(curPanel);
                break;
            }
            else
                curPanel = curPanel.Next;
        }

        if (curPanel != null)
        {
            Destroy(curPanel.Value.gameObject);
            if (OnAgentRemoved != null)
                OnAgentRemoved();
        }
        else
            Debug.LogWarning("Tried to remove runner from selected runners, that wasn't in agentPool.");
    }

    public Agent[] GetSelectedAgents()
    {
        Agent[] agents = new Agent[agentPool.Count];
        int i = 0;
        foreach (AgentPanel a in agentPool)
            agents[i++] = a.Agent;

        return agents;
    }

    public void Clear()
    {
        foreach (AgentPanel panel in agentPool)
            Destroy(panel.gameObject);

        agentPool.Clear();
    }

}
