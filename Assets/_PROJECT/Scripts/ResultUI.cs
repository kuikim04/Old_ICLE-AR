using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    public static ResultUI singleton;

    public Text score;
    public Text percent;

    public Text newScore;
    public InputField usernameInput;

    private int NewScore;
    private int LastScore;
    private int SongID;
    
    public GradeText[] grades;
    [System.Serializable]
    public class GradeText
    {
        public bool show;
        public Text text;
        public GameObject root;

        public void Set(float avg_value, float x1)
        {
            root.SetActive(show);
            text.text = to_percent(avg_value * 100f);
            //text.text = to_percent(mapping_value(avg_value, x1: x1));
        }
    }

    public AudioClip[] clips;

    // Use this for initialization
    void OnEnable()
    {
        singleton = this;
        // looking for song to get score
        var detection = GamePlayUI.song.GetComponent<PoseDetectionScript>();
        NewScore = detection.MyScore.GetScore();
        SongID = SelectSongUIScript.Selected.id;
        score.text = NewScore.ToString("N0");

        percent.text = "TOTAL " + detection.OnResult().ToString("N0") + "%";
        //leftHand.text = "95%";
        // 0.4 > 0.96 === 0.6 > 0.94
        var x1 = 1f - detection.error * 2f;
        grades[0].Set(detection.MyScore.left_hand_avg, x1);
        grades[1].Set(detection.MyScore.right_hand_avg, x1);
        grades[2].Set(detection.MyScore.left_foot_avg, x1);
        grades[3].Set(detection.MyScore.right_foot_avg, x1);

        // wait for input to check high score
        CheckHighScore();

        var rand = Random.Range(0, clips.Length);
        GetComponent<AudioSource>().PlayOneShot(clips[rand]);
    }

    static float mapping_value(float x0, float y1 = 0f, float y2 = 100f, float x1 = 0.97f, float x2 = 1f)
    {
        //var y1 = 50f; // output start
        //var y2 = 100f; // output end
        //var x1 = 0.97f; // input start
        //var x2 = 1f; // input end

        x0 = Mathf.Clamp(x0, x1, x2);

        return y1 + ((y2 - y1) / (x2 - x1)) * (x0 - x1);
    }

    static string to_percent(float value)
    {
        return value.ToString("N0") + "%";
    }

    public void CheckHighScore()
    {
        var high_score = LeaderboardScript.GetHighScore(SongID, 9);
        LastScore = high_score.score;
        var is_new_high_score = NewScore > LastScore;
        
        if (is_new_high_score)
        {
            newScore.text = NewScore.ToString("N0");
            var fsm = GetComponent<PlayMakerFSM>();
            fsm.SendEvent("HIGH");
        }
        else
        {
            var fsm = GetComponent<PlayMakerFSM>();
            fsm.SendEvent("NO");
        }
        // wait for input to save high score
    }

    public void SaveHighScore()
    {
        Debug.Log("move this phase to LeaderboardScript.cs");
        //PlayerPrefs.SetInt("song" + SongID, NewScore);
        //PlayerPrefs.SetString("song_username" + SongID, usernameInput.text);
    }

    public void DeleteSong()
    {
        GameObject.Destroy(GamePlayUI.song.gameObject);
    }
}
