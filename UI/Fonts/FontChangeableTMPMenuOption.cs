using UnityEngine;
using UnityEditor;

public static class FontChangeableTMPMenuOption
{
#if UNITY_EDITOR
    private const string assetPath = "Assets/scripts/ui/Fonts/Text (Font Changable TMP).prefab";

    [MenuItem("GameObject/UI/Font Changable Text (TMP)")]

    public static void SpawnPrefab()
    {
        Object o = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(o);
        if (Selection.activeTransform) { instance.transform.SetParent(Selection.activeTransform); }

        Undo.RegisterCreatedObjectUndo(instance, $"Create {instance.name}");
        Selection.activeObject = instance;
    }

#endif
}
