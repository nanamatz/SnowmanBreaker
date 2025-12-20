using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    public Animator cameraAnimator;
    public CanvasGroup startUICanvasGroup;

    public List<Snowman> snowmans;
    public StatusOverlay statusOverlay;
    public float moveDistance = 20.0f;
    public float duration = 0.5f;

    public int maxKeyQueueSize = 8;
    public Queue<KeyEnum> TargetKeyQueue;

    private Queue<KeyEnum> m_UpKeyPool;
    private Queue<KeyEnum> m_DownKeyPool;
    private Queue<KeyEnum> m_LeftKeyPool;
    private Queue<KeyEnum> m_RightKeyPool;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        statusOverlay.SetStatus(snowmans[0]);

        TargetKeyQueue = new Queue<KeyEnum>();
        m_UpKeyPool = new Queue<KeyEnum>(Enumerable.Repeat(KeyEnum.Up, 6));
        m_DownKeyPool = new Queue<KeyEnum>(Enumerable.Repeat(KeyEnum.Down, 6));
        m_LeftKeyPool = new Queue<KeyEnum>(Enumerable.Repeat(KeyEnum.Left, 6));
        m_RightKeyPool = new Queue<KeyEnum>(Enumerable.Repeat(KeyEnum.Right, 6));
        RefillTargetKey();
    }

    public void GameStart()
    {
        StartCoroutine(FadeOutStartUI());
    }

    private System.Collections.IEnumerator FadeOutStartUI()
    {
        float fadeDuration = 2.0f;
        float elapsed = 0f;
        float startAlpha = startUICanvasGroup.alpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            startUICanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
            yield return null;
        }

        startUICanvasGroup.alpha = 0f;
        startUICanvasGroup.interactable = false;
        startUICanvasGroup.blocksRaycasts = false;

        cameraAnimator.SetTrigger("GameStart");
    }

    public void RecycleTargetKey()
    {
        KeyEnum keyEnum = TargetKeyQueue.Dequeue();

        if (keyEnum == KeyEnum.Up)
            m_UpKeyPool.Enqueue(keyEnum);
        else if (keyEnum == KeyEnum.Down)
            m_DownKeyPool.Enqueue(keyEnum);
        else if (keyEnum == KeyEnum.Left)
            m_LeftKeyPool.Enqueue(keyEnum);
        else if (keyEnum == KeyEnum.Right)
            m_RightKeyPool.Enqueue(keyEnum);

        RefillTargetKey();
    }

    void RefillTargetKey()
    {
        while (TargetKeyQueue.Count != maxKeyQueueSize)
        {
            KeyEnum keyEnum = (KeyEnum)Random.Range(0, 3);

            if (keyEnum == KeyEnum.Up && m_UpKeyPool.Count != 0)
                TargetKeyQueue.Enqueue(m_UpKeyPool.Dequeue());
            else if (keyEnum == KeyEnum.Down && m_DownKeyPool.Count != 0)
                TargetKeyQueue.Enqueue(m_DownKeyPool.Dequeue());
            else if (keyEnum == KeyEnum.Left && m_LeftKeyPool.Count != 0)
                TargetKeyQueue.Enqueue(m_LeftKeyPool.Dequeue());
            else if (keyEnum == KeyEnum.Right && m_RightKeyPool.Count != 0)
                TargetKeyQueue.Enqueue(m_RightKeyPool.Dequeue());
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckSnowmanRespawn();
    }

    void CheckSnowmanRespawn()
    {
        if (snowmans[0].hp > 0.2f)
        {
            return;
        }

        foreach (Snowman obj in snowmans)
        {
            if (obj != null)
            {
                // �� ������Ʈ���� �������� �ڷ�ƾ ����
                StartCoroutine(SmoothMoveRoutine(obj.transform, Vector3.left * moveDistance));
            }
        }

        for (int i = 0; i < snowmans.Count - 1; i++)
        {
            Snowman temp = snowmans[i + 1];
            snowmans[i + 1] = snowmans[i]; // swap\
            snowmans[i] = temp;
        }

        var deadSnowman = snowmans[snowmans.Count - 1];
        deadSnowman.Respawn(deadSnowman.level + snowmans.Count);

        statusOverlay.SetStatus(snowmans[0]);
    }

    private System.Collections.IEnumerator SmoothMoveRoutine(Transform targetTransform, Vector3 offset)
    {
        Vector3 startPos = targetTransform.localPosition;
        Vector3 endPos = startPos + offset;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // �ð� �帧�� ���� ���� ��� (0 ~ 1)
            float t = elapsed / duration;

            // �ε巯�� ����/���� ȿ���� ���� ��� (���� ����)
            // t = t * t * (3f - 2f * t); 

            targetTransform.localPosition = Vector3.Lerp(startPos, endPos, t);

            elapsed += Time.deltaTime;

            yield return null; // ���� �����ӱ��� ���
        }

        // ���� ��ġ ����
        targetTransform.localPosition = endPos;
        if (endPos.x < -10.0f)
        {
            targetTransform.localPosition = new Vector3(40.0f, 0.0f, 0.0f);
        }
    }
}
