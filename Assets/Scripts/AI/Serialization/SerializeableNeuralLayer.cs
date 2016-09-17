// REDOX Game Labs 2016

#region INCLUDES
using UnityEngine;
#endregion

public class SerializeableNeuralLayer 
{
    public ActivationFunctions curentActivationFunction;
    public int nodeCount;
    public int outputCount;

    public double[,] Weights;
}
