using UnityEngine;

public class ElasticObject : MonoBehaviour
{
    public Transform target;
    public float springConstant = 100f;
    public float damping = 5f;

    private Vector3 velocity = Vector3.zero;

    void Update() {
        Vector3 displacement = target.position - transform.position;
        Vector3 force = displacement * springConstant;
        Vector3 acceleration = force / GetComponent<Rigidbody>().mass;
        velocity += acceleration * Time.deltaTime;
        Vector3 dampingForce = -velocity * damping;
        velocity += dampingForce * Time.deltaTime;
        Vector3 position = transform.position + velocity * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 10f);
        transform.localScale = Vector3.Lerp(transform.localScale, target.localScale, Time.deltaTime * 10f);
    }
}

