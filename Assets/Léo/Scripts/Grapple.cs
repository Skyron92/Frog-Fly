using UnityEngine;

public class Grapple : MonoBehaviour
{
    [Range(0,100)] [SerializeField] private float grappleSpeed;
    [Range(0,100)] [SerializeField] private float maxDistance = 100f;
    [SerializeField] private LayerMask grappleMask;
    [SerializeField] private Transform tongueTarget;
    [SerializeField] private Transform tongueOrigin;
    [SerializeField] private LineRenderer tongueLineRenderer;
    public static bool isGrappling;

    private Rigidbody _rb;
    private Vector3 _grapplePoint;

    private void Start() {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        tongueLineRenderer.enabled = isGrappling;
        tongueTarget.gameObject.SetActive(isGrappling);
        if (Input.GetButtonDown("Fire1")) {
            StartGrapple();
        }
        else if (Input.GetButtonUp("Fire1")) {
            StopGrapple();
        }
    }

    private void FixedUpdate() {
        if (isGrappling) {
            _rb.AddForce((_grapplePoint - transform.position).normalized * grappleSpeed);
            tongueTarget.position = _grapplePoint;
            tongueLineRenderer.SetPosition(0, tongueOrigin.position);
            if (Vector3.Distance(transform.position, _grapplePoint) < 1f) {
                StopGrapple();
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }

    private void StartGrapple() {
        if (Physics.Raycast(transform.position, transform.forward, out var hit, maxDistance, grappleMask)) {
            isGrappling = true;
            _grapplePoint = hit.point;
            tongueLineRenderer.SetPosition(1, _grapplePoint);
        }
    }

    private void StopGrapple() {
        isGrappling = false;
    }
}

