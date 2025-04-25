using Riders.Managers;
using Riders.Player;
using System;
using UnityEngine;

namespace Riders
{
    public class InSceneRaceHelper : MonoBehaviour
    {
        public static InSceneRaceHelper rh;
        public Transform Spawnpoints;
        public GameObject PlayerPrefab;
        public float automaticRespawnHeight = -30f;
        [SerializeField] private AudioSource raceFinishMusic;
        [SerializeField] private Transform levelBGM;


        void Start()
        {
            if (!rh) { rh = this; }
            else { Destroy(gameObject); }
            Spawnpoints = GameObject.FindGameObjectWithTag("Spawnpoints").transform;

            Transform spawnpoint = Spawnpoints.GetChild(0);
            GameObject playerGameObject = Instantiate(PlayerPrefab, spawnpoint.position, spawnpoint.rotation);
            PlayerController playerController = playerGameObject.GetComponent<PlayerController>();

#if UNITY_EDITOR
            if (RaceController.currentLevelMetadataToken == null)
            {
                Debug.LogWarning("RaceController.currentLevelMetadataToken is NULL, and the Unity Editor is detected, so we are assuming this is an editor test and overriding that value.\nThis should not happen unless the race scene is being run directly in the editor!");
                RaceController.currentLevelMetadataToken = new RaceMetadataToken(RaceMetadataToken.RaceMode.SOLO, "NotImplemented");
            }
#endif

            //Spawn a specifc type of player based on the raceMode
            switch (RaceController.currentLevelMetadataToken.raceMode)
            {
                case RaceMetadataToken.RaceMode.SOLO:
                    playerController.inputWrapper = new PlayerInputWrapper(playerController);
                    break;
                case RaceMetadataToken.RaceMode.BOT:
                    playerController.inputWrapper = new COMInputWrapper(playerController);
                    break;
                case RaceMetadataToken.RaceMode.GHOST:
                    throw new NotImplementedException();
            }
        }


        public void PlayRaceFinishMusic()
        {
            levelBGM.gameObject.SetActive(false);
            raceFinishMusic.Play();
        }
    }
}

