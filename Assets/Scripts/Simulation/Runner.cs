using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public class Runner : Agent
{
    private Sensor[] sensors;

    public RunnerAppearance Appearance
    {
        get;
        private set;
    }

    public RunnerMovement Movement
    {
        get;
        private set;
    }

    private float lifeTime = 0;
    private Selectable selectableComponent;

    public static readonly string[] FirstNamePool = new string[]
    {
        "Quirgle", "Sqacknu", "Herpes", "Sporn", "Paranta", "Qarantu", "Vin", "Polanka",
        "Beed", "Korus", "Burlas", "Jokata"
    };

    private static List<string> UnusedNames = new List<string>();
    static Runner()
    {
        UnusedNames.AddRange(FirstNamePool);
    }
    public static string GetRandomFirstName()
    {
        if (UnusedNames.Count == 0)
            UnusedNames.AddRange(FirstNamePool);

        System.Random r = new System.Random();
        int index = r.Next(UnusedNames.Count);
        string name = UnusedNames[index];
        UnusedNames.RemoveAt(index);
        return name;
    }


    public string FirstName
    {
        get;
        set;
    }

    public string GenerationName
    {
        get;
        private set;
    }

    private int generationCount;
    public int GenerationCount
    {
        get
        {
            return generationCount;
        }
        set
        {
            generationCount = value;
            RegenerateGenerationName();
        }
    }

    public bool Selectable
    {
        get { return selectableComponent.enabled; }
        set
        {
            selectableComponent.enabled = value;
            dragComponenet.enabled = value;

            if (value)
                Appearance.SetOpaque(selectableComponent.Selected);
            else
                Appearance.SetOpaque(true);
        }
    }
    private TrailRenderer trailRenderer;
    private Dragable dragComponenet;


    public override void Init()
    {
        sensors = GetComponentsInChildren<Sensor>();
        Movement = GetComponent<RunnerMovement>();
        selectableComponent = GetComponent<Selectable>();
        selectableComponent.OnSelectChanged += SelectThisAgent;
        selectableComponent.enabled = false;

        dragComponenet = GetComponent<Dragable>();
        dragComponenet.enabled = false;
        dragComponenet.OnDrag += DragThisAgent;

        Appearance = GetComponentInChildren<RunnerAppearance>();

        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.sortingOrder = -10;

        fitnessMethod = UpdateFitness;

        GenerationCount = 0;
        this.FirstName = GetRandomFirstName();

        base.Init();
    }

    protected override void SetGenome(Genome genome)
    {
        base.SetGenome(genome);
        Appearance.UpdateAppearance(Genome.NeuralNet);
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
        Appearance.SetOpaque(true);

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
        Appearance.SetOpaque(selected);
        if (selected)
        {
            GUIController.Instance.BreedMenu.SelectedAgents.AddAgent(this);
            trailRenderer.sortingLayerName = Appearance.body.sortingLayerName;
        }
        else
        {
            trailRenderer.sortingLayerName = "Background";
            GUIController.Instance.BreedMenu.SelectedAgents.RemoveAgent(this);
        }
    }

    private void DragThisAgent()
    {
        Vector3 newPos = GameStateManager.Instance.Camera.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = transform.position.z;
        this.transform.position = newPos;
    }


    private void RegenerateGenerationName()
    {
        GenerationName = GenerationCount > 0 ? "Jr. " + ToRoman(GenerationCount - 1) : "";
    }

    private static string ToRoman(int number)
    {
        if (number > 3999) return "times alot";
        if (number < 1) return string.Empty;
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900);
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        else return "I" + ToRoman(number - 1);
    }
}
