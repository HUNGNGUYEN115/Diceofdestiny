using System.Collections.Generic;

public class PointCalculator
{
    public int Total { get; private set; }
    public int Multiplier { get; private set; } = 10;
    public int DisplayValue { get; private set; }
    public bool matchedCard { get; private set; }
    
    public int ApplyFace(int face, List<Cards> selectedCards)
    {
        // Default values
        DisplayValue = face;
        Multiplier = 10;
        

        foreach (var card in selectedCards)
        {
            if (card == null) continue;

            if (face == card.cardData.cardnum && card.isActivate)
            {
                matchedCard = true;
                switch (face)
                {
                    case 6:
                        DisplayValue = face;
                        Multiplier = 2;
                        break;

                    case 3:
                        DisplayValue = face + 10;
                        Multiplier = 10;
                        break;

                    case 1:
                        DisplayValue = face;
                        Multiplier = 10;
                        Total += 20;
                        break;

                    case 2:
                        DisplayValue = 1;
                        Multiplier = 10;
                        break;

                    case 4:
                        DisplayValue = face;
                        Multiplier = 10;
                        Total -= 12;
                        break;

                    case 5:
                        DisplayValue = face;
                        Multiplier = 15;
                        break;
                }

                break;
            }
            else
            {
                matchedCard = false;
            }
        }

        int gained = DisplayValue * Multiplier;
        Total += gained;
        return gained;
    }

    public void Reset()
    {
        Total = 0;
        Multiplier = 10;
        DisplayValue = 0;
    }
}
