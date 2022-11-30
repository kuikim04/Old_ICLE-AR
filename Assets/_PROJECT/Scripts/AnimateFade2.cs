using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateFade2 : MonoBehaviour
{
    public Image image;
    private Color _color;
    public AnimationCurve curve;
    private float _time;

    void Start()
    {

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
