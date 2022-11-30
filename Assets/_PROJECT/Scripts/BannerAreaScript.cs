using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BannerAreaScript : MonoBehaviour
{

    [InlineButton("UpdateArea", "Update")]
    [PropertyRange(0f, 1f)]
    public float bannerArea = 0.1f;
    public enum Style
    {
        Top, Bottom
    }
    public Style style = Style.Top;

    public RectTransform root;
    public RectTransform banner;

    public Camera[] cameras;

    void UpdateArea()
    {


        if (style == Style.Top)
        {
            var banner_area = (int)(root.rect.height * bannerArea);
            var gameplay_area = root.rect.height - banner_area;
            banner.sizeDelta = new Vector2(banner.sizeDelta.x, banner_area);
            banner.pivot = new Vector2(0.5f, 1f);
            banner.anchorMin = new Vector2(0f, 1f);
            banner.anchorMax = new Vector2(1f, 1f);
            //gameplay.sizeDelta = new Vector2(gameplay.sizeDelta.x, gameplay_area);
            
            foreach (var item in cameras)
                item.rect = new Rect(0, 0, 1, 1f - bannerArea);
        }
        else
        {
            var banner_area = (int)(root.rect.height * bannerArea);
            var gameplay_area = root.rect.height - banner_area;
            banner.sizeDelta = new Vector2(banner.sizeDelta.x, banner_area);
            banner.pivot = new Vector2(0.5f, 0f);
            banner.anchorMin = new Vector2(0f, 0f);
            banner.anchorMax = new Vector2(1f, 0f);
            //gameplay.sizeDelta = new Vector2(gameplay.sizeDelta.x, gameplay_area);
            
            foreach (var item in cameras)
                item.rect = new Rect(0, bannerArea, 1f, 1f);
        }
    }

    void Start()
    {

    }
}
