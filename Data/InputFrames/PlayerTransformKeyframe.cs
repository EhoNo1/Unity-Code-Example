using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Riders.Player
{
    /// <summary>
    /// Stores data about the player's physics properties to correct for simulation error during ghost playback. 
    /// </summary>
    [Serializable]
    public class PlayerTransformKeyframe : PlayerInputFrame
    {
        public Vector3 overridePosition;
        public Vector3 overrideRotation;
        public Vector3 overrideVelocity;

        public void ApplyKeyframe(Transform targetTransform, Rigidbody targetRigidbody)
        {
            targetTransform.position = overridePosition;
            targetTransform.eulerAngles = overrideRotation;
            targetRigidbody.linearVelocity = overrideVelocity;
        }

        public PlayerTransformKeyframe(Transform targetTransform, Rigidbody targetRigidbody)
        {
            overridePosition = targetTransform.position;
            overrideRotation = targetTransform.eulerAngles;
            overrideVelocity = targetRigidbody.linearVelocity;
        }
    }

}
