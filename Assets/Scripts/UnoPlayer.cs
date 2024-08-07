using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoPlayer : MonoBehaviour
{
    [SerializeField]
    private UnoCardStack cardStack;

    public void Start()
    {
      //  DebugControl.Log("this"+transform.rotation, 2);
    }


    public void DrawCard(UnoCard card)
    {
        card.transform.rotation = transform.rotation;
        cardStack.Push(card);
    }
}
