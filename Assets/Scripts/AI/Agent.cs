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

    protected Genome genome;
    public virtual Genome Genome
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

    private int generationCount;
    public virtual int GenerationCount
    {
        get
        {
            return generationCount;
        }
        set
        {
            generationCount = value;
        }
    }

    public bool IsAlive
    {
        get;
        protected set;
    }

    private bool isInitialized = false;
    public bool IsInitialized
    {
        get { return isInitialized; }
        private set
        {
            isInitialized = value;
        }
    }

    void Awake()
    {
        IsAlive = true;
    }

    public event Action<Agent> OnAgentDied;

    protected Genome.FitnessFunction fitnessMethod;

    public virtual Agent CreateInstance()
    {
        Agent newAgent = Instantiate(this);
        newAgent.StartPosition = this.StartPosition;
        return newAgent;
    }

    public virtual void Init()
    {

        Restart();

        isInitialized = true;
    }

    public virtual void LoadCurrentGenome()
    {
        SerializeableNeuralNetwork loadedNetwork = Serializer.LoadNetwork();

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

