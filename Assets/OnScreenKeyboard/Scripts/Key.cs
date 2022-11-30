using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{

    public int id;
    public string value;

    public Text text
    {
        get
        {
            if (_text == null)
                _text = GetComponentInChildren<Text>();
            return _text;
        }
    }
    private Text _text;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Click);
    }

    public void OnShift(bool is_shift)
    {
        text.text = is_shift ? value.ToUpper() : value.ToLower();
    }

    public void Click()
    {
        OnScreenKeyboard.Add(value);
    }
}
