using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Chaser : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject playerEye;
    public float maxChaseDistance = 10.0f;
    public float chaseDistance = 10.0f;
    public float duration = 0.5f;

    public bool isMainChaser = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Chase()
    {
        Vector3 playerPos = playerEye.transform.position;
        Vector3 distance = playerEye.transform.position - transform.position;
        float distLen = distance.sqrMagnitude;
        if (distLen < maxChaseDistance)
        {
            StartCoroutine(StareRoutine(playerEye.transform));
            if (isMainChaser)
            {
                GameManager.Instance.GameOver();
            }
        }
        else
        {
            StartCoroutine(ChaseRoutine(playerEye.transform, chaseDistance));
        }
    }

    IEnumerator ChaseRoutine(Transform targetTransform, float distance)
    {
        if (targetTransform == null) yield break;

        Vector3 startPos = transform.position;
        // 방향 벡터 계산 (정규화)
        Vector3 direction = (targetTransform.position - startPos).normalized;
        // 최종 목적지 계산: 시작점 + (방향 * 거리)
        Vector3 endPos = startPos + (direction * distance);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // SmoothStep을 적용하여 부드러운 가감속 구현
            float easedT = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(startPos, endPos, easedT);
            yield return null;
        }

        // 오차 보정: 최종 위치 확정
        transform.position = endPos;

        yield return null;
    }

    IEnumerator StareRoutine(Transform targetTransform)
    {
        if (targetTransform == null) yield break;

        Quaternion startRotation = transform.rotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            Vector3 direction = (targetTransform.position - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                float easedT = Mathf.SmoothStep(0f, 1f, t);
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, easedT);
            }

            yield return null;
        }

        Vector3 finalDir = (targetTransform.position - transform.position).normalized;
        if (finalDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(finalDir);
        }
    }
}
