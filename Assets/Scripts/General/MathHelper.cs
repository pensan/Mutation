using System;

public static class MathHelper
{

    private static double SigmoidFunction(double xValue)
    {
        if (xValue > 10) return 1.0;
        else if (xValue < -10) return 0.0;
        else return 1.0 / (1.0 + Math.Exp(-xValue));
    }

    private static double TanHFunction(double xValue)
    {
        if (xValue > 10) return 1.0;
        else if (xValue < -10) return -1.0;
        else return Math.Tanh(xValue);
    }

    public static NeuralLayer.ActivationFunction GetActivationFunction(ActivationFunctions f)
    {
        switch (f)
        {
            case (ActivationFunctions.SIGMOID):
                return SigmoidFunction;
            case (ActivationFunctions.TANH):
                return TanHFunction;
            default:
                return SigmoidFunction;
        }
    }

}

public enum ActivationFunctions
{
    SIGMOID = 0,
    TANH = 1
}
