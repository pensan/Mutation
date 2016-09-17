﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EvolutionController : MonoBehaviour
{
    private static System.Random randomizer = new System.Random();

    public Agent DummyAgent;
    public int PopulationCount = 5;

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

    private Genome alphaGenome = null;

    private float crossBreedPerc = 0.3f;
    private float mutatePerc = 1f;

    private float mutationProb = 0.25f;
    private float mutationAmount = 0.5f;

    public event System.Action<Agent> OnBestChanged;

    void Start()
    {
        Population = new Agent[PopulationCount];

        for (int i = 0; i < Population.Length-1; i++)
        {
            Population[i] = Instantiate(DummyAgent);
            Population[i].OnAgentDied += AgentDied;
            Population[i].Init();
        }
        Population[Population.Length - 1] = DummyAgent;
        Population[Population.Length - 1].OnAgentDied += AgentDied;
        Population[Population.Length - 1].Init();

        MutateAll(mutatePerc, mutationProb, mutationAmount, DummyAgent);

        aliveCount = Population.Length;
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
            BreedUIController.Instance.Show();
        }
    }

    private void DetermineAlpha()
    {
        //Determine new alpha genome
        if (alphaGenome == null || alphaGenome.Fitness < BestAgent.Genome.Fitness)
        {
            alphaGenome = BestAgent.Genome.DeepCopy();
            Debug.Log("New alpha genome");
        }
        else
        {
            BestAgent.Genome = alphaGenome.DeepCopy();
            Debug.Log("Reusing alpha genome");
        }
    }

    public void AutoRepopulate()
    {
        CrossBestSecondBest();

        MutateAll(mutatePerc, mutationProb, mutationAmount);

        aliveCount = Population.Length;
    }

    public void Repopulate(float mutationProb, float mutationAmount, params Agent[] crossAgents)
    {
        CrossAgents(crossAgents, false);

        MutateAll(1f, mutationProb, mutationAmount);

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

        foreach (Agent agent in Population)
        {
            agent.Restart();
            agent.OnAgentDied += AgentDied;
        }
    }

    private void CrossAgents(Agent[] agents, bool keepAgents)
    {
        int i = 0;
        if (keepAgents)
        {
            for (; i < agents.Length; i++)
            {
                Population[i].Genome = agents[i].Genome.DeepCopy();
            }
        }

        int a = 0, otherIndex = a + 1;
        for (; i < Population.Length; i++)
        {

            Genome newGenome = agents[a].Genome.CrossBreed(agents[otherIndex++].Genome);
            Population[i].Genome = newGenome;
            Population[i].Restart();
            Population[i].OnAgentDied += AgentDied;

            if (otherIndex > agents.Length)
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

            Population[i].Restart();
            Population[i].OnAgentDied += AgentDied;
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
