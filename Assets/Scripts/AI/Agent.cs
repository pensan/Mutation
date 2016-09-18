using System;
using UnityEngine;


public class Agent : MonoBehaviour, IComparable<Agent>
{
    [HideInInspector]
    public Vector3 StartPosition;

    public enum GenomeCreation
    {
        Random, Load, None
    }

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
    protected Action CreateGenome;
    private GenomeCreation genomeCreation = GenomeCreation.None;
    public GenomeCreation GenomeCreationMethod
    {
        get { return genomeCreation; }
        set
        {
            genomeCreation = value;

            switch (genomeCreation)
            {
                case GenomeCreation.Load:
                    CreateGenome = LoadCurrentGenome;
                    break;
                case GenomeCreation.Random:
                    CreateGenome = RandomizeGenome;
                    break;
                case GenomeCreation.None:
                    CreateGenome = null;
                    break;
            }
        }

    }
    protected Genome.FitnessFunction fitnessMethod;

    public virtual Agent CreateInstance()
    {
        Agent newAgent = Instantiate(this);
        newAgent.GenomeCreationMethod = this.GenomeCreationMethod;
        newAgent.StartPosition = this.StartPosition;
        return newAgent;
    }

    public virtual void Init()
    {

        Restart();
    }

    public virtual void LoadCurrentGenome()
    {
        SerializeableNeuralNetwork loadedNetwork = Serializer.Instance.LoadNetwork();

        if (loadedNetwork != null)
        {
            NeuralNetwork neuralNet = new NeuralNetwork(loadedNetwork);
            neuralNet.Layers[2].CurrentActivationFunction = ActivationFunctions.TANH;
            this.Genome = new Genome(neuralNet);
        }
        else
        {
            RandomizeGenome();
        }
    }

    public virtual void RandomizeGenome()
    {
        
    }

    /// <summary>
    /// Resets all variables to starting position.
    /// </summary>
    public virtual void Restart()
    {
        IsAlive = true;
    }

    public void Kill()
    {
        if (IsAlive)
            Die();
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

