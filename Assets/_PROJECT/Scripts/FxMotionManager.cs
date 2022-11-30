using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxMotionManager : MonoBehaviour
{
    //public RectTransform canvas;
    public static FxMotionManager singleton;

    public Canvas canvas;
    public Transform root;
    public Camera cam;
    public float RandomRange = 5f;
    /// <summary>
    /// 0=perfect, 1=great, 2=miss
    /// </summary>
    //public List<Sprite> grades;
    public List<MyGrade> grades;
    [System.Serializable]
    public class MyGrade
    {
        public Sprite sprite;
        public AudioClip clip;
        public float scale;
    }

    // prefab of fx (hold, follow)
    // model info
    public FXMotionEvent prefabFx;
    //public FXMotionEvent failed;
    private new AudioSource audio;
    //public AudioClip[] clips;

    public Vector2 volumeRange = new Vector2(0.75f, 1f);
    public float GetRandomVolume()
    {
        return Random.Range(volumeRange.x, volumeRange.y);
    }
    
    //public List<FXMotionEvent> list;
    public Dictionary<string, FXMotionEvent> dict;
    
    void Start()
    {
        singleton = this;
        audio = GetComponent<AudioSource>();
        dict = new Dictionary<string, FXMotionEvent>();
    }

    static Vector3 RandomInsideUnitCircle()
    {
        var rand = Random.insideUnitCircle;
        return new Vector3(rand.x, rand.y, 0);
    }

    /// <summary>
    /// 0=perfect, 1=great(ok), 2=miss
    /// </summary>
    /// <param name="score"></param>
    public static void CreateFx(PoseDetectionScript.ScoreEnum score)
    {
        var index = (int)score;

        var go = GameObject.Instantiate<FXMotionEvent>(singleton.prefabFx);
        go.transform.SetParent(singleton.root, false);
        go.transform.SetAsLastSibling();
        go.transform.position += RandomInsideUnitCircle() * singleton.RandomRange;
        go.result.sprite = singleton.grades[index].sprite;
        go.multiplyScale = singleton.grades[index].scale;
        go.gameObject.SetActive(true);

        if (index < singleton.grades.Count) // 0-2 < (2)
        {
            if (singleton.grades[index].clip)
                singleton.audio.PlayOneShot(singleton.grades[index].clip, singleton.GetRandomVolume());
        }

        Debug.Log(Time.time.ToString("N2") + " Get " + score);
    }
    
}
