using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class LoginUIScript : MonoBehaviour
{
    public UnityEvent Next;
    public AudioMixerSnapshot menuShot;

    void Start()
    {
       
    }

    private void OnEnable()
    {
        menuShot.TransitionTo(1f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            Next.Invoke();
            // next screen
            // to select song.
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            LeaderboardScript.DeleteFile();
        }
    }
}
