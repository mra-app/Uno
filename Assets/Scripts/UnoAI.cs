using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnoAI : MonoBehaviour
{
    List<UnoCard> cards;
    public Owner Owner;
    public UnoGameManager gameManager;


    void Play()
    {        
        if (cards.Count > 0)
        {
            cards[0].OnClick((int)Owner);
            cards.RemoveAt(0);
        }
    }
    void PrepareToPlay(UnoCardStack PlayerCardStack, UnoCardStack DrawStack,int TryNumber)
    {
        cards =new List<UnoCard>(PlayerCardStack.GetAllCards().Count+ DrawStack.GetAllCards().Count);
        List<UnoCard> AvailableCards= new List<UnoCard>();
        for (int i = 0; i < PlayerCardStack.GetAllCards().Count; i++)
        {
            if (i >= TryNumber)
            {
                AvailableCards.Add(PlayerCardStack.GetAllCards()[i]);
            }
        }

        cards.AddRange(AvailableCards);
        List<UnoCard> DrawCards = DrawStack.GetAllCards();
        DrawCards.Reverse();
        cards.AddRange(DrawCards);
    }
    public void StartPlay( UnoCardStack PlayerCardStack = null, UnoCardStack DrawStack = null,int TryNumber = 0)
    {
        PrepareToPlay(PlayerCardStack, DrawStack,TryNumber);
        Play();
        int x = Random.Range(0, 10);
        if(x<1)
            StartCoroutine(CheckForUno());
    }
    public UnoCard.CardType SelectColorForWild(UnoCardStack PlayerCardStack)
    {
        List<int> colorCount = new List<int> { 0, 0, 0, 0 };

        foreach (var card in PlayerCardStack.GetAllCards()) {
            colorCount[(int)card.GetColor()]++;
        }

        int max = 0;
        UnoCard.CardType color = 0 ;
        for (int i = 0; i < 4; i++)
        {
            if(colorCount[i]>max)
            {
                color = (UnoCard.CardType)i;
                max = colorCount[i];
            }
        }
       
        return color;
    }
    IEnumerator CheckForUno()
    {
        yield return new WaitForSeconds(2*UnoGameManager.WaitForOneMoveDuration);

        for (int i = 0; i < gameManager.PlayerCount; i++)
        {
            if (gameManager.Players[i].IsUno() && !gameManager.Players[i].IsImmune())
            {
                gameManager.Players[i].Uno((int)Owner);
            }
        }

    }

}
