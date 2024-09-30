using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoDrawPile : MonoBehaviour
{
    const int TOTAL_CARDS = 108;
    const int PLAYER_INIT_CARDS = 5;
    public UnoCardStack DrawStack;//TODO
    public UnoGameManager GameManager;
    public GameObject cardPrefab;
    Sprite[] CardSprites;
    List<UnoCard> AllCards = new List<UnoCard>();

    private void Awake()
    {
        DrawStack = GetComponent<UnoCardStack>();
        CardSprites = Resources.LoadAll<Sprite>("");
        DrawStack.OnCardSelected += OnCardSelected;

    }
    public void RemoveFromDraw(UnoCard card)
    {
        DrawStack.Pop(card);
        //DrawStack.OnCardSelected -= OnCardSelected;
        //call discard tremove acc card here
    }

    private void OnCardSelected(UnoCard cardScript, int arg2, Owner owner)
    {
        //DebugControl.Log("draw", 1);

        //if (GameManager.IsLockToPlayTurn() ||
         if( GameManager.GetTurn() != (int)cardScript.LastClicked){
            return;
        }
       // GameManager.LockCardsToPlayTurn(true);

        GameManager.Players[GameManager.GetTurn()].DrawCard(cardScript,false, () =>
        {
            //if (GameManager.GetTurn() == 0)//TODO: in online have to change
            //    cardScript.ShowBackImg(false);
            //TODO: check empty draw pile and show finish panle.
            GameManager.DiscardPile.CardDrawn();
          //  GameManager.LockCardsToPlayTurn(false);
            if (GameManager.DiscardPile.CanPlayOnUpCard())
            {
                GameManager.ChangeTurn();

            }
            else
            {
                GameManager.Players[GameManager.GetTurn()].PlayAgain();
            }

           
        });
    }

    public void ShuffleAndDistribute(int playerCount)
    {
        List<int> allNumbers = new List<int>();
        for (int i = 0; i < TOTAL_CARDS; i++) {
            allNumbers.Add(i);
        }
        allNumbers = Utility.Shuffle(allNumbers);

        for (int i = 0; i < TOTAL_CARDS; i++)
        {
            AllCards.Add(MakeCard(allNumbers[i], i));
        }

        int j = 0;
        while (AllCards.Count > j)
        {
            DrawStack.PushAndMove(AllCards[j],true, () => { });
           // DrawStack.Push(AllCards[j]);
            //  AllCards[j].ShowBackImg(false);//todo

            j++;
        }
        int drawCardCount = AllCards.Count - (4 * PLAYER_INIT_CARDS + 1);
        int x = 0;
        while (!GameManager.IsAcceptableToStart(AllCards[x]))
        {
            x++;
        }
        UnoCard firstCard = AllCards[x];
        RemoveFromDraw(AllCards[x]);


        StartCoroutine(DistCardtoPlayers(drawCardCount + 1, () => {
            //DiscardPile.DiscardedCard(firstCard, () =>
            //  {
            GameManager.GameStart(firstCard);
            // }));
            
        }));
        //; DiscardPile.DiscardedCard(AllCards[x], () => {
        //    StartCoroutine(DistCardtoPlayers(drawCardCount + 1, () => {
        //        GameManager.GameStart(AllCards[x]);
        //    }));
            
        //});


    }
    IEnumerator DistCardtoPlayers(int initj, Action callback)
    {
        yield return new WaitForSeconds((5/2)*UnoGameManager.WaitForOneMoveDuration );

        initj = 0;
        for (int i = 0; i < GameManager.Players.Count; i++)
        {
            for (int j = 0; j < PLAYER_INIT_CARDS; j++)
            {
                int index = initj+i*PLAYER_INIT_CARDS+j;
                int id = DrawStack.GetAllCards().Count-1;
                UnoCard card = DrawStack.GetAllCards()[id];
                RemoveFromDraw(card);
                GameManager.Players[i].DrawCard(card, false, () => {
              //  Players[i].DrawCard(AllCards[index],false, () => {
                    if (i == 0)//TODO: in online have to change
                        card.ShowBackImg(false);
                     //   AllCards[index].ShowBackImg(false);
                   // AllCardIdx++;
                    
                });
                yield return new WaitForSeconds(UnoGameManager.WaitForOneMoveDuration*3/4);
            }
        }
        callback();
    }
    private UnoCard MakeCard(int id, int globalCardIdx)
    {
        GameObject card = Instantiate(cardPrefab);
        UnoCard cardScript = card.GetComponent<UnoCard>();
        cardScript.InitCard(id, CardSprites[id], globalCardIdx);
        return cardScript;
    }
    public UnoCard GetaCard()
    {
        return DrawStack.GetAllCards()[0];
    }

}
