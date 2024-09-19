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
    //const int PLAYER_INIT_CARDS = 3;
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
    public static float WaitForOneMoveDuration = 0.5f;
   // public bool GameLocked = false;
   // public UnoDiscardPile DiscardPile;
   public int MainPlayer = 0;
    public NotifiControl NotifiControl;
    void Awake()
    {
        // GameLocked = true;
        //CardSprites = Resources.LoadAll<Sprite>("");
        LockCardsToPlayTurn(true);
        PlayerCount = 4;
        Turn = 0;

        for (int i = 0; i < PlayerCount; i++)
        {
            Players[i].SetOwner((Owner)i);
            Players[i].GameManager = this;
            Players[i].Init();
            Players[i].InteractableUnoButton(false);//if called in awake, button would be null

        }
        DrawPile.GameManager = this;
        DiscardPile.GameManager = this;
        //ShuffleAndDistribute(PlayerCount);
        NotifiControl= GetComponent<NotifiControl>();
        

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
    public void Start()
    {
       DrawPile.ShuffleAndDistribute(PlayerCount);
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
        //DebugControl.Log("turn" + Turn, 3);
        UpdatePlayersTurn();
    }
    private void UpdatePlayersTurn()
    {
        for (int i = 0; i < PlayerCount; ++i)
        {
            Players[i].ChangeTurnToMe(Turn == i);
        }
    }

    public void GameStart(UnoCard firstCard)
    {
        DiscardPile.DiscardedCard(firstCard, () => { });

        if (firstCard.Type == UnoCard.SpecialCard.Reverse)
        {
            Turn = 1;
            ChangeTurn(firstCard);
        }
        else if (firstCard.Type == UnoCard.SpecialCard.Skip)
        {
            Turn = -1;
            ChangeTurn(firstCard);
        }

        else
            UpdatePlayersTurn();

        LockCardsToPlayTurn(false);
        for (int i = 0; i < PlayerCount; ++i)
        {
            Players[i].InteractableUnoButton(true);
        }

    }
    public bool IsAcceptableToStart(UnoCard card)
    {
        //return card.Type == UnoCard.SpecialCard.Skip;
        return card.Type != UnoCard.SpecialCard.Draw4Wild;
    }
 
    public void ShowWinner(int turn)
    {
        LockCardsToPlayTurn(true);
        text.text = "Player " + turn.ToString() + " has won!";
        FinishPanel.SetActive(true);
    }
    public void LockCardsToPlayTurn(bool _lock)//tODO:
    {
        DebugControl.Log("mym" + _lock,3);
        LockCards = false; //_lock;
    }
    public bool IsLockToPlayTurn()
    {
        return LockCards;
    }



}
