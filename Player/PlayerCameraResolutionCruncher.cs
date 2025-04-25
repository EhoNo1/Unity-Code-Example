#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

public class PlayerCameraResolutionCruncher : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private float resolutionScale;
    
    [SerializeField] private RenderTexture exampleRenderTexture;
    [SerializeField] private RenderTexture transferRenderTexture;
    [SerializeField] private RawImage targetImage;

    void Start()
    {
        UpdateResolution();
    }


    public void UpdateResolution()
    {
        int newWidth = Mathf.RoundToInt(Screen.width * resolutionScale);
        int newHeight = Mathf.RoundToInt(Screen.height * resolutionScale);

        transferRenderTexture = new RenderTexture(exampleRenderTexture);
        transferRenderTexture.width = newWidth;
        transferRenderTexture.height = newHeight;
        transferRenderTexture.filterMode = FilterMode.Point;

        targetCamera.targetTexture = transferRenderTexture;
        targetImage.texture = transferRenderTexture;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PlayerCameraResolutionCruncher))]
public class PlayerCameraResolutionCruncherEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerCameraResolutionCruncher script = (PlayerCameraResolutionCruncher)target;
        if (GUILayout.Button("Update Resolution"))
        {
            script.UpdateResolution();
        }
    }
}
#endif