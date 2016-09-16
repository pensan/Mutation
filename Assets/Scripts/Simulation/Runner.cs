using UnityEngine;
using System.Collections;

public class Runner : Agent
{
    private Vector3 startPosition;

    private Sensor[] sensors;

    public RunnerMovement Movement
    {
        get;
        private set;
    }


    void Awake()
    {
        startPosition = this.transform.position;

        sensors = GetComponentsInChildren<Sensor>();
        Movement = GetComponent<RunnerMovement>();

        NeuralNetwork neuralNet = new NeuralNetwork(4, 5, 4, 2);
        neuralNet.Layers[2].ActivationMethod = MathHelper.TanHFunction;
        base.Genome = new Genome(neuralNet);
        base.Genome.FitnessMethod = UpdateFitness;
        Genome.RandomizeNeuralNet(-1, 1);
    }


    public override void Restart()
    {
        base.Restart();

        this.transform.position = startPosition;
    }

    void FixedUpdate()
    {
        if (!Movement.UseUserInput)
            CalculateInputs();
    }

    private void CalculateInputs()
    {
        double[] sensorInput = new double[sensors.Length];

        for (int i = 0; i < sensors.Length; i++)
            sensorInput[i] = sensors[i].Output;

        double[] outputs = base.Genome.CalculateOutputs(sensorInput);

        Movement.CurInput = outputs;
    }


    private float UpdateFitness()
    {
        return 0;
    }
}
