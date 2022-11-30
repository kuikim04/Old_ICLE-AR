using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// should spawn at hand or foot location and shouldn't move
/// </summary>
public class FXMotionEvent : MonoBehaviour
{
    //public Sprite[] results;
    public Image result;
    public AnimationCurve scale;
    public AnimationCurve alpha;
    public float life = 1.5f;
    public float multiplyScale = 3f;

    private float _time;
    private Color _color = new Color(1f, 1f, 1f, 1f);

    //IEnumerator Start()
    //{
    //    iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.one, "time", fx_time, "easetype", iTween.EaseType.easeOutBack));
    //    iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.one * 1.5f, "delay", fx_time + view_time, "time", fx_time));
    //    iTween.ValueTo(gameObject, iTween.Hash("from", 1f, "to", 0f, "delay", fx_time + view_time, "time", fx_time, "onupdate", "fadeout"));

    //    yield return new WaitForSeconds(fx_time + fx_time + view_time + 0.5f);

    //    GameObject.Destroy(gameObject);
    //}

    private void OnEnable()
    {
        //result.sprite = results[0]; // result image.
        // show result root
        //gameObject.transform.localScale = Vector3.zero;
        _color = Color.white;
        _time = 0;
        UpdateFX(_time);

        GameObject.Destroy(gameObject, life + 0.2f);
    }

    private void Update()
    {
        _time += Time.deltaTime;
        UpdateFX(_time / life);
    }

    void UpdateFX(float time)
    {
        transform.localScale = Vector3.one * scale.Evaluate(time) * multiplyScale;
        _color.a = alpha.Evaluate(time);
        result.color = _color;
    }
}
