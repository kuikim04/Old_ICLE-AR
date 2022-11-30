using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoseScript : MonoBehaviour
{
    public CanvasGroup canvas;
    public Text Order;
    public Text Name;
    public Image Pose;

    void Start()
    {

    }

    /// <summary>
    /// order should start at 1
    /// </summary>
    /// <param name="order"></param>
    /// <param name="name"></param>
    /// <param name="pose"></param>
    public void Set(int order, string name, Sprite pose)
    {
        Order.text = "ท่า " + order;
        Name.text = name;
        Pose.sprite = pose;
        canvas.alpha = 0.5f;
    }
}
