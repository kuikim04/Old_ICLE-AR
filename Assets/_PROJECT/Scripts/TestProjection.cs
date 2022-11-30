using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TestProjection : MonoBehaviour
{
    public SkeletonPose basePose; // should use the game model.
    public SkeletonPose userPose;
    public float value;
    public Text show;
    public float error = 0.03f;
    private SkeletonPoseRecord recorded;

    IEnumerator Start()
    {
        yield return null;
        recorded = new SkeletonPoseRecord(basePose);
    }

    public void WritePoseFile()
    {
        var save_motion = new SkeletonPoseRecord(userPose);
        var json = JsonUtility.ToJson(save_motion);
        File.WriteAllText("E://play_prompt.mot", json);
        Debug.Log("DONE");
    }

    void Update()
    {
        if (userPose != null)
        {
            value = userPose.CompareRecord(recorded).avg;
            //value = basePose.Compare(userPose);
            show.text = value.ToString("N4");
            if (value > 1f - error)
                show.text += "\nNICE";
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            WritePoseFile();
        }
    }

    public void OnUserPoseCreated(GameObject user)
    {
        userPose = user.GetComponent<SkeletonPose>();
    }
}
