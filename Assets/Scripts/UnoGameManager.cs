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

    public void ChangeTurn()
    {
        Turn = (Turn +1) % PlayerCount;
    } 

    public void ShuffleAndDistribute(int playerCount)
    {
        List<int> allNumbers = new List<int>();
        for(int i = 0; i < TOTAL_CARDS; i++) { allNumbers.Add(i); }
        allNumbers = Utility.Shuffle(allNumbers);

        for (int i = 0; i < TOTAL_CARDS; i++)
        {
            AllCards.Add(MakeCard(allNumbers[i],i));
          //  DebugControl.Log(allNumbers[i]+" ",3);
        }

        int j = 0;
        while (AllCards.Count > j)
        {
            DrawStack.Push(AllCards[j]);
            j++;
        }
        int drawCardCount = AllCards.Count - (4 * PLAYER_INIT_CARDS + 1);

        DiscardStack.PushAndMove(AllCards[0], () => {
            StartCoroutine(DistCardtoPlayers(drawCardCount + 1));
        });
        


        }
    IEnumerator DistCardtoPlayers(int AllCardIdx)
    {
        int initj = AllCardIdx;
        for(int i = 0; i < PlayerCount; i++)
        {
            while (AllCardIdx < initj + PLAYER_INIT_CARDS * (i + 1))
            {
                Players[i].DrawCard(AllCards[AllCardIdx], () => {});
              //  if (i == 0)//TODO: in online have to change
                   AllCards[AllCardIdx].ShowBackImg(false);
                AllCardIdx++;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    private UnoCard MakeCard(int id,int globalCardIdx)
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
        
       // DebugControl.Log("were yo" + globalCardIdx + owner.ToString()+ AllCards[globalCardIdx].id+" turn "+Turn, 3);
        UnoCard cardScript = AllCards[globalCardIdx];
        if ((int)owner == Turn)
        {
            LockCards = true;
            DiscardStack.PushAndMove(cardScript, () =>
            {
               // DebugControl.Log("free", 3);
                ChangeTurn();
                LockCards = false;
             });  
        }
        else if(owner == Owner.Draw)
        {
            LockCards = true;
            Players[(int)Turn].DrawCard(cardScript, () =>
            {
             //   if ((int)Turn == 0)//TODO: in online have to change
                    cardScript.ShowBackImg(false);
               // DebugControl.Log("free2", 3);
                ChangeTurn();
                LockCards = false;
            });
        }

       

    }
   


}
