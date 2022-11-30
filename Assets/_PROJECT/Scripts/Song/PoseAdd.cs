using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseAdd : MonoBehaviour
{
    PoseDetectionScript manager;
    SkeletonPoseRecord poseRecord;

    public void OnEnable()
    {
        if (manager == null)
            manager = GetComponentInParent<PoseDetectionScript>();
        if (manager.list == null)
            manager.list = new List<SkeletonPoseRecord>();

        poseRecord = new SkeletonPoseRecord(manager.baseModel);
        manager.list.Add(poseRecord);
        manager.PoseCount++;
    }

    public void OnDisable()
    {
        if (manager == null)
            manager = GetComponentInParent<PoseDetectionScript>();
        if (manager.list == null)
            manager.list = new List<SkeletonPoseRecord>();

        if (manager.list.Remove(poseRecord))
        {
            manager.MyScore.Add(PoseDetectionScript.BodyPartScore.CreateFailed());
            manager.MyScore.grades.Add(new PoseDetectionScript.ScoreGrade() { value = 0, score = PoseDetectionScript.ScoreEnum.Miss });
            //FxMotionManager.CreateFailedFx();
            FxMotionManager.CreateFx(PoseDetectionScript.ScoreEnum.Miss);
        }
    }
}
