using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private BrickHandler brickHandler;
    public int levelsComplete;


    public enum Difficulty //move into game manager
    {
        ReallyEasy,
        Easy,
        Normal
    }
    public Difficulty difficulty;

    public void Awake()
    {
        GameObject[] gameManagers = GameObject.FindGameObjectsWithTag("GameManager");

        if (gameManagers.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        levelsComplete = 0;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void LoadLevel(int level)
    {
        levelsComplete = Mathf.Max(level, levelsComplete);

        SceneManager.LoadScene("level" + level);
    }

    public void setDifficulty(System.Single newDifficulty)
    {
        difficulty = (Difficulty)newDifficulty;

        if (brickHandler == null)
        { 
            brickHandler = BrickHandler.FindFirstObjectByType<BrickHandler>();
        }

        switch (difficulty) 
        {
            case GameManager.Difficulty.ReallyEasy:
                brickHandler.setDifficulty(difficulty);
                brickHandler.gameObject.SetActive(true);
                break;
            case GameManager.Difficulty.Easy:
                brickHandler.setDifficulty(difficulty);
                brickHandler.gameObject.SetActive(true);
                break;
            case GameManager.Difficulty.Normal:
                brickHandler.setDifficulty(difficulty);
                brickHandler.gameObject.SetActive(false);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
