using System;
using System.Collections.Generic;
using UnityEngine;

public class UnoDiscardPile : MonoBehaviour
{
    List<UnoCard> AllCards = new List<UnoCard>();
    private UnoCard LastCard = null;
    UnoCardStack cardStack;
    int AccumulatedCards = 0;
    public UnoGameManager GameManager;
    public GameObject WildColorGameObject;
    public List<GameObject> WildColors = new List<GameObject>();

    private Animation ChooseColorAnim;


    // Start is called before the first frame update
    void Awake()
    {
        cardStack = GetComponent<UnoCardStack>();
        ChooseColorAnim = WildColorGameObject.GetComponent<Animation>();

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
        cardStack.PushAndMove(card, () => { callback(); });
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
    public void SetWildLastCardColor(UnoCard.CardType color)
    {
        LastCard.SetWildColor(color);
        for (int i = 0; i < 4; i++)
        {
            WildColors[i].SetActive(i== (int)color);
        }
        
        WildColorGameObject.transform.SetAsLastSibling();
        ChooseColorAnim.Play();
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
