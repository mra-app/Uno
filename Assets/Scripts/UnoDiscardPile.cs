using System;
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

        cardStack.PushAndMove(card, () => { callback(); });
        LastCard = card;
        DebugControl.Log(LastCard.Type.ToString(),3);
    }
    /*
    public void OnCardSelected(int globalCardIdx, Owner owner)//owner d? TODO!
    {
        //if (LockCards)
        //    return;

        UnoCard cardScript = AllCards[globalCardIdx];
        if ((int)owner == Turn && LastCard.AccumulatedCards <= 0)
        {

            if (LastCard.AcceptsCard(cardScript))
            {
                LockCards = true;

                DiscardStack.PushAndMove(cardScript, () =>
                {
                    Players[Turn].RemoveCard(cardScript);//for every? TODO!
                    if (cardScript.Type == UnoCard.SpecialCard.Wild || cardScript.Type == UnoCard.SpecialCard.Take4)
                        Players[Turn].SelectWildCardColor(cardScript);
                    CheckFinish(Turn);
                    ChangeTurn(cardScript);
                    LastCard = cardScript;


                    LockCards = false;
                    DebugControl.Log("yo", 3);
                });

            }
        }
        else if (owner == Owner.Draw)
        {
            //DebugControl.Log("2", 3);
         //   LockCards = true;

            Players[(int)Turn].DrawCard(cardScript, () =>
            {
                if ((int)Turn == 0)//TODO: in online have to change
                    cardScript.ShowBackImg(false);

                LastCard.AccumulatedCards--;
                if (LastCard.AccumulatedCards <= 0)
                {
                     ChangeTurn();
                }

                LockCards = false;
            });
        }



    }
    */
    public bool CanPlayOnUpCard()
    {
        return AccumulatedCards <= 0;
    }    
    public bool CanPlayThisCard(UnoCard cardScript)
    {
        return (LastCard.AcceptsCard(cardScript));
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
        if (AccumulatedCards > 0)
        {
            AccumulatedCards--;
        }
    }

}
