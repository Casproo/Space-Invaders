using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int lives = 3;
    [SerializeField] private int enemyScore = 20;
    [SerializeField] private int mysteryScore = 100;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject mystery;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI livesText;

    private int _score;
    private int _highscore;
    private bool _isSpawnerAvailable = true;

    public static UnityAction PlayerKilled;
    public static UnityAction EnemyKilled;
    public static UnityAction MysteryKilled;
    public static UnityAction GameRestarted;
    public static UnityAction NextWave;
    public static UnityAction GameOverScreen;

    private Player _player;

    private void Start()
    {
        AudioManager.instance.Play("Music");
        
        InvokeRepeating(nameof(MysterySpawner),25,25);
    }

    private void MysterySpawner()
    {
        if (_isSpawnerAvailable)
        {
            mystery.SetActive(true);
            AudioManager.instance.Play("Mystery");
        }
        else return;
    }

    private void OnEnable()
    {
        PlayerKilled += OnPlayerKilled;
        EnemyKilled += OnEnemyKilled;
        MysteryKilled += OnMysteryKilled;
        GameRestarted += OnNewGame;
        NextWave += Respawn;
        GameOverScreen += GameOver;
    }

    private void OnDisable()
    {
        PlayerKilled -= OnPlayerKilled;
        EnemyKilled -= OnEnemyKilled;
        MysteryKilled -= OnMysteryKilled;
        GameRestarted -= OnNewGame;
        NextWave -= Respawn;
        GameOverScreen -= GameOver;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameRestarted?.Invoke();
            GameRestarted?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnPlayerKilled()
    {
        lives--;
        UpdateLivesText(lives);
        
        player.SetActive(false);
        AudioManager.instance.Play("PlayerKilled");
        Player.Instance.isAbleToMove = false;
        if (lives > 0)
        {
            Invoke(nameof(Respawn), 1f);
        }
        else
        {
            GameOver();
        }
    }

    private void OnEnemyKilled()
    {
        UpdateScore(enemyScore);
    }

    private void OnMysteryKilled()
    {
        UpdateScore(mysteryScore);
        AudioManager.instance.Stop("Mystery");
    }

    private void OnNewGame()
    {
        if (_score > _highscore)
        {
            _highscore = _score;
        }
        
        UpdateScore(-_score);
        _score = 0;
        lives = 3;
        _isSpawnerAvailable = true;
        gameOverUI.SetActive(false);
        Enemy.Instance.OnEnemyLocationResetted();
        UpdateLivesText(lives);
        UpdateHighScore(_highscore);
        Respawn();
        InvokeRepeating(nameof(MysterySpawner),25,25);
        Time.timeScale = 1;
    }

    private void UpdateScore(int score)
    {
        _score += score;
        scoreText.text = "SCORE: " + _score;
    }

    private void UpdateHighScore(int score)
    {
        highScoreText.text = "HIGHSCORE: " + score;
    }

    private void Respawn()
    {
        player.transform.position = new Vector3(-3.96f, 0, 0);
        player.SetActive(true);
        Player.Instance.isAbleToMove = true;
    }

    private void GameOver()
    {
        CancelInvoke(nameof(MysterySpawner));
        gameOverUI.SetActive(true);
        _isSpawnerAvailable = false;
        Time.timeScale = 0;
    }

    private void UpdateLivesText(int amountLives)
    {
        livesText.text = amountLives.ToString();
    }
}