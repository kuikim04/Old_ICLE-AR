using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateFading : MonoBehaviour
{
    public Text image;
    private Color _color;
    public AnimationCurve curve;
    private float _time;

    void Start()
    {
        _color = image.color;
    }

    private void OnEnable()
    {
        _time = 0;
    }
    
    void Update()
    {
        _time += Time.deltaTime;
        _color.a = curve.Evaluate(_time);
        image.color = _color;
    }
}
