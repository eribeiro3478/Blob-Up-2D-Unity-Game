// CameraFollow.cs file
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float baseScrollSpeed = 2f; // Base speed at which the camera moves upwards
    public float scrollSpeed; // Variable speed of the camera that we can adjust
    public Transform player; // Reference to the player transform
    private Vector3 startPosition; // Initial position of the camera
    private bool isStopped = false; // Flag to stop camera movement
    public ScoreManager scoreManager; // Reference to the ScoreManager

    void Start()
    {
        startPosition = transform.position; // Store the initial position of the camera
        scrollSpeed = baseScrollSpeed; // Set initial scroll speed to base speed
    }

    void Update()
    {
        if (isStopped) return; // Stop camera movement if flag is true

        // Adjust the speed based on score
        if (scoreManager != null)
        {
            int currentScore = scoreManager.GetCurrentScore();
            if (currentScore >= 116)
            {
                scrollSpeed = baseScrollSpeed * 2f; // Double speed when score reaches 116
            }
            else if (currentScore >= 35)
            {
                scrollSpeed = baseScrollSpeed * 1.5f; // Increase speed by 1.5 when score reaches 35
            }
        }

        // Ensure camera moves at a constant speed upwards
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
    }

    public void StopCameraMovement()
    {
        isStopped = true;
    }

    public void RestartCameraMovement()
    {
        isStopped = false;
        transform.position = startPosition; // Reset camera to the initial position
        scrollSpeed = baseScrollSpeed; // Reset the camera speed to its original value
    }
}
