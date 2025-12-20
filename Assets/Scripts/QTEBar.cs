using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class QTEBar : MonoBehaviour
{
    const int visibleBlockListCount = 9;

    public GameObject upBlock = null;
    public GameObject downBlock = null;
    public GameObject leftBlock = null;
    public GameObject rightBlock = null;
    //public GameObject reverseUpBlock = null;
    //public GameObject reverseDownBlock = null;
    //public GameObject reverseLeftBlock = null;
    //public GameObject reverseRightBlock = null;

    public float defaultDuration = 0.05f;

    private float m_MinBlockXPos = -350.0f;
    private float m_MaxBlockXPos = 450.0f;
    [SerializeField] private Block[] m_VisibleBlockLists = new Block[visibleBlockListCount];
    [SerializeField] private int m_CurrentBlockIndex = 0;
    private Snowman m_Snowman;

    private Queue<Block> m_UpBlocks = new Queue<Block>();
    private Queue<Block> m_DownBlocks = new Queue<Block>();
    private Queue<Block> m_LeftBlocks = new Queue<Block>();
    private Queue<Block> m_RightBlocks = new Queue<Block>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        m_VisibleBlockLists = new Block[visibleBlockListCount];

        upBlock.GetComponent<Block>().BoundKeyEnum = KeyEnum.Up;
        downBlock.GetComponent<Block>().BoundKeyEnum = KeyEnum.Down;
        leftBlock.GetComponent<Block>().BoundKeyEnum = KeyEnum.Left;
        rightBlock.GetComponent<Block>().BoundKeyEnum = KeyEnum.Right;

        for (int i = 0; i < 6; i++)
        {
            GameObject upBlockInstantiated = GameObject.Instantiate(upBlock);
            upBlockInstantiated.transform.SetParent(transform, false);
            upBlockInstantiated.GetComponent<Block>().BoundKeyEnum = KeyEnum.Up;

            GameObject downBlockInstantiated = GameObject.Instantiate(downBlock);
            downBlockInstantiated.transform.SetParent(transform, false);
            downBlockInstantiated.GetComponent<Block>().BoundKeyEnum = KeyEnum.Down;


            GameObject leftBlockInstantiated = GameObject.Instantiate(leftBlock);
            leftBlockInstantiated.transform.SetParent(transform, false);
            leftBlockInstantiated.GetComponent<Block>().BoundKeyEnum = KeyEnum.Left;


            GameObject rightBlockInstantiated = GameObject.Instantiate(rightBlock);
            rightBlockInstantiated.transform.SetParent(transform, false);
            rightBlockInstantiated.GetComponent<Block>().BoundKeyEnum = KeyEnum.Right;


            m_UpBlocks.Enqueue(upBlockInstantiated.GetComponent<Block>());
            m_DownBlocks.Enqueue(downBlockInstantiated.GetComponent<Block>());
            m_LeftBlocks.Enqueue(leftBlockInstantiated.GetComponent<Block>());
            m_RightBlocks.Enqueue(rightBlockInstantiated.GetComponent<Block>());
        }

        //InitVisibleBlock();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitVisibleBlock()
    {
        for (int i = 0; i < visibleBlockListCount; i++)
        {
            if (m_VisibleBlockLists[i])
            {
                IdleBlock(m_VisibleBlockLists[i]);
            }

            m_VisibleBlockLists[i] = GetRandomBlock();
            m_VisibleBlockLists[i].GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(m_MaxBlockXPos + (100.0f * i), 0.0f);
            if (i >= m_Snowman.remainingBlockCount)
            {
                m_VisibleBlockLists[i].gameObject.SetActive(false);
            }
            else
            {
                m_VisibleBlockLists[i].gameObject.SetActive(true);
            }
        }

        for (int i = visibleBlockListCount - 1; i >= 0; i--)
        {
            StartCoroutine(VisibleBlockMoveRotine(m_VisibleBlockLists[i].gameObject.GetComponent<Image>().rectTransform,
                new Vector2(-100.0f * (visibleBlockListCount - 1), 0.0f),
                defaultDuration * (visibleBlockListCount - 1)));
        }

        m_CurrentBlockIndex = 0;
    }

    Block GetRandomBlock()
    {
        if (m_UpBlocks.Count + m_DownBlocks.Count + m_LeftBlocks.Count + m_RightBlocks.Count == 0)
        {
            throw new System.Exception();
        }

        Block block = null;
        bool hasFound = false;
        while (!hasFound)
        {
            int randomIndex = Random.Range(0, 4);
            switch (randomIndex)
            {
                case 0:
                    {
                        if (m_UpBlocks.Count == 0)
                        {
                            continue;
                        }
                        block = m_UpBlocks.Dequeue();
                        block.BoundKeyEnum = KeyEnum.Up;
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
                        block.BoundKeyEnum = KeyEnum.Down;
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
        block.gameObject.SetActive(false);
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

    public void SetSnowman(Snowman snowman)
    {
        m_Snowman = snowman;
        InitVisibleBlock();
    }

    public IBreakable.Status TryProcess(KeyEnum keyEnum)
    {
        IBreakable.Status processedStatus = m_VisibleBlockLists[m_CurrentBlockIndex].Process(keyEnum);

        if (processedStatus == IBreakable.Status.Broken)
        {
            foreach (Block block in m_VisibleBlockLists)
            {
                StartCoroutine(VisibleBlockMoveRotine(block.gameObject.GetComponent<Image>().rectTransform, new Vector2(-100.0f, 0.0f), defaultDuration));
            }
            IdleBlock(m_VisibleBlockLists[m_CurrentBlockIndex]);

            m_VisibleBlockLists[m_CurrentBlockIndex] = GetRandomBlock();
            m_VisibleBlockLists[m_CurrentBlockIndex].GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(m_MaxBlockXPos, 0.0f);

            if (m_Snowman.remainingBlockCount > visibleBlockListCount)
            {
                m_VisibleBlockLists[m_CurrentBlockIndex].gameObject.SetActive(true);
            }
            else
            {
                m_VisibleBlockLists[m_CurrentBlockIndex].gameObject.SetActive(false);
            }
            m_CurrentBlockIndex = (m_CurrentBlockIndex + 1) % visibleBlockListCount;
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

        if (rectTransform.anchoredPosition.x < m_MinBlockXPos - 1.0f /*Epsilon*/)
        {
            rectTransform.anchoredPosition = new Vector2(m_MaxBlockXPos, 0.0f);
        }
    }
}
