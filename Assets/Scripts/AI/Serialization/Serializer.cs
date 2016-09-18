// REDOX Game Labs 2016

#region INCLUDES
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEditor;
using UnityEngine;
#endregion

public static class Serializer 
{

    private static string filename = "neuralNet.txt";
    private static string resourcesPath = Path.Combine(Application.dataPath, "Resources");
    private static string fullFilePath  = Path.Combine(resourcesPath, filename);


    public static SerializeableNeuralNetwork ToSerializable(NeuralNetwork net)
    {
        SerializeableNeuralNetwork saveNetwork = new SerializeableNeuralNetwork();
        saveNetwork.topology = net.Topology;

        saveNetwork.layers = new SerializeableNeuralLayer[net.Layers.Length];

        for (int i = 0; i < net.Layers.Length; i++)
        {
            saveNetwork.layers[i] = new SerializeableNeuralLayer();

            saveNetwork.layers[i].Weights = net.Layers[i].Weights;
            saveNetwork.layers[i].nodeCount = net.Layers[i].NodeCount;
            saveNetwork.layers[i].outputCount = net.Layers[i].OutputCount;
            saveNetwork.layers[i].curentActivationFunction = net.Layers[i].CurrentActivationFunction;
        }

        return saveNetwork;
    }

    public static void SaveNetwork(NeuralNetwork network)
    {
        SerializeableNeuralNetwork saveNetwork = ToSerializable(network);
        string json = ToJsonString(saveNetwork);

        Directory.CreateDirectory(resourcesPath);

        File.WriteAllText(fullFilePath, json);

        AssetDatabase.Refresh();

        Debug.Log(json);
        Debug.Log("Saved network.");
    }

    public static string ToJsonString(SerializeableNeuralNetwork net)
    {
        return JsonConvert.SerializeObject(net);
    }

    public static SerializeableNeuralNetwork LoadNetworkFromServerResponse(string data)
    {
        JObject parsed = JObject.Parse(data);
        return JsonConvert.DeserializeObject<SerializeableNeuralNetwork>(parsed["data"].ToString());
    }

    public static SerializeableNeuralNetwork LoadNetwork()
    {
        if (File.Exists(fullFilePath))
        {
            string contents = File.ReadAllText(fullFilePath);
            return JsonConvert.DeserializeObject<SerializeableNeuralNetwork>(contents);
        }
        else
        {
            return null;
        }
    }

    private static string FilePath()
    {
        return Path.Combine(resourcesPath, filename);
    }
}
