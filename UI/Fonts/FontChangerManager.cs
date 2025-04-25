using System.Collections.Generic;
using System.Linq;
using TMPro;

public static class FontChangerManager
{
    private static TMP_FontAsset currentFont;
    private static List<FontChangeableTextMesh> changableTexts = new List<FontChangeableTextMesh>();


    public static void RegisterTextComponent(FontChangeableTextMesh text) { changableTexts.Add(text); }
    public static void DeRegisterTextComponent(FontChangeableTextMesh text) { changableTexts.Remove(text); }
    public static void SetNewFont(TMP_FontAsset newFont)
    {
        currentFont = newFont;

        for (int i = 0; i < changableTexts.Count; i++)
        {
            changableTexts.ElementAt(i).UpdateFont();
        }
    }


    public static TMP_FontAsset GetFont() { return currentFont; }
}
