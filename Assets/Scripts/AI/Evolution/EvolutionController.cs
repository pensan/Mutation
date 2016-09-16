using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EvolutionController : MonoBehaviour
{
    private static System.Random randomizer = new System.Random();

    public Agent DummyAgent;
    public int PopulationCount = 5;

    private Agent[] population;
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

    private float crossBreedPerc = 1f;
    private float mutatePerc = 1f;

    private float mutationProb = 0.15f;
    private float mutationAmount = 5f;

    public event System.Action<Agent> OnBestChanged;

    void Start()
    {
        population = new Agent[PopulationCount];

        for (int i = 0; i < population.Length-1; i++)
        {
            population[i] = Instantiate(DummyAgent);
            population[i].OnAgentDied += AgentDied;
        }
        population[population.Length - 1] = DummyAgent;
        population[population.Length - 1].OnAgentDied += AgentDied;

        aliveCount = population.Length;
    }

    void FixedUpdate()
    {
        foreach (Agent agent in population)
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
	

    private void AgentDied(Agent agent)
    {
        agent.OnAgentDied -= AgentDied;

        aliveCount--;
        if (aliveCount <= 0)
            StartCoroutine(Repopulate());
    }

    private IEnumerator Repopulate()
    {
        yield return new WaitForSeconds(0.3f);

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

        CrossBestSecondBest();
        //CrossBest(3, 2);
        MutateAll();

        aliveCount = population.Length;
    }


    private void CrossBest(int bestCount, int childsPerCross)
    {
        List<Agent> sorted = new List<Agent>();
        sorted.AddRange(population);
        sorted.Sort();

        Genome bestGenome = BestAgent.Genome.DeepCopy();
        population[0].Genome = bestGenome;

        int k = 1;
        for (int i = 0; i<bestCount; i++)
        {
            for (int j = i+1; j<bestCount; j++)
            {
                Genome child1, child2;
                sorted[i].Genome.CrossBreedCross(sorted[j].Genome, out child1, out child2);
                population[k].Genome = child1;
                population[k + 1].Genome = child2;
                k+=2;
                if (k >= population.Length-1)
                    break;
            }
            if (k >= population.Length-1)
                break;
        }

        //Fill remaining with random children
        for (; k < population.Length; k++)
            population[k].Genome.RandomizeNeuralNet(-1, 1);

        foreach (Agent agent in population)
        {
            agent.Restart();
            agent.OnAgentDied += AgentDied;
        }
    }

    private void CrossBestSecondBest()
    {
        for (int i = 0; i < population.Length; i++)
        {
            if (!(population[i].Genome == BestAgent.Genome || population[i].Genome == SecondBestAgent.Genome))
            {
                if (randomizer.NextDouble() < crossBreedPerc)
                {
                    //Genome newGenome = BestGenome.Genome.CrossBreedSimilarity(SecondBestGenome.Genome, 1f, mutationProb, mutationAmount);
                    Genome newGenome = BestAgent.Genome.CrossBreed(SecondBestAgent.Genome);
                    //Genome newGenome = BestGenome.Genome.CrossBreedCross(SecondBestGenome.Genome);
                    population[i].Genome = newGenome;
                }
            }

            population[i].Restart();
            population[i].OnAgentDied += AgentDied;
        }
    }


    private void MutateAll()
    {
        for (int i = 0; i < population.Length; i++)
        {
            if (randomizer.NextDouble() < mutatePerc)
                population[i].Genome.Mutate(mutationProb, mutationAmount);
        }
    }
}
