using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // ---- Init Variables ---- //

    [Header("Metronome:")]
    [SerializeField, Tooltip("Global Metronome utility object (ensures everything runs off the same clock!)")]
    private Metronome metronome;

    [Header("Timing Multipliers:")]
    [SerializeField, Tooltip("The velocity multiplier applied when the player mistimes an input.")]
    private float inputTimingMiss = 0.8f;

    [SerializeField, Tooltip("The velocity multiplier applied when the player times an input correctly.")]
    private float inputTimingHit = 1.2f;

    [SerializeField, Tooltip("The velocity multiplier applied when the player times an input perfectly.")]
    private float inputTimingPerfect = 1.4f;

    [Header("Input Controls:")]
    [SerializeField, Tooltip("Assign an Action Set to set player controls.")]
    private InputActionAsset inputActions;

    private InputAction shootJump;
    private InputAction shootDirection;
    private InputAction shootDown;
    private InputAction shootGun;

    // Beat Tracking Variables:

    [Header("Debug:")]

    [SerializeField, Tooltip("Displays the most recent action taken by the player.")]
    private InputAction lastAction;

    [SerializeField, Tooltip("Tracks the player's current combo count.")]
    private int currentCombo = 0;

    [SerializeField, Tooltip("The exact time that the next beat will happen.")]
    private double nextBeat;

    [SerializeField, Tooltip("What time the input window opens.")]
    private double inputWindowStart;

    [SerializeField, Tooltip("What time the input window closes.")]
    private double inputWindowEnd;



    // ---- UNITY FUNCTIONS ---- //

    private void Awake()
    {
        InputActionMap actionMap = inputActions.FindActionMap("Player");

        shootDirection = actionMap.FindAction("Move");
        shootJump = actionMap.FindAction("Jump");
        shootDown = actionMap.FindAction("Down");
        shootGun = actionMap.FindAction("Attack");

        // We can either subscribe to the actions here and get an event trigger when a button is pressed (probably a good idea).



        shootJump.started += StartJump;
        shootJump.canceled += OnJump;
        shootDirection.performed += OnMove;
        shootDown.performed += OnDown;
        shootGun.performed += OnShoot;


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        metronome.Beat += BeatTracker;


    }


    
    void Update()
    {

        


    }

    // ---- CONTROLS FUNCTIONS ---- //

    // Left & Right Input:
    private void OnMove(InputAction.CallbackContext context) {
        float moveInput = context.ReadValue<float>();
        Debug.Log("Move Input Detected: " + moveInput);
        Debug.Log("Move Input Timing: " + InputTimingTracker(AudioSettings.dspTime));
    }

    // Down Input:
    private void OnDown(InputAction.CallbackContext context) {
        Debug.Log("Down Input Detected!");
        Debug.Log("Down Input Timing: " + InputTimingTracker(AudioSettings.dspTime));
    }

    // Jump Input:
    private void StartJump(InputAction.CallbackContext context) {

        Debug.Log("Jump Started...");
        Debug.Log("Jump Start Timing: " + InputTimingTracker(AudioSettings.dspTime));

    }
    private void OnJump(InputAction.CallbackContext context) { 
    
        Debug.Log("Jump Release Detected!");
        Debug.Log("Jump Release Timing: " + InputTimingTracker(AudioSettings.dspTime));

    }
    private void OnShoot(InputAction.CallbackContext context) {
        Debug.Log("Shoot Input Detected!");
        Debug.Log("Shoot Input Timing: " + InputTimingTracker(AudioSettings.dspTime));
    }


    // ---- MY FUNCTIONS ---- //

    private float InputTimingTracker(double inputTiming) {

        // This should be called on an input event not every frame!
        
        if (inputTiming > inputWindowStart && inputTiming < inputWindowEnd) {

            if (inputTiming == nextBeat) {

                // If the input is perfectly in time:
                Debug.Log("Perfect Timing!");
                return inputTimingPerfect;

                // NOTE: May need to increase the window for the 'perfect' hit as i dont think the exact timing is possible.

            }

            // If the input is inside the input window:
            Debug.Log("Within time!");
            return inputTimingHit;

        }

        // If the input is outside of the input window:
        Debug.Log("Missed!");
        return inputTimingMiss;

    }

    private int ComboTracker(bool successfulInput) {
        // This function will track the player's current combo based on successful inputs.

        if (successfulInput) {
            currentCombo += 1;
        }

        else {
            currentCombo = 0;
        }

        return currentCombo;
    }


    private void BeatTracker(double nextBeatTime, double inputWindow) {

        // Saving the next beat time.
        nextBeat = nextBeatTime;

        // Saving the input windows for easy comparison.
        inputWindowStart = nextBeatTime - inputWindow;
        inputWindowEnd = nextBeatTime + inputWindow;


    }


}
