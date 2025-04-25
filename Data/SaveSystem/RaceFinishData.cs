using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to save the player's race times to their save file
/// </summary>
[Serializable]
public class RaceFinishData
{
    public int placement;
    public string boardUsed;
    public RaceTime raceTime;
    public string location;

    public RaceFinishData(int placement, string boardUsed, RaceTime raceTime, string location)
    {
        this.placement = placement;
        this.boardUsed = boardUsed;
        this.raceTime = raceTime;
        this.location = location;
    }
}
