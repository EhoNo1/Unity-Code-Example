using Riders.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Riders.Objects
{


    /// <summary>
    /// Simplifies getting the player from a collision or trigger overlap.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public abstract class InteractibleObject : MonoBehaviour
    {
        public bool isCollisionWithPlayer(Collider other)
        {
            if (other.tag != "Player") return false;

            PlayerController player = getPlayerFromCollider(other);

            if (player == null) return false;
            return true;
        }

        public PlayerController getPlayerFromCollider(Collider other)
        {
            return other.GetComponent<PlayerController>();
        }
    }
}