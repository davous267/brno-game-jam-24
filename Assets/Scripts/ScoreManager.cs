using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private void Start()
    {
        if(_lastScoreField != null)
        {
            _lastScoreField.text = GetLastScore();
        }

        if(_bestScoreField != null)
        {
            _bestScoreField.text = GetBestScore();
        }
    }

    public static string ConvertSecondsToTimeString(int seconds)
    {
        var timespan = TimeSpan.FromSeconds(seconds);
        return timespan.ToString(@"hh\:mm\:ss");
    }

    public string GetLastScore()
    {
        if (PlayerPrefs.HasKey(_lastScoreKey))
        {
            var lastScoreSec = PlayerPrefs.GetInt(_lastScoreKey);
            return ConvertSecondsToTimeString(lastScoreSec);
        }

        return _nonExistentKeyValue;
    }

    public string GetBestScore()
    {
        if (PlayerPrefs.HasKey(_bestScoreKey))
        {
            var bestScoreSec = PlayerPrefs.GetInt(_bestScoreKey);
            return ConvertSecondsToTimeString(bestScoreSec);
        }

        return _nonExistentKeyValue;
    }

    public void SetLastScore(int seconds)
    {
        PlayerPrefs.SetInt(_lastScoreKey, seconds);

        if (!PlayerPrefs.HasKey(_bestScoreKey))
        {
            SetBestScore(seconds);
        }
        else
        {
            var bestScore = PlayerPrefs.GetInt(_bestScoreKey);
            if (seconds < bestScore)
            {
                SetBestScore(seconds);
            }
        }
    }

    private void SetBestScore(int seconds)
    {
        PlayerPrefs.SetInt(_bestScoreKey, seconds);
    }

    [SerializeField]
    private TMPro.TMP_Text _lastScoreField;

    [SerializeField]
    private TMPro.TMP_Text _bestScoreField;

    private readonly string _bestScoreKey = "BestGameScore";

    private readonly string _lastScoreKey = "LastScoreKey";

    private readonly string _nonExistentKeyValue = "N/A";
}
