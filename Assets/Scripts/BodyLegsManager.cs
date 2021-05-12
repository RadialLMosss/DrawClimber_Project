using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] Material legsMaterial = null;


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
        lineOriginalPositions.Clear();

        //Recreate the legDrawing so it starts from the body's center
        Vector3 firstPointPosition = lineDrawing.GetPosition(0);
        for (int i = 0; i < lineDrawing.positionCount; i++)
        {
            //Using vector2 to automatically set position.z to 0 and avoid crooked lines/legs
            Vector2 newPosition = lineDrawing.GetPosition(i) - firstPointPosition;
            lineDrawing.SetPosition(i, newPosition);


            lineOriginalPositions.Add(newPosition);
        }

        //Generate Legs
        StartCoroutine(GenerateSingleLeg(currentLeftLeg, lineDrawing, leftLegContainer, legColliderPrefab));
        StartCoroutine(GenerateSingleLeg(currentRightLeg, lineDrawing, rightLegContainer, legColliderPrefab));
    }

    List<Vector3> lineOriginalPositions = new List<Vector3>();

    /// <summary>
    /// Generate a single leg inside a container (based on a line renderer drawing).
    /// </summary>
    /// <param name="legContainer">The transform that will be the parent for the generated leg GameObject.</param>
    /// <param name="legPointCollider">The Collider GameObject that will be instantiated at each point of the leg's drawing.</param>
    private IEnumerator GenerateSingleLeg(GameObject currentLeg, LineRenderer legDrawing, Transform legContainer, GameObject legPointCollider)
    {

        legDrawing.sharedMaterial = legsMaterial;


        GameObject legVariable = Instantiate(legDrawing.gameObject, legContainer);
        legVariable.transform.localPosition = Vector3.zero;
        legVariable.transform.localRotation = Quaternion.identity;
        legColliderPrefab.GetComponent<SphereCollider>().radius = legDrawing.startWidth / 2;
        LineRenderer legLineRenderer = legVariable.GetComponent<LineRenderer>();


        //Set all points of the line to the same spot, so we can show it being drawn later
        for (int i = 0; i < legDrawing.positionCount; i++)
        {
            legLineRenderer.SetPosition(i, Vector3.zero);
        }



        //Draw leg and generate a collider at each one of the line's point positions
        for (int i = 0; i < legLineRenderer.positionCount; i++)
        {
            legLineRenderer.SetPosition(i, lineOriginalPositions[i]);

            GameObject legCollider = Instantiate(legPointCollider, legVariable.transform);
            legCollider.transform.localPosition = lineOriginalPositions[i];
            yield return new WaitForSeconds(0.0001f);
        }




        if(currentLeg == currentLeftLeg)
        {
            currentLeftLeg = legVariable;
        }
        else
        {
            currentRightLeg = legVariable;
        }
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
