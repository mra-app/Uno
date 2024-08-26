using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoAI : MonoBehaviour
{

    private bool IsPlaying = false;
    List<UnoCard> cards;//= new List<UnoCard>();

    void Start()
    {
        
    }


    void Play()
    {        
        if (cards.Count > 0)
        {
            cards[0].OnClick();
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
        cards.AddRange(DrawStack.GetAllCards());
    }
    public void StartPlay(bool Start, UnoCardStack PlayerCardStack = null, UnoCardStack DrawStack = null,int TryNumber = 0)
    {
       // DebugControl.Log("start0" + Start, 3);
        if (Start)
        {
            DebugControl.Log("start" + Start, 3);
            PrepareToPlay(PlayerCardStack, DrawStack,TryNumber);
            Play();
        }
    }
    public UnoCard.CardType SelectColorForWild(UnoCardStack PlayerCardStack)
    {
        List<int> colorCount = new List<int>();
        for(int i = 0;i<4;i++)
        {
            colorCount.Add(0) ;
        }

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
        DebugControl.Log(color.ToString(),3);
        return color;
    }

}
