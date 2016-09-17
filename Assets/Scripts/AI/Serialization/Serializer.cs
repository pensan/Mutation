// REDOX Game Labs 2016

#region INCLUDES
using Newtonsoft.Json;
using System.IO;
using UnityEditor;
using UnityEngine;
#endregion

public class Serializer : MonoBehaviour 
{
    public EvolutionController evolutionController;

    public static Serializer Instance;

    private string filename = "neuralNet.txt";
    private string resourcesPath;
    private string fullFilePath;
    /// <summary>
    /// Used for initialisation and creating references
    /// </summary>
    void Awake () 
	{
        resourcesPath = Path.Combine(Application.dataPath, "Resources");
        fullFilePath = Path.Combine(resourcesPath, filename);
        Instance = this;
	}
	
	/// <summary>
	/// Used for setting up MonoBehaviour
	/// </summary>
	void Start () 
	{
	
	}

	void OnDestroy () 
	{
        SaveNetwork();
	}

    void SaveNetwork()
    {
        NeuralNetwork network = evolutionController.BestAgent.Genome.neuralNet;

        SerializeableNeuralNetwork saveNetwork = new SerializeableNeuralNetwork();
        saveNetwork.topology = network.Topology;

        saveNetwork.layers = new SerializeableNeuralLayer[network.Layers.Length];

        for (int i = 0; i < network.Layers.Length; i++)
        {
            saveNetwork.layers[i] = new SerializeableNeuralLayer();

            saveNetwork.layers[i].Weights = network.Layers[i].Weights;
            saveNetwork.layers[i].nodeCount = network.Layers[i].NodeCount;
            saveNetwork.layers[i].outputCount = network.Layers[i].OutputCount;
            saveNetwork.layers[i].curentActivationFunction = network.Layers[i].CurrentActivationFunction;
        }


        string json = JsonConvert.SerializeObject(saveNetwork);

        Directory.CreateDirectory(resourcesPath);

        File.WriteAllText(fullFilePath, json);

        AssetDatabase.Refresh();

        Debug.Log(json);
    }

    public SerializeableNeuralNetwork LoadNetwork()
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

    private string FilePath()
    {
        return Path.Combine(resourcesPath, filename);
    }
}
