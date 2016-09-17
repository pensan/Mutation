using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class Runner : Agent
{
    private const float MAX_LIFE_TIME = 5f;

    private Vector3 startPosition;

    private Sensor[] sensors;

    public RunnerAppearance appearance;

    public RunnerMovement Movement
    {
        get;
        private set;
    }

    private float lifeTime = 0;
    private Selectable selectableComponent;

    public bool Selectable
    {
        get { return selectableComponent.enabled; }
        set
        {
            selectableComponent.enabled = value;
            if (value)
                appearance.SetOpaque(selectableComponent.Selected);
            else
                appearance.SetOpaque(true);
        }
    }
    private TrailRenderer trailRenderer;

    void Start()
    {
        startPosition = this.transform.position;
        selectableComponent = GetComponent<Selectable>();
        selectableComponent.OnSelectChanged += SelectThisAgent;
        selectableComponent.enabled = false;

        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.sortingLayerName = "Background";
        trailRenderer.sortingOrder = -10;
    }

    public override void Init()
    {
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

        appearance.UpdateAppearance(Genome.neuralNet);
    }


    public override void Restart()
    {
        base.Restart();

        appearance.UpdateAppearance(Genome.neuralNet);

        appearance.SetOpaque(true);
        trailRenderer.sortingLayerName = "Background";
        trailRenderer.Clear();

        this.Movement.enabled = true;
        this.Movement.Reset();
        

        lifeTime = 0;
        this.transform.position = startPosition;

        selectableComponent.Select(false);
        selectableComponent.enabled = false;
    }

    void FixedUpdate()
    {
        if (!IsAlive) return;

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

        this.Movement.Reset();
        this.Movement.RigidBodyComponent.isKinematic = true;
        this.Movement.enabled = false;
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Trap"))
        {
            Die();
        }
    }


    private void SelectThisAgent(bool selected)
    {
        appearance.SetOpaque(selected);
        if (selected)
        {
            BreedUIController.Instance.SelectedAgents.AddAgent(this);
            trailRenderer.sortingLayerName = appearance.GetComponent<SpriteRenderer>().sortingLayerName;
        }
        else
        {
            trailRenderer.sortingLayerName = "Background";
            BreedUIController.Instance.SelectedAgents.RemoveAgent(this);
        }
    }

}
