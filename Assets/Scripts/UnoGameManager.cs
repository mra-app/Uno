using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    const int TOTAL_CARDS = 100;
    const int PLAYER_INIT_CARDS = 5;
    public GameObject DrawStacks;
    public GameObject DiscardStacks;
    public List<UnoPlayer> Players;
    public GameObject cardPrefab;
   
    private int PlayerCount;
    void Start()
    {
        PlayerCount = 4;
        ShuffleAndDistribute(PlayerCount);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShuffleAndDistribute(int playerCount)
    {
        List<int> allNumbers = new List<int>();
        for (int i = 0; i < TOTAL_CARDS; i++)
            allNumbers.Add(i);

        allNumbers = Utility.Shuffle(allNumbers);

        List<UnoCard> allCards = new List<UnoCard>();
        for (int i = 0; i < TOTAL_CARDS; i++)
        {
            GameObject card = Instantiate(cardPrefab);
            UnoCard cardScript = card.GetComponent<UnoCard>();
            cardScript.setID(allNumbers[i]);
            allCards.Add(cardScript);
        }
        

        for(int i = 0;i < playerCount; i++)
        {
            int j = 0;
            while (j < PLAYER_INIT_CARDS)
            {
                DebugControl.Log(i + " " + j,1);
                Players[i].DrawCard(allCards[0]);
                allCards.RemoveAt(0);
                j++;

            }
        }
    }


}
