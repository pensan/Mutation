using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

#if UNITY_5_3 
using UnityEngine.Experimental.Networking;
#elif UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#endif

public class NetworkManager : MonoBehaviour {

    string server_url   = "http://mutinder.herokuapp.com";
    string post_user    = "/api/users";
    GameObject GUI;

	void Start () {
        Debug.Log("API server url: " + server_url);

        GUI = GameObject.FindGameObjectWithTag("GUI");

        StartCoroutine(GetServerstatus());
	}
	
    /// <summary>
    /// Restarts the game by reloading the current scene
    /// </summary>
    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Tests the connection to the server
    /// </summary>
    IEnumerator GetServerstatus () {
        UnityWebRequest www = UnityWebRequest.Get(server_url);
        yield return www.Send();


        if(www.isError) {
            Debug.LogError(www.error + ": " + server_url);
        }
        else {
            // Server response was not 2XX
            if ( www.responseCode.ToString()[0] != '2' ) {

                Transform abc = GUI.transform.Find("ConnectionError");
                abc.gameObject.SetActive(true);
            }
            else
            {
                // Continue if response was 2XX
                StartCoroutine(PostLogin());
            }
                
        }
    }

    /// <summary>
    /// Sends the UUID to the server and fetches the user data
    /// </summary>
    IEnumerator PostLogin () {
        WWWForm form = new WWWForm();

        form.AddField("user[uuid]", SystemInfo.deviceUniqueIdentifier);

        WWW www = new WWW(server_url + post_user, form);
        yield return www;

        if (!string.IsNullOrEmpty(www.error)) {
            Debug.LogError(www.error + ": " + server_url + post_user);
        } else {
            Debug.Log(www.text);
        }

    }

}
