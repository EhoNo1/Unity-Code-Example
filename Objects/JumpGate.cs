using Riders.Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Riders.Objects
{
    public class JumpGate : Gate
    {
        [SerializeField] private float jump_time_duration = 2.5f;
        [SerializeField] private float jumpHeight = 15f;
        [SerializeField] private float refillFuel = 30f;
        private List<PlayerController> playerScriptedControllers = new List<PlayerController>();
        public Gate endGate;

        void Update()
        {
            for (int i = 0; i < playerScriptedControllers.Count; i++)
            {
                PlayerController pc = playerScriptedControllers.ElementAt(i);

                if (pc.getTimeInScriptedSegment() > jump_time_duration)
                {
                    playerScriptedControllers.Remove(pc);
                    Vector3 exitPosition = GetJumpPositionAtPercentage(pc.getEntryPosition(), 1f);
                    pc.transform.position = exitPosition;
                    pc.getBody().position = exitPosition;
                    pc.StopScriptedSegment(endGate);
                }
                else
                {
                    float jumpComplete = pc.getTimeInScriptedSegment() / jump_time_duration;
                    pc.transform.position = GetJumpPositionAtPercentage(pc.getEntryPosition(), jumpComplete);
                    pc.AddFuel(refillFuel);
                }
            }
        }

        private Vector3 GetJumpPositionAtPercentage(Vector3 startPosition, float percentageComplete)
        {
            float clampedPercentage = Mathf.Clamp01(percentageComplete);

            Vector3 localPosition = transform.InverseTransformPoint(startPosition);
            Vector3 endPosition = GetEndGate().transform.TransformPoint(localPosition);

            Vector3 pos = Vector3.Lerp(startPosition, endPosition, clampedPercentage);
            pos.y += Mathf.Sin(clampedPercentage * Mathf.PI) * jumpHeight;
            return pos;
        }

        void OnTriggerExit(Collider other)
        {
            PlayerController player = getPlayerFromCollider(other);
            player.StartScriptedSegment();
            playerScriptedControllers.Add(player);
        }


        Gate GetEndGate()
        {
            return nextGates.ElementAt(0);
        }


#if UNITY_EDITOR
        [SerializeField] const int arcVisualizerSegments = 7;

        void OnDrawGizmos()
        {
            for (int i = 0; i < nextGates.Count; i++)
            {
                if (nextGates.ElementAt(i) == null || nextGates.ElementAt(i) == this) { continue; }

                Vector3[] arcPoints = new Vector3[arcVisualizerSegments + 1];

                for (int j = 0; j < arcVisualizerSegments; j++)
                {
                    arcPoints[j] = GetJumpPositionAtPercentage(transform.position, j / (float)arcVisualizerSegments);
                }
                arcPoints[arcVisualizerSegments] = GetEndGate().transform.position;
                for (int k = 1; k < arcVisualizerSegments + 1; k++)
                {
                    Debug.DrawLine(arcPoints[k - 1], arcPoints[k], Color.green);
                }

                Debug.DrawLine(transform.position, nextGates.ElementAt(i).transform.position, Color.green);
            }
        }
#endif



    }
}