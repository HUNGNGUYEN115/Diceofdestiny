using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreUI : MonoBehaviour
{
    public TMP_Text[] scoreTexts;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        List<int> scores = HighScoreManager.LoadScores();

        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (i < scores.Count)
                scoreTexts[i].text = $"{i + 1}. {scores[i]}";
            else
                scoreTexts[i].text = $"{i + 1}. ---";
        }
        List<string> names = HighScoreManager.LoadNames();
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (i < names.Count)
                scoreTexts[i].text = $"{i + 1}. {names[i]}";
            else
                scoreTexts[i].text = $"{i + 1}. ---";
        }
    }
}
