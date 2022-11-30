using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class SongItemScript : FancyScrollViewCell<BaseInfo, SelectSongScrollviewContext>
{
    //public int index;
    public Transform bg;
    public Image image;
    public Text songName;
    public Image imageSongName;
    public Text hiScore;
    public Color select, unselect;
    public Button button;
    public Image lockImage;

    public Animator animator;
    readonly int scrollHash = Animator.StringToHash("scroll");
    SelectSongScrollviewContext context;

    void Start()
    {
        var rect = transform as RectTransform;
        rect.anchoredPosition3D = Vector3.zero;
        UpdatePosition(0);
        button.onClick.AddListener(OnPressedCell);
        rect.localRotation = Quaternion.identity;
    }

    public override void SetContext(SelectSongScrollviewContext context)
    {
        this.context = context;
    }

    public override void UpdateContent(BaseInfo itemData)
    {
        button.interactable = !itemData.isLock;
        lockImage.gameObject.SetActive(itemData.isLock);

        // image cover
        image.sprite = itemData.sprite;
        // song name
        //songName.text = itemData.GetName.AdjustThaiFont();
        imageSongName.sprite = itemData.songNameSprite;
        imageSongName.SetNativeSize();
        //Debug.Log(Time.time.ToString("N2") + ": " + songName.text);
        // stat info
        var high_score = LeaderboardScript.GetHighScore(itemData.id);
        if (high_score.score <= 0)
        {
            hiScore.text = "";
        }
        else
        {
            hiScore.text = "<size=30>High Score</size> " + high_score.score.ToString("N0");
        }
        
        if (context != null)
        {
            var is_selected = context.SelectedIndex == DataIndex;
            image.color = is_selected ? select : unselect;
            bg.localScale = Vector3.one * (is_selected ? 1f : 0.75f);

            //if (is_selected)
            //{
            //    transform.SetAsLastSibling();
            //}
        }

        transform.SetAsLastSibling();
    }

    public override void UpdatePosition(float position)
    {
        animator.Play(scrollHash, -1, position);
        animator.speed = 0f;
    }

    public void OnPressedCell()
    {
        if (context != null)
            context.OnPressedCell(this);
    }
}

//public class SongInfo : MonoBehaviour
//{
//    public Sprite sprite;
//    public string songName;
//}

public class SelectSongScrollviewContext
{
    public System.Action<SongItemScript> OnPressedCell;
    public int SelectedIndex;
}
