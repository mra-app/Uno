using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoCardStack : MonoBehaviour
{
    List<UnoCard> cards = new List<UnoCard>();
    public bool isDiscard = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Push(UnoCard card) {
    cards.Add(card);
        card.transform.parent = transform;
        if(isDiscard)
        {
            card.transform.position = transform.position;//TODO: turn the cards
        }
    }
}
