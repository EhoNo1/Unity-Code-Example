using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Riders.Data
{
    [Serializable]
    public class Timestamp
    {
        public int minutes;
        public int seconds;
        public int milliseconds;

        public Timestamp(int minutes, int seconds, int milliseconds)
        {
            this.minutes = minutes;
            this.seconds = seconds;
            this.milliseconds = milliseconds;
        }

        public Timestamp(float raceTime)
        {
            int secondsUnBounded = Mathf.RoundToInt(raceTime);
            float milisecondsInSeconds = raceTime - secondsUnBounded;
            milliseconds = (int)(milisecondsInSeconds * 1000);
            minutes = secondsUnBounded / 60;
            seconds = secondsUnBounded % 60;
        }


    }
}

