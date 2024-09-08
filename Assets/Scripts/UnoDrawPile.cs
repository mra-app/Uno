using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoDrawPile : MonoBehaviour
{
    const int TOTAL_CARDS = 108;
    const int PLAYER_INIT_CARDS = 5;
    public UnoCardStack DrawStack;//TODO
    //public UnoCardStack DiscardStack;
    public UnoDiscardPile DiscardPile;
    public UnoGameManager GameManager;

    public List<UnoPlayer> Players;
    public GameObject cardPrefab;
    Sprite[] CardSprites;
    List<UnoCard> AllCards = new List<UnoCard>();
    private int PlayerCount;

    private void Start()
    {
        DrawStack = GetComponent<UnoCardStack>();
        CardSprites = Resources.LoadAll<Sprite>("");
        DrawStack.OnCardSelected += OnCardSelected;
        PlayerCount = 4;

        for (int i = 0; i < PlayerCount; i++)
        {
            Players[i].SetOwner((Owner)i);//is it needed? move to manager//TODO
            //Players[i].DrawStack = DrawStack;
        }

        ShuffleAndDistribute(PlayerCount);
    }
    public void RemoveFromDraw(UnoCard card)
    {
        DrawStack.Pop(card);
        //DrawStack.OnCardSelected -= OnCardSelected;
        //call discard tremove acc card here
    }

    private void OnCardSelected(UnoCard cardScript, int arg2, Owner owner)
    {
        //DebugControl.Log("draw", 1);

        if (GameManager.IsLockToPlayTurn() ||( GameManager.GetTurn() != (int)cardScript.LastClicked)
)
        {
            return;
        }
        GameManager.LockCardsToPlayTurn(true);

        Players[GameManager.GetTurn()].DrawCard(cardScript, () =>
        {
            //if (GameManager.GetTurn() == 0)//TODO: in online have to change
            //    cardScript.ShowBackImg(false);

            GameManager.DiscardPile.CardDrawn();
            if (GameManager.DiscardPile.CanPlayOnUpCard())
            {
                GameManager.ChangeTurn();

            }
            else
            {
                Players[GameManager.GetTurn()].PlayAgain();
            }

            GameManager.LockCardsToPlayTurn(false);
        });
    }

    public void ShuffleAndDistribute(int playerCount)
    {
        List<int> allNumbers = new List<int>();
        for (int i = 0; i < TOTAL_CARDS; i++) {
            allNumbers.Add(i);
        }
        allNumbers = Utility.Shuffle(allNumbers);

        for (int i = 0; i < TOTAL_CARDS; i++)
        {
            AllCards.Add(MakeCard(allNumbers[i], i));
        }

        int j = 0;
        while (AllCards.Count > j)
        {
            DrawStack.Push(AllCards[j]);
            //  AllCards[j].ShowBackImg(false);//todo

            j++;
        }
        int drawCardCount = AllCards.Count - (4 * PLAYER_INIT_CARDS + 1);
        int x = 0;
        while (!GameManager.IsAcceptableToStart(AllCards[x]))
        {
            x++;
        }
        RemoveFromDraw(AllCards[x]);
        DiscardPile.DiscardedCard(AllCards[x], () => {
            StartCoroutine(DistCardtoPlayers(drawCardCount + 1, () => {
                GameManager.GameStart(AllCards[x]);
            }));
            
        });


    }
    IEnumerator DistCardtoPlayers(int AllCardIdx,Action callback)
    {
        int initj = AllCardIdx;
        int i = 0;
        //for (int i = 0; i < PlayerCount; i++)
        while(i < PlayerCount)
        {
            int j = 0;
            bool waiting = false; 
            while (j<PLAYER_INIT_CARDS)//AllCardIdx < initj + PLAYER_INIT_CARDS * (i + 1))
            {
                if (!waiting)
                {
                    waiting = true;
                    Players[i].DrawCard(AllCards[AllCardIdx], () => {
                        if (i == 0)//TODO: in online have to change
                            AllCards[AllCardIdx].ShowBackImg(false);
                        AllCardIdx++;
                        j++;
                        waiting = false;
                   });
                }

                yield return new WaitForSeconds(0.02f);
            }
            i++;
        }
        callback();
    }
    private UnoCard MakeCard(int id, int globalCardIdx)
    {
        GameObject card = Instantiate(cardPrefab);
        UnoCard cardScript = card.GetComponent<UnoCard>();
        cardScript.InitCard(id, CardSprites[id], globalCardIdx);
        return cardScript;
    }
    public UnoCard GetaCard()
    {
        return DrawStack.GetAllCards()[0];
    }

}
