using System;
using System.Collections.Generic;
using UnityEngine;

public class UnoDiscardPile : MonoBehaviour
{
    List<UnoCard> AllCards = new List<UnoCard>();
    private UnoCard LastCard = null;
    UnoCardStack cardStack;
    int AccumulatedCards = 0;


    // Start is called before the first frame update
    void Awake()
    {
        cardStack = GetComponent<UnoCardStack>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DiscardedCard(UnoCard card, Action callback)
    {
        card.ShowBackImg(false);
        cardStack.PushAndMove(card, () => { callback(); });
        LastCard = card;
        //DebugControl.Log(LastCard.Type.ToString(),3);
    }
    public bool CanPlayOnUpCard()
    {
        return LastCard.AccumulatedCards <= 0;
    }    
    public bool CanPlayThisCard(UnoCard cardScript)
    {
        return (LastCard.AcceptsCard(cardScript) || (cardStack.HasOneCard()&&LastCard.Type==UnoCard.SpecialCard.Wild));
    }
    public bool ColorSelectIsNeeded()
    {

        return (LastCard.Type == UnoCard.SpecialCard.Wild || LastCard.Type == UnoCard.SpecialCard.Draw4Wild);

    }
    public void SetWildLastCardColor(UnoCard.CardType color)
    {
        LastCard.SetWildColor(color);
    }
    public void CardDrawn()
    {
        if (LastCard.AccumulatedCards > 0)
        {
            LastCard.AccumulatedCards--;
        }
    }
    public UnoCard GetLastCard()
    {
        return LastCard;
    }

}
