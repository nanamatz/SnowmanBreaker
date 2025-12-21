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
    public GameObject reverseUpBlock = null;
    public GameObject reverseDownBlock = null;
    public GameObject reverseLeftBlock = null;
    public GameObject reverseRightBlock = null;

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
    private Queue<ReverseBlock> m_ReverseUpBlocks = new Queue<ReverseBlock>();
    private Queue<ReverseBlock> m_ReverseDownBlocks = new Queue<ReverseBlock>();
    private Queue<ReverseBlock> m_ReverseLeftBlocks = new Queue<ReverseBlock>();
    private Queue<ReverseBlock> m_ReverseRightBlocks = new Queue<ReverseBlock>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        m_VisibleBlockLists = new Block[visibleBlockListCount];

        upBlock.GetComponent<Block>().BoundKeyEnum = KeyEnum.Up;
        downBlock.GetComponent<Block>().BoundKeyEnum = KeyEnum.Down;
        leftBlock.GetComponent<Block>().BoundKeyEnum = KeyEnum.Left;
        rightBlock.GetComponent<Block>().BoundKeyEnum = KeyEnum.Right;
        reverseUpBlock.GetComponent<Block>().BoundKeyEnum = KeyEnum.ReverseUp;
        reverseDownBlock.GetComponent<Block>().BoundKeyEnum = KeyEnum.ReverseDown;
        reverseLeftBlock.GetComponent<Block>().BoundKeyEnum = KeyEnum.ReverseLeft;
        reverseRightBlock.GetComponent<Block>().BoundKeyEnum = KeyEnum.ReverseRight;

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

            GameObject reverseUpBlockInstantiated = GameObject.Instantiate(reverseUpBlock);
            reverseUpBlockInstantiated.transform.SetParent(transform, false);
            reverseUpBlockInstantiated.GetComponent<ReverseBlock>().BoundKeyEnum = KeyEnum.ReverseUp;

            GameObject reverseDownBlockInstantiated = GameObject.Instantiate(reverseDownBlock);
            reverseDownBlockInstantiated.transform.SetParent(transform, false);
            reverseDownBlockInstantiated.GetComponent<ReverseBlock>().BoundKeyEnum = KeyEnum.ReverseDown;

            GameObject reverseLeftBlockInstantiated = GameObject.Instantiate(reverseLeftBlock);
            reverseLeftBlockInstantiated.transform.SetParent(transform, false);
            reverseLeftBlockInstantiated.GetComponent<ReverseBlock>().BoundKeyEnum = KeyEnum.ReverseLeft;

            GameObject reverseRightBlockInstantiated = GameObject.Instantiate(reverseRightBlock); ;
            reverseRightBlockInstantiated.transform.SetParent(transform, false);
            reverseRightBlockInstantiated.GetComponent<ReverseBlock>().BoundKeyEnum = KeyEnum.ReverseRight;

            m_UpBlocks.Enqueue(upBlockInstantiated.GetComponent<Block>());
            m_DownBlocks.Enqueue(downBlockInstantiated.GetComponent<Block>());
            m_LeftBlocks.Enqueue(leftBlockInstantiated.GetComponent<Block>());
            m_RightBlocks.Enqueue(rightBlockInstantiated.GetComponent<Block>());

            m_ReverseUpBlocks.Enqueue(reverseUpBlockInstantiated.GetComponent<ReverseBlock>());
            m_ReverseDownBlocks.Enqueue(reverseDownBlockInstantiated.GetComponent<ReverseBlock>());
            m_ReverseLeftBlocks.Enqueue(reverseLeftBlockInstantiated.GetComponent<ReverseBlock>());
            m_ReverseRightBlocks.Enqueue(reverseRightBlockInstantiated.GetComponent<ReverseBlock>());
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
            return null;
            throw new System.Exception();
        }

        Block block = null;
        bool hasFound = false;
        bool canUseReverseBlocks = (m_Snowman != null && GameManager.Instance != null &&
                                   m_Snowman.level >= GameManager.Instance.reverseBlockMinLevel);

        while (!hasFound)
        {
            int maxRange = canUseReverseBlocks ? 8 : 4; // 레벨에 따라 범위 조정
            int randomIndex = Random.Range(0, maxRange);
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
                case 4:
                    {
                        if (m_ReverseUpBlocks.Count == 0)
                        {
                            continue;
                        }
                        block = m_ReverseUpBlocks.Dequeue();
                        block.BoundKeyEnum = KeyEnum.ReverseUp;
                        hasFound = true;
                        break;
                    }
                case 5:
                    {
                        if (m_ReverseDownBlocks.Count == 0)
                        {
                            continue;
                        }
                        block = m_ReverseDownBlocks.Dequeue();
                        block.BoundKeyEnum = KeyEnum.ReverseDown;
                        hasFound = true;
                        break;
                    }
                case 6:
                    {
                        if (m_ReverseLeftBlocks.Count == 0)
                        {
                            continue;
                        }
                        block = m_ReverseLeftBlocks.Dequeue();
                        block.BoundKeyEnum = KeyEnum.ReverseLeft;
                        hasFound = true;
                        break;
                    }
                case 7:
                    {
                        if (m_ReverseRightBlocks.Count == 0)
                        {
                            continue;
                        }
                        block = m_ReverseRightBlocks.Dequeue();
                        block.BoundKeyEnum = KeyEnum.ReverseRight;
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
            switch (block.BoundKeyEnum)
            {
            case KeyEnum.ReverseUp:
            {
                m_ReverseUpBlocks.Enqueue(block as ReverseBlock);
                break;
            }
            case KeyEnum.ReverseDown:
            {
                m_ReverseDownBlocks.Enqueue(block as ReverseBlock);
                break;
            }
            case KeyEnum.ReverseLeft:
            {
                m_ReverseLeftBlocks.Enqueue(block as ReverseBlock);
                break;
            }
            case KeyEnum.ReverseRight:
            {
                m_ReverseRightBlocks.Enqueue(block as ReverseBlock);
                break;
            }
            default:
            {
                break;
            }
            }
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

    public Block.Status TryProcess(KeyEnum keyEnum)
    {
        Block.Status processedStatus = m_VisibleBlockLists[m_CurrentBlockIndex].Process(keyEnum);

        if (processedStatus == Block.Status.Broken)
        {
            GameObject effect = Instantiate(m_VisibleBlockLists[m_CurrentBlockIndex].gameObject, m_VisibleBlockLists[m_CurrentBlockIndex].transform.parent);
            effect.transform.position = m_VisibleBlockLists[m_CurrentBlockIndex].transform.position;
            Destroy(effect.GetComponent<Block>());
            StartCoroutine(AnimateBreakEffect(effect));

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

    private IEnumerator AnimateBreakEffect(GameObject target)
    {
        // 원본 이미지를 가져옴
        Image originalImg = target.GetComponent<Image>();
        Sprite blockSprite = originalImg.sprite;
        Color blockColor = originalImg.color;
        Vector2 startPos = target.transform.position;

        // 파편 개수
        int shardCount = 30;
        GameObject[] shards = new GameObject[shardCount];
        Vector2[] velocities = new Vector2[shardCount];
        float[] rotations = new float[shardCount];

        for (int i = 0; i < shardCount; i++)
        {
            // 파편 생성
            shards[i] = new GameObject("Shard_" + i);
            shards[i].transform.SetParent(target.transform.parent);
            shards[i].transform.position = startPos;
            shards[i].transform.localScale = target.transform.localScale * 0.2f;

            Image shardImg = shards[i].AddComponent<Image>();
            shardImg.sprite = blockSprite;
            shardImg.color = blockColor;
            shardImg.raycastTarget = false;

            shards[i].AddComponent<CanvasGroup>();

            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float speed = Random.Range(200f, 500f);
            velocities[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
            rotations[i] = Random.Range(-360f, 360f);
        }

        Destroy(target);

        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            for (int i = 0; i < shardCount; i++)
            {
                if (shards[i] == null) continue;

                velocities[i] += Vector2.down * 3000f * Time.deltaTime;
                shards[i].transform.position += (Vector3)velocities[i] * Time.deltaTime;
                shards[i].transform.Rotate(Vector3.forward, rotations[i] * Time.deltaTime);
                shards[i].GetComponent<CanvasGroup>().alpha = 1f - t;
            }
            yield return null;
        }

        foreach (var shard in shards)
        {
            if (shard != null) Destroy(shard);
        }
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
