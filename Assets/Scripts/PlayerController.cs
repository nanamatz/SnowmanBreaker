using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public GameObject leftArm;
    public GameObject rightArm;
    public GameObject leftLeg;

    public float moveDistance = 1.0f;
    public float moveSpeed = 0.05f;

    private bool m_IsMoving = false;

    private Transform m_LeftArmTransform;
    private Transform m_RightArmTransform;
    private Transform m_LeftLegTransform;

    private Vector3 m_LeftArmStartLocalPos;
    private Vector3 m_RightArmStartLocalPos;
    private Vector3 m_LeftLegStartLocalPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_LeftArmTransform = leftArm.transform;
        m_RightArmTransform = rightArm.transform;
        m_LeftLegTransform = leftLeg.transform;

        m_LeftArmStartLocalPos = m_LeftArmTransform.localPosition;
        m_RightArmStartLocalPos = m_RightArmTransform.localPosition;
        m_LeftLegStartLocalPos = m_LeftLegTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsMoving) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(MovePlayerBodyRoutine(rightArm));
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(MovePlayerBodyRoutine(leftArm));
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(MovePlayerBodyRoutine(leftLeg));
        }
    }

    IEnumerator MovePlayerBodyRoutine(GameObject playerBody)
    {
        m_IsMoving = true;
        Vector3 startLocalPos = Vector3.zero;
        Vector3 direction = Vector3.zero;
        Transform targetTransform = null;
        if (playerBody == leftArm)
        {
            targetTransform = m_LeftArmTransform;
            startLocalPos = m_LeftArmStartLocalPos;
            direction = targetTransform.localRotation * Vector3.up;
        }
        else if (playerBody == rightArm)
        {
            targetTransform = m_RightArmTransform;
            startLocalPos = m_RightArmStartLocalPos;
            direction = targetTransform.localRotation * Vector3.up;
        }
        else if (playerBody == leftLeg)
        {
            targetTransform = m_LeftLegTransform;
            startLocalPos = m_LeftLegStartLocalPos;
            direction = targetTransform.localRotation * Vector3.forward;
        }

        // 1. 목표 위치 계산 (로컬 Z축 방향)
        Vector3 targetLocalPos = startLocalPos + direction * moveDistance;

        // 2. 전진 (Fast Move Out)
        yield return StartCoroutine(LerpPosition(targetTransform, startLocalPos, targetLocalPos, 0.05f));

        // 3. 복귀 (Fast Move Back)
        yield return StartCoroutine(LerpPosition(targetTransform, targetLocalPos, startLocalPos, 0.05f));

        targetTransform.localPosition = startLocalPos; // 부동 소수점 오차 방지
        m_IsMoving = false;
    }

    IEnumerator LerpPosition(Transform targetTransform, Vector3 start, Vector3 end, float time)
    {
        float elapsed = 0;
        while (elapsed < time)
        {
            targetTransform.localPosition = Vector3.Lerp(start, end, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }
        targetTransform.localPosition = end;
    }

}
