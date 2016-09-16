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

        Movement.UseUserInput = true;
    }


    public override void Restart()
    {
        base.Restart();

        this.transform.position = startPosition;


    }



}
