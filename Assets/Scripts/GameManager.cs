using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    RollDice dice;
    PointController pointController;
    public GameObject choosepanel;
    public GameObject textfirst;
    public GameObject textsecond;

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
        if(pointController.turn==6)
            {
            dice.Isendgame = true;
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


    }
