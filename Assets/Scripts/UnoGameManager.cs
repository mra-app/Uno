using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Owner
{

    Player1,
    Player2,
    Player3,
    Player4,
    Discard,
    Draw,
}

public class UnoGameManager : MonoBehaviour
{
    
    const int TOTAL_CARDS = 108;
    const int PLAYER_INIT_CARDS = 5;
    //public UnoCardStack DiscardStack;
    public List<UnoPlayer> Players;
    public UnoDiscardPile DiscardPile;
    public UnoDrawPile DrawPile;
    //public GameObject cardPrefab;
    //Sprite[] CardSprites;
    //List<UnoCard> AllCards = new List<UnoCard>();

    private int Turn = -1;
    private int PlayerCount;
    bool LockCards = false;
    //private UnoCard LastCard = null;
    private int ChangeTurnOrder = 1;
    public GameObject FinishPanel;
    public TMP_Text text;
    public static float WaitForOneMoveDuration = 0.2f;
   // public bool GameLocked = false;
   // public UnoDiscardPile DiscardPile;
    void Awake()
    {
        // GameLocked = true;
        //CardSprites = Resources.LoadAll<Sprite>("");
        LockCardsToPlayTurn(true);
        PlayerCount = 4;

        for (int i = 0; i < PlayerCount; i++)
        {
            Players[i].SetOwner((Owner)i);
            Players[i].GameManager = this;
        }
        DrawPile.GameManager = this;
        //ShuffleAndDistribute(PlayerCount);

        Turn = 0;
      //  ChangeTurn();
    }
    public void ContinueGame(UnoCard card=null)
    {
        if (card == null)
        {
            ChangeTurn(DiscardPile.GetLastCard());
        }
        else
        {
             ChangeTurn(card);
        }
        
        LockCardsToPlayTurn(false);
    }
    public int GetTurn()
    {
        return Turn;
    }
    /// <summary>
    /// change turn based on the last card played(could be skip or reverse)
    /// </summary>
    /// <param name="card"></param>
    public void ChangeTurn(UnoCard card = null)
    {
        if(card != null)//-1+1=0   -1+-1=-2   0+-1=--1
        {
            Turn = (Turn + card.TurnChangeAmount* ChangeTurnOrder) % PlayerCount;
            if(card.TurnChangeAmount < 0)
            {
                ChangeTurnOrder = ChangeTurnOrder * -1;
            }

        }
        else
         Turn = (Turn + ChangeTurnOrder) % PlayerCount;
        if (Turn < 0)
            Turn += PlayerCount;
        DebugControl.Log("turn" + Turn, 3);
        UpdatePlayersTurn();
    }
    private void UpdatePlayersTurn()
    {
        for (int i = 0; i < PlayerCount; ++i)
        {
            Players[i].ChangeTurnToMe(Turn == i);
        }
    }

    public void GameStart(UnoCard lastCard)
    {
        if (lastCard.Type == UnoCard.SpecialCard.Reverse)
        {
            Turn = 1;
            ChangeTurn(lastCard);
        }
        else if (lastCard.Type == UnoCard.SpecialCard.Skip)
        {
            Turn = -1;
            ChangeTurn(lastCard);
        }

        else
            UpdatePlayersTurn();

        LockCardsToPlayTurn(false);

    }
    public bool IsAcceptableToStart(UnoCard card)
    {
        return card.Type != UnoCard.SpecialCard.Draw4Wild;
    }

    //public void ShuffleAndDistribute(int playerCount)
    //{
    //    List<int> allNumbers = new List<int>();
    //    for (int i = 0; i < TOTAL_CARDS; i++) { allNumbers.Add(i); }
    //    allNumbers = Utility.Shuffle(allNumbers);

    //    for (int i = 0; i < TOTAL_CARDS; i++)
    //    {
    //        AllCards.Add(MakeCard(allNumbers[i], i));
    //        //  DebugControl.Log(allNumbers[i]+" ",3);
    //    }

    //    int j = 0;
    //    while (AllCards.Count > j)
    //    {
    //        DrawStack.Push(AllCards[j]);
    //      //  AllCards[j].ShowBackImg(false);//todo

    //        j++;
    //    }
    //    int drawCardCount = AllCards.Count - (4 * PLAYER_INIT_CARDS + 1);


    //    DiscardStack.PushAndMove(AllCards[0], () => {
    //        StartCoroutine(DistCardtoPlayers(drawCardCount + 1));
    //    });
    //     LastCard = AllCards[0];


    //}
    //IEnumerator DistCardtoPlayers(int AllCardIdx)
    //{
    //    int initj = AllCardIdx;
    //    for (int i = 0; i < PlayerCount; i++)
    //    {
    //        while (AllCardIdx < initj + PLAYER_INIT_CARDS * (i + 1))
    //        {
    //            Players[i].DrawCard(AllCards[AllCardIdx], () => { });
    //            if (i == 0)//TODO: in online have to change
    //              AllCards[AllCardIdx].ShowBackImg(false);
    //            AllCardIdx++;
    //            yield return new WaitForSeconds(0.1f);
    //        }
    //    }
    //}
    //private UnoCard MakeCard(int id, int globalCardIdx)
    //{
    //    GameObject card = Instantiate(cardPrefab);
    //    UnoCard cardScript = card.GetComponent<UnoCard>();
    //    cardScript.InitCard(id, CardSprites[id], globalCardIdx);
    //    cardScript.OnSelected += OnCardSelected;
    //    return cardScript;
    //}
    /*public void OnCardSelected(int globalCardIdx, Owner owner)//owner d? TODO!
    {
        if (LockCards)
            return;

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
            LockCards = true;

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



    }*/
 

    private void CheckFinish(int turn)
    {
       // DebugControl.Log("turn" + turn + Players[turn].AllCardsPlayed(), 3);
        if (Players[turn].AllCardsPlayed())
        {
            text.text = turn.ToString();
            FinishPanel.SetActive(true);
        }
    } 
    public void ShowWinner(int turn)
    {
        text.text = turn.ToString();
        FinishPanel.SetActive(true);
    }
    public void LockCardsToPlayTurn(bool _lock)
    {
        LockCards = _lock;
    }
    public bool IsLockToPlayTurn()
    {
        return LockCards;
    }



}
