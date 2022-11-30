using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;

public class BannerScript : MonoBehaviour
{
    public InputField delayInput;
    public int defaultDelay = 10;
    public string defaultFolder = "D://Banner/";
    public InputField folderInput;
    public string[] supportExtensions = { ".jpg", ".jpeg", ".png" };

    public RawImage image;
    public RawImage image2;

    public int index = -1;
    public List<Texture2D> loaded_images;

    void Start()
    {
        var delay_time = PlayerPrefs.GetInt("BANNER_DELAY", defaultDelay);
        delayInput.text = delay_time.ToString();
        var banner_location = PlayerPrefs.GetString("BANNAER_LOCATION", defaultFolder);
        folderInput.text = banner_location;

        GetResolution();
        StartCoroutine(LoadImageFromFolder());
    }

    public void Reload()
    {
        PlayerPrefs.SetString("BANNER_LOCATION", folderInput.text);
        PlayerPrefs.SetInt("BANNER_DELAY", int.Parse(delayInput.text));

        StopAllCoroutines();
        StartCoroutine(LoadImageFromFolder());
    }

    void GetResolution()
    {
        var rect = GetComponent<RectTransform>();
        Debug.Log("banner: " + rect.rect.width + ", " + rect.rect.height);
        Debug.Log("ratio: " + rect.rect.width / rect.rect.height);
    }

    IEnumerator LoadImageFromFolder()
    {
        ClearTempImages();

        if (!Directory.Exists(folderInput.text))
            yield break;

        var files = Directory.GetFiles(folderInput.text);
        var supported_files = files.Where(x => IsSupportExtension(x)).ToArray();

        foreach (var item in supported_files)
        {
            Debug.Log(item);
            yield return StartCoroutine(download_image(item));
        }

        index = 0;
        if (loaded_images.Count > 0)
            StartCoroutine(running_banner());
    }
    IEnumerator download_image(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        loaded_images.Add(www.texture);
    }
    IEnumerator running_banner()
    {
        yield return StartCoroutine(fade_effect1());

        //image.texture = loaded_images[index];
        var delay = int.Parse(delayInput.text); 
        yield return new WaitForSeconds(delay);

        index = (index + 1) % loaded_images.Count;
        StartCoroutine(running_banner());
    }

    void ClearTempImages()
    {
        if (loaded_images == null)
            loaded_images = new List<Texture2D>();

        for (int i = 0; i < loaded_images.Count; i++)
        {
            if (loaded_images[i] != null)
                GameObject.Destroy(loaded_images[i]);
        }
        loaded_images = new List<Texture2D>();
    }
    bool IsSupportExtension(string fullpath)
    {
        var file = new FileInfo(fullpath);
        var ext = file.Extension;
        //Debug.Log(ext);
        for (int i = 0; i < supportExtensions.Length; i++)
        {
            if (ext == supportExtensions[i])
                return true;
        }

        return false;
    }

    IEnumerator fade_effect1()
    {
        // img1 old
        // img2 new

        // img2 fadein
        var value = 0f;
        var color = new Color(1f, 1f, 1f, 0f);
        image2.texture = loaded_images[index];
        image2.color = color;
        image2.gameObject.SetActive(true);
        while (value < 1f)
        {
            value += Time.deltaTime;
            color.a = value;
            image2.color = color;
            yield return null;
        }
        // img1 set new img
        image.texture = loaded_images[index];
        // img2 hide
        image2.gameObject.SetActive(false);
    }

}
