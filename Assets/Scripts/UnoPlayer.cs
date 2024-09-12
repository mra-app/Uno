using PlasticPipe.PlasticProtocol.Messages;
using System;
using System.Collections;
using UnityEngine;

public class UnoPlayer : MonoBehaviour
{
    [SerializeField]
    public UnoCardStack cardStack;
    private Owner handOwner;
    public GameObject MyTurnImage;
    public GameObject SelectColorPanel;
    UnoAI AI;
    //UnoCardStack DrawStack;//TODO: unserialized?
    //UnoCard LastCard = null;
    public UnoGameManager GameManager;
    const string ManagerTagName = "GameController";
    private int TryNumber = 0;
    private bool UnoImmune = false;
    void Start()
    {
        cardStack.OnCardSelected += OnCardSelected;
        if(GetComponent<UnoAI>() != null) {
            AI = GetComponent<UnoAI>();
            AI.gameManager = GameManager;
        }
    }
    public bool AllCardsPlayed() {
        return cardStack.IsEmpty();
    }
    public void RemoveFromHand(UnoCard card)
    {
        cardStack.Pop(card);
    }
    public void SetOwner(Owner _owner)
    {
        handOwner = _owner;
        cardStack.owner = _owner; //stacks without player have assigned owners from editor.
    }
    public void DrawCard(UnoCard card, Action callback)
    {
        GameManager.DrawPile.RemoveFromDraw(card);
        Immune(false);
        cardStack.PushAndMove(card, () =>
        {
           
            if ((int)handOwner == GameManager.MainPlayer)//TODO: in online have to change
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
        if (AI == null)
        {
            SelectColorPanel.SetActive(true);
        }
        else
        {
            yield return new WaitForSeconds(UnoGameManager.WaitForOneMoveDuration);

            GameManager.DiscardPile.SetWildLastCardColor(
                AI.SelectColorForWild(cardStack)
                );
            GameManager.ContinueGame();


        }
    }
    public void PlayAgain()
    {
        TryNumber++;
        //DebugControl.Log("play again", 3);
        if( AI != null ) 
            StartCoroutine(AIPlay());
    }
    public void ColorSelected(int color)
    {
        GameManager.DiscardPile.SetWildLastCardColor((UnoCard.CardType)color);

        SelectColorPanel.SetActive(false);
        GameManager.ContinueGame();
    }
    IEnumerator AIPlay()
    {
        AI.Owner = handOwner;
        yield return new WaitForSeconds(0.2f);//UnoGameManager.WaitForOneMoveDuration);
        AI.StartPlay(cardStack, GameManager.DrawPile.DrawStack,TryNumber);


    }
    public void OnCardSelected(UnoCard card,int globalCardIdx, Owner owner)
    {
        DebugControl.Log("ay domino!" + (int)handOwner+" "+ GameManager.GetTurn()+" " +(int)card.LastClicked, 3);
        DebugControl.Log("l"+GameManager.IsLockToPlayTurn(), 3);
        if (GameManager.GetTurn() == (int)handOwner && GameManager.GetTurn() == (int)card.LastClicked)
        {
            if (GameManager.DiscardPile.CanPlayOnUpCard() && GameManager.DiscardPile.CanPlayThisCard(card)) 
            {
                if (GameManager.IsLockToPlayTurn())
                {
                    return;
                }
               // DebugControl.Log("h", 3);
                GameManager.LockCardsToPlayTurn(true);
                RemoveFromHand(card);//TODO: move in discard in pile code
                Immune(false);
                GameManager.DiscardPile.DiscardedCard(card, () => {   
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
                //    GameManager.ChangeTurn(card);
                //GameManager.LockCardsToPlayTurn(false);
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

    public void Uno(int callerID)//?
    {
        if(handOwner==0)
            DebugControl.Log("UNO"+callerID+ " Immune " + UnoImmune, 3);
        if (callerID != (int)handOwner)
        {
            if (IsUno()&& !UnoImmune)
            {   
                Immune(true);
                GameManager.NotifiControl.ShowNotification("you forgot uno!",1);
                DebugControl.Log("youd be immune"+callerID, 3);//TODO:test
                
                DrawCard(GameManager.DrawPile.GetaCard(), () =>
                {
                    DrawCard(GameManager.DrawPile.GetaCard(), () => {
                        DebugControl.Log("immune my .."+callerID, 3);
                    });
                });
            }
        }
        else
        {
            DebugControl.Log("immune",3);
            Immune(true);
        }
        

    }
    public void UnoClicked()
    {
            Uno(GameManager.MainPlayer);//TODO: send current view player network id
    }
    public bool IsUno()
    {
        return cardStack.HasOneCard();
    }

    public void Immune(bool immune)
    {
        if (handOwner == 0)
        {
            DebugControl.Log("UNO:" + immune, 3);
            if (immune)
                DebugControl.Log(" immuned", 3);
        }
        UnoImmune = immune;
    }

}
