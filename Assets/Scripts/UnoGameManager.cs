using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    const int TOTAL_CARDS = 121;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShuffleAndDistribute(int playerCount)
    {
        List<int> allCards = new List<int>();
        for (int i = 0; i < TOTAL_CARDS; i++)
        {
            allCards.Add(i);
        }
        allCards = Utility.Shuffle(allCards);
    }


}
