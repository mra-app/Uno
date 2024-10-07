using System;
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
    public List<UnoPlayer> Players;
    public UnoDiscardPile DiscardPile;
    public UnoDrawPile DrawPile;

    public GameObject FinishPanel;
    public TMP_Text WinText;
    public NotifiControl NotifiControl;

    [NonSerialized]
    public static float WaitForOneMoveDuration = 0.5f;
    [NonSerialized]
    public int MainPlayer = 0;
    [NonSerialized]
    public int PLAYER_INIT_CARDS = 3;

    private int Turn = -1;
    private int PlayerCount;
    private int ChangeTurnOrder = 1;
    private bool Paused = false;

    void Awake()
    {
        PlayerCount = 4;
        Turn = 0;
        PrepareUnoPlayers(UnoColorSelect.ColorSelected);
        DrawPile.GameManager = this;
        DiscardPile.GameManager = this;
    }
    public void Start()
    {
        DrawPile.ShuffleAndDistribute(PlayerCount);
    }
    private void PrepareUnoPlayers(UnoCard.CardType MainPlayerColor)
    {
        //make a list of colors from enum
        List<UnoCard.CardType> Colors = new List<UnoCard.CardType>();
        foreach (UnoCard.CardType color in Enum.GetValues(typeof(UnoCard.CardType)))
        {
            Colors.Add(color);
        }
        //set player colors and owners and disable uno button at first
        for (int i = 0; i < PlayerCount; i++)
        {
            Players[i].SetOwner((Owner)i);
            Players[i].GameManager = this;
            Players[i].Init();
            if (i == 0)
            {
                Players[i].SetColor(MainPlayerColor);
                Colors.Remove(MainPlayerColor);
            }
            else
            {
                Players[i].SetColor(Colors[0]);
                Colors.Remove(Colors[0]);
            }
            Players[i].InteractableUnoButton(false);//if called in awake, button would be null
        }
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
    }

    public void Pause(bool _pause)
    {
        Paused = _pause;
        if (!_pause)
        {
            UpdatePlayersTurn();

        }
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
        if(!Paused)
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

        for (int i = 0; i < PlayerCount; ++i)
        {
            Players[i].InteractableUnoButton(true);
        }

    }
    public bool IsCardAcceptableToStart(UnoCard card)
    {
        return card.Type != UnoCard.SpecialCard.Draw4Wild;
    }
 
    public void ShowWinner(int turn)
    {
        WinText.text = "Player " + (turn+1) + " has won!";
        FinishPanel.SetActive(true);
    }

}
