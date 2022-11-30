using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PracticeResultUI : MonoBehaviour
{
    public PracticeModeUI ui;
    public Text score;

    private int NewScore;
    private int SongID;

    public ResultUI.GradeText[] grades;

    public UnityEvent OnSpacebar;
    private float _delay;

    // Use this for initialization
    void OnEnable()
    {
        NewScore = ui.score.value;
        SongID = SelectSongUIScript.Selected.id;
        score.text = NewScore.ToString("N0");

        var x1 = 0.92f;
        grades[0].Set(ui.pose.myScore.left_hand_avg, x1);
        grades[1].Set(ui.pose.myScore.right_hand_avg, x1);
        grades[2].Set(ui.pose.myScore.left_foot_avg, x1);
        grades[3].Set(ui.pose.myScore.right_foot_avg, x1);

        // try to save highscore
        UpdateHighScore();

        _delay = Time.time + 2f;
    }

    private void Update()
    {
        if (Time.time < _delay)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpacebar.Invoke();
            gameObject.SetActive(false);
        }
    }
    
    void UpdateHighScore()
    {
        var high_score = LeaderboardScript.GetHighScore(SongID, 9);
        var is_new_high_score = NewScore > high_score.score;

        if (is_new_high_score)
        {
            LeaderboardScript.highscore_json aa = new LeaderboardScript.highscore_json(SongID);
            aa.Add("practice", NewScore);
            aa.WriteFile();
        }
    }
}
