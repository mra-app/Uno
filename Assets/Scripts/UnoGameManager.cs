using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public enum Owner
{

    Player1,
    Player2,
    Player3,
    Player4,
    Discard,
    Draw,
}

public class UnoGameManager : MonoBehaviourPunCallbacks
{
    public List<UnoPlayer> Players;
    public List<GameObject> WinnerPlayerIcons;
    public UnoDiscardPile DiscardPile;
    public UnoDrawPile DrawPile;
    public EventSender EventSender;

    public GameObject FinishPanel;
    public TMP_Text WinText;
    public NotifiControl NotifiControl;
    [SerializeField]
    private GameObject NameTagUp;
    [SerializeField]
    private GameObject NameTagDown;


    [NonSerialized]
    public static float WaitForOneMoveDuration = 0.5f;
    [NonSerialized]
    public static int MainPlayer = 0;//TODO:must be assigned in online game, based on player id
    [NonSerialized]
    public int PLAYER_INIT_CARDS = 5;
    [NonSerialized ]
    public int PlayerCount;
    public int MaxPlayerCount = 4;
    

    private int Turn = -1;
    private int ChangeTurnOrder = 1;
    private bool Paused = false;
    public bool OnlineGame = false;
    public bool isMasterClient = false;
    

    void Awake()
    {
        if (DontDestroy.TempData == 1)
        {
            OnlineGame = true;
            PlayerCount = 2;
            Online_ShowPlayerName(PhotonNetwork.IsMasterClient, PhotonNetwork.NickName);
            Online_ShowPlayerName(!PhotonNetwork.IsMasterClient, Online_GetOpponent().NickName);
            MainPlayer = PhotonNetwork.IsMasterClient ? 0 : 1;
            isMasterClient = PhotonNetwork.IsMasterClient;
            UnoAI AI = Players[2].GetComponent<UnoAI>();
            if (AI != null)
            {
                Destroy(AI);
            }
        }
        else {
            PlayerCount = DontDestroy.TempData;
        }

        Turn = 0;
        
        PrepareUnoPlayers(UnoColorSelect.ColorSelected);
       
        DrawPile.SetManager(this);
        DiscardPile.SetManager(this);
    }
    public void Start()
    {
        DrawPile.ShuffleAndDistAllCards();
    }
    //master is the down player
    private void Online_ShowPlayerName(bool isMaster,string Name)
    {
        GameObject TargetGameObject = isMaster ? NameTagDown : NameTagUp;
        TargetGameObject.SetActive(true);
        TMP_Text text = TargetGameObject.GetComponentInChildren<TMP_Text>();
        text.text = Name;
    }
    private Player Online_GetOpponent()
    {
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (!player.IsLocal)
            {
                return player;
            }
        }
        return null;
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
        for (int i = 0; i < Players.Count; i++)
        {
            //#if its not a four player game, remove extra game objects(player 3 and 1)
            if (PlayerCount!=MaxPlayerCount &&( i== Players.Count -1|| i==1))
            {
                Destroy(Players[i].gameObject);
                Players.RemoveAt(i);
                if(i >= Players.Count)
                 continue;
            }
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
        if(card != null)//-1+1=0   -1+-1=-2   0+-1=--1  1-1=0 
        {
            if (!(PlayerCount == 2 && card.Type == UnoCard.SpecialCard.Reverse))//else everything stays the same
            {


                Turn = (Turn + card.TurnChangeAmount * ChangeTurnOrder) % PlayerCount;
                if (card.TurnChangeAmount < 0)
                {
                    ChangeTurnOrder = ChangeTurnOrder * -1;
                }
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
        WinnerPlayerIcons[(int)Players[turn].PlayerColor].SetActive(true);
       
    } 
    /// <summary>
    /// when a player selects a card from draw pile, after the card is given, the condition is checked
    /// when a player forgets to say uno, before giving a card, the condition is checked.S
    /// </summary>
    public void EmptyDrawPileShowWinner()
    {
        int Winner = 0;
        int MinCards = int.MaxValue;
        for (int i = 0; i < PlayerCount; ++i)
        {
            if (Players[i].cardStack.GetAllCards().Count < MinCards)
            {
                Winner = i;
                MinCards = Players[i].cardStack.GetAllCards().Count;
            }
        }
       ShowWinner(Winner);
       
    }

}
