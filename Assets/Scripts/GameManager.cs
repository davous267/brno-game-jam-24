using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError("Duplicated Game Manager instances!");
        }

        _gameStartTime = Time.time;
        IsGamePaused = false;
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            IsGamePaused = !IsGamePaused;
        }

        UpdateScoreUI();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        _pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
    }

    public void GameOver()
    {
        _levelManager.LoadGameOverScene();
    }

    public void Victory()
    {
        _scoreManager.SetLastScore(GameplayTimeSeconds);
        _levelManager.LoadVictoryScene();
    }

    public static GameManager Instance { get; private set; }

    public Player Player => _player;

    public int GameplayTimeSeconds => (int)(Time.time - _gameStartTime);

    private bool IsGamePaused
    {
        get => _pauseMenu.activeInHierarchy;
        set
        {
            if(value)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    private void UpdateScoreUI()
    {
        _scoreField.text = ScoreManager.ConvertSecondsToTimeString(GameplayTimeSeconds);
    }

    [SerializeField]
    private GameObject _pauseMenu;

    [SerializeField]
    private LevelManager _levelManager;

    [SerializeField]
    private ScoreManager _scoreManager;

    [SerializeField]
    private TMPro.TMP_Text _scoreField;

    [SerializeField]
    private Player _player;

    private float _gameStartTime;
}
