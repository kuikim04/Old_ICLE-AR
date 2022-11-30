using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class PoseDetectionScript : MonoBehaviour
{
    public List<SkeletonPoseRecord> list;
    public SkeletonPose baseModel;
    public SkeletonPose userModel;
    public float error = 0.03f;
    public int multiScore = 100;

    public BodyPartScore MyScore;

    [System.Serializable]
    public class BodyPartScore
    {
        //public int current_score;
        public List<ScoreGrade> grades;
        public float score;
        public int count;
        public float avg { get { return score / count; } }

        /// <summary>
        /// score from dot value( -1f - 1f)
        /// </summary>
        public float left_hand;
        public float right_hand;
        public float left_foot;
        public float right_foot;
        public float GetAvgPartWithoutCount
        {
            get
            {
                return (left_hand + right_hand + left_foot + right_foot) / 4f;
            }
        }

        public float easyArm = 0;
        public float easyLeg = 5;
        public float GetAvgPart
        {
            get
            {
                return ((mapped_left_hand + easyArm) + (mapped_right_hand + easyArm) + (mapped_left_foot + easyLeg) + (mapped_right_foot + easyLeg)) / 4f;
            }
        }

        public int left_hand_count;
        public int right_hand_count;
        public int left_foot_count;
        public int right_foot_count;

        public float mapped_left_hand;
        public float mapped_right_hand;
        public float mapped_left_foot;
        public float mapped_right_foot;

        public void Map(float min_dot, float max_dot, float min, float max)
        {
            mapped_left_hand = left_hand.Map(min_dot, max_dot, min, max);
            mapped_right_hand = right_hand.Map(min_dot, max_dot, min, max);
            mapped_left_foot = left_foot.Map(min_dot, max_dot, min, max);
            mapped_right_foot = right_foot.Map(min_dot, max_dot, min, max);
        }

        public float left_hand_avg
        {
            get
            {
                Debug.Log("left hand: " + left_hand + " / " + left_hand_count);
                return left_hand / left_hand_count;
            }
        }
        public float right_hand_avg
        {
            get
            {
                Debug.Log("right hand: " + right_hand + " / " + right_hand_count);
                return right_hand / right_hand_count;
            }
        }
        public float left_foot_avg { get { return left_foot / left_foot_count; } }
        public float right_foot_avg { get { return right_foot / right_foot_count; } }

        public void Add(BodyPartScore other)
        {
            score += other.score;
            count += other.count;

            left_hand += other.left_hand;
            right_hand += other.right_hand;
            left_foot += other.left_foot;
            right_foot += other.right_foot;

            left_hand_count += other.left_hand_count;
            right_hand_count += other.right_hand_count;
            left_foot_count += other.left_foot_count;
            right_foot_count += other.right_foot_count;
        }

        private int multi_score;
        public int GetScore(int multi_score)
        {
            this.multi_score = multi_score;
            var score = 0;
            for (int i = 0; i < grades.Count; i++)
            {
                score += (int)(grades[i].value * multi_score);
            }

            //current_score = score;
            return score;
        }

        public int GetScore()
        {
            var score = 0;
            for (int i = 0; i < grades.Count; i++)
            {
                score += (int)(grades[i].value * multi_score);
            }

            //current_score = score;
            return score;
        }

        public static BodyPartScore CreateFailed()
        {
            var fail = new BodyPartScore();
            fail.count = 1;
            fail.left_foot_count = 1;
            fail.left_hand_count = 1;
            fail.right_foot_count = 1;
            fail.right_hand_count = 1;
            return fail;
        }
    }

    //public float Score;
    public int PoseCount;
    private GamePlayUI ui;

    [System.Serializable]
    public class ScoreGrade
    {
        public float time = 0.25f;
        public float value = 1f;
        public ScoreEnum score;
    }
    public List<ScoreGrade> grades;
    public MyEvent OnScored;

    [System.Serializable]
    public class MyEvent : UnityEvent<ScoreGrade> { }

    public enum ScoreEnum : int
    {
        Perfect, Great, Miss
    }
    
    void Start()
    {
        ui = GameObject.FindObjectOfType<GamePlayUI>();
        //Score = 0f;
        MyScore = new BodyPartScore();
        MyScore.grades = new List<ScoreGrade>();

        UpdateScoreText();
        userModel = PromptToSelectSong.Player;
    }
 
    void Update()
    {
        if (userModel == null)
        {
            // pause game
            if (is_lost == false)
                OnLostPlayer();
            return;
        }
        is_lost = false;

        for (int i = 0; i < list.Count; i++)
        {
            i = DetectAndScoring(i);
        }
    }

    /// <summary>
    /// try to display current value.
    /// </summary>
    public BodyPartScore currentValue;
    int DetectAndScoring(int i)
    {
        currentValue = userModel.CompareRecord(list[i]);
        // if good
        if (currentValue.avg > 1f - error) // 1f-value=0.02f > *100=2f
        {
            var grade = Scoring(i, currentValue);

            OnScored.Invoke(grade);

            list.RemoveAt(i--);
        }

        return i;
    }

    ScoreGrade Scoring(int i, BodyPartScore value)
    {
        // calculate grade
        var reaction_time = Time.time - list[i].time;
        ScoreGrade grade = null;
        for (int j = 0; j < grades.Count; j++)
        {
            if (reaction_time < grades[i].time)
            {
                grade = grades[i];
                break;
            }
        }

        if (grade == null)
            grade = grades.Last();

        // show perfect fx
        FxMotionManager.CreateFx(grade.score);

        //var get_score = GetAPoseScore(value) * multiScore;
        var get_score = grade.value * multiScore;
        MyScore.Add(value);
        MyScore.grades.Add(grade);

        UpdateScoreText();
        //Debug.Log(value);
        //Debug.Log(Time.time.ToString("N2") + " score: " + MyScore.current_score + " value:" + value + " get score:" + get_score);

        return grade;
    }

    bool is_lost;
    void OnLostPlayer()
    {
        is_lost = true;
        var result = MyScreen.singleton.GetScreen("RESULT");
        var leaderboard = MyScreen.singleton.GetScreen("LEADERBOARD");
        var is_show_result = result.gameObject.activeSelf || leaderboard.gameObject.activeSelf;

        if (!is_show_result)
        {
            var pause = MyScreen.singleton.GetScreen("PAUSE");
            if (pause)
                pause.gameObject.SetActive(true);
        }
            
    }

    void UpdateScoreText()
    {
        if (MyScore != null && ui != null)
            ui.score.text = MyScore.GetScore((int)multiScore).ToString("N0");
    }
    
    /// <summary>
    /// try to get result
    /// </summary>
    /// <returns></returns>
    public float OnResult()
    {
        var _score = MyScore.GetScore((int)multiScore); // already *multiScore.
        var _max_score = MyScore.grades.Count * multiScore;
        var _percent = (float)_score / (float)_max_score * 100f;

        return _percent;
    }
}
