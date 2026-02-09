using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    public RollDice dice;
    public HandManager handManager;

    public List<Cards> deck = new();

    public bool Ischeck;

    
    void Start()
    {
        Ischeck = false;
        

    }
    void Update()
    {
        deck = handManager.selectedCards.ConvertAll(card => card.GetComponent<Cards>());
    }
    public void Checkcard()
    {

        for (int i = 0; i < deck.Count; i++)
        {

            if (dice.facenum == deck[i].cardData.cardnum && dice.Isrolled && !dice.Iscount && deck[i].IsReady)
            {
                
                deck[i].CardFeedback();
                dice.Isrolled = false;


            }





        }


    }

}
