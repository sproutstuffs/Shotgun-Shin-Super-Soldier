using System;
using System.ComponentModel;
using System.Threading;
using UnityEngine;


// METRONOME CLASS:

/* 
 
This class is responsible for keeping time.
Music and player inputs will be synchronized to the metronomes beats allowing it to be easily changed at any time.
This can also be used to change the difficulty of the game as the BPM is increased or decreased
 
--- HOW TO USE ---

1) Attach this script to a GameObject in your Unity scene.
2) Adjust the BPM and subdivisions in the Inspector to set the desired tempo and beat structure.
3) Subscribe to the Beat event to receive notifications when a beat occurs.
    3.5) Listen for ticks using "metronome.Beat += YourFunctionName;" to pass the beat time over.
4) In your subscribed function, implement the logic you want to execute on each beat.


 */

public class Metronome : MonoBehaviour
{

    // ---- Init Variables ---- //

    [Header("Metronome")]
    [SerializeField, Tooltip("A visual representation of the metronome ticking.")]
    private bool visualiser = false;

    [Header("Settings")]

    [SerializeField, Tooltip("Mute the metronome's ticking.")]
    private bool mute = false;
    private AudioSource audioSource;

    [SerializeField, Tooltip("Sets the speed at which the game is played.")]
    private double BPM = 100;

    [SerializeField, Tooltip("Sets how many beats there are in a bar (keep to a multiple of four)."), Range(1, 8)]
    private int subdivisions = 4;

    [SerializeField, Tooltip("Increases or decreases the input multiplier window."), Range(1, 10)]
    private float inputWindowMultiplier = 4f;

    //[SerializeField, Tooltip("A multiplier for how long the input window is for each beat (customise for difficulty modification)"), Range(0.5f, 1.5f)] // May need to change the range later!
    //private float beatLengthMult = 1;

    [Header("Debug")]

    // How long a beat is active for in seconds.
    [SerializeField] private double beatLength;

    // Holding when the next beat should be played.
    [SerializeField] private double nextBeatTime;

    // The window of time before and after a beat where inputs are valid.
    private double inputWindow;

    // ---- EVENTS ---- //

    // Event for when a beat is hit. Passes the next beat time and input window to any listeners.

    public event Action<double, double> Beat;
     
    // ---- UNITY FUNCTIONS ---- //

    private void Awake() {

        Debug.Log("Metronome Started!");
        audioSource = GetComponent<AudioSource>();
        Recalculate();
    }

    private void Update() {

        // Getting the current time in the audio system.
        double currentTime = AudioSettings.dspTime;

        while (currentTime > nextBeatTime) {

            //Passing the next beat time and input window to any listeners.

            Beat.Invoke(nextBeatTime, inputWindow);
            nextBeatTime += beatLength;
            visualiser = true;

            if (mute != true) {

                audioSource.Play();
            
            }

        }

        

        visualiser = false;
    }

    // ---- MY FUNCTIONS ---- //

    // Recalculating the beat length and input window based on the BPM and subdivisions.
    private void Recalculate() {

        beatLength = 60 / (BPM * subdivisions);
        nextBeatTime = AudioSettings.dspTime + beatLength;
        inputWindow = beatLength / inputWindowMultiplier;

    }

    private void OnValidate()
    {
        Recalculate();
    }

    public void SetTempo(double newBPM) {
        BPM = newBPM;
        // Need to update the FMOD bpm too!
        Recalculate();
    }

}

// With help from Charlie Huge! https://github.com/charliehuge/DesigningSoundMusicSystem/tree/master