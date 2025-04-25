using Riders.Managers;
using Riders.Objects;
using Riders.Player;
using UnityEngine;

namespace Riders
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(AudioSource))]
    public class StartLine : Gate
    {
        public float startingFuelPercentage = 0.5f;
        private AudioSource startPling;

        void Start()
        {
            startPling = GetComponent<AudioSource>();
            SetIndex(0);
        }


        void OnTriggerEnter(Collider other)
        {
            if (!isCollisionWithPlayer(other)) return;

            PlayerController pc = getPlayerFromCollider(other);
            RaceController.AddLap(pc);

            if (pc.movementMode == MovementMode.RIDING) return;

            pc.SetMinimumFuel(startingFuelPercentage);
            pc.movementMode = MovementMode.RIDING;
            if (pc.ShouldPlayEffects()) { startPling.Play(); }
        }
    }
}