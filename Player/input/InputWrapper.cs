using Riders.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Riders.Player
{
    [Serializable]
    public abstract class InputWrapper
    {
        public PlayerController pc;
        public bool showUI = false;

        public InputWrapper(PlayerController pc)
        {
            this.pc = pc;
        }

        public abstract PlayerInputFrame QueryInputFrame();
        public abstract void OnDestroy();

        public abstract string QueryDebugText();
    }

}

