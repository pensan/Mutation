using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;

#if UNITY_5_3
using UnityEngine.Experimental.Networking;
#elif UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#endif

public static class NetworkManager
{

    static string server_url   = "http://mutinder.herokuapp.com";
    static string post_user    = "/api/users";
    static string post_nn      = "/api/users/{0}";
    static string get_user     = "/api/users";
    static string get_opponent = "/api/users/{0}/opponent/{1}";
    static int player_id;


    /// <summary>
    /// Tests the connection to the server
    /// </summary>
    public static IEnumerator GetServerstatus ()
    {
        UnityWebRequest www = UnityWebRequest.Get(server_url);
        yield return www.Send();

        if(www.isError)
        {
            Debug.LogError(www.error + ": " + server_url);
            yield return false;
        }
        else
        {
            // Server response was not 2XX
            if ( www.responseCode.ToString()[0] != '2' )
            {
                Debug.LogError("ConnectionError!");
                yield return false;
            }
            else
            {
                yield return true;
            }                
        }
    }

    /// <summary>
    /// Sends the UUID to the server and fetches the user data
    /// </summary>
    public static IEnumerator PostLogin ()
    {
        WWWForm form = new WWWForm();

        form.AddField("user[uuid]", SystemInfo.deviceUniqueIdentifier);

        WWW www = new WWW(server_url + post_user, form);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError(www.error + ": " + server_url + post_user);
            yield return false;
        }
        else
        {
            Debug.Log(www.text);
            JObject parsed = JObject.Parse(www.text);
            player_id = (int)parsed["data"]["user"]["id"];

            yield return new NeuralNetwork(Serializer.LoadNetworkFromServerResponse(www.text));
        }

    }


    /// <summary>
    /// Sends the NeuralNetwork to the server with current user data
    /// </summary>
    public static IEnumerator PostNeuralNet(NeuralNetwork net)
    {
        WWWForm form = new WWWForm();

        form.AddField("user[neuronal_network]", Serializer.ToJsonString(Serializer.ToSerializable(net)));

        WWW www = new WWW(string.Format(server_url + post_nn, player_id), form);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError(www.error + ": " + www.url);
            yield return false;
        }
        else
        {
            Debug.Log(www.text);
            Debug.Log("Successfully posted network.");
            yield return true;
        }

    }

    /// <summary>
    /// Returns an opponent
    /// </summary>
    public static IEnumerator GetOpponent(string opponent_id="")
    {
        string get_op = string.Format(get_opponent, player_id, opponent_id);
        UnityWebRequest www = UnityWebRequest.Get(server_url + get_op);
        yield return www.Send();

        if (www.isError)
        {
            Debug.LogError(www.error + ": " + server_url + get_op);
            yield return false;
        }
        else
        {
            // Server response was not 2XX
            if (www.responseCode.ToString()[0] != '2')
            {
                Debug.LogError("Connection Error!");
                yield return false;
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                yield return new NeuralNetwork(Serializer.LoadNetworkFromServerResponse(www.downloadHandler.text));
            }
        }
    }


}
