using UnityEngine;

/// <summary>
/// Responsible for rotating the legs and moving the body forward
/// </summary>
public class BodyMovementManager : MonoBehaviour
{
    [SerializeField] private Transform _legsPivot = null;
    [SerializeField] private float _rotationSpeed = 300f;
    [SerializeField] private float _movementSpeed = 10f;
    
    [HideInInspector] public bool canStartMoving;

    private Rigidbody _rbody;

    private void Awake()
    {
        _rbody = GetComponent<Rigidbody>();
        _rbody.useGravity = false;
    }

    private void FixedUpdate()
    {
        if (!canStartMoving) return; //if false, the rest of the FixedUpdate won't work


        _legsPivot.Rotate(new Vector3(0, 0, _rotationSpeed * -1 * Time.deltaTime));
        _rbody.velocity = new Vector3(_movementSpeed, _rbody.velocity.y, 0);

        if (!_rbody.useGravity)
        {
            _rbody.useGravity = true;
        }
    }
}
