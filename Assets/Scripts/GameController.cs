using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AppsFlyerSDK;
using System.Collections.Generic;
using UnityEngine.UIElements.Experimental;

public class GameController : MonoBehaviour
{

    [Header("StartGame View Settings")]
    [SerializeField] private GameObject startGameView;
    [SerializeField] private Animation startViewAnim;
    [SerializeField] private TMPro.TMP_Text startHightScoreTxt;
    [SerializeField] private Button startBtn;


    [Header("Game View Settings")]
    [SerializeField] private GameObject gameView;
    [SerializeField] private Animation gameViewAnim;
    [SerializeField] private TMPro.TMP_Text scoreTxt;
    [SerializeField] private Button pauseBtn;
    private int currentScore;


    [Header("GameOver View Settings")]
    [SerializeField] private GameObject gameOverView;
    [SerializeField] private Animation gameOverAnim;
    [SerializeField] private TMPro.TMP_Text gameOverCurrScoreTxt, gameOverHightScoreTxt;
    [SerializeField] private TMPro.TMP_Text gameOverNewScoreTxt;
    [SerializeField] private Button restartBtn;


    [Header("Pause View Settings")]
    [SerializeField] private GameObject pauseView;
    [SerializeField] private Animation pauseViewAnim;
    [SerializeField] private Button musicBtn;
    [SerializeField] private GameObject musicAccepted;
    [SerializeField] private Button soundBtn;
    [SerializeField] private GameObject soundAccepted;
    [SerializeField] private Button backBtnBtn;

    [Header("Editor")]
    [SerializeField] private GameObject mainCamera;


    public static GameController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Score = 0;

        restartBtn.onClick.AddListener(Restart);
        startBtn.onClick.AddListener(StartGame);
        pauseBtn.onClick.AddListener(PauseOn);
        backBtnBtn.onClick.AddListener(PauseOff);
        musicBtn.onClick.AddListener(MusicBtnClick);
        soundBtn.onClick.AddListener(SoundBtnClick);

        UpdatePauseBtns();
        StartViewSettings();
    }

    //текущий рекорд
    public int Score
    {
        get 
        { 
            return currentScore; 
        }
        set 
        { 
            currentScore = value;
            UpdateScore();
        }
    }

    //лучший рекорд
    private int HightScore
    {
        get
        {
            if (!PlayerPrefs.HasKey("HightScore"))
            {
                PlayerPrefs.SetInt("HightScore", 0);
            }
            return PlayerPrefs.GetInt("HightScore");
        }

        set
        {
            PlayerPrefs.SetInt("HightScore", value);
        }
    }

    //начальные настройки
    private void StartViewSettings()
    {
        startHightScoreTxt.text = $": {HightScore}";
        startGameView.SetActive(true);

        gameView.SetActive(false);
        gameOverView.SetActive(false);
    }

    //обновляем рекорд
    private void UpdateScore()
    {
        scoreTxt.text = $"{Score}";
    }

    //начинаем игру
    private void StartGame()
    {
        StartCoroutine("StartAnim");
    }

    //начинаем игру
    private IEnumerator StartAnim()
    {
        startViewAnim.Play("ViewOff");
        yield return new WaitForSeconds(0.3f);

        gameViewAnim.Play("ViewOn");
        gameView.SetActive(true);

        //запускаем игру
        PlayerController.instance.StartGame();
        PipeController.instance.StartMove();

        yield return new WaitForSeconds(0.5f);
        startGameView.SetActive(false);
    }

    //проиграли
    public void GameOver()
    {
        PipeController.instance.StopMove();
        ParallaxController.instance.StopMove();
        StopCoroutine("PauseViewOff");
        StartCoroutine("StartGameOverView");
        SendEvent();
    }

    //отпарвляем событие засчитывание рекорда после поражения
    private void SendEvent()
    {
        Dictionary<string, string> eventValues = new Dictionary<string, string>();
        eventValues.Add(AFInAppEvents.SCORE, $"{Score}");
        AppsFlyer.sendEvent(AFInAppEvents.SCORE, eventValues);
    }

    //запускакем экран конца игры
    private IEnumerator StartGameOverView()
    {
        gameView.SetActive(false);
        pauseView.SetActive(false);
        yield return new WaitForSeconds(1f);

        gameOverNewScoreTxt.gameObject.SetActive(false);
        gameOverCurrScoreTxt.text = $": {Score}";
        if (Score > HightScore)
        {
            //новый рекорд
            HightScore = Score;
            gameOverNewScoreTxt.gameObject.SetActive(true);
        }
        gameOverHightScoreTxt.text = $": {HightScore}";

        gameOverAnim.Play("ViewOn");
        gameOverView.SetActive(true);
    }

    //включаем паузу
    private void PauseOn()
    {
        //останавлиавем все
        PipeController.instance.StopMove();
        ParallaxController.instance.StopMove();
        PlayerController.instance.StopPlayer();

        gameView.SetActive(false);

        pauseViewAnim.Play("ViewOn");
        pauseView.SetActive(true);
    }

    //выключаем паузу
    private void PauseOff()
    {
        StartCoroutine("PauseViewOff");
    }

    //выключаем паузу
    private IEnumerator PauseViewOff()
    {
        pauseViewAnim.Play("ViewOff");
        yield return new WaitForSeconds(0.8f);
        pauseView.SetActive(false);

        gameViewAnim.Play("ViewOn");
        gameView.SetActive(true);

        //запускаем все
        PipeController.instance.StartMove();
        ParallaxController.instance.StartMove();
        PlayerController.instance.ContinuePlayer();
    }

    //обновляем иконки кнопок звука и музыки
    private void UpdatePauseBtns()
    {
        soundAccepted.SetActive(BgSoundsController.instance.Sound);
        musicAccepted.SetActive(BgSoundsController.instance.Music);
    }

    //нажатие на музыку
    private void MusicBtnClick()
    {
        BgSoundsController.instance.Music = !BgSoundsController.instance.Music;
        UpdatePauseBtns();
    }

    //нажатие на звуки
    private void SoundBtnClick()
    {
        BgSoundsController.instance.Sound = !BgSoundsController.instance.Sound;
        UpdatePauseBtns();
    }

    private void Restart()
    {
        StartCoroutine(openScene(SceneManager.GetActiveScene().name));
    }

    //открываем сцену после задержки для перехода
    IEnumerator openScene(string sceneName)
    {
        float fadeTime = mainCamera.GetComponent<SceneFading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }
}
