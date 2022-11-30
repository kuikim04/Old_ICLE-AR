using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public static class PlayableDirectorHelper
{

    public static void SetPlaySpeed(this PlayableDirector director, double speed)
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(speed);
    }
    
    public static bool IsRunning(this PlayableDirector director)
    {
        return director.playableGraph.GetRootPlayable(0).GetSpeed() > 0.1f;
    }


}
