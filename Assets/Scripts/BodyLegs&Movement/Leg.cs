using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg : MonoBehaviour
{
    private GameObject _legObjInstance;
    private int _activeCollidersCount;

    [HideInInspector] public Transform[] legCollidersTransform;



    public void SwitchFromOldDrawingToTheNewOne(LineRenderer legDrawing, Transform legParent)
    {
        if(_legObjInstance != null)
        {
            Destroy(_legObjInstance.gameObject);
        }
        
        GameObject leg = Instantiate(legDrawing.gameObject, legParent);
        leg.transform.localPosition = Vector3.zero;
        leg.transform.localRotation = Quaternion.identity;
        _legObjInstance = leg;
    }



    public IEnumerator RegenerateLegBasedOnTheNewDrawing(List<Vector3> linePositions, Material newLegMaterial, float newLegWidth)
    {
        LineRenderer legLineRenderer = _legObjInstance.GetComponent<LineRenderer>();

        legLineRenderer.sharedMaterial = newLegMaterial;
        legLineRenderer.startWidth = newLegWidth;

        //Reset line to 0 so we can show it being draw based on the linePositions list
        legLineRenderer.positionCount = 0;


        //Disable all current active colliders so we can enable just the necessaary ones
        for (int i = 0; i < _activeCollidersCount; i++)
        {
            legCollidersTransform[i].gameObject.SetActive(false);
        }


        //Draw leg and generate a collider at each one of the line's point positions
        for (int i = 0; i < linePositions.Count; i++)
        {
            if(legLineRenderer != null)
            {
                legLineRenderer.positionCount++;
                legLineRenderer.SetPosition(i, linePositions[i]);

                legCollidersTransform[i].localPosition = linePositions[i];
                legCollidersTransform[i].gameObject.SetActive(true);
                yield return new WaitForSeconds(0.0001f);
            }
        }

        _activeCollidersCount = linePositions.Count;
    }
}
