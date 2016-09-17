using System;
using UnityEngine;


public class Agent : MonoBehaviour, IComparable<Agent>
{
    private Genome genome;
    public Genome Genome
    {
        get
        {
            return genome;
        }
        set
        {
            genome = value;
            genome.ParentAgent = this;
            genome.FitnessMethod = this.fitnessMethod;
        }
    }

    public bool IsAlive
    {
        get;
        protected set;
    }

    void Awake()
    {
        IsAlive = true;
    }

    public event Action<Agent> OnAgentDied;

    protected Genome.FitnessFunction fitnessMethod;

    public virtual void Init()
    {

    }

    /// <summary>
    /// Resets all variables to starting position.
    /// </summary>
    public virtual void Restart()
    {
        IsAlive = true;
    }

    protected virtual void Die()
    {
        IsAlive = false;

        if (OnAgentDied != null)
            OnAgentDied(this);
    }

    public int CompareTo(Agent other)
    {
        return this.Genome.CompareTo(other.Genome);
    }
}

