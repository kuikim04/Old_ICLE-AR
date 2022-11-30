using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class OnScreenKeyboard : MonoBehaviour
{
    public static OnScreenKeyboard singleton
    {
        get
        {
            if (_singleton == null)
            {
                var prefab = Resources.Load<OnScreenKeyboard>("OSK");
                _singleton = GameObject.Instantiate<OnScreenKeyboard>(prefab);
            }
            return _singleton;
        }
    }
    private static OnScreenKeyboard _singleton;
    public string Text;
    public InputField Field;

    public bool IsShift;
    public bool IsCaplock;
    public bool IsUpperCase
    {
        get
        {
            if (IsShift && IsCaplock)
                return false;
            else if (IsShift && !IsCaplock)
                return true;
            else if (!IsShift && IsCaplock)
                return true;
            else if (!IsShift && !IsCaplock)
                return false;
            else
                return false;
        }
    }
    public delegate void MyEvent(bool is_shift);
    public event MyEvent OnShift;

    public UnityEvent OnClickEnter;

    void Awake()
    {
        _singleton = this;
    }

    private void OnEnable()
    {
        IsShift = true;
        IsCaplock = false;

        var keys = GameObject.FindObjectsOfType<Key>();
        foreach (var item in keys)
        {
            OnShift -= item.OnShift;
            OnShift += item.OnShift;
        }

        OnShift.Invoke(IsUpperCase);
    }

    public static void Add(string key)
    {
        singleton.Text += singleton.IsShift ? key.ToUpper() : key.ToLower();
        singleton.UpdateInputField();
    }

    void UpdateInputField()
    {
        if (Field)
        {
            Field.text = Text;
            Field.onValueChanged.Invoke(Text);
        }
    }

    // call by shift button
    public void ToggleShift()
    {
        IsShift = !IsShift;
        if (OnShift != null)
            OnShift.Invoke(IsUpperCase);
    }

    // call by caplock button
    public void ToggleCaplock()
    {
        IsCaplock = !IsCaplock;
        if (OnShift != null)
            OnShift.Invoke(IsUpperCase);
    }

    public void ClickEnter()
    {
        // done
        OnClickEnter.Invoke();
    }

    public void ClickBackspace()
    {
        if (Text.Length > 0)
        {
            Text = Text.Remove(Text.Length - 1);
            UpdateInputField();
        }
    }

    #region Tools
    public List<Transform> lines;

    [ContextMenu("Set Key ID")]
    void SetAutoKeyID()
    {
        var index = 0;
        // line 4 > 3 > 2 > 1
        foreach (var l in lines)
        {
            foreach (Transform k in l)
            {
                var key = k.GetComponent<Key>();
                if (key)
                {
                    key.id = index;
                    index++;
                }
            }
        }
        // get keys in each line
        // start at index = 0
    }

    public KeyProfile profile;
    [ContextMenu("Set Profile")]
    void ApplyProfile()
    {
        var dict = profile.GetDict();
        var keys = GameObject.FindObjectsOfType<Key>();
        foreach (var item in keys)
        {
            if (dict.ContainsKey(item.id))
            {
                item.value = dict[item.id].lowercase;
                item.name = "key: " + item.value;
                item.text.text = item.value;
            }
            else
                Debug.LogError("don't have key: " + item.id);
        }
    }

    #endregion

}
