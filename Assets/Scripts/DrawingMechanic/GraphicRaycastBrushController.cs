using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// Responsible for keeping the 'drawing brush' position equal to the mouse position
/// </summary>
public class GraphicRaycastBrushController : MonoBehaviour, IDrawingBrushController
{
    [SerializeField] private EventSystem _eventSystem = null;

    /// <summary>
    /// Get the brush position using graphic raycast (UI Elements only)
    /// </summary>
    public Vector3 GetBrushPosition()
    {
        var pointerEventData = new PointerEventData(_eventSystem);
        pointerEventData.position = Input.mousePosition;
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        if (raycastResults.Count > 0)
        {
            return raycastResults[0].worldPosition;
        }
        else
        {
            return Vector3.zero; // => the brush is not on the drawing board.
        }
    }
}
