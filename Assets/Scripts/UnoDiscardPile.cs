using System;
using System.Collections.Generic;
using UnityEngine;

public class UnoDiscardPile : MonoBehaviour
{

    UnoGameManager GameManager;
    public GameObject WildColorGameObject;
    public List<GameObject> WildColors = new List<GameObject>();

    private Animation ChooseColorAnim;
    private UnoCard LastCard = null;
    private UnoCardStack cardStack;

    void Awake()
    {
        cardStack = GetComponent<UnoCardStack>();
        cardStack.SetAsDiscardStack();
        ChooseColorAnim = WildColorGameObject.GetComponent<Animation>();


    }
    public void SetManager(UnoGameManager manager)
    {
        GameManager = manager;
    }
    public void DiscardedCard(UnoCard card, Action callback)
    {
        if (card.Type == UnoCard.SpecialCard.Reverse)
        {
            GameManager.NotifiControl.ShowNotification("Reverse!", 3);
        }
        else if (card.Type == UnoCard.SpecialCard.Skip)
        {
            GameManager.NotifiControl.ShowNotification("Skip!", 4);
        }
        else if (card.Type == UnoCard.SpecialCard.Draw2)
        {
            GameManager.NotifiControl.ShowNotification("draw 2 cards!", 2);
        }
        else if (card.Type == UnoCard.SpecialCard.Draw4Wild)
        {
            GameManager.NotifiControl.ShowNotification("draw 4 cards!", 5);
        }
        card.ShowBackImg(false);

        cardStack.PushAndMove(card,false, () => {
            callback();
        });
        LastCard = card;
    }
    public bool CanPlayOnUpCard()
    {
        return LastCard.AccumulatedCards <= 0;
    }    
    public bool CanPlayThisCard(UnoCard cardScript)
    {
        return (LastCard.AcceptsCard(cardScript) || (cardStack.HasOneCard()&&LastCard.Type==UnoCard.SpecialCard.Wild));
    }
    public bool ColorSelectIsNeeded()
    {
        return (LastCard.Type == UnoCard.SpecialCard.Wild || LastCard.Type == UnoCard.SpecialCard.Draw4Wild);
    }
    public void SetWildLastCardUIColor(UnoCard.CardType color)
    {
        LastCard.SetWildColor(color);
        for (int i = 0; i < 4; i++)
        {
            WildColors[i].SetActive(i== (int)color);
        }
        
        WildColorGameObject.transform.SetAsLastSibling();
        ChooseColorAnim.Play();
        GameManager.NotifiControl.ShowNotification("", 0);

    }

    public void CardDrawn()
    {
        if (LastCard.AccumulatedCards > 0)
        {
            LastCard.AccumulatedCards--;
        }
    }
    public UnoCard GetLastCard()
    {
        return LastCard;
    }

}
