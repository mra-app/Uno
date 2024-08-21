using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoAI : MonoBehaviour
{
    // Start is called before the first frame update
    private bool IsMyTurn = false;
    List<UnoCard> cards;//= new List<UnoCard>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMyTurn)
            Play();
        
    }
    void Play()
    {

    }
    public void AIPlay(UnoCardStack PlayerCardStack, UnoCardStack DrawStack)
    {
        cards =new List<UnoCard>(PlayerCardStack.GetAllCards().Count+ DrawStack.GetAllCards().Count);
        cards.AddRange(PlayerCardStack.GetAllCards());
        cards.AddRange(DrawStack.GetAllCards());
        //foreach (UnoCard card in DrawStack.GetAllCards())
        //{
        //    cards.Add(card);
        //    List<UnoCard>
        //}
        DebugControl.Log("here", 3);
       // IsMyTurn = true;
    }

}
