using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSongSessionEvent : MonoBehaviour
{
    public enum Mode
    {
        Gameplay, Practice
    }
    public Mode mode = Mode.Gameplay;

    private void OnEnable()
    {
        if (mode == Mode.Gameplay)
        {
            var gameplay_ui = GameObject.FindObjectOfType<GamePlayUI>();
            if (gameplay_ui)
                gameplay_ui.OnResult.Invoke();
        }
        else if (mode == Mode.Practice)
        {
            var ui = GameObject.FindObjectOfType<PracticeModeUI>();
            if (ui)
            {
                ui.OnEndSession.Invoke();
            }
                
        }
        
    }

    private void OnDisable()
    {
        
    }
}
