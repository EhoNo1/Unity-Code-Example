using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldForceRange : MonoBehaviour
{
    private TMP_InputField myInput;
    [SerializeField] private int min = 0;
    [SerializeField] private int max = 20;


    void Start()
    {
        myInput = GetComponent<TMP_InputField>();
        
        if (myInput == null)
        {
            Debug.LogWarning("An range enforce was placed on object " + gameObject.name + " which doesn't have an input field!");
            return;
        }

        myInput.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

     void OnValueChanged()
    {
        int i = int.Parse(myInput.text);
        i = Mathf.Clamp(i, min, max);
        myInput.text = i + "";
    }
}
