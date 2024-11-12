using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnoPlayer : MonoBehaviour
{
    public UnoCardStack cardStack;
    public GameObject MyTurnImage;
    public GameObject SelectColorPanel;
    [NonSerialized]
    public UnoGameManager GameManager;
    [SerializeField]
    public Owner handOwner;
    private UnoAI AI;


    private int TryNumber = 0;
    private bool UnoImmune = false;
    private Button UnoButon;
    public List<Sprite> PlayerColorImg;
    public List<string> PlayerColorHex;
    public Image CardPlaceImage;
    public Image PlayerImg;
    public UnoCard.CardType PlayerColor;
    public Animator animator;
    public void Init()
    {
        cardStack.OnCardSelected += OnCardSelected;
        if (GetComponent<UnoAI>() != null)
        {
            AI = GetComponent<UnoAI>();
            AI.gameManager = GameManager;
        }
        UnoButon = GetComponentInChildren<Button>();
        animator = GetComponent<Animator>();
        //PlayerColorHex = new List<string>();
        //PlayerColorHex.Add("EDD0BD");yello
        //PlayerColorHex.Add("88E0E8");blu
        //PlayerColorHex.Add("CCA9E5");red
        //PlayerColorHex.Add("82E894");green

    }
    public void InteractableUnoButton(bool interactable)
    {
        UnoButon.interactable = interactable;
    }
    public bool AllCardsPlayed() {
        return cardStack.IsEmpty();
    }
    public void SetColor(UnoCard.CardType color)
    {
        Color temp;
        PlayerColor = color;
        if (ColorUtility.TryParseHtmlString(PlayerColorHex[(int)color]+"20", out temp))
        {
            CardPlaceImage.color = temp;
        }
        PlayerImg.sprite=PlayerColorImg[(int)color];
    }
    public void RemoveFromHand(UnoCard card)
    {
        cardStack.Pop(card);
    }
    public void SetOwner(Owner _owner)
    {
        handOwner = _owner; //stacks without player have assigned owners from editor.
    }
    public void DrawCard(UnoCard card,bool isForUno, Action callback)
    {
        GameManager.DrawPile.RemoveFromDraw(card);
        if(!isForUno)
            Immune(false);

        cardStack.PushAndMove(card,false, () =>
        {
            if ((int)handOwner == UnoGameManager.MainPlayer)//TODO:Online
                card.ShowBackImg(false);
            callback();
        });
    }
    public void ChangeTurnToMe(bool isMyTurn)
    {
        MyTurnImage.SetActive(isMyTurn);
        TryNumber = 0;
       // DebugControl.Log("turn" + isMyTurn, 3);
        if( AI != null)
        {
            if (isMyTurn )
            {
                 StartCoroutine(AIPlay());
            }
        }
    }
    IEnumerator SelectWildCardColor(UnoCard cardScript)
    {
        if (GameManager.OnlineGame)
        {
            if (GameManager.GetTurn() == UnoGameManager.MainPlayer)
            {
                SelectColorPanel.SetActive(true);//then ColorSelected function will be called
            }
        }
        else
        {
            if (AI == null)
            {
                SelectColorPanel.SetActive(true);//then ColorSelected function will be called
            }
            else
            {
                yield return new WaitForSeconds(UnoGameManager.WaitForOneMoveDuration);

                GameManager.DiscardPile.SetWildLastCardUIColor(AI.SelectColorForWild(cardStack));
                GameManager.ContinueGame();


            }
        }
    }
    public void PlayAgain()
    {
        GameManager.LockGame(false);

        TryNumber++;
        //DebugControl.Log("play again", 3);
        if( AI != null ) 
            StartCoroutine(AIPlay());
    }
    public void ColorSelected(int color)//called from button clicked
    {
        GameManager.DiscardPile.SetWildLastCardUIColor((UnoCard.CardType)color);

        SelectColorPanel.SetActive(false);
        GameManager.ContinueGame();
        if (GameManager.OnlineGame)
        {
            GameManager.EventSender.Online_OnWildColorSelected(color);
        }
    }
    IEnumerator AIPlay()
    {
        AI.Owner = handOwner;
        yield return new WaitForSeconds(0.4f*UnoGameManager.WaitForOneMoveDuration);
        AI.StartPlay(cardStack, GameManager.DrawPile.GetAllCards(),TryNumber);


    }
    public void OnCardSelected(UnoCard card)
    {
        if (GameManager.isGameLocked()){
            return;
        }
        if (GameManager.GetTurn() == (int)handOwner && GameManager.GetTurn() == (int)card.LastClicked)
        {
            if (GameManager.DiscardPile.CanPlayOnUpCard() && GameManager.DiscardPile.CanPlayThisCard(card)) 
            {
                GameManager.LockGame(true);

                RemoveFromHand(card);//TODO: move in discard in pile code
                Immune(false);
                GameManager.DiscardPile.DiscardedCard(card, () => {

                    //if the card is mine, notify the other player in online game
                    if (GameManager.OnlineGame && (Owner)UnoGameManager.MainPlayer == card.LastClicked)
                    {
                        GameManager.EventSender.Online_OnPlayerHandCardSelected(card);
                    }
                    if (HasWon()) 
                    {
                         GameManager.ShowWinner((int)handOwner);
                            return;
                    }

                    if (GameManager.DiscardPile.ColorSelectIsNeeded())
                    {
                         StartCoroutine(SelectWildCardColor(card));
                    }
                    else
                    {
                        GameManager.ContinueGame(card);
                     }
                });
                
            }
            else
            {
                PlayAgain();
            }
        }
           // DebugControl.Log(globalCardIdx + " " + owner.ToString(), 3);

    }
    public bool HasWon()
    {
        return cardStack.IsEmpty();
    }

    public void Uno(int callerID)
    {
        animator.SetTrigger("UNOC");

        if (callerID != (int)handOwner)
        {
            if (IsUno()&& !IsImmune())
            {   
                Immune(true);
                GameManager.NotifiControl.ShowNotification("forgot uno!",NotifiControl.NotificationCode.UNOF);

                GetOnePenaltyCard(() =>
                {
                    GetOnePenaltyCard(() =>
                    {
                        if (GameManager.DrawPile.IsEmpty())
                        {
                            GameManager.EmptyDrawPileShowWinner();
                            return;
                        }
                    });
                });
            }
        }
        else
        {
            Immune(true);
        }
        

    }

    public void GetOnePenaltyCard(Action callback)
    {
        UnoCard PenaltyCard = GameManager.DrawPile.GetaCard();
        if (PenaltyCard != null)
            DrawCard(PenaltyCard, true, () =>
            {
                callback();
            });
    }
    public UnoCard GetaCard(int id = -1)
    {
        if (id != -1)
        {
            return cardStack.GetCard(id);
        }
        else//TODO:unused
        {
            if (cardStack.IsEmpty())
            {
               // GameManager.EmptyDrawPileShowWinner();
                Debug.LogError("Empty");
                return null;
            }
            else
                return cardStack.GetAllCards()[0];
        }
    }

    public void UnoClicked()
    {
        Uno(UnoGameManager.MainPlayer);
        if (GameManager.OnlineGame)
        {
            GameManager.EventSender.Online_OnUnoCalled(UnoGameManager.MainPlayer, handOwner);
        }
    }
    public bool IsUno()
    {
        return cardStack.HasOneCard();
    }

    public void Immune(bool immune)
    {
        UnoImmune = immune;
    }
    public bool IsImmune()
    {
        return UnoImmune;
    }

}
