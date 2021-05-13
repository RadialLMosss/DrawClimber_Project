using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg : MonoBehaviour
{
    GameObject legObjInstance;
    int activeCollidersCount;

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

    public IEnumerator GenerateLeg(List<Vector3> linePositions, SphereCollider legPointCollider, Transform[] legColliderObjs)
    {
        LineRenderer legLineRenderer = legObjInstance.GetComponent<LineRenderer>();
        

        //Reset line to 0 so we can show it being draw based on the lineOriginalPositions list
        legLineRenderer.positionCount = 0;


        //Disable all current colliders
        for (int i = 0; i < activeCollidersCount; i++)
        {
            legColliderObjs[i].gameObject.SetActive(false);
        }


        //Draw leg and generate a collider at each one of the line's point positions
        for (int i = 0; i < linePositions.Count; i++)
        {
            if(legLineRenderer != null)
            {
                legLineRenderer.positionCount++;
                legLineRenderer.SetPosition(i, linePositions[i]);

                legColliderObjs[i].localPosition = linePositions[i];
                legColliderObjs[i].gameObject.SetActive(true);
                yield return new WaitForSeconds(0.0001f);
            }
        }

        activeCollidersCount = linePositions.Count;
    }
}
