using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonPose : MonoBehaviour
{
    public static new Camera camera;
    public Body body;
    public IsCompare compare;

    public List<CompareEntry> list;

    void Start()
    {
        if (camera == null)
            camera = Camera.main;

        list = new List<CompareEntry>();
        list.Add(new CompareEntry() { name = "left_hand", IsCompare = compare.LeftHand, origin = body.center, target = body.left_elbow });
        list.Add(new CompareEntry() { name = "right_hand", IsCompare = compare.RightHand, origin = body.center, target = body.right_elbow });
        list.Add(new CompareEntry() { name = "left_hand", IsCompare = compare.LeftHand, origin = body.center, target = body.left_hand });
        list.Add(new CompareEntry() { name = "right_hand", IsCompare = compare.RightHand, origin = body.center, target = body.right_hand });

        list.Add(new CompareEntry() { name = "left_foot", IsCompare = compare.LeftFoot, origin = body.hip, target = body.left_knee });
        list.Add(new CompareEntry() { name = "right_foot", IsCompare = compare.RightFoot, origin = body.hip, target = body.right_knee });
        list.Add(new CompareEntry() { name = "left_foot", IsCompare = compare.LeftFoot, origin = body.hip, target = body.left_foot });
        list.Add(new CompareEntry() { name = "right_foot", IsCompare = compare.RightFoot, origin = body.hip, target = body.right_foot });
    }

    void Update()
    {

    }
    
    public float Compare(SkeletonPose other)
    {
        var avg_value = 0f;
        var count = 0f;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].IsCompare)
            {
                var dot_value = Vector3.Dot(list[i].Direction, other.list[i].Direction);
                list[i].dotValue = dot_value;
                other.list[i].dotValue = dot_value;
                avg_value += dot_value;
                count++;
            }
        }

        avg_value /= count;

        return avg_value;
        // project bone position to plane space
        // get dot value from direction center-left_elbow, left_elbow-left_hand
        // compare each value
    }

    /// <summary>
    /// only compare dot value !!! don't user avg
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public PoseDetectionScript.BodyPartScore CompareOther(SkeletonPose other)
    {
        var body_score = new PoseDetectionScript.BodyPartScore();
        
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].IsCompare)
            {
                var dot_value = Vector3.Dot(list[i].Direction, other.list[i].Direction);
                list[i].dotValue = dot_value;

                if (list[i].name == "left_hand")
                {
                    body_score.left_hand = dot_value;
                    body_score.left_hand_count = 1;
                }
                else if (list[i].name == "right_hand")
                {
                    body_score.right_hand = dot_value;
                    body_score.right_hand_count = 1;
                }
                else if (list[i].name == "left_foot")
                {
                    body_score.left_foot = dot_value;
                    body_score.left_foot_count = 1;
                }
                else if (list[i].name == "right_foot")
                {
                    body_score.right_foot = dot_value;
                    body_score.right_foot_count = 1;
                }
            }
        }

        return body_score;
    }

    public PoseDetectionScript.BodyPartScore CompareRecord(SkeletonPoseRecord recorded)
    {
        var body_score = new PoseDetectionScript.BodyPartScore();
        //var avg_value = 0f;
        //var count = 0f;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].IsCompare)
            {
                var dot_value = Vector3.Dot(list[i].Direction, recorded.recorded_directions[i]);
                list[i].dotValue = dot_value;
                body_score.score += dot_value;
                body_score.count++;

                if (list[i].name == "left_hand")
                {
                    body_score.left_hand += dot_value;
                    body_score.left_hand_count++;
                } 
                else if (list[i].name == "right_hand")
                {
                    body_score.right_hand += dot_value;
                    body_score.right_hand_count++;
                }
                else if (list[i].name == "left_foot")
                {
                    body_score.left_foot += dot_value;
                    body_score.left_foot_count++;
                }
                else if (list[i].name == "right_foot")
                {
                    body_score.right_foot += dot_value;
                    body_score.right_foot_count++;
                }
            }
        }

        //body_score.avg = body_score.score / body_score.count;

        return body_score;
    }

    [System.Serializable]
    public class Body
    {
        public Transform center, left_elbow, left_hand, right_elbow, right_hand;
        public Transform hip, left_knee, left_foot, right_knee, right_foot;
    }
    [System.Serializable]
    public class IsCompare
    {
        public bool LeftHand, RightHand, LeftFoot, RightFoot;
    }
}

[System.Serializable]
public class CompareEntry
{
    public bool IsCompare;
    public string name;
    public Transform origin, target;
    public float dotValue;

    private Vector3 planed_origin, planed_target;
    private float cached_time;
    private Vector3 cached_dir;
    private void ProjectPositionToPlane()
    {
        planed_origin = SkeletonPose.camera.WorldToScreenPoint(origin.position);
        planed_target = SkeletonPose.camera.WorldToScreenPoint(target.position);
        cached_time = Time.time;
    }

    public Vector3 Direction
    {
        get
        {
            if (Time.time > cached_time)
            {
                ProjectPositionToPlane();
                cached_dir = (planed_target - planed_origin).normalized;
            }
            return cached_dir;
        }
    }

    public Vector3 recorded_direction;
}

[System.Serializable]
public class SkeletonPoseRecord
{
    public List<Vector3> recorded_directions;
    public float time;

    public SkeletonPoseRecord()
    {

    }

    public SkeletonPoseRecord(SkeletonPose skel)
    {
        time = Time.time;
        recorded_directions = new List<Vector3>();
        for (int i = 0; i < skel.list.Count; i++)
        {
            recorded_directions.Add(skel.list[i].Direction);
        }
    }
}
