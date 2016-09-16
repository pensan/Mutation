using System;
using UnityEngine;


public class Agent : MonoBehaviour
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
            genome.Parent = this;
            genome.FitnessMethod = this.fitnessMethod;
        }
    }


    public event Action<Agent> OnAgentDied;

    protected Genome.FitnessFunction fitnessMethod;

    /// <summary>
    /// Resets all variables to starting position.
    /// </summary>
    public virtual void Restart()
    {

    }

}

