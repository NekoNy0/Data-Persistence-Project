using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public GameObject _GameManager;
    public Text ScoreText;
    public GameObject GameOverText;
    public Text highScoreText;

    private string playerName;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<GameManager>() != null)
        { playerName = GameManager.instance.playerNameText; }
        else
        {
            Instantiate(_GameManager);
            GameManager.instance.playerNameText = playerName = "Player";
        }

        ScoreText.text = playerName + $" score: {m_Points}";

        if (GameManager.instance.highScoreName.Length > 1)
        { highScoreText.text = $"Best Score : {GameManager.instance.highScoreName} : {GameManager.instance.highScore}"; }
        else { highScoreText.gameObject.SetActive(false); }

        // ...

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = playerName + $" score: {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        if (GameManager.instance.highScore < m_Points)
        {
            GameManager.instance.highScore = m_Points;
            GameManager.instance.highScoreName = playerName;
        }
        highScoreText.text = $"Best Score : {GameManager.instance.highScoreName} : {GameManager.instance.highScore}";
        highScoreText.gameObject.SetActive(true);
        GameManager.instance.SaveScore();
    }
}
