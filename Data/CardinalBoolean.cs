using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardinalBoolean
{
    public bool north;
    public bool south;
    public bool east;
    public bool west;

    public CardinalBoolean(bool north, bool south, bool east, bool west)
    {
        this.north = north;
        this.south = south;
        this.east = east;
        this.west = west;
    }
}
