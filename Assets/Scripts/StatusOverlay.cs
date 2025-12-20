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
        float curSnowmanHp = snowman.hp;
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
        m_MaxHp = snowman.maxHp;
        m_CurHp = snowman.maxHp;
    }
}