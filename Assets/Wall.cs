using UnityEngine;

public class Wall : MonoBehaviour
{
    Ray ray = new Ray();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ray = new Ray(transform.position, transform.forward);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 1.0f);
    }

    public void SetForward(int direction)
    {
        //ray = new Ray(transform.position, direction * transform.forward);
    }
}
