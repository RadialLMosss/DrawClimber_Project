using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image drawingBoard = null;
    [SerializeField] private GameObject textPanel = null;
    [SerializeField] private Text centralText = null;
    [SerializeField] private Button startGameButton = null;

    public void OnStartGameButtonClick()
    {
        startGameButton.gameObject.SetActive(false);
        StartCoroutine(ShowStartGameInterface());
    }


    private IEnumerator ShowStartGameInterface()
    {
        var interval = new WaitForSeconds(1);

        centralText.text = "3";
        yield return interval;
        centralText.text = "2";
        yield return interval;
        centralText.text = "1";
        yield return interval;
        centralText.text = "Draw!";
        yield return interval;

        textPanel.SetActive(false);
        drawingBoard.gameObject.SetActive(true);
        GameManager.hasGameStarted = true;
    }


    public IEnumerator ShowEndGameInterface()
    {
        Time.timeScale = 0.25f; //slow motion effect

        drawingBoard.gameObject.SetActive(false);
        centralText.text = "Congratulations!";
        
        textPanel.SetActive(true);
        
        yield return new WaitForSeconds(1f);
        
        Time.timeScale = 1f;

        SceneManager.LoadScene(0); //restart scene
    }
}
