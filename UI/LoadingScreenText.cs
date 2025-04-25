using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LoadingScreenText : MonoBehaviour
{
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        TextAsset ta = (TextAsset)Resources.Load("LoadingScreenQuotes");
        string[] textStrings = ta.text.Split('\n');
        int randomIndex = Random.Range(0, textStrings.Length);
        text.text = textStrings[randomIndex];
    }
}
