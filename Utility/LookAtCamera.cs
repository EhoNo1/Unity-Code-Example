using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class LookAtCamera : MonoBehaviour
{
    private Transform myCamera;
    void Start()
    {
        myCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Update()
    {
        if (myCamera == null) return;
        transform.LookAt(myCamera);
    }
}
