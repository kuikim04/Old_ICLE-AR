using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntryScript : MonoBehaviour
{
    public Image image;
    public Text Rank, Name, Score;

    void Start()
    {
        
    }

    public void Set(int rank, string name, int score)
    {
        gameObject.transform.localScale = Vector3.zero;

        Rank.text = "อันดับที่ " + rank;
        Name.text = name;
        Score.text = score.ToString("N0");
        SetRank(rank);

        iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.one, "time", 0.5f, "delay", rank * 0.1f));
    }

    public void Set(int rank, LeaderboardScript.highscore_entry data)
    {
        Set(rank, data.name, data.score);
    }

    public void SetRank(int rank)
    {
        var rect = transform as RectTransform;
        var size = rect.sizeDelta;

        if (rank == 1)
        {
            
            size.y = 115;
            rect.sizeDelta = size;
        }
        else
        {
            size.y = 100;
            rect.sizeDelta = size;
        }
    }
}
