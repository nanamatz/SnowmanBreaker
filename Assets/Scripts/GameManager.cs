using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    public List<Snowman> snowmans;
    public StatusOverlay statusOverlay;
    public float moveDistance = 20.0f;
    public float duration = 0.5f;
    public QTEBar qteBar;

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        statusOverlay.SetStatus(snowmans[0]);
        qteBar.RemainingBlockCount = snowmans[0].remainingBlockCount;
    }
    // Update is called once per frame
    void Update()
    {
        CheckSnowmanRespawn();
    }

    void CheckSnowmanRespawn()
    {
        if (snowmans[0].remainBlockCount > 0)
        {
            return;
        }

        foreach (Snowman obj in snowmans)
        {
            if (obj != null)
            {
                StartCoroutine(SnowmanMoveRoutine(obj.transform, Vector3.left * moveDistance));
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

    private System.Collections.IEnumerator SnowmanMoveRoutine(Transform targetTransform, Vector3 offset)
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

    public bool TryHitProcess(KeyEnum keyEnum)
    {
        IBreakable.Status status = qteBar.TryProcess(keyEnum);

        return IBreakable.Status.NotInteracted == status;
    }
}
