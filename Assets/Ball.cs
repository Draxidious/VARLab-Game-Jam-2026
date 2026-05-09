
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // -- Private variables --
    private Rigidbody rb;

    public float currentSpeed = 1f; // temp public variable for testing

    private Vector3 lastPosition;

    private LayerMask collisionMask;

    private bool initialHit = false;

    private float forwardDirection = 1f;

    private GameObject lastHitObject = null;

    private GameObject currentHitObject = null;

    private Vector3 velocityDirection = Vector3.zero;

    // -- Public variables --

    [SerializeField]float speedMultiplier = 1.1f;
    [SerializeField]float maxSpeed = 20f; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;
        collisionMask = LayerMask.GetMask("Racket", "Wall");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.DrawLine(lastPosition, transform.position, Color.red, 1.0f);

        if(initialHit && Physics.Linecast(lastPosition, transform.position, out RaycastHit hitInfo, collisionMask))
        {
            //Debug.Log("Hit");

            currentHitObject = hitInfo.transform.gameObject;

            if(currentHitObject != lastHitObject)
            {
                transform.position = hitInfo.point;

                // When it hits a wall, we want to maintain the same speed but reflect the ball rather than just using the forward vector
                if(currentHitObject.layer == LayerMask.NameToLayer("Wall")) 
                {

                    Vector3 direction = Vector3.Reflect(rb.linearVelocity.normalized, hitInfo.normal);

                    rb.linearVelocity = direction * currentSpeed;
                }
                // When it hits a racket, we want to apply force based on the direction of the hit and increase the speed
                else 
                {
                    

                    if( currentSpeed * speedMultiplier >= maxSpeed)
                    {
                        currentSpeed = maxSpeed;
                    }
                    else
                    {
                        currentSpeed *= speedMultiplier;
                    }

                    rb.AddForce(hitInfo.normal * currentSpeed, ForceMode.Impulse);
                }

                

                lastHitObject = currentHitObject;
            }
            else
            {
                Debug.Log("Hit the same object again, ignoring.");
            }

            
            //Debug.Log("Raycast hit: " + hitInfo.collider.gameObject.name);
        }

        lastPosition = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!initialHit && other.gameObject.layer == LayerMask.NameToLayer("Racket"))
        {

            Vector3 localPos = other.transform.InverseTransformPoint(transform.position);

            if(localPos.z > 0)
            {
                // Ball is in FRONT of the other object
                rb.linearVelocity = Vector3.zero;
                currentSpeed *= speedMultiplier;
                rb.AddForce(other.transform.forward * currentSpeed, ForceMode.Impulse);
            }
            else
            {
                // Ball is BEHIND the other object
                rb.linearVelocity = Vector3.zero;
                currentSpeed *= speedMultiplier;
                rb.AddForce(-other.transform.forward * currentSpeed, ForceMode.Impulse);
            }

            initialHit = true;
        }
    }
}
