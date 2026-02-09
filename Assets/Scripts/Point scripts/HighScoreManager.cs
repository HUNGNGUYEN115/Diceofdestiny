using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class HighScoreManager
{
    private const string HighScoreKey = "HIGH_SCORES";
    private const string NameKey = "HIGH_SCORE_NAMES";
    private const int MaxScores = 5;

    public static List<int> LoadScores()
    {
        if (!PlayerPrefs.HasKey(HighScoreKey))
            return new List<int>();

        string raw = PlayerPrefs.GetString(HighScoreKey);
        return raw.Split(',')
                  .Where(s => !string.IsNullOrEmpty(s))
                  .Select(int.Parse)
                  .ToList();
    }
    public static List<string> LoadNames()
    {
        if (!PlayerPrefs.HasKey(NameKey))
            return new List<string>();
        string raw = PlayerPrefs.GetString(NameKey);
        return raw.Split(',')
                  .Where(s => !string.IsNullOrEmpty(s))
                  .ToList();
    }

    public static void SaveScore(int newScore)
    {
        List<int> scores = LoadScores();
        scores.Add(newScore);

        scores = scores
            .OrderByDescending(s => s)
            .Take(MaxScores)
            .ToList();

        string raw = string.Join(",", scores);
        PlayerPrefs.SetString(HighScoreKey, raw);
        PlayerPrefs.Save();
    }
    public static void SaveNames(string newName)
    {
        List<string> names = LoadNames();
        names.Add(newName);
        names = names
            .Take(MaxScores)
            .ToList();
        string raw = string.Join(",", names);
        PlayerPrefs.SetString(NameKey, raw);
        PlayerPrefs.Save();
    }
    public static void ResetHighScores()
    {
        PlayerPrefs.DeleteKey(HighScoreKey);
        PlayerPrefs.DeleteKey(NameKey);
        PlayerPrefs.Save();
        Debug.Log("High scores reset.");
    }


}
