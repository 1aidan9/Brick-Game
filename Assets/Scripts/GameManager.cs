using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Ball ball { get; private set; }
    public Paddle paddle { get; private set; }
    public Brick[] bricks { get; private set; }

    const int NUM_LEVELS = 2;

    public int level = 1;
    public int score = 0;
    public int lives = 3;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        score = 0;
        lives = 3;

        LoadLevel(1);
    }

    private void LoadLevel(int level)
    {
        this.level = level;

        if (level > NUM_LEVELS)
        {
            
            LoadLevel(1);
            return;
        }

        SceneManager.LoadScene("Level" + level);
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        ball = FindObjectOfType<Ball>();
        paddle = FindObjectOfType<Paddle>();
        bricks = FindObjectsOfType<Brick>();
    }

    public void Miss()
    {
        lives--;

        if (lives > 0) {
            ResetLevel();
        } else {
            GameOver();
        }
    }

    private void ResetLevel()
    {
        paddle.ResetPaddle();
        ball.ResetBall();

        
    }

    private void GameOver()
    {
        
        NewGame();
    }

    public void Hit(int points)
    {
        score += points;

        if (Cleared()) {
            LoadLevel(level + 1);
        }
    }

    private bool Cleared()
    {
        for (int i = 0; i < bricks.Length; i++)
        {
            if (bricks[i].gameObject.activeInHierarchy && !bricks[i].unbreakable) {
                return false;
            }
        }

        return true;
    }

}
