using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScore;

    public Image[] lives;

    public GameObject startUI;
    public GameObject menu;
    public GameObject buttons;
    public GameObject aboutSection;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;

    public Button restartButton;

    public float score;

    public int livesLeft;

    public bool gameOver = false;
    public bool paused = false;

    Animator playerAnim;

    SpawnManager spawnManager;

    Button startButton;
    Button aboutButton;
    Button backButton;

    int lifeToTake;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GameObject.Find("Player").GetComponent<Animator>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        startButton = GameObject.Find("Start").GetComponent<Button>();
        aboutButton = GameObject.Find("About").GetComponent<Button>();

        startButton.onClick.AddListener(StartOpening);
        aboutButton.onClick.AddListener(AboutText);
    }

    // Update is called once per frame
    void Update()
    {
        // To display score
            scoreText.text = "Score: " + score;

        // To pause the game

        if(Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }
    }

    // When you click start
    void StartOpening()
    {
        gameOver = false;

        score = 0;
        livesLeft = 3;
        lifeToTake = 2;

        startUI.gameObject.SetActive(true);
        menu.gameObject.SetActive(false);

        spawnManager.SpawnEverything();
    }

    // When you enter about menu
    void AboutText()
    {
        buttons.gameObject.SetActive(false);
        aboutSection.gameObject.SetActive(true);

        backButton = GameObject.Find("Back Button").GetComponent<Button>();

        backButton.onClick.AddListener(BackToMenu);
    }

    // When you click back button
    void BackToMenu()
    {
        buttons.gameObject.SetActive(true);
        aboutSection.gameObject.SetActive(false);
    }

    // When the player gets hit by an enemy or overlooks one and loses lives
    public void livesUpdate(int lifeLost)
    {
        if(!gameOver)
        {
            livesLeft += lifeLost;
            lives[lifeToTake].color = new Color32(78, 89, 99, 100);
            lifeToTake--;
        }

        if(livesLeft == 0)
        {
            gameOver = true;

            playerAnim.SetBool("Dead", true);

            gameOverScreen.gameObject.SetActive(true);

            finalScore.text = "Your Score is: " + score;

            restartButton.onClick.AddListener(ReloadGame);
        }
    }

    // For restarting
    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // For pausing
    void PauseGame()
    {
        if (!paused)
        {
            paused = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0;
        }

        else
        {
            paused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
