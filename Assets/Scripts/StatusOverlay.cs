using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusOverlay : MonoBehaviour
{
    public Snowman snowman;
    public Image hpImage;

    static private float s_Epsilon = 1.732e-3f;
    private float m_MaxHp;
    private float m_CurHp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // float curSnowmanHp = snowman.hp;
        float curSnowmanHp = snowman.qteQueue.Count;
        if (m_CurHp - curSnowmanHp < s_Epsilon)
        {
            return;
        }

        m_CurHp = curSnowmanHp;

        float hpSliderWidth = 1200.0f * (m_CurHp / m_MaxHp);
        hpImage.rectTransform.sizeDelta = new Vector2(hpSliderWidth, hpImage.rectTransform.sizeDelta.y);
    }

    public void SetStatus(Snowman snowman)
    {
        this.snowman = snowman;
        m_MaxHp = snowman.qteMaxCount;
        m_CurHp = snowman.qteMaxCount;
    }
}













// public class StatusOverlay : MonoBehaviour
// {
//     public Snowman snowman;
//     public Image hpImage;

//     static private float s_Epsilon = 1.732e-3f;
//     private float m_MaxHp;
//     private float m_CurHp;

//     public Transform qteIconContainer;
//     private List<Image> m_SpawnedIcons = new List<Image>();
//     private Sprite m_SpriteUp;
//     private Sprite m_SpriteDown;
//     private Sprite m_SpriteLeft;
//     private Sprite m_SpriteRight;

//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         m_SpriteUp = Resources.Load<Sprite>("Sprites/up");
//         m_SpriteDown = Resources.Load<Sprite>("Sprites/down");
//         m_SpriteLeft = Resources.Load<Sprite>("Sprites/left");
//         m_SpriteRight = Resources.Load<Sprite>("Sprites/right");

//         if (qteIconContainer == null && hpImage != null)
//         {
//             GameObject containerObj = new GameObject("QTEContainer", typeof(RectTransform));
//             containerObj.transform.SetParent(hpImage.transform.parent, false);
//             // Position it similarly to hpImage, maybe slightly offset or on top
//             RectTransform rt = containerObj.GetComponent<RectTransform>();
//             rt.anchorMin = hpImage.rectTransform.anchorMin;
//             rt.anchorMax = hpImage.rectTransform.anchorMax;
//             rt.anchoredPosition = hpImage.rectTransform.anchoredPosition;
//             rt.sizeDelta = new Vector2(1200, 100); // Approximate size, user can adjust

//             HorizontalLayoutGroup layout = containerObj.AddComponent<HorizontalLayoutGroup>();
//             layout.childAlignment = TextAnchor.MiddleLeft;
//             layout.childControlWidth = false;
//             layout.childControlHeight = false;
//             layout.spacing = 10;

//             qteIconContainer = containerObj.transform;
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         UpdateQTEIcons();

//         // float curSnowmanHp = snowman.hp;
//         float curSnowmanHp = snowman.qteQueue.Count;
//         if (m_CurHp - curSnowmanHp < s_Epsilon)
//         {
//             return;
//         }

//         m_CurHp = curSnowmanHp;

//         float hpSliderWidth = 1200.0f * (m_CurHp / m_MaxHp);
//         hpImage.rectTransform.sizeDelta = new Vector2(hpSliderWidth, hpImage.rectTransform.sizeDelta.y);
//     }

//     void UpdateQTEIcons()
//     {
//         if (snowman == null || qteIconContainer == null) return;

//         KeyEnum[] currentQueue = snowman.qteQueue.ToArray();
//         int count = currentQueue.Length;

//         // Ensure we have enough icons
//         while (m_SpawnedIcons.Count < count)
//         {
//             GameObject iconObj = new GameObject("QTEIcon", typeof(Image));
//             iconObj.transform.SetParent(qteIconContainer, false);
//             Image img = iconObj.GetComponent<Image>();
//             img.preserveAspect = true;
//             // Set a default size
//             img.rectTransform.sizeDelta = new Vector2(200, 200);
//             m_SpawnedIcons.Add(img);
//         }

//         // Update and activate needed icons
//         for (int i = 0; i < count; i++)
//         {
//             Image icon = m_SpawnedIcons[i];
//             icon.gameObject.SetActive(true);

//             switch (currentQueue[i])
//             {
//                 case KeyEnum.Up: icon.sprite = m_SpriteUp; break;
//                 case KeyEnum.Down: icon.sprite = m_SpriteDown; break;
//                 case KeyEnum.Left: icon.sprite = m_SpriteLeft; break;
//                 case KeyEnum.Right: icon.sprite = m_SpriteRight; break;
//             }
//         }

//         // Deactivate unused icons
//         for (int i = count; i < m_SpawnedIcons.Count; i++)
//         {
//             m_SpawnedIcons[i].gameObject.SetActive(false);
//         }
//     }

//     public void SetStatus(Snowman snowman)
//     {
//         this.snowman = snowman;
//         m_MaxHp = snowman.qteMaxCount;
//         m_CurHp = snowman.qteMaxCount;
//     }
// }
