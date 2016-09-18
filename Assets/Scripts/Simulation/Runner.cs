using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class Runner : Agent
{
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


    public override void Init()
    {
        sensors = GetComponentsInChildren<Sensor>();
        Movement = GetComponent<RunnerMovement>();
        selectableComponent = GetComponent<Selectable>();
        selectableComponent.OnSelectChanged += SelectThisAgent;
        selectableComponent.enabled = false;

        appearance = GetComponentInChildren<RunnerAppearance>();

        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.sortingOrder = -10;

        fitnessMethod = UpdateFitness;

        base.CreateGenome();

        appearance.UpdateAppearance(Genome.NeuralNet);

        base.Init();
    }

    public override void RandomizeGenome()
    {
        base.RandomizeGenome();

        NeuralNetwork neuralNet = new NeuralNetwork(sensors.Length, 6, 4, 2);
        neuralNet.Layers[2].CurrentActivationFunction = ActivationFunctions.TANH;
        this.Genome = new Genome(neuralNet);
        Genome.RandomizeNeuralNet(-1, 1);
    }

    public override void Restart()
    {
        base.Restart();

        selectableComponent.Select(false);
        appearance.SetOpaque(true);
        appearance.UpdateAppearance(Genome.NeuralNet);

        appearance.SetOpaque(true);
        trailRenderer.sortingLayerName = "Background";
        trailRenderer.Clear();

        this.Movement.enabled = true;
        this.Movement.Reset();
        

        lifeTime = 0;
        this.transform.position = StartPosition;

        selectableComponent.enabled = false;
    }

    void FixedUpdate()
    {
        if (!IsAlive) return;

        lifeTime += Time.deltaTime;

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
        return transform.position.x - StartPosition.x;
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
            GUIController.Instance.BreedMenu.SelectedAgents.AddAgent(this);
            trailRenderer.sortingLayerName = appearance.body.sortingLayerName;
        }
        else
        {
            trailRenderer.sortingLayerName = "Background";
            GUIController.Instance.BreedMenu.SelectedAgents.RemoveAgent(this);
        }
    }

}
