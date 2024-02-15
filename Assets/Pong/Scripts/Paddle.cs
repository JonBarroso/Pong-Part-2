using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float maxTravelHeight;
    public float minTravelHeight;
    public float speed;
    public float collisionBallSpeedUp = 1.5f;
    public string inputAxis;

    public AudioClip paddleHitSound; // Sound clip to play when the paddle gets hit
    private AudioSource audioSource;
    public AudioClip ballSpeedUpSound;

    // Update is called once per frame
    void Update()
    {
        float direction = Input.GetAxis(inputAxis);
        Vector3 newPosition = transform.position + new Vector3(0, 0, direction) * speed * Time.deltaTime;
        newPosition.z = Mathf.Clamp(newPosition.z, minTravelHeight, maxTravelHeight);

        transform.position = newPosition;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ball")) // Make sure the collision is with the ball
        {
            // Play the paddle hit sound
            if (paddleHitSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(paddleHitSound);
            }
        }
        // Debug.Log("hit");
        var paddleBounds = GetComponent<BoxCollider>().bounds;
        float maxPaddleHeight = paddleBounds.max.z;
        float minPaddleHeight = paddleBounds.min.z;

        // Get the percentage height of where it hit the paddle (0 to 1) and then remap to -1 to 1 so we have symmetry
        float pctHeight = (other.transform.position.z - minPaddleHeight) / (maxPaddleHeight - minPaddleHeight);
        float bounceDirection = (pctHeight - 0.5f) / 0.5f;
        // Debug.Log($"pct {pctHeight} + bounceDir {bounceDirection}");

        // flip the velocity and rotation direction
        Vector3 currentVelocity = other.relativeVelocity;
        float newSign = currentVelocity.x > 0 ? -1f: 1f;
        float newRotSign = newSign < 0f ? 1f: -1f;

        // Change the velocity between -60 to 60 degrees based on where it hit the paddle
        float newSpeed = currentVelocity.magnitude * collisionBallSpeedUp;
        Vector3 newVelocity = new Vector3(newSign, 0f, 0f) * newSpeed;
        newVelocity = Quaternion.Euler(0f, newRotSign * 60f * bounceDirection, 0f) * newVelocity;
        other.rigidbody.velocity = newVelocity;

        if (ballSpeedUpSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(ballSpeedUpSound);
        }

        // Debug.DrawRay(Vector3.zero, Vector3.right, Color.red);
        // Debug.DrawRay(Vector3.zero, Quaternion.Euler(0f, 30f, 0f) * Vector3.right, Color.yellow);
        // Debug.Break();

        // Debug.Log("cur vel " + currentVelocity);
        // Debug.Log("new vel " + newVelocity);
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
