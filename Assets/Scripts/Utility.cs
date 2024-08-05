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
}
