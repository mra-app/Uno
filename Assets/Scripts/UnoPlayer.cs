using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoPlayer : MonoBehaviour
{
    [SerializeField]
    private UnoCardStack cardStack;




public void DrawCard(UnoCard card)
    {
        cardStack.Push(card);
    }
}
