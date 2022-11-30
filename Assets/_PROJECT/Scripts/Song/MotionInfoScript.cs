using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;
using Sirenix.OdinInspector;

public class MotionInfoScript : MonoBehaviour
{
    #region Properties
    public int startTrack = 0;
    public int trackCount = 2;
    public Dictionary<string, MotionItem> dict;

    //[InlineButton("UpdateQueue", "Queue")]
    //public List<MotionItem> queue;
    public List<MotionAndPose> queue;

    PlayableDirector director;
    //List<TimelineClip> clips;

    MotionAndPose current;
    double prev_time;

    [HideInInspector]
    public UnityEvent OnEnter;
    [HideInInspector]
    public UnityEvent OnExit;
    #endregion

    #region Tools
    [ContextMenu("Create Motion Data")]
    void CreateMotionData()
    {
        Debug.Log("run");
        Debug.Log(trackCount);
        var director = GetComponent<PlayableDirector>();
        var timeline = director.playableAsset as TimelineAsset;
        var clips = new List<TimelineClip>();
        for (int i = 0; i < trackCount; i++)
        {
            var track = timeline.GetOutputTrack(i + startTrack);
            clips.AddRange(track.GetClips());
        }

        clips = clips.OrderBy(x => x.start).ToList();
        Debug.Log(clips.Count);
        var dict = new Dictionary<string, string>();
        var index = 0;
        foreach (var item in clips)
        {
            Debug.Log(item.displayName);
            if (dict.ContainsKey(item.displayName) == false)
            {
                var go = new GameObject(index + " " + item.displayName);
                var script = go.AddComponent<MotionItem>();
                script.clip = item.animationClip;
                go.transform.SetParent(transform, false);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;

                dict.Add(item.displayName, "");
                index++;
            }
        }
    }

    //[Button("Set Blend 30 frames")]
    //void AutoBlendClip()
    //{
    //    need duo channel system for blend dance move.

    //    set blend time in every clip

    //    set idle in every clip

    //    var director = GetComponent<PlayableDirector>();
    //    var timeline = director.playableAsset as TimelineAsset;
    //    var clips = new List<TimelineClip>();
    //    2, 3
    //    for (int i = 0; i < trackCount; i++)
    //    {
    //        var track = timeline.GetOutputTrack(i + startTrack);
    //        var clip = track.GetClips().ToList();
    //        clips.AddRange(clip);
    //    }

    //    clips = clips.OrderBy(x => x.start).ToList();
    //    4
    //    foreach (var item in clips)
    //    {
    //        item.blendInDuration = 0.5f;
    //        item.blendOutDuration = 0.5f;
    //    }
    //}
    #endregion

    #region Functions
    public static MotionInfoScript singleton;
    private void Awake()
    {
        singleton = this;
    }
    bool _init;
    /// <summary>
    /// call this for init queue data.
    /// </summary>
    public void Init()
    {
        SetupMotionsQueue();
        prev_time = -1;
        _init = true;
    }

    /// <summary>
    /// 0 Prepare variables
    /// 1 Get Timeline
    /// 2 Get Tracks
    /// 3 Get Clips
    /// 4 Create MotionAndPose from clips
    /// </summary>
    void SetupMotionsQueue()
    {
        // 0
        var motions = new List<MotionItem>();
        motions.AddRange(GetComponentsInChildren<MotionItem>());
        dict = new Dictionary<string, MotionItem>();
        dict = motions.ToDictionary(x => x.clip.name, x => x);

        //queue = new List<MotionItem>();
        queue = new List<MotionAndPose>();

        // 1
        director = GetComponent<PlayableDirector>();
        var timeline = director.playableAsset as TimelineAsset;
        var clips = new List<TimelineClip>();
        // 2, 3
        for (int i = 0; i < trackCount; i++)
        {
            var track = timeline.GetOutputTrack(i + startTrack);
            var clip = track.GetClips().ToList();
            Debug.Log("clips: " + clip.Count);
            clips.AddRange(clip);
        }

        clips = clips.OrderBy(x => x.start).ToList();
        Debug.Log("clips: " + clips.Count);
        // 4
        foreach (var item in clips)
            if (dict.ContainsKey(item.displayName))
            {
                var motion_data = new MotionAndPose();
                motion_data.motion = dict[item.displayName];
                motion_data.clip = item;
                // pose data set at PoseUI
                queue.Add(motion_data);
            }
    }

    void Update()
    {
        if (!_init)
            return;

        if (queue.Count <= 0)
        {
            //_init = false; // out of clips.
            return;
        }

        // detect timeline event
        var cur_time = director.time;

        if (queue[0].IsEnterTime(cur_time, prev_time))
        {
            Debug.Log(Time.time.ToString("N2") + " on enter clip " + queue[0].clip.displayName);
            if (current != null)
                OnExitClip();

            current = queue[0];
            OnEnter.Invoke();
        }

        if (queue.Count > 1)
            if (queue[1].IsEnterTime(cur_time, prev_time))
            {
                Debug.Log(Time.time.ToString("N2") + " on enter clip " + queue[1].clip.displayName);
                if (current != null)
                    OnExitClip();

                current = queue[0];
                OnEnter.Invoke();
            }

        //// update current motion data.
        //queue[0].Update(director.time);

        if (current != null)
            if (current.IsExitTime(cur_time, prev_time))
                OnExitClip();

        prev_time = cur_time;
    }

    void OnExitClip()
    {
        Debug.Log(Time.time.ToString("N2") + " on exit clip " + queue[0].clip.displayName);
        OnExit.Invoke();
        //queue[0].OnRemove();
        queue.RemoveAt(0);
        current = null;
    }

    //private void OnGUI()
    //{
    //    if (director != null)
    //        GUILayout.Box("Time: " + director.time);

    //    if (current != null)
    //        GUILayout.Box("Clip: " + current.clip.displayName);
    //}

    [System.Serializable]
    public class MotionAndPose
    {
        public MotionItem motion;
        public PoseScript pose;
        public TimelineClip clip;

        //private double time;
        //private double prev_time = -1;
        //private double duration;

        public bool IsEnterTime(double time, double prev_time)
        {
            return time > clip.start && prev_time <= clip.start;
        }
        public bool IsExitTime(double time, double prev_time)
        {
            return time > clip.end && prev_time <= clip.end;
        }

        //public void Update(double current_time)
        //{
        //    duration = clip.duration;
        //    time = current_time - clip.start;
        //    //Debug.Log(clip.displayName + ": " + time.ToString("N2") + " " + current_time.ToString("N2") + " " + clip.start.ToString("N2"));
        //    for (int i = 0; i < motion.Motions.Count; i++)
        //    {
        //        motion.Motions[i].time = time;
        //        if (motion.Motions[i].IsEnterEvent(time, prev_time, duration))
        //        {
        //            // need to instance motion event
        //            FxMotionManager.CreateFX(motion.Motions[i], duration);
        //        }

        //        if (motion.Motions[i].IsExitEvent(time, prev_time, duration))
        //        {
        //            FxMotionManager.RemoveFx(motion.Motions[i]);
        //        }
        //    }

        //    prev_time = time;
        //}

        //public void OnRemove()
        //{
        //    // try to call on exit.
        //    FxMotionManager.RemoveAllFxFrom(motion);
        //}
    }
    #endregion
}
