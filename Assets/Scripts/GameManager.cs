using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    public Volume globalVolume;
    public Animator cameraAnimator;
    public CanvasGroup introCanvasGroup;
    public CanvasGroup gameCanvasGroup;
    public CanvasGroup endCanvasGroup;

    public CameraShaker mainCameraShaker;

    public List<Snowman> snowmans;
    public float moveDistance = 20.0f;
    public float duration = 0.5f;
    public QTEBar qteBar;

    public float currentTimer;
    public int blockScore = 0;
    public bool isPaused;

    private static GameManager instance;
    [SerializeField] public int reverseBlockMinLevel = 4; // reverse block이 등장하는 최소 레벨


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

    public void GameStart()
    {
        StartCoroutine(FadeOutStartUI());
    }

    private IEnumerator FadeOutStartUI()
    {
        float fadeDuration = 2.0f;
        float elapsed = 0f;
        float startAlpha = introCanvasGroup.alpha;
        float targetAlpha = 0f;
        globalVolume.profile.TryGet(out Vignette vignette);
        float startIntensity = vignette.intensity.value;
        float targetIntensity = startIntensity - 0.1f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            introCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            vignette.intensity.value = Mathf.Lerp(startIntensity, targetIntensity, elapsed / fadeDuration);
            yield return null;
        }

        introCanvasGroup.alpha = 0f;
        introCanvasGroup.interactable = false;
        introCanvasGroup.blocksRaycasts = false;

        cameraAnimator.SetTrigger("GameStart");

        while (!cameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.ReadyCameraAnimation") ||
               cameraAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        currentTimer = 60.0f;
        gameCanvasGroup.gameObject.SetActive(true);
        qteBar.SetSnowman(snowmans[0]);

        cameraAnimator.enabled = false;
    }

    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
        CheckSnowmanRespawn();
    }

    void UpdateTimer()
    {
        if (isPaused)
        {
            return;
        }
        if (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer <= 0)
            {
                currentTimer = 0;
                GameOver();
            }
        }
    }

    void GameOver()
    {
        // 게임 오버 처리 로직 추가
        if (UIController.instance != null)
        {
            UIController.instance.ShowEndGameCanvas();
        }

        // PlayerController 비활성화
        PlayerController playerController = FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    void CheckSnowmanRespawn()
    {
        if (!gameCanvasGroup.gameObject.activeInHierarchy || snowmans[0].remainingBlockCount > 0)
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

        qteBar.SetSnowman(snowmans[0]);


        // UI 업데이트
        if (UIController.instance != null)
        {
            UIController.instance.UpdateLevel();
            UIController.instance.UpdateRemainBlockCount(); // 레벨 변경 시 남은 블록 수도 업데이트
        }
    }

    private System.Collections.IEnumerator SnowmanMoveRoutine(Transform targetTransform, Vector3 offset)
    {
        Vector3 startPos = targetTransform.localPosition;
        Vector3 endPos = startPos + offset;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            t = t * t * (3f - 2f * t);

            targetTransform.localPosition = Vector3.Lerp(startPos, endPos, t);

            elapsed += Time.deltaTime;

            yield return null;
        }

        targetTransform.localPosition = endPos;
        if (endPos.x < -10.0f)
        {
            targetTransform.localPosition = new Vector3(40.0f, -3.1f, -1.8f);
        }
    }

    public bool TryHitProcess(KeyEnum keyEnum)
    {
        IBreakable.Status status = qteBar.TryProcess(keyEnum);
        if (status == IBreakable.Status.Broken)
        {
            // 블록 파괴 성공 시
            blockScore++;
            snowmans[0].remainingBlockCount--;
            // UI 업데이트
            if (UIController.instance != null)
            {
                UIController.instance.UpdateRemainBlockCount();
            }
        }
        else
        {
            // 잘못된 입력 시 피드백 효과
            if (UIController.instance != null)
            {
                UIController.instance.ShowWrongInputFeedback();
                //mainCameraShaker.ShakeCamera();
            }
        }

        return !(IBreakable.Status.NotInteracted == status);
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }
}
