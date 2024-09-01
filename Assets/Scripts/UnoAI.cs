using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoAI : MonoBehaviour
{

    private bool IsPlaying = false;
    List<UnoCard> cards;//= new List<UnoCard>();
    public Owner Owner;
    public UnoGameManager gameManager;

    void Start()
    {
        
    }


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
        cards.AddRange(DrawStack.GetAllCards());
    }
    public void StartPlay( UnoCardStack PlayerCardStack = null, UnoCardStack DrawStack = null,int TryNumber = 0)
    {
   
            //DebugControl.Log("start" + Start, 3);
            
        PrepareToPlay(PlayerCardStack, DrawStack,TryNumber);
        Play();
        CheckForUno();
        //random uno
        
    }
    public UnoCard.CardType SelectColorForWild(UnoCardStack PlayerCardStack)
    {
        List<int> colorCount = new List<int>();//TODO:get in utility
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
    public void CheckForUno()
    {
        DebugControl.Log("CheckForUno", 3);
        for (int i = 0; i < gameManager.Players.Count; i++)//TODO:Player count
        {
            DebugControl.Log("c" + i, 3);
            if (gameManager.Players[i].IsUno())
            {
                DebugControl.Log("uno" + i, 3);
                gameManager.Players[i].Uno((int)Owner);
                return;
            }
        }

    }

}
