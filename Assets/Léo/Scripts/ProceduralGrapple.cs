using System.Collections;
using UnityEngine;

public class ProceduralGrapple : MonoBehaviour
{
    public Rigidbody grappleObject;
    public LineRenderer lineRenderer;
    public float grappleSpeed = 10f;
    public float grappleRange = 10f;

    void Start()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, grappleRange))
            {
                Vector3 target = hit.point;
                Vector3 direction = target - transform.position;
                direction.Normalize();
                grappleObject.AddForce(direction * grappleSpeed, ForceMode.Impulse);
                StartCoroutine(UpdateGrapple());
            }
        }
    }

    IEnumerator UpdateGrapple()
    {
        while (grappleObject.velocity != Vector3.zero)
        {
            lineRenderer.SetPosition(1, grappleObject.transform.position);
            yield return null;
        }
    }
}

