using TMPro;
using UnityEngine;

/// <summary>
/// Attatch this to any TextMeshPro Text to have it automatically update with the user's font preference.
/// </summary>
public class FontChangeableTextMesh : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        FontChangerManager.RegisterTextComponent(this);
    }

    void OnDestroy()
    {
        FontChangerManager.DeRegisterTextComponent(this);
    }

    public void UpdateFont() { textMesh.font = FontChangerManager.GetFont(); }
}
