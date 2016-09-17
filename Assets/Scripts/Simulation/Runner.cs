using UnityEngine;
using System.Collections;

public class Runner : Agent
{
    private const float MAX_LIFE_TIME = 5f;

    private Vector3 startPosition;

    private Sensor[] sensors;

    public RunnerMovement Movement
    {
        get;
        private set;
    }

    private float lifeTime = 0;

    void Start()
    {
        startPosition = this.transform.position;

        sensors = GetComponentsInChildren<Sensor>();
        Movement = GetComponent<RunnerMovement>();

        fitnessMethod = UpdateFitness;

        SerializeableNeuralNetwork loadedNetwork = Serializer.Instance.LoadNetwork();

        if (loadedNetwork != null)
        {
            NeuralNetwork neuralNet = new NeuralNetwork(loadedNetwork);
            neuralNet.Layers[2].CurrentActivationFunction = ActivationFunctions.TANH;
            base.Genome = new Genome(neuralNet);
        }
        else
        {
            NeuralNetwork neuralNet = new NeuralNetwork(sensors.Length, 6, 4, 2);
            neuralNet.Layers[2].CurrentActivationFunction = ActivationFunctions.TANH;
            base.Genome = new Genome(neuralNet);
            Genome.RandomizeNeuralNet(-1, 1);
        }
 
    }


    public override void Restart()
    {
        base.Restart();

        this.enabled = true;
        this.Movement.enabled = true;
        this.Movement.Reset();
        

        lifeTime = 0;
        this.transform.position = startPosition;
    }

    void FixedUpdate()
    {
        lifeTime += Time.deltaTime;

        if (lifeTime >= MAX_LIFE_TIME)
        {
            Die();
        }

        if (!Movement.UseUserInput)
            CalculateInputs();

        if (this.transform.position.y < -100)
            Die();

        Genome.UpdateFitness();
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
        return transform.position.x - startPosition.x;
    }


    protected override void Die()
    {
        base.Die();

        Debug.Log("Agent died");

        this.Movement.Reset();
        this.Movement.RigidBodyComponent.simulated = false;
        this.Movement.enabled = false;
        this.enabled = false;
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Trap"))
        {
            Die();
        }
    }
}
