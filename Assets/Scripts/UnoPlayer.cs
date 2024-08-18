using System;
//using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;

public class UnoPlayer : MonoBehaviour
{
    [SerializeField]
    UnoCardStack cardStack;
    private Owner owner;
    public GameObject MyTurnImage;

    public void Start()
    {

    }
    public bool AllCardsPlayed() {
        return cardStack.IsEmpty();
    }
    public void RemoveCard(UnoCard card)
    {
        cardStack.Pop(card);
    }
    public void SetOwner(Owner _owner)
    {
        owner = _owner;
        cardStack.owner = _owner; //stacks without player have assigned owners from editor.
    }
    public void DrawCard(UnoCard card, Action callback)
    {
        cardStack.PushAndMove(card, () =>
        {
            callback();
        });
    }
    public void ChangeTurnToMe(bool isMyTurn)
    {
        MyTurnImage.SetActive(isMyTurn);
    }

}
