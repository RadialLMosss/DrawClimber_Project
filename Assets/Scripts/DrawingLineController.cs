using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Used for drawing lines on a board using the mouse position.
/// </summary>
public class DrawingLineController : MonoBehaviour
{
    [SerializeField] private BodyLegsGenerator playerLegs;
    [SerializeField] private Transform drawingBoard = null;
    [SerializeField] private GameObject linePrefab = null;
    private LineRenderer currentLineRenderer;
    private Vector3 brushPosition;
    private Vector3 lastPointPosition;

    private bool isLineAtLimitPoints;
    public int lineLimitPoints = 125;

    // raycast variables
    [SerializeField] EventSystem m_EventSystem = null;

    private void Update()
    {
        brushPosition = GetMousePositionWithGraphicRaycast();
        if(GameManager.hasGameStarted)
        {
            DrawingLine();
        }
    }
    


    /// <summary>
    /// Draw a line with the LineRenderer on the DrawingBoard LayerMask using a 'brush' object position.
    /// </summary>
    private void DrawingLine()
    {
        if (Input.GetButtonDown("Fire1") && brushPosition != Vector3.zero)
        {
            Time.timeScale = 0;
            if(currentLineRenderer != null)
            {
                EraseLine(currentLineRenderer);
            }
            CreateLine(brushPosition);
        }
        else if (Input.GetButton("Fire1") && brushPosition != Vector3.zero && !isLineAtLimitPoints)
        {
            AddPointToLine(currentLineRenderer, brushPosition);
        }
        else if (Input.GetButtonUp("Fire1") || isLineAtLimitPoints)
        {
            //trocar pernas atuais pelas novas recem desenhadas
            if(currentLineRenderer != null)
            {
                playerLegs.SwitchBodyLegs(currentLineRenderer);
                EraseLine(currentLineRenderer);
                Time.timeScale = 1;
            }
        }
    }


    private Vector3 GetMousePositionWithGraphicRaycast()
    {
        var pointerEventData = new PointerEventData(m_EventSystem);
        pointerEventData.position = Input.mousePosition;
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        if (raycastResults.Count > 0)
        {
                return raycastResults[0].worldPosition;
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
        GameObject brushInstance = Instantiate(linePrefab, drawingBoard);
        
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        //The line needs at least two points to exist
        currentLineRenderer.SetPosition(0, lineStartPosition);
        currentLineRenderer.SetPosition(1, lineStartPosition);
    }


    /// <summary>
    /// Destroy a GameObject that has a LineRenderer component.
    /// </summary>
    private void EraseLine(LineRenderer lineObj)
    {
        Destroy(lineObj.gameObject);
        isLineAtLimitPoints = false;
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
            if(lineRenderer.positionCount >= lineLimitPoints)
            {
                isLineAtLimitPoints = true;
            }
        }
    }
}
