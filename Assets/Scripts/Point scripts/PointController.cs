using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PointController : MonoBehaviour
{
    RollDice dice;
    public PointCalculator calculator;
    public PointUI ui;

    List<int> history = new(5);
    public List<Cards> selectedCards = new();
    public int turn=0;
    private void Awake()
    {
        dice = FindAnyObjectByType<RollDice>();
        ui = GetComponent<PointUI>();
        calculator = new PointCalculator();
    }

    private void Update()
    {
        selectedCards = HandManager.Instance.selectedCards
            .ConvertAll(c => c.GetComponent<Cards>());
        ui.ShowTurn(turn);
    }

    public void CheckFace(int face)
    {
        if (dice == null) return;

        int gained = calculator.ApplyFace(face, selectedCards);

        ui.ResetStyle();
        ui.ShowRoll(calculator.DisplayValue, calculator.Multiplier);

        //ui.ShowFinalPoint(calculator.finalpoint);
        if (calculator.matchedCard)
        {
            switch (face)
            {
                case 1:
                    ui.SetPositivePoint();
                    break;
                case 2:
                    ui.SetNegativePoint();
                    break;
                case 3:
                    ui.SetPositivePoint();
                    break;
                case 4:
                    ui.SetNegativePoint();
                    break;
                case 5:
                    ui.SetPositiveMultiplier();
                    break;
                case 6:
                    ui.SetNegativeMultiplier();
                    break;
            }
        }
        else
        {
            ui.ResetStyle();
        }

        //AddHistory(calculator.Total);
       
        dice.IsCountingAnimation = true;

        //ui.StartCount(this, calculator.Total,calculator.finalpoint, () =>
        //{

        //    calculator.StoreLastPoint(calculator.Total);

        //    HandManager.Instance.OnPointAnimationFinished();
        //    dice.IsCountingAnimation = false;

        //});
        calculator.StoreLastPoint(calculator.Total);
        StartCoroutine(ui.CountTotal(calculator.Total, calculator.finalpoint, () =>
        {
            
            HandManager.Instance.OnPointAnimationFinished();
            dice.IsCountingAnimation = false;
            if (turn >= 6)
            {
                SaveHighScore();
                //HighScoreManager.ResetHighScores();
            }
            //if (turn >= 1)
            //{

            //    //HighScoreManager.ResetHighScores();
            //}
        }
        ));

        //dice.IsCountingAnimation = false;
    }
    void SaveHighScore()
    {
        HighScoreManager.SaveScore(calculator.finalpoint);
        HighScoreManager.SaveNames(UserData.instance.PlayerName);
        Debug.Log("High name saved: " + UserData.instance.PlayerName);
    }

    void AddHistory(int value)
    {
        history.Insert(0, value);
        if (history.Count > 5)
            history.RemoveAt(history.Count - 1);

        ui.UpdateHistory(history);
    }

    public void ResetAll()
    {
        calculator.Reset();
        
        ui.Clear();
    }
}
