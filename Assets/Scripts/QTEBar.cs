using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class QTEBar : MonoBehaviour
{
    const int visibleListCount = 9;

    public GameObject upBlock = null;
    public GameObject downBlock = null;
    public GameObject leftBlock = null;
    public GameObject rightBlock = null;
    //public GameObject reverseUpBlock = null;
    //public GameObject reverseDownBlock = null;
    //public GameObject reverseLeftBlock = null;
    //public GameObject reverseRightBlock = null;

    public float defaultDuration = 0.25f;

    private float m_MinBlockXPos = -350.0f;
    private float m_MaxBlockXPos = 350.0f;
    private Block[] m_VisibleBlockLists;
    private int m_CurrentBlockIndex = 0;
    private int m_RemainingBlockCount = 0;
    public int RemainingBlockCount
    {
        get { return m_RemainingBlockCount; }
        set { m_RemainingBlockCount = value; }
    }
    private Queue<Block> m_UpBlocks = new Queue<Block>();
    private Queue<Block> m_DownBlocks = new Queue<Block>();
    private Queue<Block> m_LeftBlocks = new Queue<Block>();
    private Queue<Block> m_RightBlocks = new Queue<Block>();

    private List<Block> m_visibleBlocks = new List<Block>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_VisibleBlockLists = new Block[visibleListCount];

        for (int i = 0; i < 6; i++)
        {
            GameObject upBlockInstantiated = GameObject.Instantiate(upBlock);
            upBlockInstantiated.transform.SetParent(transform, false);

            GameObject downBlockInstantiated = GameObject.Instantiate(downBlock);
            downBlockInstantiated.transform.SetParent(transform, false);

            GameObject leftBlockInstantiated = GameObject.Instantiate(leftBlock);
            leftBlockInstantiated.transform.SetParent(transform, false);

            GameObject rightBlockInstantiated = GameObject.Instantiate(rightBlock);
            rightBlockInstantiated.transform.SetParent(transform, false);

            m_UpBlocks.Enqueue(upBlockInstantiated.GetComponent<Block>());
            m_DownBlocks.Enqueue(downBlockInstantiated.GetComponent<Block>());
            m_LeftBlocks.Enqueue(leftBlockInstantiated.GetComponent<Block>());
            m_RightBlocks.Enqueue(rightBlockInstantiated.GetComponent<Block>());
        }

        for (int i = 0; i < visibleListCount; i++)
        {
            m_VisibleBlockLists[i] = GetRandomBlock();
            m_VisibleBlockLists[i].GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(m_MaxBlockXPos + (100.0f * i), 0.0f);
        }

        for (int i = visibleListCount - 1; i >= 0; i--)
        {
            StartCoroutine(VisibleBlockMoveRotine(m_VisibleBlockLists[i].gameObject.GetComponent<Image>().rectTransform,
                new Vector2(-100.0f * (visibleListCount - 1), 0.0f),
                defaultDuration * (visibleListCount - 1)));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    Block GetRandomBlock()
    {
        int randomIndex = Random.Range(0, 4);

        Block block = null;
        bool hasFound = false;
        while (!hasFound)
        {
            switch (randomIndex)
            {
            case 0:
            {
                if (m_UpBlocks.Count == 0)
                {
                    continue;
                }
                block = m_UpBlocks.Dequeue();
                hasFound = true;
                break;
            }
            case 1:
            {
                if (m_DownBlocks.Count == 0)
                {
                    continue;
                }
                block = m_DownBlocks.Dequeue();
                hasFound = true;
                break;
            }
            case 2:
            {
                if (m_RightBlocks.Count == 0)
                {
                    continue;
                }
                block = m_RightBlocks.Dequeue();
                hasFound = true;
                break;
            }
            case 3:
            {
                if (m_LeftBlocks.Count == 0)
                {
                    continue;
                }
                block = m_LeftBlocks.Dequeue();
                hasFound = true;
                break;
            }
            }
        }
        return block;
    }

    void IdleBlock(Block block)
    {
        if (block is ReverseBlock)
        {
            // ...
        }
        else // Normal Block
        {
            switch (block.BoundKeyEnum)
            {
            case KeyEnum.Up:
            {
                m_UpBlocks.Enqueue(block);
                break;
            }
            case KeyEnum.Down:
            {
                m_DownBlocks.Enqueue(block);
                break;
            }
            case KeyEnum.Left:
            {
                m_LeftBlocks.Enqueue(block);
                break;
            }
            case KeyEnum.Right:
            {
                m_RightBlocks.Enqueue(block);
                break;
            }
            default:
            {
                break;
            }
            }
        }
    }


    public IBreakable.Status TryProcess(KeyEnum keyEnum)
    {
        IBreakable.Status processedStatus = m_VisibleBlockLists[m_CurrentBlockIndex].Process(keyEnum);

        if (processedStatus == IBreakable.Status.Broken)
        {
            foreach (Block block in m_VisibleBlockLists)
            {
                StartCoroutine(VisibleBlockMoveRotine(block.gameObject.GetComponent<Image>().rectTransform, new Vector2(150.0f, 0.0f), defaultDuration));
            }
            IdleBlock(m_VisibleBlockLists[m_CurrentBlockIndex]);

            m_VisibleBlockLists[m_CurrentBlockIndex] = GetRandomBlock();
            m_VisibleBlockLists[m_CurrentBlockIndex].GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(m_MaxBlockXPos, 0.0f);
            m_CurrentBlockIndex = (m_CurrentBlockIndex + 1) % visibleListCount;
        }

        return processedStatus;
    }

    private System.Collections.IEnumerator VisibleBlockMoveRotine(RectTransform rectTransform, Vector2 offset, float duration)
    {
        Vector2 startPos = rectTransform.anchoredPosition;
        Vector2 endPos = startPos + offset;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            // SmoothStep
            t = t * t * (3.0f - 2.0f * t);

            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = endPos;

        if (rectTransform.anchoredPosition.x < -350.0f)
        {
            rectTransform.anchoredPosition = new Vector2(450.0f, 0.0f);
        }
    }
}
