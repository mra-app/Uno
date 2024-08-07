using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    const int TOTAL_CARDS = 112;
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
        allNumbers = Utility.AddUnoCardNumbers(allNumbers);
        allNumbers = Utility.Shuffle(allNumbers);

        List<UnoCard> allCards = new List<UnoCard>();
        for (int i = 0; i < TOTAL_CARDS; i++)
        {
            allCards.Add(MakeCard(allNumbers[i]));
        }
        

        for(int i = 0;i < playerCount; i++)
        {
            int j = 0;
            while (j < PLAYER_INIT_CARDS)
            {
                Players[i].DrawCard(allCards[0]);
                allCards.RemoveAt(0);
                j++;
            }
        }
    }
    private UnoCard MakeCard(int id)
    {
        GameObject card = Instantiate(cardPrefab);
        UnoCard cardScript = card.GetComponent<UnoCard>();
        cardScript.setID(id);
        return cardScript;
    }
   


}
