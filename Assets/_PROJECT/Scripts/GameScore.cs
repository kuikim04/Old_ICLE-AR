using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour
{
    public int value;
    public Text score;
    public Text highScore;

    private void OnEnable()
    {
        value = 0;
        SetScore(value);
    }

    public void AddScore(int add)
    {
        value += add;
        SetScore(value);
    }

    public void SetScore(int value)
    {
        this.value = value;
        score.text = value.ToString("N0");
    }

    public void SetHighScore(int value)
    {
        highScore.text = value.ToString("N0");

    }
}
