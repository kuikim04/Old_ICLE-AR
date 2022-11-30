using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLinePause : MonoBehaviour
{
    [ContextMenu("Pause")]
    private void OnEnable()
    {
        // get current playable director in root
        var director = transform.root.GetComponent<PlayableDirector>();
        director.SetPlaySpeed(0);

        var pose = director.GetComponent<PoseValueScript>();
        if (pose)
            pose.particle.SetParticle(true);
    }

    [ContextMenu("Un Pause")]
    public void UnPause()
    {
        var director = transform.root.GetComponent<PlayableDirector>();
        director.SetPlaySpeed(1);
    }
}
