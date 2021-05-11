using UnityEngine;


/// <summary>
/// Used for drawing lines on a board using the mouse position.
/// </summary>
public class DrawingController : MonoBehaviour
{

    [SerializeField] private Camera mainCamera = null;
    [SerializeField] private GameObject linePrefab = null;
    [SerializeField] private LayerMask drawingBoardMask;
    private LineRenderer currentLineRenderer;
    private Vector3 brushPosition;
    private Vector3 lastPointPosition;



    private void Update()
    {
        brushPosition = GetMousePositionWithRaycast(drawingBoardMask);
        DrawingLine();
    }
    


    /// <summary>
    /// Draw a line with the LineRenderer on the DrawingBoard LayerMask using a 'brush' object position.
    /// </summary>
    private void DrawingLine()
    {
        if (brushPosition != Vector3.zero)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                CreateLine(brushPosition);
            }
            else if (Input.GetButton("Fire1"))
            {
                AddPointToLine(currentLineRenderer, brushPosition);
            }
            else
            {
                currentLineRenderer = null;
            }
        }
    }



    private Vector3 GetMousePositionWithRaycast(LayerMask drawingMask)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, drawingMask))
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }




    /// <summary>
    /// Instantiate a GameObject with a LineRenderer component and stores this 'Line' in a variable to be modified later.
    /// </summary>
    private void CreateLine(Vector3 lineStartPosition)
    {
        GameObject brushInstance = Instantiate(linePrefab);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        //The line needs at least two points to exist
        currentLineRenderer.SetPosition(0, lineStartPosition);
        currentLineRenderer.SetPosition(1, lineStartPosition);
    }



    /// <summary>
    /// Add a new point to a line renderer, thereby extending the drawing of its line.
    /// </summary>
    private void AddPointToLine(LineRenderer lineRenderer, Vector3 newPointPosition)
    {
        if (lastPointPosition != newPointPosition && lineRenderer != null)
        {
            lineRenderer.positionCount++;
            int positionIndex = lineRenderer.positionCount - 1;
            lineRenderer.SetPosition(positionIndex, newPointPosition);
            lastPointPosition = newPointPosition;
        }
    }
}
