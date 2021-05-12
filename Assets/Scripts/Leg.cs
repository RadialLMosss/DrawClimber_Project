using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg : MonoBehaviour
{
    GameObject legObjInstance;
    public void PrepareLeg(LineRenderer legDrawing, Transform legParent)
    {
        if(legObjInstance != null)
        {
            Destroy(legObjInstance.gameObject);
        }
        GameObject leg = Instantiate(legDrawing.gameObject, legParent);
        leg.transform.localPosition = Vector3.zero;
        leg.transform.localRotation = Quaternion.identity;
        legObjInstance = leg;
    }

    public IEnumerator GenerateLeg(List<Vector3> linePositions, SphereCollider legPointCollider)
    {
        LineRenderer legLineRenderer = legObjInstance.GetComponent<LineRenderer>();
        

        //Reset line to 0 so we can show it being draw based on the lineOriginalPositions list
        legLineRenderer.positionCount = 0;

        //Draw leg and generate a collider at each one of the line's point positions
        for (int i = 0; i < linePositions.Count; i++)
        {
            if(legLineRenderer != null)
            {
                legLineRenderer.positionCount++;
                legLineRenderer.SetPosition(i, linePositions[i]);

                GameObject legCollider = Instantiate(legPointCollider.gameObject, legObjInstance.transform);
                legCollider.transform.localPosition = linePositions[i];
                yield return new WaitForSeconds(0.0001f);
            }
        }
    }
}
