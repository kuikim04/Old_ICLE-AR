using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;
using Sirenix.OdinInspector;

public class SongInfoScript : MonoBehaviour
{
    //public ModelInfo model
    //{
    //    get
    //    {
    //        if (_model == null)
    //            _model = GetComponentInChildren<ModelInfo>(true);
    //        return _model;
    //    }
    //}
    //private ModelInfo _model;

    public VideoPlayer video
    {
        get
        {
            if (_video == null)
                _video = GetComponentInChildren<VideoPlayer>(true);
            return _video;
        }
    }
    private VideoPlayer _video;

    [Button("Set Clip")]
    [ContextMenu("Set Clip Time")]
    void SetClipTime()
    {
        var director = GetComponentInParent<PlayableDirector>();
        var timeline = director.playableAsset as TimelineAsset;
        var video_track = timeline.GetOutputTrack(0);
        var clips = video_track.GetClips();
        foreach (var item in clips)
        {
            Debug.Log(item.displayName);
            Debug.Log(video.clip.name);
            item.duration = video.clip.length;
        }
    }


}
