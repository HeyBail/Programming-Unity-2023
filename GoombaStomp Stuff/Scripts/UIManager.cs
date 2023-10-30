using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject text;
    public GameObject menu;
    public GameObject slider;

    public GameObject levelsText;
    public GameObject levelOneButton;
    public GameObject levelTwoButton;
    public GameObject levelThreeButton;
    public GameObject levelFourButton;

    public GameObject winScreen;

    public void Start()
    {
        gameManager = GameManager.FindFirstObjectByType<GameManager>();
        if (slider != null)
        {
            slider.GetComponent<Slider>().value = (int)gameManager.difficulty;

            if (gameManager.levelsComplete > 0)
            {
                levelsText.SetActive(true);
                if (gameManager.levelsComplete >= 1) levelOneButton.SetActive(true);
                if (gameManager.levelsComplete >= 2) levelTwoButton.SetActive(true);
                if (gameManager.levelsComplete >= 3) levelThreeButton.SetActive(true);
                if (gameManager.levelsComplete >= 4) levelFourButton.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (menu != null)
        { 
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            {
                if (Cursor.visible == true)
                {
                    resume();
                    menu.SetActive(false);
                }
                else 
                {
                    pause();
                    menu.SetActive(true);
                }
            }
        }
    }
    public void pause() 
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        
        //might have to do gamestate stuff
    }

    public void resume() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;

        //might have to do gamestate stuff
    }

    public void loadLevel(int level) 
    {
        resume();
        gameManager.LoadLevel(level);
    }

    public void loadMainMenu() 
    {
        resume();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameManager.LoadLevel(0);
    }
    public void setDifficulty(System.Single newDifficulty) 
    {

        gameManager.setDifficulty(newDifficulty);
        
        switch ((GameManager.Difficulty)newDifficulty) 
        {
            case GameManager.Difficulty.ReallyEasy:
                text.GetComponent<TextMeshProUGUI>().text = "Really Easy";
                text.GetComponent<TextMeshProUGUI>().color = new Color(104,148,178);
                break;
            case GameManager.Difficulty.Easy:
                text.GetComponent<TextMeshProUGUI>().text = "Easy";
                text.GetComponent<TextMeshProUGUI>().color = Color.white;
                break;
            case GameManager.Difficulty.Normal:
                text.GetComponent<TextMeshProUGUI>().text = "Normal";
                text.GetComponent<TextMeshProUGUI>().color = Color.red;
                break;
        }
    }
    public void win() 
    {
        pause();
        winScreen.SetActive(true);
    }
}
