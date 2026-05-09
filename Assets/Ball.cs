
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

    private GameObject lastHitObject = null;

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

            if(hitInfo.transform.gameObject != lastHitObject)
            {

                if(hitInfo.transform.gameObject.transform.position.z > lastPosition.z)
                {
                    //Debug.Log("Collided with Weapon from the front!");
                    //hitInfo.transform.gameObject.GetComponent<Racquet>().SetForward(-1);
                    rb.linearVelocity = Vector3.zero;

                    if( currentSpeed * speedMultiplier >= maxSpeed)
                    {
                        currentSpeed = maxSpeed;
                    }
                    else
                    {
                        currentSpeed *= speedMultiplier;
                    }

                    rb.AddForce(-hitInfo.transform.gameObject.transform.forward * currentSpeed, ForceMode.Impulse);
                }
                else
                {
                    //Debug.Log("Collided with Weapon from the back!");
                    //hitInfo.transform.gameObject.GetComponent<Racquet>().SetForward(1);
                    rb.linearVelocity = Vector3.zero;

                    if( currentSpeed * speedMultiplier >= maxSpeed)
                    {
                        currentSpeed = maxSpeed;
                    }
                    else
                    {
                        currentSpeed *= speedMultiplier;
                    }

                    rb.AddForce(hitInfo.transform.gameObject.transform.forward * currentSpeed, ForceMode.Impulse);
                }

                lastHitObject = hitInfo.transform.gameObject;
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

            if(other.gameObject.transform.position.z > transform.position.z)
            {
                //Debug.Log("Collided with Weapon from the front!");
                //other.GetComponent<Racquet>().SetForward(-1);
                rb.linearVelocity = Vector3.zero;
                currentSpeed *= speedMultiplier;
                rb.AddForce(-other.gameObject.transform.forward * currentSpeed, ForceMode.Impulse);
            }
            else
            {
                //Debug.Log("Collided with Weapon from the back!");
                //other.GetComponent<Racquet>().SetForward(1);
                rb.linearVelocity = Vector3.zero;
                currentSpeed *= speedMultiplier;
                rb.AddForce(other.gameObject.transform.forward * currentSpeed, ForceMode.Impulse);
            }

            initialHit = true;
        }
    }
}
