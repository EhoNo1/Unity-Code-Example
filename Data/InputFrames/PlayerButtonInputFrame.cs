using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Riders.Player
{
    /// <summary>
    /// Superior class to record a player's input during a race. 
    /// All forms of input recording derrive from this class.
    /// </summary>
    [Serializable]
    public class PlayerButtonInputFrame: PlayerInputFrame
    {
        public Vector2 movement;
        public bool jumpHeld;
        public bool brakeHeld;
        public bool slideHeld;
        public bool boostHeld;


        public PlayerButtonInputFrame(float timestamp, Vector2 movement, bool jumpHeld, bool brakeHeld, bool slideHeld, bool boostHeld)
        {
            this.timestamp = timestamp;
            this.movement = movement;
            this.jumpHeld = jumpHeld;
            this.brakeHeld = brakeHeld;
            this.slideHeld = slideHeld;
            this.boostHeld = boostHeld;
        }
        public PlayerButtonInputFrame(PlayerButtonInputFrame toCopy)
        {
            this.timestamp = toCopy.timestamp;
            this.movement = toCopy.movement;
            this.jumpHeld = toCopy.jumpHeld;
            this.brakeHeld = toCopy.brakeHeld;
            this.slideHeld = toCopy.slideHeld;
            this.boostHeld = toCopy.boostHeld;
        }
    }
}

