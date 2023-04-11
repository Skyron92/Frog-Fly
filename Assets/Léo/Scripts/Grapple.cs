using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : MonoBehaviour
{
    [Range(0,100)] [SerializeField] private float maxDistance = 100f;
    [Tooltip("Mettre le layer Wall.")] [SerializeField] private LayerMask grappleWall;
    [Tooltip("Mettre le layer Object.")] [SerializeField] private LayerMask grappleObject;
    [SerializeField] private Transform tongueTarget;
    [SerializeField] private Transform tongueOrigin;
    [SerializeField] private LineRenderer tongueLineRenderer;
    [SerializeField] private InputActionReference grabInputActionReference;
    [SerializeField] private InputActionReference trackInputActionReference;
    private InputAction GrabInputAction => grabInputActionReference.action;
    private InputAction TrackInputAction => trackInputActionReference.action;
    private bool _isGrappling;
    public static bool isMovingByGrapple;
    private bool _isGrapplingAWall;
    private bool IsOutOfRange => Vector3.Distance(tongueOrigin.position, _target.position) > maxDistance;

    [Header("Elastic Properties\b")]
    private Transform _target;
    public float springConstant = 100f;
    public float damping = 5f;

    private Vector3 _velocity = Vector3.zero;

    private void Awake()
    {
        GrabInputAction.Enable();
        GrabInputAction.performed += context => StartGrapple();
        GrabInputAction.canceled += context => StopGrapple();
        GrabInputAction.canceled += context => isMovingByGrapple = false;
        
        TrackInputAction.Enable();
        TrackInputAction.performed += context => isMovingByGrapple = _isGrappling;
        TrackInputAction.canceled += context => isMovingByGrapple = !_isGrappling;
    }

    private void Update() {
        tongueLineRenderer.enabled = _isGrappling;
        tongueTarget.gameObject.SetActive(_isGrappling);
       /* if (Input.GetButtonDown("Fire1")) {
            StartGrapple();
        }
        else if (Input.GetButtonUp("Fire1")) {
            StopGrapple();
            isMovingByGrapple = false;
        }
        
        if (Input.GetButtonDown("Fire2") && _isGrappling) {
            isMovingByGrapple = true;
        }
        else {
            if (Input.GetButtonUp("Fire2") && _isGrappling) {
                isMovingByGrapple = false;
            }
        }*/

        if (IsOutOfRange) {
            if(!_isGrapplingAWall) ElasticMove(_target, transform);
            else ElasticMove(transform, _target);
        }
    }

    private void FixedUpdate() {
        if (_isGrappling) {
            tongueLineRenderer.SetPosition(0, tongueOrigin.position);
            tongueLineRenderer.SetPosition(1, _target.transform.position);
        }
        if (isMovingByGrapple) {
            if (_isGrapplingAWall) ElasticMove(transform, _target);
            else ElasticMove(_target, transform);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }

    private void StartGrapple() {
        if (Physics.Raycast(transform.position, transform.forward, out var hitWall, maxDistance, grappleWall)) {
            _isGrappling = true;
            _isGrapplingAWall = true;
            _target = hitWall.transform;
        }
        if (Physics.Raycast(transform.position, transform.forward, out var hitObject, maxDistance, grappleObject)) {
            _isGrappling = true;
            _isGrapplingAWall = false;
            _target = hitObject.transform;
        }
    }

    private void StopGrapple() {
        _isGrappling = false;
    }
    
    //La grenouille est attirée de façon élastique par l'objet
    private void ElasticMove(Transform mover, Transform target) {
        Vector3 displacement = target.position - mover.position;
        Vector3 force = displacement * springConstant;
        Vector3 acceleration = force / GetComponent<Rigidbody>().mass;
        _velocity += acceleration * Time.deltaTime;
        Vector3 dampingForce = -_velocity * damping;
        _velocity += dampingForce * Time.deltaTime;
        Vector3 position = mover.position + _velocity * Time.deltaTime;
        mover.position = Vector3.Lerp(mover.position, position, Time.deltaTime * 10f);
        mover.localScale = Vector3.Lerp(mover.localScale, target.localScale, Time.deltaTime * 10f);
    }
}

