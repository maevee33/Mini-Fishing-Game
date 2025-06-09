using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    int _score;
    int _highScore;
    public TextMeshProUGUI scoreText;
    bool isHighScore = false;

    public static ScoreManager instance;

    public GameObject gameOverUI;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI endGameScoreText;
    public GameObject highscoreIndicator;
    void Awake() {
        instance = this;
        LoadHighScore(); // Load high score when the game starts
    }

    void Start()
    {
        FishingManager.instance.OnGameEnd.AddListener(EndGameSequence); // Subscribe to the game end event
        FishingManager.instance.OnGameStart.AddListener(Reset); // Subscribe to the game start event
    }

    void OnDestroy()
    {
        FishingManager.instance.OnGameEnd.RemoveListener(EndGameSequence); // Unsubscribe from the game end event
        FishingManager.instance.OnGameStart.RemoveListener(Reset); // Unsubscribe from the game start event        
    }

    public int score {
        get { return _score; }
        set
        {
            _score = value;
            
            if(_score > _highScore)
            {
                _highScore = _score;
                isHighScore = true;
            }
            
            scoreText.text = _score.ToString();
        }
    }

    public void SaveHighScore() {
        // Save the high score to PlayerPrefs
        PlayerPrefs.SetInt("HighScore", _highScore);
        PlayerPrefs.Save();

        highscoreIndicator.SetActive(true); // Show the high score indicator
    }

    public void LoadHighScore() {
        // Load the high score from PlayerPrefs
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public void EndGameSequence() {
        gameOverUI.SetActive(true); // Show the game over UI
        endGameScoreText.text = "Your score: " + score.ToString(); // Show the final score
        highScoreText.text = "High score: " + _highScore.ToString(); // Show the high score
        highscoreIndicator.SetActive(false); // Hide the high score indicator
        

        // Check if the current score is a new high score
        if (isHighScore) {
            // Save the new high score
            SaveHighScore();
            isHighScore = false; // Reset the flag after saving
        }

        CurrencyManager.instance.AddCoins(_score); // Add score to coins

    }

    public void Reset()
    {
        score = 0;
        highscoreIndicator.SetActive(false); // Hide the high score indicator
        gameOverUI.SetActive(false); // Hide the game over UI   
    }

}
