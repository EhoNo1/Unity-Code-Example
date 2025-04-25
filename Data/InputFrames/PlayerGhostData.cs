using Riders.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace Riders.Data
{
    [Serializable]
    public class PlayerGhostData
    {
        [SerializeField] private string playerName = "PlayerName";
        [SerializeField] Timestamp timestamp;
        private List<PlayerButtonInputFrame> buttonInputFramesList = new List<PlayerButtonInputFrame>();
        [SerializeField] private SerializableArray<PlayerButtonInputFrame> buttonInputs = new SerializableArray<PlayerButtonInputFrame>();
        private List<PlayerTransformKeyframe> transformKeyframeList = new List<PlayerTransformKeyframe>();
        [SerializeField] private SerializableArray<PlayerTransformKeyframe> keyframes = new SerializableArray<PlayerTransformKeyframe>();

        public PlayerGhostData(string playerName)
        {
            this.playerName = playerName;
        }

        public void PushButtonInputFrame(PlayerButtonInputFrame toPush)
        {
            buttonInputFramesList.Add(toPush);
        }
        public void PushTransformKeyframe(PlayerTransformKeyframe toPush)
        {
            transformKeyframeList.Add(toPush);
        }

        public void SetTimestamp(float raceTime)
        {
            timestamp = new Timestamp(raceTime);
        }


        public string getPlayerName() { return playerName; }
        public Timestamp getTimeStamp() { return timestamp; }


        public void WriteGhostData()
        {
            Debug.Log("Writing ghost data...");

            buttonInputs.array = buttonInputFramesList.ToArray();
            keyframes.array = transformKeyframeList.ToArray();

            string rawJson = JsonUtility.ToJson(this);
            string ghostsDirectory = Application.dataPath + "/ghosts";
            string uniqueName = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + ".json";
            
            if (!Directory.Exists(ghostsDirectory))
            {
                Debug.Log("Ghosts directory does not exist, creating...");
                Directory.CreateDirectory(ghostsDirectory);
            }

            string filePath = ghostsDirectory + "/" + uniqueName;
            File.WriteAllText(filePath, rawJson);
            Debug.Log("Done writing ghost data to " + filePath);
        }
    }
}

