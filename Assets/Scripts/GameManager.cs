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
    }

    private void Update()
    {
        
    }

    private void OnDestroy()
    {
        Instance = null;
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

    [SerializeField]
    private LevelManager _levelManager;

    [SerializeField]
    private ScoreManager _scoreManager;

    [SerializeField]
    private Player _player;

    private float _gameStartTime;
}
