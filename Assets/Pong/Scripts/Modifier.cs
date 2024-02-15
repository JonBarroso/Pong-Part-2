using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Modifier : MonoBehaviour
{
    public float slowdownFactor = 0.5f;
    public AudioClip pickupSound; // Sound clip to play when the ball picks up the object
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to the ball
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Modifier"))
        {
            Rigidbody ballRigidbody = GetComponent<Rigidbody>(); // Get the Rigidbody component of the ball
            if (ballRigidbody != null)
            {
                ballRigidbody.velocity *= slowdownFactor; // Slow down the ball's velocity
            }
            else
            {
                Debug.LogWarning("Ball Rigidbody not found!");
            }
            if (pickupSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(pickupSound);
            }
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("Modifier2"))
        {
            // Warp the ball to a different position on the x-z plane
            Vector3 newPosition = new Vector3(Random.Range(-2f, 2f), transform.position.y, Random.Range(-2f, 2f));
            transform.position = newPosition;

            // Play the pickup sound effect if it's assigned and the AudioSource is available
            if (pickupSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(pickupSound);
            }

            other.gameObject.SetActive(false); // Deactivate the modifier2 object (capsule)
        }
    }
}
