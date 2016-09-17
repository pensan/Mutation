// REDOX Game Labs 2016

#region INCLUDES
using System.Collections.Generic;
using UnityEngine;
#endregion

public class RunnerAppearance : MonoBehaviour 
{
    public List<Sprite> bodies;
    public List<GameObject> bodyPartsPrefabs;
    public List<Transform> slots;

    public SpriteRenderer body;

    private NeuralNetwork network;

    private List<GameObject> currentLimbs = new List<GameObject>();

    public void UpdateAppearance(NeuralNetwork network)
    {
        this.network = network;

        Color tintColor = new Color(Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f));

        for (int i = currentLimbs.Count - 1; i > 0; i--)
        {
            Destroy(currentLimbs[i]);
        }

        currentLimbs.Clear();

        for (int i = 0; i < network.Layers[1].Weights.GetLength(0); i++ )
        {
            if (i < slots.Count)
            {
                double summedWeight = GetSummedWeight(1, i);

                GameObject go = Instantiate(GetSpriteForNode(summedWeight, ref bodyPartsPrefabs));

                currentLimbs.Add(go);
                go.transform.parent = slots[i];

                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;

                go.GetComponentInChildren<SpriteRenderer>().color = tintColor;

                Debug.Log(GetSummedWeight(1, i));
            }

        }

        body.color = tintColor;
    }

    private GameObject GetSpriteForNode(double summedWeight, ref List<GameObject> bodyParts)
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
            
        }
        else
        {

        }
    }
}
