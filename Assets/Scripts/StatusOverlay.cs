using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusOverlay : MonoBehaviour
{
    public Snowman snowman;

    private int m_MaxBlockCount = 0;
    private int m_RemainBlockCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int curSnowmanRemainBlockCount = snowman.remainingBlockCount;
        if (m_RemainBlockCount == curSnowmanRemainBlockCount)
        {
            return;
        }

        m_RemainBlockCount = curSnowmanRemainBlockCount;

        float hpSliderWidth = 1200.0f * ((float)m_RemainBlockCount / m_MaxBlockCount);
    }

    public void SetStatus(Snowman snowman)
    {
        this.snowman = snowman;
        m_MaxBlockCount = snowman.maxBlockCount;
        m_RemainBlockCount = snowman.remainingBlockCount;
    }
}