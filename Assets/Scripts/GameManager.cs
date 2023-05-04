using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScore;

    public Image[] lives;

    public GameObject startUI;
    public GameObject menu;
    public GameObject buttons;
    public GameObject aboutSection;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;

    [SerializeField]
    [Header ("Buttons")]
    public Button restartButton;
    public Button startButton;
    public Button aboutButton;
    public Button backButton;

    public float score;

    public int livesLeft;

    public bool gameOver = false;
    public bool paused = false;
    public bool isInvincible = false;

    public Animator playerAnim;

    SpawnManager spawnManager;


    int lifeToTake;
    Color32 takenLifeColor = new Color32(78, 89, 99, 100);
    Color32 lifeColor = new Color32(0, 5, 255, 255);
    Color32 invincibleLifeColor = new Color32(164, 164, 164, 255);

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (gameManagerInstance != null && gameManagerInstance != this)
        {
            Destroy(this);
        }
        else
        {
            gameManagerInstance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        spawnManager = SpawnManager.spawnManagerInstance;
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
    public void StartOpening()
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
    public void AboutText()
    {
        buttons.gameObject.SetActive(false);
        aboutSection.gameObject.SetActive(true);
    }

    // When you click back button
    public void BackToMenu()
    {
        buttons.gameObject.SetActive(true);
        aboutSection.gameObject.SetActive(false);
    }

    // When the player gets hit by an enemy or overlooks one and loses lives
    public void livesUpdate(int lifeLost)
    {
        if(!isInvincible) 
        {
            if (!gameOver)
            {
                livesLeft += lifeLost;
                lives[lifeToTake].color = takenLifeColor;
                lifeToTake--;
            }

            if (livesLeft == 0)
            {
                gameOver = true;

                playerAnim.SetBool("Dead", true);

                gameOverScreen.gameObject.SetActive(true);

                finalScore.text = "Your Score is: " + score;
            }
        }
    }

    // to add life point
    public void AddLife()
    {
        if(livesLeft < 3)
        {
            livesLeft++;
            lifeToTake++;
            lives[lifeToTake].color = isInvincible ? invincibleLifeColor : lifeColor;
        }
    }

    // Change color of health when invincible
    public IEnumerator InvisibilityOnOff(float invTime)
    {
        isInvincible = true;
        // Change color to Invincible color
        foreach (var l in lives)
        {
            if (l.color == lifeColor)
            {
                l.color = invincibleLifeColor;
            }
        }
        yield return new WaitForSeconds(invTime);
        isInvincible = false;
        foreach (var l in lives)
        {
            if (l.color == invincibleLifeColor)
            {
                l.color = lifeColor;
            }
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
