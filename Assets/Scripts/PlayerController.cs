using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    // ---- Init Variables ---- //

    [SerializeField, Tooltip("Global Metronome utility object (ensures everything runs off the same clock!)")]
    private Metronome metronome;

    //[SerializeField, Tooltip("Represents the window of which a player's input will be counted as on time.")]
    //private bool timingWindow = false;

    // Beat Tracking Variables:

    [Header("Debug:")]

    //[SerializeField, Tooltip("The exact time of the previous beat.")]
    //private double previousBeat;

    [SerializeField, Tooltip("The exact time that the next beat will happen.")]
    private double nextBeat;

    [SerializeField, Tooltip("What time the input window opens.")]
    private double inputWindowStart;

    [SerializeField, Tooltip("What time the input window closes.")]
    private double inputWindowEnd;



    // ---- UNITY FUNCTIONS ---- //

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {



    }
    
    void Update()
    {

        // This should be in update :)
        metronome.Beat += BeatTracker;


        // This should be called on an input event not every frame!

        inputTimingManager(0);

        while (AudioSettings.dspTime > inputWindowStart && AudioSettings.dspTime < inputWindowEnd)
        {

            inputTimingManager(1);

            if (AudioSettings.dspTime == nextBeat) { 
                
                Debug.Log("Perfect Timing!");
                inputTimingManager(2);

            }

        }

        // Move everything above into an input event!

    }

    /* INPUT HANDLING */

    // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.5/manual/index.html

    // LOOK AT THAT AND SEE IF IT HELPS WITH INPUT HANDLING :)

    // ---- MY FUNCTIONS ---- //

    private void BeatTracker(double nextBeatTime, double inputWindow) {

        //previousBeat = AudioSettings.dspTime;
        nextBeat = nextBeatTime;

        inputWindowStart = nextBeatTime - inputWindow;
        inputWindowEnd = nextBeatTime + inputWindow;


    }

    private void inputTimingManager(int timingPrecision)
    {

        /* TIMING PRECISION VALUES:
            0 = Outside Window
            1 = Inside Window
            2 = Perfect Timing */

        switch (timingPrecision)
        {
            case 0:
                Debug.Log("Input registered outside of timing window.");
                break;

            case 1:
                Debug.Log("Input registered inside timing window.");
                break;

            case 2:
                Debug.Log("Input registered with perfect timing!");
                break;

        }

    }

}
