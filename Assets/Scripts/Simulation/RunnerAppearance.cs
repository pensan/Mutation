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

    private bool isHidden = false;

    public void Hide(bool hide)
    {
        isHidden = hide;
        foreach (RunnerAppearanceLimb limb in limbs)
            limb.gameObject.SetActive(!hide);

        if (body != null)
            body.enabled = !hide;

    }

    private IEnumerator SetAppearanceCo(NeuralNetwork network)
    {
        this.network = network;

        //Destroy previous limbs
        foreach (RunnerAppearanceLimb limb in limbs)
        {
            Destroy(limb.gameObject);
        }

        limbs.Clear();

        yield return new WaitForEndOfFrame();

        // Get index of highest weight to get a color which links the appearance somehow to the network
        int totalcount = 0;
        double highestValue = float.MinValue;
        int highestIndex = -1;

        // iterate over hidden layers
        for (int l = 0; l < network.Layers.Length - 1; l++)
        {
            // iterate all nodes
            for (int w = 0; w < network.Layers[l].Weights.GetLength(0) - 1; w++)
            {
                // iterate all weights
                for (int wo = 0; wo < network.Layers[l].Weights.GetLength(1) - 1; wo++)
                {
                    // Remember highest weight
                    double val = Math.Abs(network.Layers[l].Weights[w, wo]);

                    if (val > highestValue)
                    {
                        highestIndex = totalcount;
                        highestValue = val;
                    }

                    totalcount++;
                }
            }
        }

        float hueVal = highestIndex / (float)totalcount;
        Color tintColor = Color.HSVToRGB(hueVal, 1.0f, 0.5f);

        for (int i = 0; i < slots.Count; i++)
        {
            GameObject go = Instantiate(GetSpriteForNode(1, bodyPartsPrefabs));

            go.transform.SetParent(slots[i].transform, false);
            go.transform.localScale *= 1.5f;
            go.transform.localPosition = Vector2.zero;
            go.transform.localRotation = Quaternion.identity;

            RunnerAppearanceLimb limb = go.GetComponent<RunnerAppearanceLimb>();
            FixedJoint2D joint = limb.masterJoint;
            if (joint != null)
            {
                joint.connectedBody = GetComponent<Rigidbody2D>();
            }

            foreach (SpriteRenderer spr in limb.SpriteRenderers)
            {
                spr.color = tintColor;
            }

            limb.gameObject.SetActive(!isHidden);
            limbs.Add(limb);
            
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
