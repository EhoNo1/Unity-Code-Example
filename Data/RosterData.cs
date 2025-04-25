using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RosterData
{
    private static RosterEntry[] roster;
    public static RosterEntry[] Roster
    {
        get 
        {
            if (roster == null)
            {
                TextAsset rosterFile = (TextAsset)Resources.Load("RosterData");
                String text = rosterFile.text;
                //Debug.Log(text);
                roster = JsonUtility.FromJson<RosterArray>(text).array;
            }
            return roster;
        }
        set {
            roster = value;
        }
    }
}

[Serializable]
public class RosterArray {
    public RosterEntry[] array;
}
[Serializable]
public class RosterEntry {
    public string name;
    public string[] colors;

    public Color GetColor(int i)
    {
        Color output = new Color(1f,1f,0f);
        ColorUtility.TryParseHtmlString(colors[i].ToString(), out output);
        return output;
    }
}
