using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Point : MonoBehaviour
{
    
    RollDice dice;

    public TMP_Text pointText;
    public TMP_Text multiplierText;
    public TMP_Text totalText;
    public TMP_Text multiplicationsignText;
    public TMP_Text equalsignText;
    public TMP_Text turnText;
    // Initial multiplier and total
    public int multiplier = 10;
    public int total = 0;

    // Coroutine reference for counting animation
    private Coroutine countCoroutine;

    // Sound
    public AudioSource audioSource;
    public AudioClip pointsound;
    public AudioClip totalpoint;

    // History storage (latest first)
    public TMP_Text[] rollHistoryTexts;
    private readonly List<int> rollHistory = new List<int>(5);
    public int displayValue = 0;

    public static Point instance;

    public List<Cards> selectedcards = new();

    private void Awake()
    {
        dice = FindAnyObjectByType<RollDice>();
        instance = this;
    }

    void Update()
    {
        // Guard early against missing dice reference
        if (dice == null) return;

        // Update turn UI
        //if (turnText != null)
        //    turnText.text = "Turn " + dice.turn.ToString();

        selectedcards = HandManager.Instance.selectedCards.ConvertAll(card => card.GetComponent<Cards>());
    }




    public void Checkface(int face)
    {
        for (int i = 0; i < selectedcards.Count; i++)
        {
            if (face == selectedcards[i].cardData.cardnum)
            {
                if (face == 6)
                {
                    displayValue = face;
                    multiplier = 2;
                    // Text effect
                    if (multiplierText != null)
                    {
                        multiplierText.color = Color.green;
                        multiplierText.fontSize = 100;
                    }
                }
                else if (face == 3)
                {
                    displayValue = face + 10;
                    multiplier = 10;
                    // Text effect
                    if (pointText != null)
                    {
                        pointText.color = Color.red;
                        pointText.fontSize = 80;
                    }
                }
                else if (face == 1)
                {
                    displayValue = face;
                    multiplier = 10;
                    total += 20;
                }
                else if (face == 2)
                {
                    displayValue = 1;
                    multiplier = 10;
                    if (pointText != null)
                    {
                        pointText.color = Color.red;
                        pointText.fontSize = 80;
                    }
                }
                else if (face == 4)
                {
                    displayValue = face;
                    multiplier = 10;
                    total -= 12;
                    if (pointText != null)
                    {
                        pointText.color = Color.red;
                        pointText.fontSize = 80;
                    }
                }
                else if (face == 5)
                {
                    displayValue = face;
                    multiplier = 15;
                    if (pointText != null)
                    {
                        pointText.color = Color.green;
                        pointText.fontSize = 100;
                    }

                }





            }
            else
            {
                displayValue = face;
                multiplier = 10;
                // Text effect reset
                if (multiplierText != null)
                {
                    multiplierText.color = Color.white;
                    multiplierText.fontSize = 90;
                }

                if (pointText != null)
                {
                    pointText.color = Color.white;
                    pointText.fontSize = 90;
                }
            }
        }
        Debug.Log("face number is: " + face);
        Showtoltalpoint();

    }

    private void Showtoltalpoint()
    {
        if (pointText != null) pointText.text = "<jump>" + displayValue.ToString();
        if (multiplierText != null) multiplierText.text = "<jump>" + multiplier.ToString();
        if (multiplicationsignText != null) multiplicationsignText.text = "<shake>" + "x";
        if (equalsignText != null) equalsignText.text = "<shake>" + "=";
        // Not used in updated flow; kept for compatibility
        total += displayValue * multiplier;
        if (countCoroutine == null)
            countCoroutine = StartCoroutine(CountTotal(total));
        AddToHistory(total);


    }
    public void ResetUI()
    {
               total = 0;
        // Text effect reset
        if (multiplierText != null)
        {
            multiplierText.color = Color.white;
            multiplierText.fontSize = 80;
        }

        if (pointText != null)
        {
            pointText.color = Color.white;
            pointText.fontSize = 80;
        }
        pointText.text = ""; 
        multiplierText.text = "";
        totalText.text = "";
        multiplicationsignText.text = "";
        equalsignText.text = "";


    }
    IEnumerator CountTotal(int target)
    {

        // Prevent new rolls while counting
        if (dice != null)
        {
            dice.IsCountingAnimation = true;
        }

        int current = 0;
        while (current <= target)
        {
            if (totalText != null)
                totalText.text = "<grow>" + $"{current}";

            if (audioSource != null && pointsound != null)
                audioSource.PlayOneShot(pointsound);

            current++;
            yield return new WaitForSeconds(0.03f); // speed of counting
        }

        if (audioSource != null && totalpoint != null)
            audioSource.PlayOneShot(totalpoint);

        if (totalText != null)
        {
            totalText.fontSize = 120; // final size
            totalText.color = Color.yellow; // final color
        }

        if (dice != null)
        {
            dice.IsCountingAnimation = false;
            HandManager.Instance.OnPointAnimationFinished();
        }

        countCoroutine = null;
    }

    private void AddToHistory(int value)
    {
        // keep latest at index 0
        rollHistory.Insert(0, value);

        while (rollHistory.Count > 5) rollHistory.RemoveAt(rollHistory.Count - 1);
        // update UI if fields are assigned
        if (rollHistoryTexts != null && rollHistoryTexts.Length > 0)
        {
            for (int i = 0; i < rollHistoryTexts.Length; i++)
            {
                if (rollHistoryTexts[i] == null) continue;
                if (i < rollHistory.Count)
                {
                    rollHistoryTexts[i].text = rollHistory[i].ToString();
                }
                else
                {
                    rollHistoryTexts[i].text = "";
                }
            }
        }
    }
}