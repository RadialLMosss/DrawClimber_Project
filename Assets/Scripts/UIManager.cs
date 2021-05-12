using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image drawingBoard;
    public GameObject textPanel;
    public Text centralText;
    public Button startGameButton;

    public void OnStartGameButtonClick()
    {
        startGameButton.gameObject.SetActive(false);
        StartCoroutine(ShowStartGameInterface());
    }

    IEnumerator ShowStartGameInterface()
    {
        centralText.text = "3";
        yield return new WaitForSeconds(1);
        centralText.text = "2";
        yield return new WaitForSeconds(1);
        centralText.text = "1";
        yield return new WaitForSeconds(1);
        centralText.text = "Draw!";
        yield return new WaitForSeconds(0.25f);
        drawingBoard.gameObject.SetActive(true);
        GameManager.hasGameStarted = true;
        yield return new WaitForSeconds(0.25f);
        textPanel.SetActive(false);
    }

    public IEnumerator ShowEndGameInterface()
    {
        Time.timeScale = 0.25f;
        drawingBoard.gameObject.SetActive(false);
        centralText.text = "Congratulations!";
        textPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
