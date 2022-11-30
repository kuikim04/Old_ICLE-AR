using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Audio;

public class PracticeModeUI : MonoBehaviour {

    public References references;
    [HideInInspector]
    public PoseValueScript pose;
    public AudioMixerSnapshot gameplayShot;
    public UnityEvent OnEndSession;
    public AudioSource source;
    public AudioClip clip;
    public AudioClip failedClip;

    public GameObject mainUI;
    public GameObject resultUI;

    public GameScore score
    {
        get
        {
            if (_score == null)
                _score = GetComponent<GameScore>();
            return _score;
        }
    }

    private GameScore _score;
    private void OnEnable()
    {
        resultUI = GetComponentInChildren<PracticeResultUI>(true).gameObject;
        resultUI.SetActive(false);
        mainUI.SetActive(true);
        // try to instance practice
        var prefab = Resources.Load<GameObject>(SelectSongUIScript.Selected.prefabName);
        var go = GameObject.Instantiate<GameObject>(prefab);
        references.MoveName.text = SelectSongUIScript.Selected.GetName.AdjustThaiFont();
        references.MoveSprite.sprite = SelectSongUIScript.Selected.songNameSprite;

        pose = go.GetComponent<PoseValueScript>();
        pose.ui = this;
        pose.OnNext.RemoveAllListeners();
        //pose.OnNext.AddListener(() =>
        //{
        //    source.PlayOneShot(clip, Random.Range(0.75f, 1f));
        //});
        gameplayShot.TransitionTo(1f);

        references.Init();
        
        var high_score = LeaderboardScript.GetHighScore(SelectSongUIScript.Selected.id);
        score.SetHighScore(high_score.score);
    }

    private void OnDisable()
    {
        GameObject.Destroy(pose.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        references.Update(pose.current);
    }

    [System.Serializable]
    public class References
    {
        //private float minDot = 0.94f, maxDot = 1f;
        //private float minValue = 0f, maxValue = 100f;

        public ScoreRef leftHand, rightHand, leftFoot, rightFoot;

        public Text MoveName;
        public Image MoveSprite;

        public void Init()
        {
            leftHand.UpdateShow();
            rightHand.UpdateShow();
            leftFoot.UpdateShow();
            rightFoot.UpdateShow();
        }

        public void Update(PoseDetectionScript.BodyPartScore value)
        {
            leftHand.SetValue(value.mapped_left_hand);
            rightHand.SetValue(value.mapped_right_hand);
            leftFoot.SetValue(value.mapped_left_foot);
            rightFoot.SetValue(value.mapped_right_foot);
        }
    }

    [System.Serializable]
    public class ScoreRef
    {
        public bool show;
        public GameObject root;
        public Text value;
        public Image fill;
        public Gradient gradient;

        public void UpdateShow()
        {
            root.SetActive(show);
            fill.fillAmount = 1f;
        }
        
        public void SetValue(float value)
        {
            this.value.text = value.ToString("N0");
            //fill.fillAmount = value / 100f;
            fill.color = gradient.Evaluate(value / 100f);
        }
    }
}


