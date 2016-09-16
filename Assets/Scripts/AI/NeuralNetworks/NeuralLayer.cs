using System;
using System.Collections;

public class NeuralLayer
{
    private static Random r = new Random();


    public delegate double ActivationFunction(double xValue);

    public ActivationFunction ActivationMethod;


    public int NodeCount
    {
        get;
        private set;
    }
    public int OutputCount
    {
        get;
        private set;
    }

    public double[,] Weights
    {
        get;
        private set;
    }


	public NeuralLayer(int nodeCount, int outputCount)
    {
        this.NodeCount = nodeCount;
        this.OutputCount = outputCount;

        Weights = new double[nodeCount + 1, outputCount];
    }


    public void SetWeights(double[] weights)
    {
        if (weights.Length != this.Weights.Length)
            throw new ArgumentException("Input weights do not match layer weight count.");

        int k = 0;
        for (int i = 0; i < this.Weights.GetLength(0); i++)
            for (int j = 0; j < this.Weights.GetLength(1); j++)
                this.Weights[i, j] = weights[k++];
    }

    public void FillWithRandomWeights(double rangeStart, double rangeEnd)
    {
        double range = Math.Abs(rangeEnd - rangeStart);
        for (int i = 0; i < Weights.GetLength(0); i++)
            for (int j = 0; j < Weights.GetLength(1); j++)
                Weights[i, j] = rangeStart + (r.NextDouble() * range); //random number between -10, 10
    }

    public double[] CalculateYValues(double[] xValues)
    {
        if (xValues.Length != NodeCount)
            throw new ArgumentException("Given xValues do not match layer input count.");

        double[] sums = new double[OutputCount];
        double[] inputs = new double[NodeCount + 1];
        xValues.CopyTo(inputs, 0);
        inputs[xValues.Length] = 1.0;

        for (int j = 0; j < this.Weights.GetLength(1); j++)
            for (int i = 0; i < this.Weights.GetLength(0); i++)
                sums[j] += inputs[i] * Weights[i, j];

        if (ActivationMethod != null)
        {
            for (int i = 0; i < sums.Length; i++)
                sums[i] = ActivationMethod(sums[i]);
        }

        return sums;
    }


    public NeuralLayer DeepCopy()
    {
        double[,] copiedWeights = new double[this.Weights.GetLength(0), this.Weights.GetLength(1)];

        for (int x = 0; x < this.Weights.GetLength(0); x++)
            for (int y = 0; y < this.Weights.GetLength(1); y++)
                copiedWeights[x, y] = this.Weights[x, y];

        NeuralLayer newLayer = new NeuralLayer(this.NodeCount, this.OutputCount);
        newLayer.Weights = copiedWeights;
        newLayer.ActivationMethod = this.ActivationMethod;

        return newLayer;
    }
}
