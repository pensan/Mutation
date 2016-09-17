using UnityEngine;
using System.Collections;

#if UNITY_5_3 
using UnityEngine.Experimental.Networking;
#elif UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#endif

public class NetworkManager : MonoBehaviour {

    string server_url   = "http://fierce-journey-76439.herokuapp.com";
    string post_user    = "/user";

	void Start () {
        Debug.Log("API server url: " + server_url);

        StartCoroutine(GetServerstatus());
	}
	
	void Update () {
	
	}

    IEnumerator GetServerstatus () {
        UnityWebRequest www = UnityWebRequest.Get(server_url);
        yield return www.Send();


        if(www.isError) {
            Debug.LogError(www.error);
        }
        else {

            // Server response was not 2XX
            if ( www.responseCode.ToString()[0] != 2 )
                return false;

            // Continue if response was 2XX
            Debug.Log(www.downloadHandler.text);

            StartCoroutine(PostLogin());
        }
    }

    IEnumerator PostLogin () {
        WWWForm form = new WWWForm();
        form.AddField("udid", 1);

        WWW www = new WWW(server_url + post_user, form);
        yield return www;


        if (!string.IsNullOrEmpty(www.error)) {
            Debug.Log(www.error);
        } else {
            Debug.Log(www.text);
        }

    }

}
