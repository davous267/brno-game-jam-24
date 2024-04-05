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
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            _levelManager.LoadMainMenuScene();
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public static GameManager Instance { get; private set; }

    public Player Player => _player;

    [SerializeField]
    private LevelManager _levelManager;

    [SerializeField]
    private Player _player;
}
