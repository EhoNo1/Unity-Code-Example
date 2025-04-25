using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Matchmaking : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(YieldableFunction());
    }


    IEnumerator YieldableFunction()
    {
        //UnityWebRequest formSubmission = UnityWebRequest.PostWwwForm("localhost:3000/registerserver", "{{\"test\":\"value\"}}");
        WWWForm form = new WWWForm();
        form.AddField("frameCount", Time.frameCount.ToString());


        UnityWebRequest w;
        // Upload to a cgi script
        using (w = UnityWebRequest.Post("http://localhost:3000/registerserver", form))
        {
            yield return w.SendWebRequest();
            if (w.result != UnityWebRequest.Result.Success)
            {
                print(w.error);
            }
            else
            {
                print("Finished Uploading Screenshot");
                Debug.Log(w.downloadHandler.text);
            }
        }

        





        UnityWebRequest www = UnityWebRequest.Get("localhost:3000/serverlist");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
