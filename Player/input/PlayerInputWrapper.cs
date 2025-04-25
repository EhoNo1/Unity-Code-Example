using Riders.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Riders.Player
{
    /// <summary>
    /// Captures and wraps inputs from the player's physical controller for live gameplay and recording purposes.
    /// </summary>
    public partial class PlayerInputWrapper : InputWrapper
    {
        public float pressTriggerValue = 0.9f;
        public float unpressTriggerValue = 0.3f;
        public PlayerGhostData ghostData;

        InputAction moveAction;
        InputAction boostAction;
        InputAction driftAction;

        public PlayerInputWrapper(PlayerController pc) : base(pc)
        {
            this.pc = pc;
            moveAction = InputSystem.actions.FindAction("Move");
            boostAction = InputSystem.actions.FindAction("Boost");
            driftAction = InputSystem.actions.FindAction("Drift");

            if (Accelerometer.current != null)
            {
                InputSystem.EnableDevice(Accelerometer.current);
                Debug.Log("Enabled Acceleromteter");
            } else { Debug.Log("Acceleromter not found."); }

            showUI = true;
            ghostData = new PlayerGhostData("PlayerName");
        }

        public override void OnDestroy()
        {
            Debug.Log("Player input wrapper was destroyed!");
            ghostData.WriteGhostData();

            if (Accelerometer.current != null && Accelerometer.current.enabled)
            {
                InputSystem.DisableDevice(Accelerometer.current);
                Debug.Log("Disabled accelerometer.");
            }
        }

        public override string QueryDebugText()
        {
            string debugText = "";

            if (Accelerometer.current != null)
            {
                var acceleration = Accelerometer.current.acceleration.ReadValue();

                debugText += "Accelerometer: " + acceleration + "\n";
            }

            return debugText;
        }

        public override PlayerInputFrame QueryInputFrame()
        {
            Vector2 inputVector = moveAction.ReadValue<Vector2>();

            if (Accelerometer.current != null)
            {
                Vector3 acceleration = Accelerometer.current.acceleration.ReadValue();
                float turningAngle = acceleration.x * 2f;
                turningAngle = Mathf.Clamp(turningAngle, -1f, 1f);

                inputVector.x = turningAngle;
            }

            bool brakeHeld = inputVector.y < 0f;
            bool boostHeld = boostAction.ReadValue<float>() > 0;
            bool slideHeld = driftAction.ReadValue<float>() > 0;
            bool jumpHeld = false;

            //Post processing
            //Autoacelerate
            if (!brakeHeld) inputVector.y = 1f;
            
            PlayerButtonInputFrame pbif = new PlayerButtonInputFrame(Managers.RaceController.GetRaceTimestamp(), inputVector, jumpHeld, brakeHeld, slideHeld, boostHeld);
            ghostData.PushButtonInputFrame(pbif);
            return pbif;
        }

        
    }
}