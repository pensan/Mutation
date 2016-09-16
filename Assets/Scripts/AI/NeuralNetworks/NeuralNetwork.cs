using System;
using System.Collections;

public class NeuralNetwork
{

    public NeuralLayer[] Layers
    {
        get;
        private set;
    }

    public int[] Topology
    {
        get;
        private set;
    }

    public NeuralNetwork(params int[] topology)
    {
        this.Topology = topology;

        Layers = new NeuralLayer[topology.Length - 1];


        for (int i = 0; i<Layers.Length; i++)
        {
            Layers[i] = new NeuralLayer(topology[i], topology[i + 1]);
        }
    }


    public double[] CalculateYValues(double[] xValues)
    {
        if (xValues.Length != Layers[0].NodeCount)
            throw new ArgumentException("Given xValues do not match network input amount.");

        double[] output = xValues;
        foreach (NeuralLayer layer in Layers)
            output = layer.CalculateYValues(output);

        return output;
    }

    public void FillWithRandomWeights(double rangeStart, double rangeEnd)
    {
        foreach (NeuralLayer layer in Layers)
            layer.FillWithRandomWeights(rangeStart, rangeEnd);
    }

    public NeuralNetwork GetTopologyCopy()
    {
        NeuralNetwork copy = new NeuralNetwork(this.Topology);
        for (int i = 0; i < Layers.Length; i++)
            copy.Layers[i].ActivationMethod = this.Layers[i].ActivationMethod;

        return copy;
    }

    public NeuralNetwork DeepCopy()
    {
        NeuralNetwork newNet = new NeuralNetwork(this.Topology);
        for (int i = 0; i < this.Layers.Length; i++)
            newNet.Layers[i] = this.Layers[i].DeepCopy();

        return newNet;
    }
}
