using Riders;
using Riders.Managers;
using Riders.Objects;
using Riders.Player;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// General purpose controller for all of the player's scripts;
/// </summary>
[RequireComponent(typeof(ContactCounter))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private ContactCounter contactCounter;
    [SerializeField] private Transform cameraAxis;
    [SerializeField] private Rigidbody body;
    public PlayerInput playerInput;
    public Rigidbody getBody() { return body; }

    [SerializeField] private Canvas UI;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider myCollider;
    private Gate lastGate;
    private Gate targetGate;
    [SerializeField] private BoardStats board = new BoardStats();
    


    [Header("Boosting")]
    [SerializeField] private ForceMode boostMode = ForceMode.VelocityChange;
    [SerializeField] private float boostTimer = 0f;


    [Header("Braking")]
    private bool isSliding;
    private float slideDuration = 0f;
    [SerializeField] private PhysicsMaterial normalPlayerPhysics;
    [SerializeField] private PhysicsMaterial brakingPlayerPhysics;


    [Header("Movement")]
    [Tooltip("The amount of acceleration applied to the player walking around normally.")]
    [SerializeField] private float walkingAccelerationAmount = 15;
    [Tooltip("An impulse acceleration given to a player at a dead stop to overcome static friction.")]
    [SerializeField] private float kickOffImpulse = 0.1f;

    [SerializeField] private float walkingMovementMaxSpeed = 60f;
    
    [SerializeField] private MovementMode _movementMode = MovementMode.WALKING;
    public MovementMode movementMode
    {
        get { return _movementMode; }
        set
        {
            //Debug.Log("Changing to movementMode to " + value);
            _movementMode = value;
            animator.SetInteger("MovementMode", (int)value);
        }
    }


    [Header("Collision")]
    [SerializeField] private float minimumBumpForce;
    [SerializeField] private float maximumWallAngleDeg = 20f;
    [SerializeField] private float floorDetectionDistance = 1.5f;
//    [SerializeField] private float timerMult = 1f;
    

    [Header("Physics")]
    [SerializeField] private float groundClingGravity = 9.8f;


    [Header("Scripted Segments")]
    [SerializeField] private float enteredScriptedSegment;
    [SerializeField] private Vector3 enterPosition;
    [SerializeField] private Vector3 enterVelocity;


    [Header("Fuel")]
    [SerializeField] private float _fuel = 100f;
    private float fuel
    {
        get { return _fuel; }
        set
        {
            _fuel = Mathf.Clamp(value,0f,board.maxFuel);
            float fuelPercentage = GetFuelPercent();

            animator.SetFloat("FuelPercentage", fuelPercentage);
        }
    }
    



    [Header("Race Metadata")]
    [SerializeField] private int _laps = 0;
    public int getLaps() { return _laps; }
    public void addLap() { _laps++; }


    [Header("Player Input")]
    [SerializeField] private PlayerButtonInputFrame previousButtonInputFrame;
    [SerializeField] private InputWrapper _inputWrapper;
    public InputWrapper inputWrapper
    {
        get { return _inputWrapper; }
        set
        {
            _inputWrapper = value;
            UI.gameObject.SetActive(value.showUI);
        }
    }

    //Camera Effects
    [SerializeField] private float[] cameraTiltAverageCache;
    private int cameraTiltAverageIndex = 0;
    private int _cameraTiltAverageSize = 20;
    public int cameraTiltAverageSize
    {
        get { return _cameraTiltAverageSize; }
        set
        {
            cameraTiltAverageCache = new float[value];
            _cameraTiltAverageSize = value;
        }
    }



    void Start()
    {
        cameraTiltAverageCache = new float[cameraTiltAverageSize];

        playerInput = GetComponent<PlayerInput>();

        Gate startline = GameObject.FindGameObjectWithTag("StartLine").GetComponent<Gate>();
        lastGate = startline;
        targetGate = startline;

        RaceController.RegisterPlayer(this);
    }

    void FixedUpdate()
    {
        if (movementMode == MovementMode.SCRIPTED) return;
        PlayerInputFrame pif = inputWrapper.QueryInputFrame();
        if (pif.GetType() == typeof(PlayerButtonInputFrame)) { TakeUserInput((PlayerButtonInputFrame)pif); }
        else if (pif.GetType() == typeof(PlayerTransformKeyframe)) { ((PlayerTransformKeyframe)pif).ApplyKeyframe(transform, body); }


        if (IsBoosting() && IsOnGround())
        {
            boostTimer -= Time.fixedDeltaTime;
            body.AddForce(body.transform.forward * board.boostForce, boostMode);
        }

        if (isSliding)
        {
            slideDuration += Time.deltaTime;
            if (!IsOnGround()) { StopSlide(); }
        }

        //Force the player's velocity to be within allowed bounds
        if (body.linearVelocity.magnitude > board.maxSpeed) body.linearVelocity = body.linearVelocity.normalized * board.maxSpeed;

        //Respawn the player if they fall out of bounds
        if (transform.position.y < InSceneRaceHelper.rh.automaticRespawnHeight) Respawn();

        if (contactCounter.GetContactCount() > 0) { body.AddForce(Vector3.down * groundClingGravity * Time.deltaTime, ForceMode.Acceleration); }


        if (!IsFuelDepleted()) { 
            if (movementMode == MovementMode.RIDING)
            {
                fuel -= board.fuelConsumedPerSecond * Time.fixedDeltaTime;
            }
        }
        else
        {
            movementMode = MovementMode.WALKING;
        }
    }


    
    void Update()
    {
        float topSpeedPercentage = Mathf.Clamp01(body.linearVelocity.magnitude / GetStateSpecificMaxSpeed());
        if (topSpeedPercentage < 0.05f) topSpeedPercentage = 0f;
        animator.SetFloat("TopSpeedPercentage", topSpeedPercentage);


        if (Accelerometer.current != null)
        {
            Vector3 acceleration = Accelerometer.current.acceleration.ReadValue();
            float cameraTilt = acceleration.x;
            cameraTilt = Mathf.Clamp(cameraTilt, -1f, 1f);

            cameraTiltAverageIndex = (cameraTiltAverageIndex + 1) % cameraTiltAverageSize;
            cameraTiltAverageCache[cameraTiltAverageIndex] = cameraTilt;

            float averageTilt = 0f;
            for (int i = 0; i < cameraTiltAverageSize; i++)
            {
                averageTilt += cameraTiltAverageCache[i];
            }
            averageTilt /= (float)cameraTiltAverageSize;

            cameraAxis.localRotation = Quaternion.Euler(Vector3.forward * -averageTilt * 45f);
        }

    }

    public bool CanControl() { return _movementMode != MovementMode.SCRIPTED; }

    public void Stun() { body.linearVelocity *= board.stunMult; }

    public void Respawn()
    {
        Vector3 respawnPosition = lastGate.transform.position;
        Ray r = new Ray(lastGate.transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, 20f))
        {
            respawnPosition = hit.point + Vector3.up * 1.5f;
        }

        body.position = respawnPosition;
        body.linearVelocity = Vector3.zero;
    }


    private void StartSlide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
    }
    private void StopSlide()
    {
        isSliding = false;
        animator.SetBool("isSliding", false);
        if (slideDuration > board.minimumSlideDurationForBoost) Boost();
        //Debug.Log("Stopped sliding, duration: " + slideDuration);
        slideDuration = 0f;
    }


    private void Boost()
    {
        if (!CanControl() || IsBoosting() || movementMode != MovementMode.RIDING || !IsOnGround()) return;

        if (attemptFuelTransaction(board.boostCost))
        {
            animator.SetTrigger("onBoost");
            boostTimer = board.boostDuration;
        }
        animator.SetTrigger("BoostTrigger");
    }

    public void AlignToForward(Vector3 forwardVector)
    {
        Vector3 cleanedForward = forwardVector;
        cleanedForward.y = 0;

        body.transform.LookAt(cleanedForward + body.position);
        body.linearVelocity = forwardVector.normalized * body.linearVelocity.magnitude;
    }

    void OnTriggerExit(Collider other)
    {
        Gate g = other.GetComponent<Gate>();
        if (g == null) { return; }
        lastGate = g;
        ChooseTargetGate();
    }


    /// <summary>
    /// Selects a random gate from the previous gate's forward connections
    /// </summary>
    /// <exception cref="System.Exception"></exception>
    public void ChooseTargetGate()
    {
        if (lastGate == null)
        {
            GameObject sg = GameObject.FindGameObjectWithTag("StartLine");
            lastGate = sg.GetComponentInChildren<Gate>();
        }
        if (lastGate.nextGates.Count != 0)
        {
            int choice = UnityEngine.Random.Range(0, lastGate.nextGates.Count);
            targetGate = lastGate.nextGates.ElementAt(choice);
        }
    }
    public Gate GetTargetGate()
    {
        if (targetGate != null) { return targetGate; }
        ChooseTargetGate();
        return GetTargetGate();
    }
    public void SetTargetGate(Gate newTarget)
    {
        targetGate = newTarget;
    }

    public Gate GetLastGate() { return lastGate; }


    private bool boostReleased = true;
    private bool slideReleased = true;
    public void TakeUserInput(PlayerButtonInputFrame inputFrame)
    {
        Turn(inputFrame);
        Accelerate(inputFrame);


        if (inputFrame.boostHeld && boostReleased)
        {
            Boost();
            boostReleased = false;
        }
        else if (!inputFrame.boostHeld && !boostReleased) boostReleased = true;


        if (inputFrame.slideHeld && slideReleased)
        {
            StartSlide();
            slideReleased = false;
        }
        else if (!inputFrame.slideHeld && !slideReleased)
        {
            StopSlide();
            slideReleased = true;
        }

        Brake(inputFrame.brakeHeld);

        previousButtonInputFrame = inputFrame;
    }


    /// <summary>
    /// Turn the player based on an input amount.
    /// </summary>
    /// <param name="amount">The amount to turn clamped between -1f and 1f</param>
    private void Turn(PlayerButtonInputFrame inputFrame)
    {

        float clampedDirection = Mathf.Clamp(inputFrame.movement.x, -1f, 1f);

        float angle = board.turningSpeed * clampedDirection;
        if (isSliding) { angle *= board.slidingTurningMult; }
        body.linearVelocity = Quaternion.AngleAxis(angle, Vector3.up) * body.linearVelocity;
        body.transform.Rotate(Vector3.up * angle);
    }

    private void Accelerate(PlayerButtonInputFrame inputFrame)
    {
        ForceMode mode = ForceMode.VelocityChange;
        //Use impulse instead of velocity change when the player starts moving to get them going
        if (body.linearVelocity.magnitude < kickOffImpulse) { mode = ForceMode.Impulse; }

        if (body.linearVelocity.magnitude < GetStateSpecificMaxSpeed())
        {
            float amount = inputFrame.movement.y * walkingAccelerationAmount;
            if (movementMode == MovementMode.RIDING && IsOnGround() && inputFrame.movement.y >= 0f) amount = board.autoAccelerateAmount;

            Vector3 force = body.transform.forward * amount * Time.fixedDeltaTime;
            body.AddForce(force, mode);
        }
    }

    public void Brake(bool value)
    {
        if (!IsOnGround()) return;

        if (value)
        {
            myCollider.material = brakingPlayerPhysics;
        }
        else
        {
            myCollider.material = normalPlayerPhysics;
        }
        
    }

    public void Dash(float impulseMult, Vector3 forwardDirection, float minimumSpeed = -1f)
    {
        AlignToForward(forwardDirection);
        body.AddForce(body.transform.forward * impulseMult);
        if (minimumSpeed > 0f && body.linearVelocity.magnitude < minimumSpeed)
        {
            body.linearVelocity = body.linearVelocity.normalized * minimumSpeed;
        }
    }

    /// <summary>
    /// Returns the max speed the player should adhere to depending on their current movement state.
    /// </summary>
    /// <returns></returns>
    public float GetStateSpecificMaxSpeed()
    {
        if (movementMode == MovementMode.WALKING)
        {
            return walkingMovementMaxSpeed;
        }
        return board.maxSpeed;
    }
    public Ray GetGroundRay() { return new Ray(transform.position, -transform.up); }
    public bool IsOnGround() { return Physics.Raycast(GetGroundRay(), floorDetectionDistance); }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player" || collision.gameObject.GetComponent<Breakable>() != null) { return; }

        //calculate the horizontal impulse
        Vector2 horizontalImpulse = new Vector2(collision.impulse.x, collision.impulse.z);

        //if the the impulse is to low for a redirect just skip everything else
        if (horizontalImpulse.magnitude < minimumBumpForce) return;

        //calculate the angle of the wall
        float wallAngle = Mathf.Tan(collision.impulse.y / horizontalImpulse.magnitude) * Mathf.Rad2Deg;

        //if the angle isn't high enough, then don't rebound
        if (Mathf.Abs(wallAngle) > maximumWallAngleDeg) return;

        //bounce
        AlignToForward(Vector3.Reflect(transform.forward, collision.contacts[0].normal));
        body.linearVelocity = body.linearVelocity.normalized * (collision.relativeVelocity.magnitude * board.bumpMult);
    }


    public void StartScriptedSegment()
    {
        movementMode = MovementMode.SCRIPTED;
        body.useGravity = false;
        enteredScriptedSegment = Time.realtimeSinceStartup;
        enterPosition = body.position;
        enterVelocity = body.linearVelocity;
        body.linearVelocity = Vector3.zero;
    }

    /// <summary>
    /// Resets the player to be controlled normally again.
    /// </summary>
    public void StopScriptedSegment(Gate nextGate)
    {
        movementMode = MovementMode.RIDING;
        body.useGravity = true;
        SetTargetGate(nextGate);
        body.linearVelocity = GetTargetGate().transform.forward * enterVelocity.magnitude;
    }
    public float getTimeInScriptedSegment() { return (Time.realtimeSinceStartup - enteredScriptedSegment); }
    /// <returns>The position the player entered the scripted segment from.</returns>
    public Vector3 getEntryPosition() { return enterPosition; }

    public void AddFuel(float amount)
    {
        fuel += amount;
        movementMode = MovementMode.RIDING;
    }


    public void SetMinimumFuel(float minimumPercent)
    {
        float amount = board.maxFuel * minimumPercent;

        if (fuel > amount) return;

        fuel = amount;
    }


    public bool attemptFuelTransaction(float cost)
    {
        if (fuel > cost)
        {
            fuel -= cost;
            return true;
        }
        return false;
    }

    float GetFuelPercent()
    {
        return Mathf.Clamp01(fuel / board.maxFuel);
    }
    bool IsFuelDepleted() {
        return fuel <= 0f; 
    }
    public bool IsBoosting()
    {
        return boostTimer > 0f;
    }
    public bool ShouldPlayEffects()
    {
        if (inputWrapper.GetType() == typeof(PlayerInputWrapper)) return true;
        return false;
    }
    public bool IsBraking()
    {
        return IsOnGround() && previousButtonInputFrame.brakeHeld;
    }

    void OnGUI()
    {
        string debugText = "Debug Mode On\n";

        if (inputWrapper != null) { debugText += inputWrapper.QueryDebugText(); }

        GUI.Label(new Rect(100f, 100f, 300f, 300f), debugText);
    }
}

public enum MovementMode { WALKING = 0, RIDING = 1, SCRIPTED = 2 };