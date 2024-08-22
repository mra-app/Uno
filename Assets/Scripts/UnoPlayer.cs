using System;
using System.Collections;
using UnityEngine;

public class UnoPlayer : MonoBehaviour
{
    [SerializeField]
    UnoCardStack cardStack;
    private Owner owner;
    public GameObject MyTurnImage;
    public GameObject SelectColorPanel;
    UnoAI AI;
    public UnoCardStack DrawStack;//TODO: unserialized?
    UnoCard LastCard = null;
    public void Start()
    {
        if(GetComponent<UnoAI>() != null) {
            AI = GetComponent<UnoAI>();
        }
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
        if( AI != null)
        {
            if (!isMyTurn )
            {
                AIStop();
            }
            else
            {
                 StartCoroutine(AIPlay());

            }
        }
    }
    public void SelectWildCardColor(UnoCard cardScript)
    {
        if (AI == null)
        {
            LastCard = cardScript;
            SelectColorPanel.SetActive(true);

        }
        else
        {
            AIStop();
            cardScript.SetWildColor(AI.SelectColorForWild(cardStack));

        }
    }
    public void ColorSelected(int color)
    {
        LastCard.SetWildColor((UnoCard.CardType)color);
        SelectColorPanel.SetActive(false);
    }
    IEnumerator AIPlay()
    {
        yield return new WaitForSeconds(0.1f);
        AI.StartPlay(true, cardStack, DrawStack);


    }
    void AIStop()
    {
        AI.StartPlay(false);

    }

}
