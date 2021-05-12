using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Responsible to switch the body's current legs to new ones.
/// </summary>
public class BodyLegsGenerator : MonoBehaviour
{
    [SerializeField] Transform leftLegContainer = null;
    [SerializeField] Transform rightLegContainer = null;
    [SerializeField] SphereCollider legColliderPrefab = null;
    [SerializeField] BodyMovementManager movimentManager = null;
    private Leg currentLeftLeg;
    private Leg currentRightLeg;
    [SerializeField] Material legsMaterial = null;
    List<Vector3> lineOriginalPositions = new List<Vector3>();



    /// <summary>
    /// Delete old legs and create new ones based on the drawing.
    /// </summary>
    public void SwitchBodyLegs(LineRenderer legDrawing)
    {
        LineRenderer modifiedLeg = PrepareLegDrawing(legDrawing);
        CreateConcreteBodyLegs(modifiedLeg);

        //Give permission to walk with the new legs
        movimentManager.ToggleMovimentCapacity(true);
    }


    private LineRenderer PrepareLegDrawing(LineRenderer lineDrawing)
    {
        lineOriginalPositions.Clear();

        //Recreate the lineDrawing so it starts from the body's center
        Vector3 firstPointPosition = lineDrawing.GetPosition(0);
        for (int i = 0; i < lineDrawing.positionCount; i++)
        {
            //Using vector2 to automatically set position.z to 0 and avoid crooked lines/legs
            Vector2 newPosition = lineDrawing.GetPosition(i) - firstPointPosition;
            lineDrawing.SetPosition(i, newPosition);


            lineOriginalPositions.Add(newPosition);
        }


        lineDrawing.sharedMaterial = legsMaterial;
        lineDrawing.startWidth = 0.4f;
        legColliderPrefab.radius = lineDrawing.startWidth / 2;

        return lineDrawing;
    }


    /// <summary>
    /// It will create two legs for the body based on a line renderer drawing. 
    /// </summary>
    private void CreateConcreteBodyLegs(LineRenderer legDrawing)
    {
        if(currentLeftLeg == null)
        {
            currentLeftLeg = gameObject.AddComponent<Leg>();
        }
        if(currentRightLeg == null)
        {
            currentRightLeg = gameObject.AddComponent<Leg>();
        }

        //Generate Legs
        currentLeftLeg.PrepareLeg(legDrawing, leftLegContainer);
        currentRightLeg.PrepareLeg(legDrawing, rightLegContainer);

        StartCoroutine(currentLeftLeg.GenerateLeg(lineOriginalPositions, legColliderPrefab));
        StartCoroutine(currentRightLeg.GenerateLeg(lineOriginalPositions, legColliderPrefab));
    }

    
}
