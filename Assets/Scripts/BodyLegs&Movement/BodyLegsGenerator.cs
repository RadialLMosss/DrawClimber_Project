using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Responsible to generate new legs for the body.
/// </summary>
public class BodyLegsGenerator : MonoBehaviour
{ 
    [SerializeField] private SphereCollider _legColliderPrefab = null;
    [SerializeField] private Material _legMaterial = null;
    [SerializeField] private float _legWidth = 0.4f;
    [SerializeField] BodyLegsManager _bodyLegsManager = null;

    private List<Vector3> _lineAdjustedPositions = new List<Vector3>();
    private AbstractDrawingLineController _drawingLineController;

    private void Awake()
    {
        _drawingLineController = GetComponent<AbstractDrawingLineController>();
    }

    private void Start()
    {
        InstantiateLegColliders();
    }

    /// <summary>
    /// Instantiate all the necessary colliders for the legs so that later on we can just enable/disable and reposition them instead of a instantiation/destruction loop.
    /// </summary>
    void InstantiateLegColliders()
    {
        _bodyLegsManager.currentLeftLeg.legCollidersTransform = new Transform[_drawingLineController.lineLimitPoints];
        _bodyLegsManager.currentRightLeg.legCollidersTransform = new Transform[_drawingLineController.lineLimitPoints];
        
        _legColliderPrefab.radius = _legWidth / 2; //adjust the size of the collider to match the line's width

        for (int i = 0; i < _drawingLineController.lineLimitPoints; i++)
        {
            GameObject leftLegCollider = Instantiate(_legColliderPrefab.gameObject, _bodyLegsManager.leftLegParent);
            _bodyLegsManager.currentLeftLeg.legCollidersTransform[i] = leftLegCollider.transform;
            leftLegCollider.SetActive(false);
            
            GameObject rightLegCollider = Instantiate(_legColliderPrefab.gameObject, _bodyLegsManager.rightLegParent);
            _bodyLegsManager.currentRightLeg.legCollidersTransform[i] = rightLegCollider.transform; 
            rightLegCollider.SetActive(false);
        }
    }




    /// <summary>
    /// Create new legs based on the given drawing to replace the old ones.
    /// </summary>
    public void TurnDrawingIntoNewLegs(LineRenderer legDrawing)
    {
        AdjustAndStoreLineDrawingPositions(legDrawing);
        CreateConcreteBodyLegs(legDrawing);        
    }




    /// <summary>
    /// Used to generate and store new positions for the line so it can start from the body's center
    /// </summary>
    private void AdjustAndStoreLineDrawingPositions(LineRenderer lineDrawing)
    {
        _lineAdjustedPositions.Clear();

        Vector3 firstPointPosition = lineDrawing.GetPosition(0);

        for (int i = 0; i < lineDrawing.positionCount; i++)
        {
            //Using vector2 to automatically set position.z to 0 and avoid crooked lines/legs
            Vector2 newPosition = lineDrawing.GetPosition(i) - firstPointPosition;

            //Store the line's point positions so we can reset it and reconstruct it to show it being drawn later on
            _lineAdjustedPositions.Add(newPosition);
        }
    }




    /// <summary>
    /// Used to create new current concrete legs based on the line renderer drawing. 
    /// </summary>
    private void CreateConcreteBodyLegs(LineRenderer legDrawing)
    {
        _bodyLegsManager.currentLeftLeg.SwitchFromOldDrawingToTheNewOne(legDrawing, _bodyLegsManager.leftLegParent);
        _bodyLegsManager.currentRightLeg.SwitchFromOldDrawingToTheNewOne(legDrawing, _bodyLegsManager.rightLegParent);

        StartCoroutine(_bodyLegsManager.currentLeftLeg.RegenerateLegBasedOnTheNewDrawing(_lineAdjustedPositions, _legMaterial, _legWidth));
        StartCoroutine(_bodyLegsManager.currentRightLeg.RegenerateLegBasedOnTheNewDrawing(_lineAdjustedPositions, _legMaterial, _legWidth));
    }

    
}
