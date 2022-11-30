using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSortingLayer : MonoBehaviour
{
    public string sortingLayer = "Background";
    public int order = 0;

    void Start()
    {
        var render = GetComponent<Renderer>();
        if (render)
        {
            render.sortingLayerName = sortingLayer;
            render.sortingOrder = order;
        }

        var canvas = GetComponent<Canvas>();
        if (canvas)
        {
            canvas.sortingLayerName = sortingLayer;
            canvas.sortingOrder = order;
        }
        
        
    }
}
