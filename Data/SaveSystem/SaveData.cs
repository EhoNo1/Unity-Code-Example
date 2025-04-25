using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public string saveName;
    public string lastPlayed;
    public RaceFinishData[] raceFinishData;

    public SaveData(string saveName, string lastPlayed)
    {
        this.saveName = saveName;
        this.lastPlayed = lastPlayed;
    }
}
