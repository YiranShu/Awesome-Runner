using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour {

    public static GamePlayController instance;

    [SerializeField]
    private AudioSource audioSource;

    private Text scoreText, healthText, levelText;
    private float score, health;
    private int level;

    [HideInInspector]
    public bool canCountScore;

    private BGScroller bgScroller;

    private GameObject pausePanel;

	void Awake () {
        MakeInstance();

        scoreText = GameObject.Find(Tags.SCORE_TEXT_OBJ).GetComponent<Text>();
        healthText = GameObject.Find(Tags.HEALTH_TEXT_OBJ).GetComponent<Text>();
        levelText = GameObject.Find(Tags.LEVEL_TEXT_OBJ).GetComponent<Text>();

        bgScroller = GameObject.Find(Tags.BACKGROUND_OBJ).GetComponent<BGScroller>();

        pausePanel = GameObject.Find(Tags.PAUSE_PANEL_OBJ);
        pausePanel.SetActive(false);
    }

    void Start() {
        if(GameManager.instance.canPlayMusic) {
            audioSource.Play();
        }
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneWasLoaded; //delegate!!!
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneWasLoaded;
        instance = null;
    }

    void Update() {
        IncrementScore(1f);
        level = (int)(score / 1000);
        levelText.text = level.ToString();
    }

    void MakeInstance() {
        if(instance == null) {
            instance = this;
        }
    }

    void OnSceneWasLoaded(Scene scene, LoadSceneMode mode) {
        if(scene.name == Tags.GAMEPLAY_SCENE) {
            if(GameManager.instance.gameStartedFromMainMenu == true) {
                GameManager.instance.gameStartedFromMainMenu = false;
                score = 0;
                health = 3;
                level = 0;
            } else if(GameManager.instance.gameRestartedPlayerDied) {
                GameManager.instance.gameRestartedPlayerDied = false;
                score = GameManager.instance.score; // accumulate score after death
                health = GameManager.instance.health;
            }

            scoreText.text = score.ToString();
            healthText.text = health.ToString();
            levelText.text = level.ToString();
        }
    }

    public void IncrementHealth() {
        health++;
        healthText.text = health.ToString();
    }

    public void TakeDamage() {
        health--;
        if(health > 0) {
            // restart the game
            StartCoroutine(PlayerDied(Tags.GAMEPLAY_SCENE));
        } else {
            StartCoroutine(PlayerDied(Tags.MAINMENU_SCENE));
        }

        healthText.text = health.ToString();
    }

    public void IncrementScore(float scoreValue) {
        if(canCountScore) {
            score += scoreValue;
            scoreText.text = score.ToString();
        }
    }

    IEnumerator PlayerDied(string sceneName) {
        canCountScore = false;
        // stop BG scrolling
        bgScroller.canScroll = false;

        GameManager.instance.score = score;
        GameManager.instance.health = health;
        GameManager.instance.gameRestartedPlayerDied = true;

        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene(sceneName);

    }

    public void PauseGame() {
        bgScroller.canScroll = false;
        canCountScore = false;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame() {
        bgScroller.canScroll = true;
        canCountScore = true;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BackToMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Tags.MAINMENU_SCENE);
    }
}
