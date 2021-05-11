using UnityEngine;


/// <summary>
/// Responsible to switch the player's current legs to new ones.
/// </summary>
public class PlayerLegsManager : MonoBehaviour
{
    [SerializeField] Transform leftLegContainer;
    [SerializeField] Transform rightLegContainer;
    GameObject currentLeftLeg;
    GameObject currentRightLeg;



    /// <summary>
    /// Delete old legs and create new ones based on the drawing.
    /// </summary>
    public void SwitchLegs(LineRenderer newLegDrawing)
    {
        DeleteLegs(currentLeftLeg, currentRightLeg);
        CreateLegs(newLegDrawing);
    }



    void CreateLegs(LineRenderer legDrawing)
    {
        currentLeftLeg = Instantiate(legDrawing.gameObject);
        currentLeftLeg.transform.SetParent(leftLegContainer);
        currentLeftLeg.transform.localPosition = new Vector3(-1f, 0, 0);
        currentLeftLeg.transform.rotation = new Quaternion(0, 0, 180, 0);

        currentRightLeg = Instantiate(legDrawing.gameObject);
        currentRightLeg.transform.SetParent(rightLegContainer);
        currentRightLeg.transform.localPosition = new Vector3(1f, 0, 0);
    }



    void DeleteLegs(GameObject leg1, GameObject leg2)
    {
        Destroy(leg1);
        Destroy(leg2);
    }
}
