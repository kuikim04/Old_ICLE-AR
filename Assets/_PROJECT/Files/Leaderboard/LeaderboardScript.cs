using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class LeaderboardScript : MonoBehaviour
{
    public LeaderboardEntryScript prefab;
    public Transform root;
    public List<LeaderboardEntryScript> list;

    public Sprite no1, no2, no3, other;

    public highscore_json high_score_data;

    [ContextMenu("OpenFile")]
    void OpenFile()
    {
        Application.OpenURL(Application.persistentDataPath);
    }

    void Start()
    {

    }

    [ContextMenu("Test")]
    void Test()
    {
        CreateBoard(0, "nik", Random.Range(1000, 9000));
    }

    private void OnEnable()
    {
        var detection = GamePlayUI.song.GetComponent<PoseDetectionScript>();
        var song_id = SelectSongUIScript.Selected.id;
        var username = ResultUI.singleton.usernameInput.text;
        var score = detection.MyScore.GetScore();
        CreateBoard(song_id, username, score);
    }

    /// <summary>
    /// load file from song_id
    /// then add new record and re-order
    /// then show
    /// </summary>
    /// <param name="song_id"></param>
    /// <param name="username"></param>
    /// <param name="score"></param>
    void CreateBoard(int song_id, string username, int score)
    {
        if (list == null)
            list = new List<LeaderboardEntryScript>();

        // try to clear old list
        foreach (var item in this.list)
            GameObject.Destroy(item.gameObject);
        list.Clear();

        // new one
        high_score_data = new highscore_json(song_id);

        // then add new gameplay score
        // then sort by .score
        // then re save again.
        high_score_data.Add(username, score);
        high_score_data.WriteFile();

        // instance entries.
        for (int i = 0; i < 10; i++)
        {
            //list[i].WriteToDisk(song_id, i);
            var go = GameObject.Instantiate<LeaderboardEntryScript>(prefab, root, false);
            go.transform.localScale = Vector3.zero;
            go.image.sprite = GetSprite(i);
            go.gameObject.SetActive(true);
            go.Set(i + 1, high_score_data.Get(i));
            list.Add(go);
        }
    }
    Sprite GetSprite(int index)
    {
        switch (index)
        {
            case 0: return no1;
            case 1: return no2;
            case 2: return no3;
            default: return other;
        }
    }

    [System.Serializable]
    public class highscore_entry
    {
        public string name;
        public int score;
    }

    [System.Serializable]
    public class highscore_json
    {
        public int songID;
        public List<highscore_entry> list;

        private string path
        {
            get { return Application.persistentDataPath + "/high_score" + songID + ".txt"; }
        }

        public highscore_json() { }
        public highscore_json(int song_id)
        {
            ReadFile(song_id);
        }

        public highscore_entry Get(int index)
        {
            if (list == null)
                list = new List<highscore_entry>();
            if (index < list.Count)
                return list[index];
            else
                return new highscore_entry() { name = "---", score = 0 };
        }

        public void Add(string name, int score)
        {
            if (list == null)
                list = new List<highscore_entry>();
            list.Add(new highscore_entry() { name = name, score = score });
            reorder();
        }

        void reorder()
        {
            list = list.OrderByDescending(x => x.score).ToList();
        }

        public void WriteFile()
        {
            Debug.Log("write file: " + path);
            var json = JsonUtility.ToJson(this);
            File.WriteAllText(path, json);
        }

        public void ReadFile(int song_id)
        {
            songID = song_id;
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var obj = JsonUtility.FromJson<highscore_json>(json);
                list = new List<highscore_entry>();
                list.AddRange(obj.list);
            }
            else
            {
                list = new List<highscore_entry>();
            }
        }
    }

    public static highscore_entry GetHighScore(int song_id, int rank = 0)
    {
        highscore_json data = new highscore_json(song_id);
        var entry = data.Get(rank);
        return entry;
    }

    public static void DeleteFile()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        var files = dir.GetFiles();
        foreach (var item in files)
        {
            if (item.Name.Contains("high_score"))
            {
                File.Delete(item.FullName);
            }
        }
    }

}
