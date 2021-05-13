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
    List<Vector3> lineAdjustedPositions = new List<Vector3>();
    private float legWidth = 0.4f;

    [SerializeField]DrawingLineController drawingLine;
    Transform[] leftLegColliders;
    Transform[] rightLegColliders;
    void InstantiateLegColliders()
    {
        leftLegColliders = new Transform[drawingLine.lineLimitPoints];
        rightLegColliders = new Transform[drawingLine.lineLimitPoints];
        
        legColliderPrefab.radius = legWidth / 2;

        for (int i = 0; i < drawingLine.lineLimitPoints; i++)
        {
            GameObject leftLegCollider = Instantiate(legColliderPrefab.gameObject, leftLegContainer);
            leftLegColliders[i] = leftLegCollider.transform;
            leftLegCollider.SetActive(false);
            
            GameObject rightLegCollider = Instantiate(legColliderPrefab.gameObject, rightLegContainer);
            rightLegColliders[i] = rightLegCollider.transform; 
            rightLegCollider.SetActive(false);
        }
    }

    private void Start()
    {
        InstantiateLegColliders();
    }

    /// <summary>
    /// Create new legs based on the drawing to replace the old ones.
    /// </summary>
    public void SwitchBodyLegs(LineRenderer legDrawing)
    {
        AdjustAndStoreLineDrawingPositions(legDrawing);
        CreateConcreteBodyLegs(legDrawing);
        
        //Give permission to walk with the new legs
        movimentManager.ToggleMovimentCapacity(true);
        
    }


    /// <summary>
    /// Used to generate and store new positions for the line so it can start from the body's center
    /// </summary>
    /// <param name="lineDrawing"></param>
    private void AdjustAndStoreLineDrawingPositions(LineRenderer lineDrawing)
    {
        lineAdjustedPositions.Clear();

        Vector3 firstPointPosition = lineDrawing.GetPosition(0);

        for (int i = 0; i < lineDrawing.positionCount; i++)
        {
            //Using vector2 to automatically set position.z to 0 and avoid crooked lines/legs
            Vector2 newPosition = lineDrawing.GetPosition(i) - firstPointPosition;

            //Later the line will be reseted so we can show it being drawn, so we need to store its points positions first
            lineAdjustedPositions.Add(newPosition);
        }
    }


    /// <summary>
    /// Used to recreate the current concrete legs based on the new line renderer drawing. 
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

        StartCoroutine(currentLeftLeg.GenerateLeg(lineAdjustedPositions, legColliderPrefab, leftLegColliders, legsMaterial, legWidth));
        StartCoroutine(currentRightLeg.GenerateLeg(lineAdjustedPositions, legColliderPrefab, rightLegColliders, legsMaterial, legWidth));
    }

    
}
