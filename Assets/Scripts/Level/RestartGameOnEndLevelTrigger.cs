using UnityEngine;

public class RestartGameOnEndLevelTrigger : AbstractOnEndLevelTrigger
{
    public override void OnEndLevelTriggerEnter()
    {
        GameManager.hasGameStarted = false;
        StartCoroutine(_uiManager.ShowEndGameInterface());
    }
}
