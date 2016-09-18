﻿// REDOX Game Labs 2016

#region INCLUDES
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

public class RunnerAppearance : MonoBehaviour 
{
    public List<Sprite> bodies;
    public List<GameObject> bodyPartsPrefabs;
    public List<Transform> slots;

    public SpriteRenderer body;

    public Rigidbody2D masterRigidBody;

    private NeuralNetwork network;

    public void UpdateAppearance(GameObject runner, NeuralNetwork network)
    {
        StartCoroutine(SetAppearanceCo(runner, network));
    }

    private IEnumerator SetAppearanceCo(GameObject runner, NeuralNetwork network)
    {
        this.network = network;

        Color tintColor = Color.HSVToRGB(UnityEngine.Random.Range(0.0f, 1.0f), 1, 1);

        List<RunnerAppearanceLimb> currentLimbs = new List<RunnerAppearanceLimb>();

        foreach (Transform slot in slots)
        {
            currentLimbs.AddRange(slot.GetComponentsInChildren<RunnerAppearanceLimb>());
        }

        for (int i = currentLimbs.Count - 1; i >= 0; i--)
        {
            Destroy(currentLimbs[i].gameObject);
        }

        yield return new WaitForEndOfFrame();

        foreach (Transform slot in slots)
        {
            GameObject go = Instantiate(bodyPartsPrefabs[UnityEngine.Random.Range(0, bodyPartsPrefabs.Count)]);
            go.transform.SetParent(slot.transform, false);
            go.transform.localScale *= 5.0f;
            go.transform.localPosition = Vector2.zero;
            go.transform.localRotation = Quaternion.identity;

            FixedJoint2D joint = go.GetComponent<RunnerAppearanceLimb>().masterJoint;
            if (joint != null)
            {
                joint.connectedBody = masterRigidBody;
            }

            foreach (SpriteRenderer spr in go.GetComponentsInChildren<SpriteRenderer>())
            {
                spr.color = tintColor;
            }

        }


        //for (int i = 0; i < network.Layers[1].Weights.GetLength(0); i++ )
        //{
        //    if (i < slots.Count)
        //    {
        //        double summedWeight = GetSummedWeight(1, i);

        //        GameObject go = Instantiate(GetSpriteForNode(summedWeight, bodyPartsPrefabs));
        //        Debug.Log("Update appearance for " + runner.name + " with " + slots.Count + " slots setting slot " + i);

        //        currentLimbs.Add(go);
        //        go.transform.parent = slots[i];

        //        HingeJoint2D joint = go.GetComponent<RunnerAppearanceLimb>().masterJoint;
        //        if (joint != null)
        //        {
        //            joint.connectedBody = masterRigidBody;
        //        }

        //        foreach (SpriteRenderer spr in go.GetComponentsInChildren<SpriteRenderer>())
        //        {
        //            spr.color = tintColor;
        //        }

        //        go.transform.localPosition = Vector3.zero;
        //        go.transform.localRotation = Quaternion.identity;

        //        go.GetComponentInChildren<SpriteRenderer>().color = tintColor;
        //    }
        //}

        body.color = tintColor;
    }

    void Update()
    {
        transform.Rotate(new Vector3(0.0f, 0.0f, -masterRigidBody.velocity.x * 0.25f));

        transform.position = masterRigidBody.transform.position;
    }

    private GameObject GetSpriteForNode(double summedWeight, List<GameObject> bodyParts)
    {
        return bodyPartsPrefabs[Mathf.FloorToInt(Mathf.Abs((float)(summedWeight * bodyParts.Count))) % bodyParts.Count];
    }

    double GetSummedWeight(int layerIndex, int nodeIndex)
    {
        double summedWeight = 0.0f;
        int weightCount = network.Layers[layerIndex].Weights.GetLength(1);

        for (int j = 0; j < weightCount; j++)
        {
            summedWeight += network.Layers[layerIndex].Weights[nodeIndex, j];
        }

        return summedWeight / weightCount;
    }

    //TODO: implement for all body parts + body
    public void SetOpaque(bool opaque)
    {
        if (opaque)
        {
            Color color = body.color;
            color.a = 1f;
            body.color = color;
        }
        else
        {
            Color color = body.color;
            color.a = 0.5f;
            body.color = color;
        }
    }
}
