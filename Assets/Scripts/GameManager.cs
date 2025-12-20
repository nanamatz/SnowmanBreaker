using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    public List<Snowman> snowmans;
    public StatusOverlay statusOverlay;
    public float moveDistance = 20.0f;
    public float duration = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        statusOverlay.SetStatus(snowmans[0]);
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
                // 각 오브젝트별로 독립적인 코루틴 실행
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
            // 시간 흐름에 따른 비율 계산 (0 ~ 1)
            float t = elapsed / duration;

            // 부드러운 가속/감속 효과를 원할 경우 (선택 사항)
            // t = t * t * (3f - 2f * t); 

            targetTransform.localPosition = Vector3.Lerp(startPos, endPos, t);

            elapsed += Time.deltaTime;
            
            yield return null; // 다음 프레임까지 대기
        }

        // 최종 위치 보정
        targetTransform.localPosition = endPos;
        if(endPos.x < -10.0f)
        {
            targetTransform.localPosition = new Vector3(40.0f, 0.0f, 0.0f);
        }
    }
}
