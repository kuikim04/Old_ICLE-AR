using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

/// <summary>
/// at song obj
/// </summary>
public class PoseValueScript : MonoBehaviour
{
    public SkeletonPose baseModel;
    public SkeletonPose userModel;
    private PlayableDirector director;

    public float Rate = 90; // percent
    public float minDot = 0.94f, maxDot = 1f;
    public float minValue = 0f, maxValue = 100f;
    private float failed_time = 0;

    public PoseDetectionScript.BodyPartScore current;
    public PoseDetectionScript.BodyPartScore myScore;
    public ParticleBodyScript particle;

    public UnityEvent OnNext;
    public PracticeModeUI ui;
    private bool isLost;


    private void Start()
    {
        isLost = false;
        director = GetComponent<PlayableDirector>();

        //myScore = new PoseDetectionScript.BodyPartScore();
        myScore = new PoseDetectionScript.BodyPartScore();
        myScore.grades = new List<PoseDetectionScript.ScoreGrade>();

        userModel = PromptToSelectSong.Player;
        if (particle)
            particle.SetParticle(false);
    }
    
    IEnumerator run_again()
    {
        var value = 0f;
        while (value < 1f)
        {
            value += Time.deltaTime;
            director.SetPlaySpeed(value);
            yield return null;
        }

        director.SetPlaySpeed(1f);
    }

    private void Update()
    {
        if (userModel)
        {
            current = baseModel.CompareOther(userModel);
            current.Map(minDot, maxDot, minValue, maxValue);
        }
        else
        {
            OnLostUser();
            return;
        }

        // wait for correct move
        if (director.IsRunning() == false)
        {
            failed_time += Time.deltaTime;

            if (failed_time > 15)
            {
                failed_time = 0;
                Failed();
            }
            else if (current.GetAvgPart > Rate)
            {
                Good(failed_time / 15f);
                failed_time = 0f;
            }

            
        }

        if (particle)
        {
            particle.DoUpdate(current);
        }
    }

    /// <summary>
    /// rating = 0f-1f
    /// </summary>
    /// <param name="rating"></param>
    void Good(float rating)
    {
        particle.SetParticle(false);
        particle.Emit();

        director.SetPlaySpeed(1); // continue
                                  //StartCoroutine(run_again());
        OnNext.Invoke();
           
        var add = 0;
        if (rating >= 1f)
            add = 25;
        else if (rating >= 0.75f)
            add = 50;
        else if (rating >= 0.5f)
            add = 75;
        else if (rating >= 0.25f)
            add = 90;
        else
            add = 100;

        ui.source.PlayOneShot(ui.clip, Random.Range(0.75f, 1f));
        ui.score.AddScore(add);

        myScore.Add(current);
    }

    void Failed()
    {
        particle.SetParticle(false);

        director.SetPlaySpeed(1); // continue
                                  //StartCoroutine(run_again());
        OnNext.Invoke();
        
        ui.score.AddScore(10);
        ui.source.PlayOneShot(ui.failedClip, Random.Range(0.75f, 1f));

        myScore.Add(current);
    }

    void OnLostUser()
    {
        // only do one time pre session.
        if (isLost)
            return;

        isLost = true;
        // back to select song
        var select_ui = MyScreen.singleton.GetScreen("SELECTSONG");
        var practice_ui = MyScreen.singleton.GetScreen("PRACTICE");
        var script = practice_ui.GetComponent<PracticeModeUI>();

        if (script.resultUI.activeSelf)
        {
            // if already result
            // do nothing.
        }
        else
        {
            // back to select song.
            select_ui.gameObject.SetActive(true);
            practice_ui.gameObject.SetActive(false);
        }
    }
}
