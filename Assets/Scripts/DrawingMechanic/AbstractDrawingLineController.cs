using UnityEngine;


/// <summary>
/// Draw lines on a drawing board using a virtual brush position.
/// </summary>
public abstract class AbstractDrawingLineController : MonoBehaviour
{
    protected LineRenderer _currentLineRenderer;
    protected IDrawingBrushController _drawingBrushController;
    
    public int lineLimitPoints = 150; //max lenght

    [SerializeField] private Transform _drawingBoardTransform = null;
    [SerializeField] private GameObject _linePrefab = null;

    private Vector3 _brushPosition;
    private Vector3 _lastAddedPointPosition;
    private bool _isLineAtLimitLenght;


    public void Update()
    {
        _brushPosition = _drawingBrushController.GetBrushPosition();
        if(GameManager.hasGameStarted)
        {
            DrawingLine();
        }
    }
    


    /// <summary>
    /// Draw a line with the LineRenderer using a 'brush' object position.
    /// </summary>
    private void DrawingLine()
    {
        if(_brushPosition != Vector3.zero)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                OnStartDrawingStroke();
                CreateLine(_brushPosition);
            }
            else if (Input.GetButton("Fire1") && !_isLineAtLimitLenght)
            {
                AddPointToLine(_currentLineRenderer, _brushPosition);
            }
        }

        
        if (Input.GetButtonUp("Fire1") || _isLineAtLimitLenght)
        {
            if (_currentLineRenderer != null)
            {
                OnFinishDrawingStroke();
            }
        }
    }

    abstract public void OnStartDrawingStroke();
    abstract public void OnFinishDrawingStroke();



    /// <summary>
    /// Instantiate a GameObject with a LineRenderer component and stores this 'Line' in a variable to be modified later.
    /// </summary>
    private void CreateLine(Vector3 lineStartPosition)
    {
        if (_currentLineRenderer != null)
        {
            EraseLine(_currentLineRenderer);
        }

        GameObject brushInstance = Instantiate(_linePrefab, _drawingBoardTransform);
        _currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        //The line needs at least two points to exist
        _currentLineRenderer.SetPosition(0, lineStartPosition);
        _currentLineRenderer.SetPosition(1, lineStartPosition);
    }



    protected void EraseLine(LineRenderer lineObj)
    {
        Destroy(lineObj.gameObject);
        _isLineAtLimitLenght = false;
    }



    /// <summary>
    /// Add a new point to a line renderer based on the brush position, thereby extending the line's lenght.
    /// </summary>
    private void AddPointToLine(LineRenderer lineRenderer, Vector3 newPointPosition)
    {
        if (_lastAddedPointPosition != newPointPosition && lineRenderer != null)
        {
            lineRenderer.positionCount++;
            int positionIndex = lineRenderer.positionCount - 1;
            lineRenderer.SetPosition(positionIndex, newPointPosition);
            _lastAddedPointPosition = newPointPosition;
            if(lineRenderer.positionCount >= lineLimitPoints)
            {
                _isLineAtLimitLenght = true;
            }
        }
    }
}
