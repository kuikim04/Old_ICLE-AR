using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PoseUI : MonoBehaviour
{
    public Transform locationRoot;
    public Transform poseRoot;
    private List<Transform> p;
    public PoseScript prefab;

    private float alpha_1 = 1f, alpha_2 = 0.3f, alpha_3 = 0.1f, alpha_4 = 0f;
    //private List<PoseScript> list;

    /// <summary>
    /// should get motion from GameplayUIScript that instance song obj
    /// </summary>
    [HideInInspector]
    public MotionInfoScript motion;

    public void OnStart(MotionInfoScript song)
    {
        motion = song;

        motion.Init();
        Init(motion.queue);
        motion.OnEnter.AddListener(MoveToFirst);
        motion.OnExit.AddListener(LeaveFirst);
    }

    void Init(List<MotionInfoScript.MotionAndPose> motions)
    {
        p = new List<Transform>();
        foreach (Transform item in locationRoot)
            p.Add(item);

        // create all the pose
        // then at to root
        //list = new List<PoseScript>();
        for (int i = 0; i < motions.Count; i++)
        {
            var go = GameObject.Instantiate<PoseScript>(prefab);
            go.transform.SetParent(poseRoot, false);
            //list.Add(go);
            motions[i].pose = go;
            motions[i].pose.Set(i + 1, motions[i].motion.GetName, motions[i].motion.Image);
            go.transform.position = p[3].position;
            go.transform.localScale = p[3].localScale;
        }
        MoveToStartSong();
    }

    void MoveToStartSong()
    {
        if (motion.queue.Count > 0)
        {
            iTween.Stop(motion.queue[0].pose.gameObject);
            iTween.MoveTo(motion.queue[0].pose.gameObject, p[1].position, 0.5f);
            iTween.ScaleTo(motion.queue[0].pose.gameObject, p[1].localScale, 0.5f);
            motion.queue[0].pose.canvas.alpha = alpha_1;
        }

        if (motion.queue.Count > 1)
        {
            iTween.Stop(motion.queue[1].pose.gameObject);
            iTween.MoveTo(motion.queue[1].pose.gameObject, p[2].position, 0.5f);
            iTween.ScaleTo(motion.queue[1].pose.gameObject, p[2].localScale, 0.5f);
            motion.queue[1].pose.canvas.alpha = alpha_2;
        }
    }

    void MoveToFirst()
    {
        if (motion.queue.Count > 0)
        {
            iTween.Stop(motion.queue[0].pose.gameObject);
            iTween.MoveTo(motion.queue[0].pose.gameObject, p[0].position, 0.5f);
            iTween.ScaleTo(motion.queue[0].pose.gameObject, p[0].localScale, 0.5f);
            motion.queue[0].pose.canvas.alpha = alpha_1;
        }

        if (motion.queue.Count > 1)
        {
            iTween.Stop(motion.queue[1].pose.gameObject);
            iTween.MoveTo(motion.queue[1].pose.gameObject, p[1].position, 0.5f);
            iTween.ScaleTo(motion.queue[1].pose.gameObject, p[1].localScale, 0.5f);
            motion.queue[1].pose.canvas.alpha = alpha_2;
        }

        if (motion.queue.Count > 2)
        {
            iTween.Stop(motion.queue[2].pose.gameObject);
            iTween.MoveTo(motion.queue[2].pose.gameObject, p[2].position, 0.5f);
            iTween.ScaleTo(motion.queue[2].pose.gameObject, p[2].localScale, 0.5f);
            motion.queue[2].pose.canvas.alpha = alpha_3;
        }

        if (motion.queue.Count > 3)
            for (int i = 3; i < motion.queue.Count; i++)
            {
                motion.queue[i].pose.transform.position = p[3].position;
                motion.queue[i].pose.canvas.alpha = alpha_4;
            }

    }
    
    void LeaveFirst()
    {
        iTween.Stop(motion.queue[0].pose.gameObject);
        iTween.MoveTo(motion.queue[0].pose.gameObject, p[4].position, 0.5f);
        iTween.ScaleTo(motion.queue[0].pose.gameObject, p[4].localScale, 0.5f);
        motion.queue[0].pose.canvas.alpha = alpha_2;
        //motion.queue.RemoveAt(0);
    }
}
