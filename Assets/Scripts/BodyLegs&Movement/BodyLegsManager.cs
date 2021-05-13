using UnityEngine;

/// <summary>
/// Responsible for storing the current legs of the body and its parents
/// </summary>
public class BodyLegsManager : MonoBehaviour
{
    public Transform leftLegParent;
    public Transform rightLegParent;
    [HideInInspector] public Leg currentRightLeg;
    [HideInInspector] public Leg currentLeftLeg;

    private void Awake()
    {
        if (currentLeftLeg == null)
        {
            currentLeftLeg = gameObject.AddComponent<Leg>();
        }
        if (currentRightLeg == null)
        {
            currentRightLeg = gameObject.AddComponent<Leg>();
        }
    }
}
