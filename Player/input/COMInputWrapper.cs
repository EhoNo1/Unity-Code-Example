using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Riders.Player
{
    /// <summary>
    /// The machine brain that controls COM players
    /// </summary>
    public class COMInputWrapper : InputWrapper
    {
        public float turnMult = 2f;
        public float autoSlideAngle = 10;

        private Vector3 randomOffset;
        public float randomOffsetClockLength = 5f;

        public float minimumStuckDetectionSpeed = 2.5f;
        public float stuckDuration = 0f;
        public float stuckAutoCorrectDuration = 5f;

        bool isSliding = false;

        public float lastRandomizationTimestamp = 0f;

        public COMInputWrapper(PlayerController pc) : base(pc)
        {
            this.pc = pc;
            this.showUI = false;
        }

        public override void OnDestroy()
        {
            //Does not need to do anything
        }


        float GetTurnAngle(Vector3 worldDestination)
        {
            Vector3 localDestinationPoint = pc.transform.InverseTransformPoint(worldDestination);

            localDestinationPoint += GetScaledOffset();
            return localDestinationPoint.normalized.x * turnMult;
        }


        void GetUnstuck()
        {
            stuckDuration = 0f;
            pc.ChooseTargetGate();
        }


        float RandomOffsetFloat() { return Random.Range(-0.5f, 0.5f); }
        Vector3 RandomOffsetVector() { return new Vector3(RandomOffsetFloat(), RandomOffsetFloat(), RandomOffsetFloat()); }

        Vector3 GetScaledOffset()
        {
            Transform targetGate = pc.GetTargetGate().transform;
            Vector3 scaledOffset = randomOffset;

            scaledOffset.x *= targetGate.lossyScale.x;
            scaledOffset.y *= targetGate.lossyScale.y;
            scaledOffset.z *= targetGate.lossyScale.z;
            return scaledOffset;
        }

        public override PlayerInputFrame QueryInputFrame()
        {
            if (Time.realtimeSinceStartup - lastRandomizationTimestamp > randomOffsetClockLength) randomOffset = GetScaledOffset();
            float angle = GetTurnAngle(pc.GetTargetGate().transform.position);


            if (!isSliding && Mathf.Abs(angle) > autoSlideAngle) { isSliding = true; }
            if (isSliding && Mathf.Abs(angle) < autoSlideAngle) { isSliding = false; }

            Vector2 inputVector = new Vector2(angle, 1f);


            //Stuck detection and resolution.
            if (stuckDuration < 0 || pc.getBody().linearVelocity.magnitude < minimumStuckDetectionSpeed)
            {
                stuckDuration += Time.fixedDeltaTime;
            }
            if (stuckDuration > stuckAutoCorrectDuration) { GetUnstuck(); }

            PlayerInputFrame pbif = new PlayerButtonInputFrame(Managers.RaceController.GetRaceTimestamp(), inputVector, false, false, isSliding, false);
            return pbif;
        }


        public override string QueryDebugText()
        {
            return "";
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (pc == null || pc == null) { return; }
            Debug.DrawLine(pc.transform.position, pc.GetTargetGate().transform.position + GetScaledOffset(), Color.green);
        }
#endif
    }
}