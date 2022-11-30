using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetResolution : MonoBehaviour
{

    public int Index;
    public Profile[] Profiles;
    [System.Serializable]
    public class Profile
    {
        public int X;
        public int Y;
        public bool FullScreen;

        public void Set()
        {
            Screen.SetResolution(X, Y, FullScreen);
            Screen.fullScreen = FullScreen;
        }
    }
   
    void Start()
    {
        Profiles[Index].Set();
    }
}
