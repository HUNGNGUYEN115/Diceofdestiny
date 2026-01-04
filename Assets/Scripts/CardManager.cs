using UnityEngine;
using System.Collections;
public class CardManager : Cards
{
    public RollDice dice;
    public Cards[] deck = new Cards[5];

    public bool Ischeck;

    
    void Start()
    {
        Ischeck = false;
        
    }
    void Update()
    {

    }
    public void Checkcard()
    {

        for (int i = 0; i < deck.Length; i++)
        {
              
            if (dice.facenum == deck[i].cardData.cardnum  && dice.Isrolled&&!dice.Iscount)
            {

                deck[i].CardFeedback();
                dice.Isrolled = false;


            }
            
            



        }


    }
    
}
