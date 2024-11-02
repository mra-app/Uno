using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class OnlinePlayer : MonoBehaviour, IOnEventCallback
{
    //public UnoGameManager gameManager;
    //[PunRPC]
    //public void OnCardClicked(int cardID,int owner,int sender)//sender:0 player0  1:player1 2:drawpile
    //{
    //    UnoCard card = new UnoCard();
    //    if (sender == 2)
    //    {
    //        card = gameManager.DrawPile.GetaCard(cardID);
    //    }

    //    cards.OnClick(owner);

    //}

    public const byte MoveUnitsToTargetPositionEventCode = 1;
    public const byte ShuffleAndDistAllCardsCode = 2;
    UnoGameManager gameManager;

    private void OnEnable()
    {
        gameManager = GetComponent<UnoGameManager>();
        if( gameManager == null)
        {
            Debug.LogWarning("yo");
        }
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == ShuffleAndDistAllCardsCode)
        {
            int[] intArray = (int[])photonEvent.CustomData;
            List<int> list = new List<int>(intArray);
            Debug.LogError("ShuffleAndDistAllCards" + list.Count);

            gameManager.DrawPile.CreateAndDistCards(list);

        }
        if (eventCode == MoveUnitsToTargetPositionEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            Debug.LogError("yo!");
            //Vector3 targetPosition = (Vector3)data[0];
            //for (int index = 1; index < data.Length; ++index)
            //{
            //    int unitId = (int)data[index];
            //    UnitList[unitId].TargetPosition = targetPosition;
            //}
        }
    }
}
