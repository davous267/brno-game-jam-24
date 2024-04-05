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
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape)) // TODO Probably remove and replace with proper pause menu
        {
            _levelManager.LoadMainMenuScene();
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void GameOver()
    {
        Debug.Log("Player died!");
    }

    public static GameManager Instance { get; private set; }

    public Player Player => _player;

    [SerializeField]
    private LevelManager _levelManager;

    [SerializeField]
    private Player _player;
}
