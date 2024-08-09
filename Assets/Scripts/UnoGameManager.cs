using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    const int TOTAL_CARDS = 108;
    const int PLAYER_INIT_CARDS = 5;
    public UnoCardStack DrawStack;
    public UnoCardStack DiscardStack;
    public List<UnoPlayer> Players;
    public GameObject cardPrefab;
    Sprite[] CardSprites;
   
    private int PlayerCount;
    void Start()
    {

        CardSprites = Resources.LoadAll<Sprite>("");

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
        

        for (int i = 0;i < playerCount; i++)
        {
            int j = 0;
            while (j < PLAYER_INIT_CARDS)
            {
                Players[i].DrawCard(allCards[0]);
                if(i == 0)
                 allCards[0].ShowBackImg(false);
                allCards.RemoveAt(0);
                j++;
            }
        }
        if (allCards.Count > 0)
        {
            DiscardStack.Push(allCards[0]);
            allCards.RemoveAt(0);
        }
        while (allCards.Count > 0)
        {
            DrawStack.Push(allCards[0]);
            allCards.RemoveAt(0);
        }
        }
    private UnoCard MakeCard(int id)
    {
        GameObject card = Instantiate(cardPrefab);
        UnoCard cardScript = card.GetComponent<UnoCard>();
        cardScript.setIDandImg(id, CardSprites[id]);

        return cardScript;
    }
   


}
