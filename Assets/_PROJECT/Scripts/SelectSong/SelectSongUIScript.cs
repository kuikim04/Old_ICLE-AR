using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class SelectSongUIScript : MonoBehaviour
{
    public static SelectSongUIScript singleton;
    public SelectSongScrollview scrollview;
    private int Index;

    private bool inFolder;
    private int max;
    private List<BaseInfo> all_data;
    private List<BaseInfo> currentList;
    private IEnumerable<BaseInfo> practices;

    public UnityEvent OnSelectSong;
    public UnityEvent OnPracticeMode;

    /// <summary>
    /// start at 0
    /// </summary>
    public static int CurrentIndex;
    public static BaseInfo Selected;

    private new AudioSource audio;
    public AudioClip moveClip, selectSongClip;
    private PromptToSelectSong gesture;

    public GameObject hintPlay, hintFolder;

    public void UpdateHintUI()
    {
        var is_select_folder = currentList[Index] is FolderInfo;
        if (hintPlay)
            hintPlay.SetActive(!is_select_folder);
        if (hintFolder)
            hintFolder.SetActive(is_select_folder);
    }

    private bool is_init;

    IEnumerator Start()
    {
        singleton = this;
        gesture = GetComponent<PromptToSelectSong>();
        gesture.IsSelected = false;
        // wait for scrollview to setup a frame.
        yield return null;

        var loaded = Resources.LoadAll<BaseInfo>("Songs");
        all_data = new List<BaseInfo>();
        all_data.AddRange(loaded);

        // sort by index
        var folder = all_data.Where(x => x is FolderInfo);
        var song = all_data.Where(x => x is SongInfo);
        practices = all_data.Where(x => x is PracticeInfo).OrderBy(x => x.order);

        // folder
        all_data = new List<BaseInfo>();
        all_data.AddRange(folder);
        all_data.AddRange(song);
        all_data = all_data.OrderBy(x => x.order).ToList();
        
        SetList(all_data);
        is_init = true;
    }

    private void OnEnable()
    {
        audio = GetComponent<AudioSource>();

        if (is_init)
        {
            SetList(all_data);
            // might need to update info again.
            scrollview.controller.ScrollTo(Index, 0.4f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Index--;
            if (Index < 0)
                Index = max - 1;
            scrollview.controller.ScrollTo(Index, 0.4f);
            audio.PlayOneShot(moveClip, Random.Range(0.8f, 1f));
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Index = (Index + 1) % max;
            scrollview.controller.ScrollTo(Index, 0.4f);
            audio.PlayOneShot(moveClip, Random.Range(0.8f, 1f));
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            SelectFolder();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inFolder)
            {
                inFolder = false;
                SetList(all_data);
            }
        }
    }

    void SetList(List<BaseInfo> list)
    {
        currentList = list;
        max = list.Count;
        scrollview.UpdateData(list);
        Index = 0;
        scrollview.controller.ScrollTo(Index, 0.4f);
    }

    void SelectFolder()
    {
        if (currentList[Index] is FolderInfo)
        {
            inFolder = true;
            // open folder
            SetList(practices.ToList());
            audio.PlayOneShot(selectSongClip);
            return;
        }
    }

    public void SelectSong()
    {
        // skip this action if select folder.
        if (currentList[Index] is FolderInfo)
            return;
        
        if (currentList[Index].isLock)
        {
            var ptss = GetComponent<PromptToSelectSong>();
            ptss.Reset();
        }
        else
        {
            // load song data for gameplay
            CurrentIndex = Index;
            Selected = currentList[Index];

            if (currentList[Index] is PracticeInfo)
            {
                // go to practice view
                OnPracticeMode.Invoke();
            }
            else if (currentList[Index] is SongInfo)
            {
                // go to gameplay view
                OnSelectSong.Invoke();
            }

            gesture.IsSelected = true;
            audio.PlayOneShot(selectSongClip);
        }
    }
}
