using UnityEngine;

/// <summary>
/// Responsible to do something when the player enters the EndLevelTrigger
/// </summary>
public abstract class AbstractOnEndLevelTrigger : MonoBehaviour
{
    [SerializeField] protected UIManager _uiManager = null;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnEndLevelTriggerEnter();
        }
    }

    public abstract void OnEndLevelTriggerEnter();
}
