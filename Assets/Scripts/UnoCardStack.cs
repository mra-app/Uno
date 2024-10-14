using System.Collections.Generic;
using System;
using UnityEngine;

public class UnoCardStack : MonoBehaviour
{
    List<UnoCard> cards = new List<UnoCard>();
    int Discard_Z = 0;

    public bool isDiscard = false;
    public Action<UnoCard> OnCardSelected;
    public AudioSource CardSound;
    public bool IsEmpty()
    {
        return cards.Count == 0;
    }

    public void Pop(UnoCard card)
    {
        card.OnSelected -= OnCardSelected;

        cards.Remove(card);
    }
    public void PlayCardSound()
    {
        CardSound.Play();
    }

    /// <summary>
    /// moves the card position to this stack position and change parents and add to list
    /// </summary>
    /// <param name="card"></param>
    /// <param name="Silent"></param>
    /// <param name="callback"></param>
    public void PushAndMove(UnoCard card,bool Silent, Action callback)
    {
        if(!Silent) {
            PlayCardSound();
            }

        cards.Add(card);
        card.OnSelected += OnCardSelected;

        if (isDiscard)
        {
            card.transform.rotation = Quaternion.Euler(0, 0, Discard_Z);
            card.ShowBackImg(false);
            Discard_Z += 45;
        }
        else
            card.transform.rotation = transform.rotation;

        card.Move(transform.position, () =>
        {
            card.transform.SetParent(transform);
            callback();
        });
    }
    public List<UnoCard> GetAllCards()
    {
        List<UnoCard> ALLcards = new List<UnoCard>();
        for (int i = 0; i < cards.Count; i++)
        {
            ALLcards.Add(cards[i]);
        }

        return ALLcards;
    }
    public bool HasOneCard()
    {
        return cards.Count == 1;
    }

}
