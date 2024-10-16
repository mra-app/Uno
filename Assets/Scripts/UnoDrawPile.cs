using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoDrawPile : MonoBehaviour
{
    
   
    public GameObject cardPrefab;

    Sprite[] CardSprites;
    const int TOTAL_CARDS = 108;
    private List<UnoCard> AllCards = new List<UnoCard>();
    UnoCardStack DrawStack;
    UnoGameManager GameManager;
    private void Awake()
    {
        DrawStack = GetComponent<UnoCardStack>();
        CardSprites = Resources.LoadAll<Sprite>("");
        DrawStack.OnCardSelected += OnCardSelected;//set a call back for it's stack

    }

    public void SetManager(UnoGameManager manager)
    {
        GameManager = manager;
    }
    public void RemoveFromDraw(UnoCard card)
    {
        DrawStack.Pop(card);
    }

    /// <summary>
    /// Draw a card to the player who's turn is now
    /// </summary>
    /// <param name="cardScript"></param>
    /// <param name="arg2"></param>
    /// <param name="owner"></param>
    private void OnCardSelected(UnoCard cardScript)
    {
         if( GameManager.GetTurn() != (int)cardScript.LastClicked){//if its not the turn of the player clicked on this card,
            return;}

        GameManager.Players[GameManager.GetTurn()].DrawCard(cardScript,false, () =>
        {
            if (DrawStack.IsEmpty())
            {
                GameManager.EmptyDrawPileShowWinner();
            }
            else
            {
                GameManager.DiscardPile.CardDrawn(); ;
                if (GameManager.DiscardPile.CanPlayOnUpCard())
                {
                    GameManager.ChangeTurn();
                }
                else
                {
                    GameManager.Players[GameManager.GetTurn()].PlayAgain();
                }
            }
        });
    }


    public void ShuffleAndDistribute(int playerCount)
    {
        CreateAllCards();
        //push all cards to draw stack
        int j = 0;
        while (AllCards.Count > j)
        {
            DrawStack.PushAndMove(AllCards[j],true, () => { });
            j++;
        }
        //the number of cards that must stay in draw pile
        int drawCardCount = AllCards.Count - (4 * GameManager.PLAYER_INIT_CARDS + 1);
        //the first card to discard to start the game
        UnoCard firstCard = FindFirstCard();
        //give cards to players then discard the first card
        StartCoroutine(DistCardtoPlayers(drawCardCount + 1, () => {
            GameManager.GameStart(firstCard);
        }));
    }
    IEnumerator DistCardtoPlayers(int initj, Action callback)
    {
        yield return new WaitForSeconds((5/2)*UnoGameManager.WaitForOneMoveDuration );

        initj = 0;
        for (int i = 0; i < GameManager.Players.Count; i++)
        {
            for (int j = 0; j < GameManager.PLAYER_INIT_CARDS; j++)
            {
                int id = DrawStack.GetAllCards().Count-1;
                UnoCard card = DrawStack.GetAllCards()[id];

                RemoveFromDraw(card);
                GameManager.Players[i].DrawCard(card, false, () => {

                    if (i == GameManager.MainPlayer)
                        card.ShowBackImg(false);
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
    public bool IsEmpty()
    {
        return DrawStack.IsEmpty();
    }
    public UnoCard GetaCard()
    {
        if (DrawStack.IsEmpty())
        {
            GameManager.EmptyDrawPileShowWinner();
            return null;
        }
        else
            return DrawStack.GetAllCards()[0];
    }
    public List<UnoCard> GetAllCards() {
        return DrawStack.GetAllCards();
    }
    private void CreateAllCards()
    {
        List<int> allNumbers = new List<int>();
        for (int i = 0; i < TOTAL_CARDS; i++)
        {
            allNumbers.Add(i);
        }
        allNumbers = Utility.Shuffle(allNumbers);

        for (int i = 0; i < TOTAL_CARDS; i++)
        {
            AllCards.Add(MakeCard(allNumbers[i], i));
        }
    }
    private UnoCard FindFirstCard()
    {
        int x = 0;
        while (!GameManager.IsCardAcceptableToStart(AllCards[x]))
        {
            x++;
        }
        UnoCard firstCard = AllCards[x];
        RemoveFromDraw(AllCards[x]);
        return firstCard;
    }

}
