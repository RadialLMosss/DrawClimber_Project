using UnityEngine;

/// <summary>
/// Responsible for turning the line drawings into legs, turning the Abstract DrawingLineController into a DrawingLegController.
/// </summary>
public class DrawingLegController : AbstractDrawingLineController
{
    private BodyLegsGenerator _legsGenerator;
    [SerializeField] private BodyMovementManager _bodyMovementManager = null;

    private void Awake()
    {
        _legsGenerator = GetComponent<BodyLegsGenerator>();
        _drawingBrushController = GetComponent<IDrawingBrushController>();
    }


    public override void OnStartDrawingStroke()
    {
        Time.timeScale = 0;
    }

    public override void OnFinishDrawingStroke()
    {
        _legsGenerator.TurnDrawingIntoNewLegs(_currentLineRenderer);
        EraseLine(_currentLineRenderer);
        Time.timeScale = 1;
        _bodyMovementManager.canStartMoving = true;
    }
}
