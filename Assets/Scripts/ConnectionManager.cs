using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI StatusText;
    private bool isConnecting = false;
    private const string gameVersion = "v1";
    [SerializeField]
    private GameObject ConnectPanel;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void SaveName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("Player name is empty");
            return;
        }
        PhotonNetwork.NickName = name;
    }

    public void OnButtonClicked()
    {
        Debug.Log(PhotonNetwork.NickName);
        Connect();
    }

    public void Connect()
    {
        isConnecting = true;
        ConnectPanel.SetActive(false);
        ShowStatus("Connecting...");

        if (PhotonNetwork.IsConnected)
        {
            ShowStatus("Joining Random Room...");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            ShowStatus("Connecting...");
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            ShowStatus("Connected, joining room...");
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        ShowStatus("Creating new room...");
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        isConnecting = false;
        ConnectPanel.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        ShowStatus("Joined room - waiting for another player.");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }
    }

    void ShowStatus(string status)
    {
        StatusText.text = status;
    }
}
