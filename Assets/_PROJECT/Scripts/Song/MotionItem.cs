using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MotionItem : MonoBehaviour
{
    [InlineButton("RenameFunc", "Rename")]
    public AnimationClip clip;
    public string Name;
    public Sprite Image;

    public string GetName
    {
        get { return Name.Replace("<br>", "\n"); }
    }

    //public List<MotionData> Motions;

    //[ContextMenu("Length")]
    //void Test()
    //{
    //    Debug.Log(clip.length);
    //    foreach (var item in Motions)
    //    {
    //        item.duration = clip.length;
    //    }
    //}

    public void RenameFunc()
    {
        name = "Motion: " + Name;
    }

    //[System.Serializable]
    //public class MotionData
    //{
    //    public string key
    //    {
    //        get { return timing.x.ToString("N2") + timing.y.ToString("N2") + Part + Motion; }
    //    }
    //    public double time;

    //    public BodyPart Part;
    //    public MotionType Motion;
    //    //[MinMaxSlider(0f, 1f, true)]
    //    public Vector2 timing; // percent base value
    //    public double startTime { get { return timing.x; } }
    //    public double endTime { get { return timing.y; } }
    //    //public float duration;

    //    public bool IsEnterEvent(double time, double prev_time, double duration)
    //    {
    //        return time > startTime * duration && prev_time <= startTime * duration;
    //    }

    //    public bool IsExitEvent(double time, double prev_time, double duration)
    //    {
    //        return time > endTime * duration && prev_time <= endTime * duration;
    //    }
    //}

    //public enum BodyPart
    //{
    //    L_Hand, R_Hand, L_Foot, R_Foot
    //}

    //public enum MotionType
    //{
    //    Hold, Follow
    //}
}
