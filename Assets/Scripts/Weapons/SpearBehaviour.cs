using UnityEngine;

public class SpearBehaviour : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasCollided = false; // Flag to ensure logic only runs once on collision
    public float rotationSpeed = 10f; // Speed of interpolation for rotation

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // If the spear has not collided, align its forward direction with its velocity
        if (!hasCollided)
        {
            Vector3 velocity = rb.linearVelocity;

            // Only adjust rotation if the object has significant velocity
            if (velocity.sqrMagnitude > 0.01f) // Avoid issues when velocity is near zero
            {
                Quaternion targetRotation = Quaternion.LookRotation(velocity, Vector3.up);

                // Smoothly interpolate the rotation towards the target rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided)
        {
            hasCollided = true; // Mark the spear as collided

            rb.isKinematic = true; // Stops physics simulation

            // Schedule the spear for destruction after 30 seconds
            Destroy(gameObject, 30f);
        }
    }
}
