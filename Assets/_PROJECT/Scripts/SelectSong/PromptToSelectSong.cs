using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PromptToSelectSong : MonoBehaviour
{
    public TextAsset GoPose;
    public List<SkeletonPose> users;
    public static SkeletonPose Player;

    public UnityEvent OnPrompt;

    private SkeletonPoseRecord goPose;
    public bool IsSelected;
    private float delay = 2;
    private float current_delay = 0;
    /// <summary>
    /// because current song is locked.
    /// </summary>
    public void Reset()
    {
        IsSelected = false;
        Player = null;
    }

    // Use this for initialization
    void Start()
    {
        goPose = JsonUtility.FromJson<SkeletonPoseRecord>(GoPose.text);
    }

    private void OnEnable()
    {
        IsSelected = false;
        current_delay = delay; 
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSelected)
            return;

        if (current_delay > 0)
        {
            current_delay -= Time.deltaTime;
            return;
        }

        for (int i = 0; i < users.Count; i++)
        {
            if (users[i] == null)
            {
                users.RemoveAt(i--);
                continue;
            }

            if (users[i].CompareRecord(goPose).avg > 0.96f)
            {
                Player = users[i];
                OnPrompt.Invoke();
                //IsSelected = true;
                return;
            }
        }
    }

    public void NewUser(GameObject go)
    {
        users.Add(go.GetComponent<SkeletonPose>());
    }

    public void SetPlayerToSong()
    {
        // find song
        var detection = MotionInfoScript.singleton.GetComponent<PoseDetectionScript>();
        detection.userModel = PromptToSelectSong.Player;
    }
}
