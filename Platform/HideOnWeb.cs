using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnWeb : MonoBehaviour
{
#if UNITY_WEBGL
    void Start()
    {
        gameObject.SetActive(false);           
    }
#endif
}
