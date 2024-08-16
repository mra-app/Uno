using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Owner
{

    Player1,
    Player2,
    Player3,
    Player4,
    Discard,
    Draw,
}
public class UnoGameManager : MonoBehaviour
{

    const int TOTAL_CARDS = 108;
    const int PLAYER_INIT_CARDS = 5;
    public UnoCardStack DrawStack;
    public UnoCardStack DiscardStack;
    public List<UnoPlayer> Players;
    public GameObject cardPrefab;
    Sprite[] CardSprites;
    List<UnoCard> AllCards = new List<UnoCard>();
    private int Turn = 0;
    private int PlayerCount;
    private bool LockCards = false;
    private UnoCard LastCard = null;
    private int ChangeTurnOrder = 1;
    public GameObject SelectColor;
    void Start()
    {

        CardSprites = Resources.LoadAll<Sprite>("");

        PlayerCount = 4;

        for (int i = 0; i < PlayerCount; i++)
        {
            Players[i].SetOwner((Owner)i);
        }

        ShuffleAndDistribute(PlayerCount);
    }

    public void ChangeTurn(UnoCard card = null)
    {
        if(card != null)
        {
            Turn = (Turn + card.TurnChangeAmount* ChangeTurnOrder) % PlayerCount;
            if(card.TurnChangeAmount < 0)
            {
                ChangeTurnOrder = ChangeTurnOrder * -1;
            }

        }
        else
         Turn = (Turn + ChangeTurnOrder) % PlayerCount;
        if (Turn < 0)
            Turn += PlayerCount;
        DebugControl.Log("turn" + Turn, 3);
    }

    public void ShuffleAndDistribute(int playerCount)
    {
        List<int> allNumbers = new List<int>();
        for (int i = 0; i < TOTAL_CARDS; i++) { allNumbers.Add(i); }
        allNumbers = Utility.Shuffle(allNumbers);

        for (int i = 0; i < TOTAL_CARDS; i++)
        {
            AllCards.Add(MakeCard(allNumbers[i], i));
            //  DebugControl.Log(allNumbers[i]+" ",3);
        }

        int j = 0;
        while (AllCards.Count > j)
        {
            DrawStack.Push(AllCards[j]);
            AllCards[j].ShowBackImg(false);//todo

            j++;
        }
        int drawCardCount = AllCards.Count - (4 * PLAYER_INIT_CARDS + 1);


        DiscardStack.PushAndMove(AllCards[0], () => {
            StartCoroutine(DistCardtoPlayers(drawCardCount + 1));
        });
         LastCard = AllCards[0];


    }
    IEnumerator DistCardtoPlayers(int AllCardIdx)
    {
        int initj = AllCardIdx;
        for (int i = 0; i < PlayerCount; i++)
        {
            while (AllCardIdx < initj + PLAYER_INIT_CARDS * (i + 1))
            {
                Players[i].DrawCard(AllCards[AllCardIdx], () => { });
                //  if (i == 0)//TODO: in online have to change
                AllCards[AllCardIdx].ShowBackImg(false);
                AllCardIdx++;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    private UnoCard MakeCard(int id, int globalCardIdx)
    {
        GameObject card = Instantiate(cardPrefab);
        UnoCard cardScript = card.GetComponent<UnoCard>();
        cardScript.InitCard(id, CardSprites[id], globalCardIdx);
        cardScript.OnSelected += OnCardSelected;
        return cardScript;
    }
    public void OnCardSelected(int globalCardIdx, Owner owner)
    {
        if (LockCards)
            return;

        UnoCard cardScript = AllCards[globalCardIdx];
        if ((int)owner == Turn && LastCard.AccumulatedCards <= 0)
        {
            DebugControl.Log("1", 3);

            if (LastCard.AcceptsCard(cardScript))
            {
                DebugControl.Log("12", 3);
                LockCards = true;
                DiscardStack.PushAndMove(cardScript, () =>
                {
                    if(cardScript.Type==UnoCard.SpecialCard.Wild|| cardScript.Type == UnoCard.SpecialCard.Take4)
                        SelectColor.SetActive(true);
                    ChangeTurn(cardScript);
                    //if (LastCard.AccumulatedCards != 0)
                    //    cardScript.AccumulatedCards+= LastCard.AccumulatedCards;
                    LastCard = cardScript;
                    

                    LockCards = false;
                });
            
            }
        }
        else if (owner == Owner.Draw)
        {
            DebugControl.Log("2", 3);
            LockCards = true;
            //if (LastCard.AccumulatedCards != 0)
            //{
                
            //    for(int i = 0; i < LastCard.AccumulatedCards; i++)
            //    {
            //        Players[(int)Turn].DrawCard(cardScript, () => { });
            //    }
            //    cardScript.AccumulatedCards = 0;

            //}
            Players[(int)Turn].DrawCard(cardScript, () =>
            {
                LastCard.AccumulatedCards--;
                if (LastCard.AccumulatedCards <= 0)
                {
                     ChangeTurn();
                }

                //   if ((int)Turn == 0)//TODO: in online have to change
                    cardScript.ShowBackImg(false);
              
                LockCards = false;
            });
        }



    }
    public void ColorSelected(int color)
    {
        LastCard.SetWildColor((UnoCard.CardType)color);
        SelectColor.SetActive(false);
    }



}
