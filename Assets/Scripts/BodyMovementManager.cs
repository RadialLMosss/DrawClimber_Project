using UnityEngine;

public class BodyMovementManager : MonoBehaviour
{
    [SerializeField] private Transform legsPivot = null;
    [SerializeField] private float rotationSpeed = 300f;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private Rigidbody rbody = null;
    private bool canMove;


    private void FixedUpdate()
    {
        if(canMove)
        {
            legsPivot.Rotate(new Vector3(0, 0, rotationSpeed * -1 * Time.deltaTime));
            rbody.velocity = new Vector3(movementSpeed, rbody.velocity.y, 0);
        }
    }

    public void ToggleMovimentCapacity(bool shouldMove)
    {
        canMove = shouldMove;
        rbody.useGravity = shouldMove;
    }
}
