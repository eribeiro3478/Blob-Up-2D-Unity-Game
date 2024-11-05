// Cube.cs file
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cube : MonoBehaviour
{
    public Transform blob; // Reference to the blob's transform
    public GameObject gameOverScreen; // Reference to the Game Over screen UI
    public CameraFollow cameraFollowScript; // Reference to the CameraFollow script
    public bool isStatic = false; // Flag to control movement
    private Vector3 startPosition; // Initial position of the blob
    public AudioSource collisionSound; // Reference to the collision sound effect
    public ParticleSystem collisionExplosion; // Reference to explosion effect

    public float movementRange = 2f; // The range of movement to the left and right
    private float speed = 2f; // Base speed of movement
    private float speedModifier = 1f; // Modifier for speed variation
    private bool movingRight = true; // Direction of movement
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position; // Store initial position
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false); // Hide Game Over screen at the start
        }
    }

    void FixedUpdate()
    {
        if (isStatic) return; // If the cube is static, do not move

        float currentSpeed = speed * speedModifier * Time.fixedDeltaTime;
        Vector3 direction = movingRight ? Vector3.right : Vector3.left;

        // Use Rigidbody to move
        rb.MovePosition(rb.position + direction * currentSpeed);

        // Reverse direction when reaching movement bounds
        if (movingRight && transform.position.x >= startPosition.x + movementRange)
        {
            movingRight = false;
            speedModifier = Random.Range(0.5f, 1.5f); // Change speed modifier when direction changes
        }
        else if (!movingRight && transform.position.x <= startPosition.x - movementRange)
        {
            movingRight = true;
            speedModifier = Random.Range(0.5f, 1.5f); // Change speed modifier when direction changes
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the blob collided with the cube using tag comparison
        if (collision.gameObject.CompareTag("Blob"))
        {
            // Play collision sound
            if (collisionSound != null)
            {
                collisionSound.Play();
            }

            // Play collision explosion effect
            if (collisionExplosion != null)
            {
                collisionExplosion.transform.position = collision.contacts[0].point; // Move particle to collision point
                collisionExplosion.Play();
            }

            // Stop camera movement
            if (cameraFollowScript != null)
            {
                cameraFollowScript.StopCameraMovement();
            }

            // Reset the blob's position to the start
            blob.position = startPosition;

            // Stop any ongoing movement
            Rigidbody blobRb = blob.GetComponent<Rigidbody>();
            if (blobRb != null)
            {
                blobRb.velocity = Vector3.zero; // Stop any movement
                blobRb.angularVelocity = Vector3.zero; // Stop any rotation
            }

            // Stop the blob from following the cursor
            Blob blobScript = blob.GetComponent<Blob>();
            if (blobScript != null)
            {
                blobScript.StopFollowingCursor();
            }

            // Show the Game Over screen
            if (gameOverScreen != null)
            {
                gameOverScreen.SetActive(true);
            }
        }
    }

    public void RestartGame()
    {
        // Hide the Game Over screen
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }

        // Reset the blob's position to the start
        blob.position = startPosition;

        // Allow the blob to follow the cursor again
        Blob blobScript = blob.GetComponent<Blob>();
        if (blobScript != null)
        {
            blobScript.RestartFollowingCursor();
        }

        // Restart the camera movement
        if (cameraFollowScript != null)
        {
            cameraFollowScript.RestartCameraMovement();
        }
    }
}
