using UnityEngine;


/// <summary>
/// Responsible to switch the body's current legs to new ones.
/// </summary>
public class BodyLegsManager : MonoBehaviour
{
    [SerializeField] Transform leftLegContainer = null;
    [SerializeField] Transform rightLegContainer = null;
    [SerializeField] GameObject legColliderPrefab = null;
    [SerializeField] MovementManager movimentManager = null;
    private GameObject currentLeftLeg;
    private GameObject currentRightLeg;



    /// <summary>
    /// Delete old legs and create new ones based on the drawing.
    /// </summary>
    public void SwitchBodyLegs(LineRenderer newLegDrawing)
    {
        DeleteBodyLegs(currentLeftLeg, currentRightLeg);
        CreateBodyLegs(newLegDrawing);

        //Give permission to walk with the new legs
        movimentManager.ToggleMovimentCapacity(true);
    }


    /// <summary>
    /// It will create two legs for the body based on a line renderer drawing. 
    /// </summary>
    private void CreateBodyLegs(LineRenderer lineDrawing)
    {
        //Recreate the legDrawing to start from the body's center (0, 0, 0).
        Vector3 firstPointPosition = lineDrawing.GetPosition(0);
        for (int i = 0; i < lineDrawing.positionCount; i++)
        {
            lineDrawing.SetPosition(i, lineDrawing.GetPosition(i) - firstPointPosition);
        }

        //Generate Legs
        currentLeftLeg = GenerateSingleLeg(lineDrawing, leftLegContainer, legColliderPrefab);
        currentRightLeg = GenerateSingleLeg(lineDrawing, rightLegContainer, legColliderPrefab);
    }



    /// <summary>
    /// Generate a single leg inside a container (based on a line renderer drawing).
    /// </summary>
    /// <param name="legContainer">The transform that will be the parent for the generated leg GameObject.</param>
    /// <param name="legPointCollider">The Collider GameObject that will be instantiated at each point of the leg's drawing.</param>
    private GameObject GenerateSingleLeg(LineRenderer legDrawing, Transform legContainer, GameObject legPointCollider)
    {
        GameObject legVariable = Instantiate(legDrawing.gameObject, legContainer);
        legVariable.transform.localPosition = Vector3.zero;
        legVariable.transform.localRotation = Quaternion.identity;

        legColliderPrefab.GetComponent<SphereCollider>().radius = legDrawing.startWidth/2;

        //Generate a collider at each one of the line's point positions
        for (int i = 0; i < legDrawing.positionCount; i++)
        {
            GameObject legCollider = Instantiate(legPointCollider, legVariable.transform);
            legCollider.transform.localPosition = legDrawing.GetPosition(i);
        }

        return legVariable;
    }



    /// <summary>
    /// Used to destroy the two current legs of the body.
    /// </summary>
    private void DeleteBodyLegs(GameObject leg1, GameObject leg2)
    {
        Destroy(leg1);
        Destroy(leg2);
    }
}
