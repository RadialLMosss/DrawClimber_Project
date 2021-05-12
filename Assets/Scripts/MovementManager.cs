using UnityEngine;

public class MovementManager : MonoBehaviour
{
    [SerializeField] private Transform legsPivot = null;
    [SerializeField] private float rotationSpeed = 750f;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private Rigidbody rbody = null;
    private Vector3 rotationVector;
    private bool canMove;

    private void Start()
    {
        rotationVector = new Vector3(0, 0, rotationSpeed * -1 * Time.deltaTime);
    }

    private void Update()
    {
        if(canMove)
        {
            legsPivot.Rotate(rotationVector);
            rbody.velocity = new Vector3(movementSpeed, rbody.velocity.y, 0);
        }
    }

    public void ToggleMovimentCapacity(bool shouldMove)
    {
        canMove = shouldMove;
        rbody.useGravity = shouldMove;
    }
}
