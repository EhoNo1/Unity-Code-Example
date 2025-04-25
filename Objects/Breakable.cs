using Riders.Player;
using System.Collections;
using UnityEngine;

namespace Riders.Objects
{
    /// <summary>
    /// Destructible object that the player can collide with
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(AudioSource))]
    public class Breakable : InteractibleObject
    {
        private Rigidbody body;
        private Collider myCollider;
        [SerializeField] private AudioSource successSound;
        [SerializeField] private AudioSource failSound;

        //public enum BreakResponseType { KNOCKAWAY, DISAPEAR }
        //[SerializeField] private BreakResponseType response;

        private Vector3 startPosition;
        private Quaternion startRotation;

        [SerializeField] private float respawnTime = 10f;
//        [SerializeField] private float miniboostTime = 0.5f;
        [SerializeField] private float knockAwayForceMult = 1f;
//        [SerializeField] private float successfulKnockAwayMult = 5f;

 //       [SerializeField] private float fuelBonus = 15f;

        private void Start()
        {
            body = GetComponent<Rigidbody>();
            myCollider = GetComponent<Collider>();

            body.useGravity = false;
            startPosition = transform.position;
            startRotation = transform.rotation;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!isCollisionWithPlayer(collision.collider)) { return; }
            PlayerController pc = getPlayerFromCollider(collision.collider);

            pc.Stun();
            if (pc.ShouldPlayEffects()) { failSound.Play(); }

            myCollider.enabled = false;
            Vector3 knockAwayForce = collision.impulse * knockAwayForceMult;
            body.AddForce(knockAwayForce, ForceMode.Impulse);
            body.useGravity = true;
            StartCoroutine(BreakableRespawnClock());
        }

        IEnumerator BreakableRespawnClock()
        {
            yield return new WaitForSeconds(respawnTime);
            body.useGravity = false;
            transform.rotation = startRotation;
            transform.position = startPosition;
            body.linearVelocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            myCollider.enabled = true;
        }
    }
}