// Blob.cs file
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{
    private Camera mainCamera; // Reference to the main camera
    private Rigidbody rb; // Reference to Rigidbody component
    private bool isStopped = false; // Flag to stop following the cursor
    public ParticleSystem dustTrail; // Reference to the particle effect for the dust trail

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();

        // Ensure the particle effect is stopped at the start
        if (dustTrail != null)
        {
            dustTrail.Stop();
        }
    }

    void FixedUpdate()
    {
        if (isStopped)
        {
            // Stop the particle effect if the Blob is stopped
            if (dustTrail != null && dustTrail.isPlaying)
            {
                dustTrail.Stop();
            }
            return; // Stop blob from following the cursor if flag is true
        }

        // Get the mouse position and convert it to world position
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.transform.position.z * -1));

        // Calculate the screen bounds to constrain the blob
        Vector3 minBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z - mainCamera.transform.position.z));
        Vector3 maxBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z - mainCamera.transform.position.z));

        // Clamp the target position to stay within the screen bounds
        Vector3 targetPosition = new Vector3(
            Mathf.Clamp(mousePosition.x, minBounds.x, maxBounds.x),
            Mathf.Clamp(mousePosition.y, minBounds.y, maxBounds.y),
            transform.position.z
        );

        // Move the blob towards the target position
        rb.MovePosition(Vector3.Lerp(transform.position, targetPosition, 10f * Time.deltaTime));

        // Play the particle effect if it's not already playing
        if (dustTrail != null && !dustTrail.isPlaying)
        {
            dustTrail.Play();
        }
    }

    public void StopFollowingCursor()
    {
        isStopped = true;
        // Stop the particle effect when the Blob stops following the cursor
        if (dustTrail != null)
        {
            dustTrail.Stop();
        }
    }

    public void RestartFollowingCursor()
    {
        isStopped = false;
        // Start the particle effect when the Blob starts moving again
        if (dustTrail != null)
        {
            dustTrail.Play();
        }
    }
}







