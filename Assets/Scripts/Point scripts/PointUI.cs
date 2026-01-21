using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointUI : MonoBehaviour
{
    public TMP_Text pointText;
    public TMP_Text multiplierText;
    public TMP_Text totalText;
    public TMP_Text multiplicationSign;
    public TMP_Text equalSign;
    public TMP_Text turnText;

    public TMP_Text[] rollHistoryTexts;

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

    public void StartCount(MonoBehaviour owner, int target, System.Action onFinished)
    {
        if (countCoroutine != null)
            owner.StopCoroutine(countCoroutine);

        countCoroutine = owner.StartCoroutine(CountTotal(target, onFinished));
    }

    IEnumerator CountTotal(int target, System.Action onFinished)
    {
        int current = 0;

        while (current <= target)
        {
            totalText.text = "<grow>" + current;

            //if (audioSource && tickSound)
            //    audioSource.PlayOneShot(tickSound);

            current++;
            yield return new WaitForSeconds(0.03f);
        }

        if (audioSource && totalSound)
            audioSource.PlayOneShot(totalSound);

        totalText.fontSize = 260;
        totalText.color = Color.yellow;

        onFinished?.Invoke();
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
}
