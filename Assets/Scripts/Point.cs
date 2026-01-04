using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.iOS.LowLevel;
public class Point : MonoBehaviour
{
    RollDice dice;

    public TMP_Text pointText;
    public TMP_Text multiplierText;
    public TMP_Text totalText;
    public TMP_Text multiplicationsignText;
    public TMP_Text equalsignText;

    // Initial multiplier and total
    public int multiplier = 10;
    public int total = 0;

 


    // Coroutine reference for counting animation
    private Coroutine countCoroutine;
    private int lastProcessedFace = 0;


    //Sound
    public AudioSource audioSource;
    public AudioClip pointsound;
    public AudioClip totalpoint;

    // History storage (latest first)
    public TMP_Text[] rollHistoryTexts;
    private readonly List<int> rollHistory = new List<int>(5);

    private void Awake()
    {
        dice = FindAnyObjectByType<RollDice>();
    }

   
    void Update()
    {

        if (dice == null) return;
        // this value will be showed in UI which not affect on the face number (Eg. when displayvalue = 13 but still facenum = 3
        int displayValue = dice.facenum;
        // no face showing -> reset UI once
        if (dice.facenum == 0)
        {
            
            if (lastProcessedFace != 0 &&dice.Isrolled)
            {
                Debug.Log("Resetting point display.");
                pointText.text = "";
                multiplierText.text = "";
                totalText.text = "";
                multiplicationsignText.text = "";
                equalsignText.text = "";
                lastProcessedFace = 0;
            }
            return;
        }

        // If same face already processed, do nothing
        if (dice.facenum == lastProcessedFace) return;

        // New face value: process once
        lastProcessedFace = dice.facenum;

        if (dice.facenum == 6)
        {
            multiplier = 2;
            dice.facenum=6;
            // Text effect
            multiplierText.color = Color.green;
            multiplierText.fontSize = 100;
        }
        else if (dice.facenum == 3)
        {
           
            displayValue = dice.facenum+10;
            multiplier = 10;
            // Text effect
            pointText.color = Color.red;
            pointText.fontSize = 100;
        }
        else
        {
            multiplier = 10;
            // Text effect
            multiplierText.color = Color.white;
            multiplierText.fontSize = 90;
            pointText.color = Color.white;
            pointText.fontSize = 90;


        }

        total = displayValue * multiplier;
        // Using TMP Animator component
        pointText.text = "<jump>" + displayValue.ToString();
        multiplierText.text = "<jump>" + multiplier.ToString();
        multiplicationsignText.text = "<shake>" + "x";
        equalsignText.text = "<shake>" + "=";
    

        // Only start counting if not already running
        if (countCoroutine == null)
        {
            countCoroutine = StartCoroutine(CountTotal(total));
        }
        AddToHistory(total);
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
            totalText.text = "<grow>"+ $"{current}";
            audioSource.PlayOneShot(pointsound);
            current++;
            yield return new WaitForSeconds(0.03f); // speed of counting
        }
        audioSource.PlayOneShot(totalpoint);
        totalText.fontSize = 120; // final size
        totalText.color= Color.yellow; // final color
        if (dice != null)
        {
            dice.IsCountingAnimation = false;
        }
        countCoroutine = null;
    }
}