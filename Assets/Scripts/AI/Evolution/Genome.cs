using System;
using UnityEngine;

public class Genome : IComparable<Genome>
{
    private static System.Random randomizer = new System.Random();

    public Agent ParentAgent
    {
        get;
        set;
    }


    public float Fitness
    {
        get;
        private set;
    }

    public delegate float FitnessFunction();
    public FitnessFunction FitnessMethod;

    private NeuralNetwork neuralNet;

    public Genome(NeuralNetwork neuralNet, Agent parent = null)
    {
        this.ParentAgent = parent;
        this.neuralNet = neuralNet;
        Fitness = 0;
    }

    public void UpdateFitness()
    {
        if (FitnessMethod != null)
            Fitness = FitnessMethod();
        else
            Fitness = 0;
    }

    public double[] CalculateOutputs(double[] inputs)
    {
        return neuralNet.CalculateYValues(inputs);
    }


    public Genome CrossBreed(Genome other)
    {
        NeuralNetwork newNeuralNet = this.neuralNet.GetTopologyCopy();

        for (int l = 0; l < newNeuralNet.Layers.Length; l++)
        {
            for (int x = 0; x < newNeuralNet.Layers[l].Weights.GetLength(0); x++)
            {
                for (int y = 0; y < newNeuralNet.Layers[l].Weights.GetLength(1); y++)
                {
                    NeuralNetwork sourceNet;
                    if (randomizer.NextDouble() >= 0.40)
                        sourceNet = this.neuralNet;
                    else
                        sourceNet = other.neuralNet;

                    newNeuralNet.Layers[l].Weights[x, y] = sourceNet.Layers[l].Weights[x, y];
                }
            }
        }

        return new Genome(newNeuralNet);
    }

    public Genome CrossBreedSimilarity(Genome other, float differenceThreshold, float mutateProb, float mutateAmount)
    {
        NeuralNetwork newNeuralNet = this.neuralNet.GetTopologyCopy();

        for (int l = 0; l < newNeuralNet.Layers.Length; l++)
        {
            for (int x = 0; x < newNeuralNet.Layers[l].Weights.GetLength(0); x++)
            {
                for (int y = 0; y < newNeuralNet.Layers[l].Weights.GetLength(1); y++)
                {
                    double smaller, bigger;
                    if (this.neuralNet.Layers[l].Weights[x, y] < other.neuralNet.Layers[l].Weights[x, y])
                    {
                        smaller = neuralNet.Layers[l].Weights[x, y];
                        bigger = other.neuralNet.Layers[l].Weights[x, y];
                    }
                    else
                    {
                        bigger = neuralNet.Layers[l].Weights[x, y];
                        smaller = other.neuralNet.Layers[l].Weights[x, y];
                    }
                    double diff = bigger - smaller;
                    if (diff <= differenceThreshold)
                        //Choose middle between weights of both
                        newNeuralNet.Layers[l].Weights[x, y] = smaller + 0.5 * diff;
                    else if (randomizer.NextDouble() <= mutateProb)
                    {
                        NeuralNetwork sourceNet;
                        if (randomizer.NextDouble() >= 0.40)
                            sourceNet = this.neuralNet;
                        else
                            sourceNet = other.neuralNet;

                        newNeuralNet.Layers[l].Weights[x, y] =  sourceNet.Layers[l].Weights[x, y] + randomizer.NextDouble() * (mutateAmount * 2) - mutateAmount;
                    }
                }
            }
        }

        return new Genome(newNeuralNet);
    }


    public void CrossBreedCross(Genome other, out Genome child1, out Genome child2)
    {
        NeuralNetwork netChild1 = this.neuralNet.GetTopologyCopy();
        NeuralNetwork netChild2 = this.neuralNet.GetTopologyCopy();
        int weightAmount = 0;
        foreach (NeuralLayer layer in netChild1.Layers)
            weightAmount += layer.Weights.Length;

        int cross = (int) randomizer.NextDouble() * weightAmount;
        NeuralNetwork first, second;
        if (randomizer.NextDouble() > 0.5)
        {
            first = this.neuralNet;
            second = other.neuralNet;
        }
        else
        {
            first = other.neuralNet;
            second = this.neuralNet;
        }

        int k = 0;
        for (int l = 0; l < netChild1.Layers.Length; l++)
        {
            for (int x = 0; x < netChild1.Layers[l].Weights.GetLength(0); x++)
            {
                for (int y = 0; y < netChild1.Layers[l].Weights.GetLength(1); y++)
                {
                    if (k < cross)
                    {
                        netChild1.Layers[l].Weights[x, y] = first.Layers[l].Weights[x, y];
                        netChild2.Layers[l].Weights[x, y] = second.Layers[l].Weights[x, y];
                    }
                    else
                    {
                        netChild1.Layers[l].Weights[x, y] = second.Layers[l].Weights[x, y];
                        netChild2.Layers[l].Weights[x, y] = first.Layers[l].Weights[x, y];
                    }
                    k++;
                }
            }
        }

        child1 = new Genome(netChild1);
        child2 = new Genome(netChild2);
    }

    public void Mutate(float mutateProb, float mutateAmount)
    {
        foreach (NeuralLayer layer in neuralNet.Layers)
        {
            for (int x = 0; x < layer.Weights.GetLength(0); x++)
            {
                for (int y = 0; y < layer.Weights.GetLength(1); y++)
                {
                    if (randomizer.NextDouble() <= mutateProb)
                    {
                        double mutate = randomizer.NextDouble() * (mutateAmount * 2) - mutateAmount;
                        Debug.Log("Mutating by: " + mutate);
                        layer.Weights[x, y] += mutate;
                    }
                }
            }
        }
    }

    public int CompareTo(Genome other)
    {
        return this.Fitness.CompareTo(other.Fitness);
    }

    public void RandomizeNeuralNet(double rangeStart, double rangeEnd)
    {
        this.neuralNet.FillWithRandomWeights(rangeStart, rangeEnd);
    }

    public Genome DeepCopy()
    {
        Genome newGenome = new Genome(this.neuralNet.DeepCopy());
        newGenome.Fitness = this.Fitness;
        return newGenome;
    }
}
