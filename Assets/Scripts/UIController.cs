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
}
