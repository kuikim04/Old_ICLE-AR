using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInfo : MonoBehaviour {

    public bool isLock;
    //[HideInInspector]
    public int order;
    public int id;
    public Sprite sprite;
    public Sprite songNameSprite;
    public string songName;

    public string prefabName;

    public string GetName
    {
        get { return songName.Replace("<br>", "\n"); }
    }
}
