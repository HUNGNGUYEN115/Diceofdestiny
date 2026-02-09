using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;

public class PointUI : MonoBehaviour
{
    public TMP_Text pointText;
    public TMP_Text multiplierText;
    public TMP_Text totalText;
    public TMP_Text multiplicationSign;
    public TMP_Text equalSign;
    public TMP_Text turnText;
    public TMP_Text finalpointText;

    public TMP_Text[] rollHistoryTexts;
    public TMP_Text[] highscoreTexts;
    public TMP_Text[] nameTexts;

    public AudioSource audioSource;
    public AudioClip tickSound;
    public AudioClip totalSound;

    Coroutine countCoroutine;

    public void ShowRoll(int value, int multiplier)
    {
        pointText.text = "<jump>" + value;
        multiplierText.text = "<jump>" + multiplier;
        multiplicationSign.text = "<shake>x";
        equalSign.text = "<shake>=";
    }
    public void ShowTurn( int turn)
    {
        
        turnText.text = "Turn " + turn;
    }
    public void ShowFinalPoint(int finalpoint)
    {
        finalpointText.text = "Final Point: " + finalpoint;
    }

    public void SetPositivePoint()
    {
        pointText.color = Color.green;
        pointText.fontSize = 240;
    }
    public void SetPositiveMultiplier()
    {
        multiplierText.color = Color.green;
        multiplierText.fontSize = 240;
    }

    public void SetNegativePoint()
    {
        pointText.color = Color.red;
        pointText.fontSize = 120;
    }
    public void SetNegativeMultiplier()
    {
        multiplierText.color = Color.red;
        multiplierText.fontSize = 120;
    }

    public void ResetStyle()
    {
        pointText.color = Color.white;
        multiplierText.color = Color.white;
        pointText.fontSize = 180;
        multiplierText.fontSize = 180;
    }

    public void StartCount(MonoBehaviour owner, int target,int finalscore, System.Action onFinished)
    {
        if (countCoroutine != null)
            owner.StopCoroutine(countCoroutine);

        countCoroutine = owner.StartCoroutine(CountTotal(target,finalscore, onFinished));
    }

    public IEnumerator CountTotal(int target,int finalscore, System.Action onFinished)
    {
        int current = 0;

        while (current <= target)
        {
            totalText.text = "<grow>" + current;

        
            current++;
            yield return new WaitForSeconds(0.03f);
        }

        if (audioSource && totalSound)
            audioSource.PlayOneShot(totalSound);

        totalText.fontSize = 260;
        totalText.color = Color.yellow;
       ShowFinalPoint(finalscore);

        onFinished?.Invoke();
    }
    IEnumerator CountFinalpoint(int finalpoint,int target)
    {
        int current = finalpoint;
        Debug.Log("Finalpoint counting started: " + finalpoint + target);
        while (current <= finalpoint+target)
        {
            finalpointText.text = "<grow> Final score: " + current;
            current++;
            yield return new WaitForSeconds(0.03f);


        }       
        Debug.Log("Finalpoint counting ended: " + finalpoint + target);
    }

    public void UpdateHistory(List<int> history)
    {
        for (int i = 0; i < rollHistoryTexts.Length; i++)
        {
            rollHistoryTexts[i].text = i < history.Count ? history[i].ToString() : "";
        }
    }

    public void Clear()
    {
        pointText.text = "";
        multiplierText.text = "";
        totalText.text = "";
        multiplicationSign.text = "";
        equalSign.text = "";
    }

    public void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        List<int> scores = HighScoreManager.LoadScores();

        for (int i = 0; i < highscoreTexts.Length; i++)
        {
            if (i < scores.Count)
               highscoreTexts[i].text = $"{scores[i]}";
            else
                highscoreTexts[i].text = $"---";
        }
        List<string> names = HighScoreManager.LoadNames();
        for (int i = 0; i < nameTexts.Length; i++)
        {
            if (i < names.Count)
                nameTexts[i].text = $"{names[i]}";
            else if (names==null)
                nameTexts[i].text = $"xxx";
            else
                nameTexts[i].text = $"---";
        }
    }


}
