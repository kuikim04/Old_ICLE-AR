using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextThaiFontFixer : MonoBehaviour
{
    void Start()
    {
        var label = GetComponent<Text>();
        label.text = label.text.AdjustThaiFont();
    }

}
