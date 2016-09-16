using System;

public static class MathHelper
{

    public static double SigmoidFunction(double xValue)
    {
        if (xValue > 10) return 1.0;
        else if (xValue < -10) return 0.0;
        else return 1.0 / (1.0 + Math.Exp(-xValue));
    }

    public static double TanHFunction(double xValue)
    {
        if (xValue > 10) return 1.0;
        else if (xValue < -10) return -1.0;
        else return Math.Tanh(xValue);
    }

}
