// REDOX Game Labs 2016

#region INCLUDES
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

public class RunnerAppearance : MonoBehaviour 
{
    public List<GameObject> bodyPartsPrefabs;
    public List<Transform> slots;

    public SpriteRenderer body;

    public Rigidbody2D masterRigidBody;

    private NeuralNetwork network;

    private List<RunnerAppearanceLimb> limbs = new List<RunnerAppearanceLimb>();

    public void UpdateAppearance(NeuralNetwork network)
    {
        StartCoroutine(SetAppearanceCo(network));
    }

    private IEnumerator SetAppearanceCo(NeuralNetwork network)
    {
        this.network = network;

        float colorValue = Mathf.Abs((float)GetSummedWeight(2, 1)) + Mathf.Abs((float)GetSummedWeight(2, 3));
        Color tintColor = Color.HSVToRGB(((colorValue * 255) % 255) / 255, 1, 0.5f);

        //Destroy previous limbs
        foreach (RunnerAppearanceLimb limb in limbs)
        {
            Destroy(limb.gameObject);
        }

        limbs.Clear();

        yield return new WaitForEndOfFrame();


        for (int i = 0; i < network.Layers[1].Weights.GetLength(0); i++)
        {
            if (i < slots.Count)
            {
                double summedWeight = GetSummedWeight(1, i);

                GameObject go = Instantiate(GetSpriteForNode(summedWeight, bodyPartsPrefabs));

                go.transform.SetParent(slots[i].transform, false);
                go.transform.localScale *= 1.5f;
                go.transform.localPosition = Vector2.zero;
                go.transform.localRotation = Quaternion.identity;

                RunnerAppearanceLimb limb = go.GetComponent<RunnerAppearanceLimb>();
                FixedJoint2D joint = limb.masterJoint;
                if (joint != null)
                {
                    joint.connectedBody = masterRigidBody;
                }

                foreach (SpriteRenderer spr in limb.SpriteRenderers)
                {
                    spr.color = tintColor;
                }

                limbs.Add(limb);
            }
        }

        body.color = tintColor;
    }

    void Update()
    {
        transform.Rotate(new Vector3(0.0f, 0.0f, -masterRigidBody.velocity.x * 25 * Time.deltaTime));
        transform.position = masterRigidBody.transform.position;

        foreach (RunnerAppearanceLimb limb in limbs)
            limb.UpdatePosition();
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

        if (limbs != null)
            foreach (RunnerAppearanceLimb limb in limbs)
                limb.SetOpaque(opaque);
    }
}
