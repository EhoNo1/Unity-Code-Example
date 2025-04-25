using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RaceMetadataToken
{
    [Serializable]
    public enum RaceMode { GHOST, SOLO, BOT }

    public RaceMode raceMode;
    public string ghostUUID;
    public string generatorSeed;
    
    public RaceMetadataToken(RaceMode raceMode, string generatorSeed)
    {
        this.raceMode = raceMode;
        this.generatorSeed = generatorSeed;
    }

    public RaceMetadataToken(RaceMode raceMode, string ghostUUID, string generatorSeed)
    {
        this.raceMode = raceMode;
        this.ghostUUID = ghostUUID;
        this.generatorSeed = generatorSeed;
    }

    public override string ToString()
    {
        return JsonUtility.ToJson(this,true);
    }
}

