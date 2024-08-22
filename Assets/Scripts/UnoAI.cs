using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoAI : MonoBehaviour
{
    // Start is called before the first frame update
    private bool IsMyTurn = false;
    private bool IsPlaying = false;
    List<UnoCard> cards;//= new List<UnoCard>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMyTurn && !IsPlaying)
            Play();        
    }
    void Play()
    {
        IsPlaying = true;
        if (cards.Count > 0)
        {
            cards[0].OnClick();
            cards.RemoveAt(0);
        }
        
        IsPlaying = false;
    }
    void PrepareToPlay(UnoCardStack PlayerCardStack, UnoCardStack DrawStack)
    {
        cards =new List<UnoCard>(PlayerCardStack.GetAllCards().Count+ DrawStack.GetAllCards().Count);
        cards.AddRange(PlayerCardStack.GetAllCards());
        cards.AddRange(DrawStack.GetAllCards());
        DebugControl.Log("here", 3);
      
    }
    public void StartPlay(bool Start, UnoCardStack PlayerCardStack = null, UnoCardStack DrawStack = null)
    {
        if(Start)
        {
            PrepareToPlay(PlayerCardStack, DrawStack);
            IsMyTurn = true;
        }
        else
        {
            IsMyTurn = false;
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
