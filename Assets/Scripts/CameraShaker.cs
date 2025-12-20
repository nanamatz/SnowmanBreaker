using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public float magnitude;
    public float duration;

    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShakeCamera()
    {
        StartCoroutine(ShakeRoutine());
    }

    System.Collections.IEnumerator ShakeRoutine()
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // -1.0 ~ 1.0 사이의 난수에 강도를 곱함
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        transform.localPosition = new Vector3(originalPos.x, originalPos.y, originalPos.z);
    }
}
