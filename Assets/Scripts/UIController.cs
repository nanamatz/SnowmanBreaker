using TMPro;
using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    [SerializeField] private TextMeshProUGUI m_LevelText;
    [SerializeField] private TextMeshProUGUI m_TimerText;
    [SerializeField] private TextMeshProUGUI m_RemainBlockText;
    [SerializeField] private TextMeshProUGUI m_BlockScoreText;

    [SerializeField] private GameObject m_EndGameCanvas;
    [SerializeField] private GameObject m_WrongInputHighlight;

    [SerializeField] private BlockAlert m_BlockAlert;

    private Color m_originalTimerColor;
    private bool m_isTimerAnimating = false;



    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (m_TimerText != null)
        {
            m_originalTimerColor = m_TimerText.color;
        }
        UpdateTimer();
        UpdateLevel();
        UpdateRemainBlockCount();
    }

    void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        if (m_TimerText != null && GameManager.Instance != null)
        {
            float currentTime = GameManager.Instance.currentTimer;
            int seconds = Mathf.FloorToInt(currentTime);
            int milliseconds = Mathf.FloorToInt((currentTime - seconds) * 100);
            m_TimerText.text = string.Format("{0:00}:{1:00}", seconds, milliseconds);

            // 5초 이하일 때 긴박감 연출
            if (currentTime <= 5f && currentTime > 0)
            {
                if (!m_isTimerAnimating)
                {
                    m_isTimerAnimating = true;
                    StartCoroutine(TimerWarningAnimation());
                }
            }
            else
            {
                if (m_isTimerAnimating)
                {
                    m_isTimerAnimating = false;
                    StopAllCoroutines();
                    m_TimerText.color = m_originalTimerColor;
                    m_TimerText.transform.localScale = Vector3.one;
                }
            }
        }
    }

    public void UpdateRemainBlockCount()
    {
        if (m_RemainBlockText != null && GameManager.Instance != null && GameManager.Instance.snowmans.Count > 0)
        {
            int remainBlockCount = GameManager.Instance.snowmans[0].remainingBlockCount;
            m_RemainBlockText.text = remainBlockCount.ToString();
        }
    }

    void UpdateBlockScore()
    {
        if (m_BlockScoreText != null && GameManager.Instance != null)
        {
            m_BlockScoreText.text = GameManager.Instance.blockScore.ToString();
        }
    }

    IEnumerator TimerWarningAnimation()
    {
        while (m_isTimerAnimating)
        {
            // 빨간색으로 변경하며 크기 애니메이션
            m_TimerText.color = Color.red;

            // 크기 확대
            for (float t = 0; t < 0.5f; t += Time.deltaTime)
            {
                float scale = Mathf.Lerp(1f, 1.2f, t / 0.5f);
                m_TimerText.transform.localScale = Vector3.one * scale;
                yield return null;
            }

            // 크기 축소
            for (float t = 0; t < 0.5f; t += Time.deltaTime)
            {
                float scale = Mathf.Lerp(1.2f, 1f, t / 0.5f);
                m_TimerText.transform.localScale = Vector3.one * scale;
                yield return null;
            }
        }
    }

    public void UpdateLevel()
    {
        if (m_LevelText != null && GameManager.Instance != null && GameManager.Instance.snowmans.Count > 0)
        {
            int currentLevel = GameManager.Instance.snowmans[0].level;
            m_LevelText.text = currentLevel.ToString();
        }
    }

    public void ShowEndGameCanvas()
    {
        if (m_EndGameCanvas.activeSelf == false)
        {
            UpdateBlockScore(); // 게임 종료 시 최종 점수 업데이트

            m_EndGameCanvas.SetActive(true);
        }
    }

    public void OnRestartButtonClicked()
    {
        GameManager.Instance.RestartGame();
    }

    public void ShowWrongInputFeedback()
    {
        if (m_BlockAlert != null)
        {
            m_BlockAlert.Alert();
        }
    }

    IEnumerator WrongInputHighlightEffect()
    {
        if (m_WrongInputHighlight == null) yield break;

        m_WrongInputHighlight.SetActive(true);
        
        float duration = 1.0f;
        float elapsed = 0f;
        Vector3 originalScale = m_WrongInputHighlight.transform.localScale;
        
        // 반짝반짝 + 크기 변화 효과
        while (elapsed < duration)
        {
            float progress = elapsed / duration;
            
            // 반짝반짝 효과 (알파 값 변화)
            CanvasGroup canvasGroup = m_WrongInputHighlight.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                float alpha = Mathf.PingPong(Time.time * 8f, 1f); // 빠른 깜빡임
                canvasGroup.alpha = alpha;
            }
            
            // 크기 변화 효과 (펄스)
            float scale = 1f + Mathf.Sin(Time.time * 10f) * 0.2f; // 펄스 효과
            m_WrongInputHighlight.transform.localScale = originalScale * scale;
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        // 원래 상태로 복구 후 비활성화
        m_WrongInputHighlight.transform.localScale = originalScale;
        CanvasGroup finalCanvasGroup = m_WrongInputHighlight.GetComponent<CanvasGroup>();
        if (finalCanvasGroup != null)
        {
            finalCanvasGroup.alpha = 1f;
        }
        m_WrongInputHighlight.SetActive(false);
    }
}
