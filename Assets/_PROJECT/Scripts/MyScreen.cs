using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class MyScreen : MonoBehaviour
{
    public static MyScreen singleton;
    public List<ScreenTag> screens;

    void Awake()
    {
        singleton = this;
    }

    public ScreenTag GetScreen(string tag)
    {
        return screens.Find(x => x.Name == tag);
    }

}
