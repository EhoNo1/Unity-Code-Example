using Riders.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Riders.Objects
{
    /// <summary>
    /// Pad that gives the player a speed boost forwards and redirects them
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class DashPad : InteractibleObject
    {
        private AudioSource dashSound;
        public float impulseMult = 5f;
        public float minimumSpeed = 30f;

        void Start()
        {
            dashSound = GetComponent<AudioSource>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (!isCollisionWithPlayer(other)) return;

            PlayerController pc = getPlayerFromCollider(other);

            pc.AlignToForward(transform.forward);
            pc.Dash(impulseMult, transform.forward, minimumSpeed);
            if (pc.ShouldPlayEffects()) dashSound.Play();
        }

    }
}