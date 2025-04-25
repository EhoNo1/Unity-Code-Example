using Riders.Objects;
using System.Collections;
using UnityEngine;

namespace Riders
{
    [RequireComponent(typeof(Collider))]
    public class ItemBox : InteractibleObject
    {
        [SerializeField] private AudioSource collectionSound;
        [SerializeField] private float respawnTimer = 5f;
        [SerializeField] private float fuelToAdd = 30f;

        [Header("Model")]
        [SerializeField] private Transform model;
        [SerializeField] private float spinSpeed = 1f;

        [Header("Particles")]
        [SerializeField] private ParticleSystem collectParticles;
        [SerializeField] private float particleVelocityMult = 1.1f;



        void Start()
        {
            model.Rotate(Vector3.forward * Random.Range(0, 360f));
        }

        void Update()
        {
            model.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);    
        }


        void OnTriggerEnter(Collider other)
        {
            if (!model.gameObject.activeSelf) return;
            if (!isCollisionWithPlayer(other)) return;

            PlayerController pc = getPlayerFromCollider(other);

            if (pc != null)
            {
                pc.AddFuel(fuelToAdd);
                model.gameObject.SetActive(false);
                StartCoroutine(Timer(respawnTimer));

                //We only care about preventing the sound from playing, particles are a visual queue
                if (pc.ShouldPlayEffects()) collectionSound.Play();

                //particles
                collectParticles.transform.LookAt(collectParticles.transform.position + pc.getBody().linearVelocity);
                var main = collectParticles.main;
                main.startSpeed = pc.getBody().linearVelocity.magnitude * particleVelocityMult;
                collectParticles.Play();

                pc.AddFuel(fuelToAdd);
            };
        }

        void OnDestroy()
        {
            StopAllCoroutines();
        }


        IEnumerator Timer(float timeInSeconds)
        {
            yield return new WaitForSeconds(timeInSeconds);
            OnTimerTimeout();
        }

        public void OnTimerTimeout()
        {
            model.gameObject.SetActive(true);
        }
    }
}