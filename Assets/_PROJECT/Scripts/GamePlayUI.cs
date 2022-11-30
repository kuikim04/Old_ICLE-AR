using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Audio;

public class GamePlayUI : MonoBehaviour
{
    public Text score;
    public Text hiScore;
    public Text songName;
    public Image songNameImage;

    public static MotionInfoScript song;

    public UnityEvent OnResult;
    public AudioMixerSnapshot gameplayShot;

    void Start()
    {

    }

    /// <summary>
    /// load song.
    /// </summary>
    private void OnEnable()
    {
        var prefab = Resources.Load<MotionInfoScript>(SelectSongUIScript.Selected.prefabName);
        song = GameObject.Instantiate<MotionInfoScript>(prefab);
        songName.text = SelectSongUIScript.Selected.GetName.AdjustThaiFont();
        var high_score = LeaderboardScript.GetHighScore(SelectSongUIScript.Selected.id);
        hiScore.text = high_score.score.ToString("N0");
        //Resources.UnloadAsset(prefab);
        songNameImage.sprite = SelectSongUIScript.Selected.songNameSprite;
        var pose_ui = GetComponent<PoseUI>();
        pose_ui.OnStart(song);
        gameplayShot.TransitionTo(1f);
    }
}
