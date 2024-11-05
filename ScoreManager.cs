// ScoreManager.cs file
using System.Collections;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the score UI component
    public TextMeshProUGUI highScoreText; // Reference to the high score UI component
    public TextMeshProUGUI levelNotificationText; // Reference to the level notification text component
    public Transform blob; // Reference to the blob's transform
    public GameObject level2; // Reference to Level 2 GameObject
    public GameObject level3; // Reference to Level 3 GameObject
    public AudioSource levelUpSound; // Reference to the level-up sound effect

    private float startHeight; // Initial height of the blob
    private int currentScore = 0;
    private int highScore = 0;

    void Start()
    {
        startHeight = blob.position.y; // Record the initial height
        highScore = PlayerPrefs.GetInt("HighScore", 0); // Load the saved high score
        UpdateScoreText();
        UpdateHighScoreText();

        // Initially disable levels 2 and 3
        if (level2 != null) level2.SetActive(false);
        if (level3 != null) level3.SetActive(false);

        // Hide the level notification text initially
        if (levelNotificationText != null)
        {
            levelNotificationText.text = "";
        }
    }

    void Update()
    {
        // Calculate the score based on how high the blob has traveled from the start point
        currentScore = Mathf.Max(0, (int)(blob.position.y - startHeight));
        UpdateScoreText(currentScore);

        // Update the high score if the current score is greater
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore); // Save the new high score
            UpdateHighScoreText();
        }

        // Activate level 2 when score reaches 35
        if (currentScore >= 35 && level2 != null && !level2.activeInHierarchy)
        {
            level2.SetActive(true);
            PlayLevelUpSound();
            StartCoroutine(ShowLevelNotification("You reached Level 2!"));
        }

        // Activate level 3 when score reaches 116
        if (currentScore >= 116 && level3 != null && !level3.activeInHierarchy)
        {
            level3.SetActive(true);
            PlayLevelUpSound();
            StartCoroutine(ShowLevelNotification("Seems like your luck got you all the way up here. LEVEL 3!!"));
        }

        // Play level-up sound and show message when score reaches 230
        if (currentScore >= 230 && levelUpSound != null && !levelUpSound.isPlaying)
        {
            PlayLevelUpSound();
            StartCoroutine(ShowLevelNotification("Winner Winner Chicken Dinner!!!"));
        }
    }

    private void UpdateScoreText(int score = 0)
    {
        scoreText.text = "Score: " + score;
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = "High Score: " + highScore;
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreText();
    }

    // Play the level-up sound effect
    private void PlayLevelUpSound()
    {
        if (levelUpSound != null)
        {
            levelUpSound.Play();
        }
    }

    // Coroutine to show level notification
    private IEnumerator ShowLevelNotification(string message)
    {
        if (levelNotificationText != null)
        {
            levelNotificationText.text = message; // Set the notification text
            yield return new WaitForSeconds(3f); // Show the message for 3 seconds
            levelNotificationText.text = ""; // Clear the notification text
        }
    }
}




