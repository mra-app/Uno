using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility 
{
    public static List<int> Shuffle(List<int> cards)
    {
        List<int> refrenceCards = new();
        foreach (int card in cards)
        {
            refrenceCards.Add(card);
        }
        List<int> shuffledCards = new();
        while (refrenceCards.Count > 0)
        {
            int rand = Random.Range(0, refrenceCards.Count);
            shuffledCards.Add(cards[rand]);
            refrenceCards.RemoveAt(rand);
        }
        return shuffledCards;
    }
    public static List<int> AddUnoCardNumbers(List<int> cards)
    {

        List<int> forbiddenList = new List<int>();
        forbiddenList.AddRange(new[] { 56, 70, 84, 98 });
        for (int i = 0; i < 121; i++)
        {
            if (!forbiddenList.Contains(i))
                cards.Add(i);
        }
        return cards;
    }
}
