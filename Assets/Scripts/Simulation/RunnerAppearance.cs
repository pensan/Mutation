// REDOX Game Labs 2016

#region INCLUDES
using System;
using System.Collections.Generic;
using UnityEngine;
#endregion

public class RunnerAppearance : MonoBehaviour 
{
    public List<Sprite> bodies;
    public List<Sprite> bodyParts;
    public List<SpriteRenderer> slots;

    private NeuralNetwork network;


    public void UpdateAppearance(NeuralNetwork network)
    {
        this.network = network;

        for (int i = 0; i < network.Layers[1].Weights.GetLength(0); i++ )
        {
            if (i < slots.Count)
            {
                double summedWeight = GetSummedWeight(1, i);

                slots[i].sprite = GetSpriteForNode(summedWeight, bodyParts);


                Debug.Log(GetSummedWeight(1, i));

            }
        }
    }

    private Sprite GetSpriteForNode(double summedWeight, List<Sprite> bodyParts)
    {
        return bodyParts[Mathf.FloorToInt((float)(summedWeight * bodyParts.Count))];
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
}
