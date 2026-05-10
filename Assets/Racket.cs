using UnityEngine;

public class Racket : MonoBehaviour
{
    public GameObject racketCollider;

    Vector3 lastPosition;

    Vector3 currentPosition;

    Vector3 racketScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastPosition = transform.position;
        currentPosition = transform.position;
        racketCollider.transform.position = currentPosition;
        racketScale = racketCollider.transform.localScale;


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(lastPosition == currentPosition)
        {
            racketCollider.transform.rotation = transform.rotation;
        }

        currentPosition = transform.position;
        racketCollider.transform.position = currentPosition;
        Vector3 colliderCenter = Vector3.Lerp(lastPosition, currentPosition, 0.5f); 
        Quaternion colliderRotation = Quaternion.Slerp(transform.rotation, racketCollider.transform.rotation, 0.5f);

        racketCollider.transform.rotation = colliderRotation;
        racketCollider.transform.position = colliderCenter;
        racketCollider.transform.localScale = new Vector3(racketScale.x, racketScale.y,racketScale.z + 1.3f *Vector3.Distance(lastPosition, currentPosition));

        lastPosition = currentPosition;

    
    }
}
