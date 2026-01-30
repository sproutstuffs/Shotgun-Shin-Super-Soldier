using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ---- Init Variables ---- //

    [SerializeField, Tooltip("For the 'Metronome' game object.")]
    private Metronome metronome;

    private AudioSource audioSource;

    // ---- UNITY FUNCTIONS ---- //

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        metronome.Beat += BeatTracker;


    }

    // ---- MY FUNCTIONS ---- //

    private void BeatTracker(double beatTime) { 
        
        audioSource.Play();

    }

}
