using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UnoCardStack : MonoBehaviour
{
    List<UnoCard> cards = new List<UnoCard>();
    public bool isDiscard = false;
    public Owner owner;
    int Discard_Z = 0;
    public Action<UnoCard,int,Owner> OnCardSelected;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool IsEmpty()
    {
        return cards.Count == 0;//TODO!
    }

    //Push: changes the card parent and changes the rotation
    public void Push(UnoCard card)
    {
        card.owner = owner;
        cards.Add(card);
        card.transform.SetParent(transform);

        card.OnSelected += OnCardSelected;//TODO


        if (isDiscard)
        {
            card.transform.rotation = Quaternion.Euler(0, 0, Discard_Z);
            card.ShowBackImg(false);
            Discard_Z += 45;
        }
        else
            card.transform.rotation = transform.rotation;
    }
    public void Pop(UnoCard card)
    {
        card.OnSelected -= OnCardSelected;

        cards.Remove(card);
    }
    //PushAndMove: moves the card position to the stack position
    public void PushAndMove(UnoCard card, Action callback)
    {
        card.Move(transform.position, () =>
        {
            Push(card);
            callback();
        });
    }
    public List<UnoCard> GetAllCards()
    {
        return cards;
    }

}
