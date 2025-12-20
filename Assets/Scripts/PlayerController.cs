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

    public KeyEnum lastPressedKey;

    private bool m_IsMoving = false;

    private Transform m_LeftArmTransform;
    private Transform m_RightArmTransform;

    private Vector3 m_LeftArmStartLocalPos;
    private Vector3 m_RightArmStartLocalPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_LeftArmTransform = leftArm.transform;
        m_RightArmTransform = rightArm.transform;

        m_LeftArmStartLocalPos = m_LeftArmTransform.localPosition;
        m_RightArmStartLocalPos = m_RightArmTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        GameManager gameManager = GameManager.Instance;
        if (m_IsMoving) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gameManager.TryHitProcess(KeyEnum.Right))
            {
                StartCoroutine(MovePlayerBodyRoutine(rightArm));
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gameManager.TryHitProcess(KeyEnum.Left))
            {
                StartCoroutine(MovePlayerBodyRoutine(leftArm));
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gameManager.TryHitProcess(KeyEnum.Up))
            {
                StartCoroutine(MovePlayerBodyRoutine(rightArm));
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gameManager.TryHitProcess(KeyEnum.Down))
            {
                // TODO: implement right leg
                StartCoroutine(MovePlayerBodyRoutine(leftArm));
            }
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
            direction = targetTransform.localRotation * -Vector3.forward;
        }
        else if (playerBody == rightArm)
        {
            targetTransform = m_RightArmTransform;
            startLocalPos = m_RightArmStartLocalPos;
            direction = targetTransform.localRotation * -Vector3.forward;
        }

        Vector3 targetLocalPos = startLocalPos + direction * moveDistance;

        yield return StartCoroutine(LerpPosition(targetTransform, startLocalPos, targetLocalPos, 0.05f));

        yield return StartCoroutine(LerpPosition(targetTransform, targetLocalPos, startLocalPos, 0.05f));

        targetTransform.localPosition = startLocalPos;
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
