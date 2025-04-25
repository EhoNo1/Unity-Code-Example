using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to save the player's finishing time to their save file
/// </summary>
[Serializable]
public class RaceTime
{
    public int minutes;
    public int seconds;
    public int milliseconds;

    public RaceTime(int minutes, int seconds, int milliseconds)
    {
        this.milliseconds = minutes;
        this.seconds = seconds;
        this.milliseconds = milliseconds;
    }
}
