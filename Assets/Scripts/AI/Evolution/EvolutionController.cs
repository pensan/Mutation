using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class EvolutionController : MonoBehaviour
{
    private static System.Random randomizer = new System.Random();

    public int PopulationCount = 5;

    public float mutationProb = 0.25f;
    public float mutationAmount = 0.5f;

    public Agent[] Population
    {
        get;
        private set;
    }

    private int aliveCount;

    private Agent bestGenome;
    public Agent BestAgent
    {
        get { return bestGenome; }
        private set
        {
            if (bestGenome != value)
            {
                if (bestGenome != null)
                    SecondBestAgent = bestGenome;

                bestGenome = value;

                if (OnBestChanged != null)
                    OnBestChanged(bestGenome);
            }
        }
    }
    private Agent secondBestGenome;
    public Agent SecondBestAgent
    {
        get { return secondBestGenome; }
        private set
        {
            if (secondBestGenome != value)
                secondBestGenome = value;
        }
    }

    public Genome AlphaGenome
    {
        get;
        private set;
    }

    private float crossBreedPerc = 1f;
    private float mutatePerc = 1f;

    public event System.Action<Agent> OnBestChanged;
    public event System.Action OnAllAgentsDied;


    public void CreatePopulation(Agent seed)
    {
        KillEvolution(seed);

        Population = new Agent[PopulationCount];

        for (int i = 0; i < Population.Length - 1; i++)
        {
            Population[i] = seed.CreateInstance();
            Population[i].OnAgentDied += AgentDied;
            Population[i].Init();
            Population[i].Genome = seed.Genome.DeepCopy();
        }
        Population[Population.Length - 1] = seed;
        Population[Population.Length - 1].OnAgentDied += AgentDied;
        seed.Restart();
        seed.GenerationCount = 0;

        MutateAll(mutatePerc, mutationProb, mutationAmount, seed);

        aliveCount = Population.Length;
    }

    //Muhahahaha
    public void KillEvolution(params Agent[] destroyExclusion)
    {
        if (Population != null)
        {
            foreach (Agent agent in Population)
            {
                agent.OnAgentDied -= AgentDied;
                bool destroy = true;
                foreach (Agent excludedAgent in destroyExclusion)
                {
                    if (excludedAgent == agent)
                    {
                        destroy = false;
                        break;
                    }
                }

                if(destroy)
                    Destroy(agent.gameObject);
            }

            Population = null;
        }

        BestAgent = null;
        SecondBestAgent = null;
        AlphaGenome = null;
    }


    public void KillAll()
    {
        if (Population != null)
        {
            foreach (Agent agent in Population)
            {
                if (agent.IsAlive)
                    agent.Kill();
            }
        }
    }



    void FixedUpdate()
    {
        foreach (Agent agent in Population)
        {
            if (agent.Genome != null)
            {
                if (BestAgent == null || agent.Genome.Fitness >= BestAgent.Genome.Fitness)
                {
                    BestAgent = agent;
                }
                else if (SecondBestAgent == null || agent.Genome.Fitness >= SecondBestAgent.Genome.Fitness)
                {
                    SecondBestAgent = agent;
                }
            }
        }
    }
	

    private void AgentDied(Agent agent)
    {
        agent.OnAgentDied -= AgentDied;

        aliveCount--;
        if (aliveCount <= 0)
        {
            DetermineAlpha();

            if (OnAllAgentsDied != null)
                OnAllAgentsDied();
        }
    }


    private void DetermineAlpha()
    {
        //Determine new alpha genome
        if (AlphaGenome == null || AlphaGenome.Fitness < BestAgent.Genome.Fitness)
        {
            AlphaGenome = BestAgent.Genome.DeepCopy();
            Debug.Log("New alpha genome");
        }
    }

    public void AutoRepopulate()
    {
        AutoRepopulate(mutatePerc);
    }

    public void AutoRepopulate(float mutatePerc)
    {
        if (AlphaGenome != null && AlphaGenome.Fitness > BestAgent.Genome.Fitness)
        {
            BestAgent.Genome = AlphaGenome.DeepCopy();
            Debug.Log("Reusing alpha genome");
        }    

        CrossBestSecondBest();

        MutateAll(mutatePerc, mutationProb, mutationAmount);

        PopulationStart();
    }

    public void Repopulate(params Agent[] crossAgents)
    {
        CrossAgents(crossAgents, false);

        MutateAll(1f, mutationProb, mutationAmount);

        PopulationStart();
    }

    private void PopulationStart()
    {
        foreach (Agent agent in Population)
        {
            agent.Restart();
            agent.OnAgentDied += AgentDied;
            agent.GenerationCount++;
        }

        LevelController.Instance.StartTimeoutTimer();

        aliveCount = Population.Length;
    }


    private void CrossBest(int bestCount, int childsPerCross)
    {
        List<Agent> sorted = new List<Agent>();
        sorted.AddRange(Population);
        sorted.Sort();

        Genome bestGenome = BestAgent.Genome.DeepCopy();
        Population[0].Genome = bestGenome;

        int k = 1;
        for (int i = 0; i<bestCount; i++)
        {
            for (int j = i+1; j<bestCount; j++)
            {
                Genome child1, child2;
                sorted[sorted.Count - 1 - i].Genome.CrossBreedCross(sorted[sorted.Count - 1 - j].Genome, out child1, out child2);
                Population[k].Genome = child1;
                Population[k + 1].Genome = child2;
                k+=2;
                if (k >= Population.Length-1)
                    break;
            }
            if (k >= Population.Length-1)
                break;
        }

        //Fill remaining with random children
        for (; k < Population.Length; k++)
            Population[k].Genome.RandomizeNeuralNet(-1, 1);
    }

    private void CrossAgents(Agent[] agents, bool keepAgents)
    {
        int i = 0;
        if (keepAgents)
        {
            for (; i < agents.Length; i++)
                Population[i].Genome = agents[i].Genome.DeepCopy();
        }

        int a = 0, otherIndex = a + 1;
        for (; i < Population.Length; i++)
        {

            Genome newGenome = agents[a].Genome.CrossBreed(agents[otherIndex++].Genome);
            Population[i].Genome = newGenome;

            if (otherIndex >= agents.Length)
            {
                a = (a + 1) % agents.Length - 1;
                otherIndex = a + 1;
            }
        }
    }

    private void CrossBestSecondBest()
    {
        for (int i = 0; i < Population.Length; i++)
        {
            if (randomizer.NextDouble() < crossBreedPerc)
            {
                Genome newGenome = BestAgent.Genome.CrossBreed(SecondBestAgent.Genome);
                //Genome newGenome, otherChild;
                //BestAgent.Genome.CrossBreedCross(SecondBestAgent.Genome, out newGenome, out otherChild);
                Population[i].Genome = newGenome;
            }
        }
    }


    private void MutateAll(float mutatePerc, float mutationProb, float mutationAmount, params Agent[] exceptions)
    {
        List<Agent> exceptionList = new List<Agent>();
        exceptionList.AddRange(exceptions);

        for (int i = 0; i < Population.Length; i++)
        {
            if (!exceptionList.Contains(Population[i]) && randomizer.NextDouble() < mutatePerc)
                Population[i].Genome.Mutate(mutationProb, mutationAmount);
        }
    }
}
