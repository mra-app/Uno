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
    void Start()
    {
        cardStack.OnCardSelected += OnCardSelected;
        if(GetComponent<UnoAI>() != null) {
            AI = GetComponent<UnoAI>();
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

        cardStack.PushAndMove(card, () =>
        {
            callback();
        });
    }
    public void ChangeTurnToMe(bool isMyTurn)
    {
        MyTurnImage.SetActive(isMyTurn);
        DebugControl.Log("turn" + isMyTurn, 3);
        if( AI != null)
        {
            if (!isMyTurn )
            {
                AIStop();
            }
            else
            {
                 StartCoroutine(AIPlay());

            }
        }
    }
    public void SelectWildCardColor(UnoCard cardScript)
    {
        if (AI == null)
        {
            SelectColorPanel.SetActive(true);

        }
        else
        {
            AIStop();
            DebugControl.Log("just", 3);
            GameManager.DiscardPile.SetWildLastCardColor(
                AI.SelectColorForWild(cardStack)
                );


        }
    }
    public void ColorSelected(int color)
    {
        GameManager.DiscardPile.SetWildLastCardColor((UnoCard.CardType)color);

        SelectColorPanel.SetActive(false);
    }
    IEnumerator AIPlay()
    {
        yield return new WaitForSeconds(0.1f);
        AI.StartPlay(true, cardStack, GameManager.DrawPile.DrawStack);


    }
    void AIStop()
    {
        AI.StartPlay(false);

    }
    public void OnCardSelected(UnoCard card,int globalCardIdx, Owner owner)
    {

        DebugControl.Log("turn="+ GameManager.GetTurn()+""+ (int)handOwner+"lock"+ GameManager.IsLockToPlayTurn()
            +"accu"+ GameManager.DiscardPile.CanPlayOnUpCard()+"|"
            , 3);
        if (GameManager.GetTurn() == (int)handOwner)
        {
            if (GameManager.DiscardPile.CanPlayOnUpCard() && GameManager.DiscardPile.CanPlayThisCard(card)) 
            {
                if (GameManager.IsLockToPlayTurn())
                {
                    return;
                }
                DebugControl.Log("h", 3);
                GameManager.LockCardsToPlayTurn(true);
                RemoveFromHand(card);
                GameManager.DiscardPile.DiscardedCard(card, () => {
                if (GameManager.DiscardPile.ColorSelectIsNeeded())
                {
                    SelectWildCardColor(card);
                }
                if (HasWon()) 
                {
                     GameManager.ShowWinner((int)handOwner);
                }
                GameManager.ChangeTurn(card);
                GameManager.LockCardsToPlayTurn(false);
                });
                
            }
        }
            DebugControl.Log(globalCardIdx + " " + owner.ToString(), 3);

    }
    public bool HasWon()
    {
        return cardStack.IsEmpty();
    }
    //public void DiscardCard(UnoCard cardScript)
    //{
    //    cardScript.OnSelected -= OnCardSelected;
    //}

}
