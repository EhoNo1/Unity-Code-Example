using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Riders.Player;


namespace Riders.Managers
{
    public static class RaceController
    {
        public static RaceMetadataToken currentLevelMetadataToken;
        private static List<PlayerController> playerControllers = new List<PlayerController>();
        private static bool isRaceInProgress = false;
        private static float raceStartTimestamp;


        public static void RegisterPlayer(PlayerController newPlayer)
        {
            playerControllers.Add(newPlayer);
        }


        public static void AddLap(PlayerController pc)
        {
            pc.addLap();
            Debug.Log("Player " + pc.name + " has reached lap " + pc.getLaps());

            /*if (pc.getLaps() >= currentLevelData.lapCount + 1)
            {
                Debug.Log("Player " + pc.name + " has finished!");

                pc.inputWrapper.OnDestroy();
                if (pc.inputWrapper.GetType() == typeof(PlayerInputWrapper))
                {
                    pc.inputWrapper = new COMInputWrapper(pc);
                    InSceneRaceHelper.rh.PlayRaceFinishMusic();
                }
            }*/
        }

        public static void LoadSceneAndStartRace(RaceMetadataToken raceMetadata, LoadSceneMode loadSceneMode)
        {
            Debug.Log("Loading race with metadata: " + raceMetadata);
            
            //Update Current level data for future reference.
            currentLevelMetadataToken = raceMetadata;

            //Load the scene
            SceneManager.LoadScene("VentusRush", loadSceneMode);
            
            //Assuming there is an InSceneRaceHelper prefab in the scene, the initialization of that prefab will spawn the player.

            //Reset the timer.
            raceStartTimestamp = Time.fixedUnscaledTime;

        }


        public static bool IsRaceInProgress() { return isRaceInProgress; }

        public static PlayerController[] GetPlacementArray()
        {

            // get the list
            List<PlayerController> list = playerControllers;

            //sort by lap
            list.Sort((p1, p2) => p2.getLaps().CompareTo(p1.getLaps()));

            //return list
            return list.ToArray();
        }

        public static int GetPlacement(PlayerController player)
        {
            PlayerController[] placementArray = GetPlacementArray();
            int i = -1;
            foreach (PlayerController p in placementArray)
            {
                i++;
                if (p == player)
                {
                    return i;
                }
            }

            return -1;
        }

        public static float GetRaceTimestamp()
        {
            return Time.fixedUnscaledTime - raceStartTimestamp;
        }
    }
}