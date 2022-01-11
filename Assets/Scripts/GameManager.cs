using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;
using Facebook.Unity;

public class GameManager : MonoBehaviour
{
    public int Score, BestScore, shootAmmo, randLevel;
    [SerializeField] GameObject Player, GameOverScreen, ScoreAndNextLevelScreen, StartScreen, gameScoreScreen, gameCamera, preCamera, tutorialTexts;
    [SerializeField] TextMeshProUGUI score, bestScoreText, _bestScoreText;
    [SerializeField] GameObject[] levels;
    [SerializeField] GameObject currentLevel;
    [SerializeField] Vector3 startPos;
    [SerializeField] Transform _startPos;
    Player_Controller pc;

    //[SerializeField] Camera gameCamera, preCamera;
    void Awake()
    {
        GameAnalytics.Initialize();
    }
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        pc = GameObject.Find("Player(Ship)").GetComponent<Player_Controller>();
        bestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
        _bestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
        score.text = PlayerPrefs.GetInt("Score", 0).ToString();
        score.text = Score.ToString();
        startTheGame();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, currentLevel.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "Score : " + PlayerPrefs.GetInt("Score");
        bestScoreText.text = "Best Score : " + PlayerPrefs.GetInt("BestScore");
        _bestScoreText.text = "Best Score : " + PlayerPrefs.GetInt("BestScore");
        CalculateBestScore();
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        GameOverScreen.SetActive(true);
        gameScoreScreen.SetActive(false);
        bestScoreText.text = "Best Score : " + BestScore;
        bestScoreText.text = BestScore.ToString();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, currentLevel.ToString());
    }
    public void ScoreModifier(int scoreMod) 
    {
        scoreMod = 5;
        if (Score % scoreMod >= 1)
        {
            Score = Score + 1;
        }
    }
    public void AddScore()
    {
        Score++;
        scoreBullet(Score);
        score.text = Score.ToString();
        PlayerPrefs.SetInt("Score", Score);
        Debug.Log(Score);
    }
    public void scoreBullet(int scoreMod) 
    {
        scoreMod = 5;
        if (Score % scoreMod >= 1)
        {
            shootAmmo++;
        }
        Debug.Log(shootAmmo);
    }
    public void levelSpawner() 
    {
        randLevel = Random.Range(0, levels.Length);
        Player.transform.position = startPos;
        currentLevel = Instantiate(levels[randLevel], _startPos);
    }
    public void nextlevelSpawner()
    {
        DestroyImmediate(currentLevel);
        randLevel = Random.Range(0, levels.Length);
        //Player.transform.position = startPos;
        startPos = Player.transform.position;
        currentLevel = Instantiate(levels[randLevel], _startPos);
        pc.transform.position = _startPos.position;
    }
    public void gettingNextLevel() 
    {
        Time.timeScale = 0f;
        ScoreAndNextLevelScreen.SetActive(true);
        _bestScoreText.text = "Best Score : " + BestScore;
        _bestScoreText.text = BestScore.ToString();
        gameScoreScreen.SetActive(false);
        pc.resTheShip();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, currentLevel.ToString());
    }
    public void startTheGame() 
    {
        preCamera.SetActive(false);
        StartScreen.SetActive(false);
        gameScoreScreen.SetActive(true);
        gameCamera.SetActive(true);
        Player.SetActive(true);
        levelSpawner();
        TutorialTextsEnabled();
        Invoke("TutorialTextsDisabled", 10f);
        Time.timeScale = 1f;
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, currentLevel.ToString());
    }
    public void restartLevel() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        pc.resTheShip();
        levelSpawner();
        Time.timeScale = 1f;
    }
    public void nextLevel() 
    {
        Player.SetActive(true);
        gameScoreScreen.SetActive(false);
        nextlevelSpawner();
        Time.timeScale = 1f;
        ScoreAndNextLevelScreen.SetActive(false);
        pc.stopTheShipForAMoment();
        gameScoreScreen.SetActive(true);
    }
    public void TutorialTextsEnabled() 
    {
        tutorialTexts.SetActive(true);
    }
    public void TutorialTextsDisabled() 
    {
        tutorialTexts.SetActive(false);
    }
    void CalculateBestScore()
    {
        if (PlayerPrefs.GetInt("BestScore") < Score)
        {
            PlayerPrefs.SetInt("BestScore", Score);
        }
    }
}
