using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    RollDice dice;

    PointController pointController;
    public GameObject choosepanel;
    public GameObject textfirst;
    public GameObject textsecond;
    public GameObject endpanel;
    public TMP_Text nametext;
    public TMP_Text scoretext;
    void Start()
    {
        dice = FindAnyObjectByType<RollDice>();
        pointController = FindAnyObjectByType<PointController>();
        textsecond.SetActive(false);
        textfirst.SetActive(true);
        choosepanel.SetActive(true);
        dice.Isendgame = false;



    }

    // Update is called once per frame
    void Update()
    {
        if (pointController.turn == 6)
        {
            dice.Isendgame = true;
            if ((!dice.IsCountingAnimation && dice.Isrolled && !dice.Iscount) || (!dice.IsCountingAnimation && !dice.Isrolled && !dice.Iscount))
            {
                StartCoroutine(EndGame());
                Debug.Log("End");
                nametext.text = "Nice roll " + UserData.instance.PlayerName + "!!!";
                scoretext.text = pointController.calculator.finalpoint.ToString();

            }
        }
    }
    public void OpenChoosePanel()
    {
        choosepanel.SetActive(true);
        textsecond.SetActive(true);

    }
    public void CloseChoosePanel()
    {
        choosepanel.SetActive(false);
        textfirst.SetActive(false);

    }
    public void OpenEndPanel()
    {   
        choosepanel.SetActive(true);
        endpanel.SetActive(true);
        textfirst.SetActive(false);
        textsecond.SetActive(false);

    }
    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(0.5f);
        OpenEndPanel();
    }

}
