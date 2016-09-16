using UnityEngine;
using System.Collections;

public class Runner : Agent
{
    private Vector3 startPosition;

    private Sensor[] sensors;


    void Awake()
    {
        startPosition = this.transform.position;

        sensors = GetComponentsInChildren<Sensor>();
    }


    public override void Restart()
    {
        base.Restart();

        this.transform.position = startPosition;


    }



}
