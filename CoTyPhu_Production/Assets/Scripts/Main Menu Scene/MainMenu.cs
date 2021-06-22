using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class MainMenu : MonoBehaviourPunCallbacks
{
    #region UI Preperties
    [SerializeField] Text playerName;
    [SerializeField] Button createGameButton;
    [SerializeField] Button createPublicGameButton;
    [SerializeField] Button createPrivateGameButton;
    [SerializeField] Button publicGamesButton;
    [SerializeField] Text privateGameCode;
    [SerializeField] Button joinPrivateGame;
    [SerializeField] GameObject tableCreateGameTypes;
    [SerializeField] Button closeTableCreateGameButton;
    [SerializeField] Text messageText;
    [SerializeField] GameObject messageObj;
    [SerializeField] Button closeMessageButton;
    [SerializeField] InputField playerNameInputField;
    #endregion

    #region properties
    [SerializeField] int roomNameLength = 6;
    #endregion

    private void MakeSelectSound()
    {
        SoundManager.Ins.Play(AudioClipEnum.Select);
    }

    private void Start()
    {
        createGameButton.onClick.AddListener(OpenTableCreateGame);
        closeTableCreateGameButton.onClick.AddListener(CloseTableCreateGame);
        publicGamesButton.onClick.AddListener(GoPublicGameScene);
        closeMessageButton.onClick.AddListener(CloseMessage);
        joinPrivateGame.onClick.AddListener(JoinPrivateRoom);
        createPublicGameButton.onClick.AddListener(CreatePublicGame);
        createPrivateGameButton.onClick.AddListener(CreatePrivateGame);
        playerNameInputField.onValueChanged.AddListener(OnPlayerNameChange);
        playerNameInputField.text = PhotonNetwork.NickName;


        createGameButton.onClick.AddListener(MakeSelectSound);
        publicGamesButton.onClick.AddListener(MakeSelectSound);
        joinPrivateGame.onClick.AddListener(MakeSelectSound);
        createPublicGameButton.onClick.AddListener(MakeSelectSound);
        createPrivateGameButton.onClick.AddListener(MakeSelectSound);
    }

    public void OnPlayerNameChange(string value)
    {
        PhotonNetwork.NickName = value;
    }

    private void OpenTableCreateGame()
    {
        if(playerName.text == "")
        {
            DisplayMessage("Please provide your name");
            return;
        }
        tableCreateGameTypes.transform.DOScale(1, 0.5f).SetEase(Ease.OutQuad);
    }

    private void CreatePublicGame()
    {
        ConnectToLobby().then((result) =>
        {
            Debug.Log("prepare to create public room");
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsVisible = true;
            roomOptions.MaxPlayers = 5;
            roomOptions.PlayerTtl = 80000;
            roomOptions.EmptyRoomTtl = 0;
            roomOptions.PublishUserId = true;
            roomOptions.CustomRoomPropertiesForLobby = new string[1];
            roomOptions.CustomRoomPropertiesForLobby[0] = "ownerName";
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
            roomOptions.CustomRoomProperties.Add("ownerName", PhotonNetwork.LocalPlayer.NickName);

            string roomName = RandomString(roomNameLength);

            PhotonNetwork.CreateRoom(roomName, roomOptions);
        });
    }

    private void CreatePrivateGame()
    {
        ConnectToLobby().then((result) =>
        {
            Debug.Log("prepare to create private room");
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsVisible = false;
            roomOptions.MaxPlayers = 5;
            roomOptions.PlayerTtl = 80000;
            roomOptions.EmptyRoomTtl = 0;
            roomOptions.PublishUserId = true;

            string roomName = RandomString(roomNameLength);

            PhotonNetwork.CreateRoom(roomName, roomOptions);
        });
    }    

    private void CloseTableCreateGame()
    {
        tableCreateGameTypes.transform.DOScale(0, 0.5f).SetEase(Ease.OutQuad);
    }

    private void GoPublicGameScene()
    {
        if (playerName.text == "")
        {
            DisplayMessage("Please provide your name");
            return;
        }

        ConnectToLobby().then((result) =>
        {
            Debug.Log("prepare to go to public games scene");
            SceneManager.LoadScene("PublicGamesScene");
        });
    }

    private void JoinPrivateRoom()
    {
        if (playerName.text == "")
        {
            DisplayMessage("Please provide your name");
            return;
        }

        if (privateGameCode.text == "")
        {
            DisplayMessage("Please provide the game id");
            return;
        }

        ConnectToLobby().then((result) =>
        {
            PhotonNetwork.JoinRoom(privateGameCode.text.ToUpper());
        });
    }

    FutureTask<bool> _connectToLobbyTask;
    private Future<bool> ConnectToLobby()
    {
        FutureTask<bool> connectTask = new FutureTask<bool>();
        _connectToLobbyTask = connectTask;

        if(!PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.SendRate = 60;
            PhotonNetwork.SerializationRate = 60;
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            connectTask.Complete(true);
        }

        return connectTask.GetFuture();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("joined mastered");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("joined lobby");
        base.OnJoinedLobby();
        _connectToLobbyTask?.Complete(true);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("room created: " + PhotonNetwork.CurrentRoom.Name);
        base.OnCreatedRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("join room successfully: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("prepare loading waiting room scene");
        SceneManager.LoadScene("WaitingRoomScene");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        DisplayMessage(message);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        PublicGameScene.UpdateRoomList(roomList);
    }

    #region Random room name
    private static System.Random random = new System.Random();
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    #endregion

    #region Message
    private void DisplayMessage(string message)
    {
        messageText.text = message;
        messageObj.transform.DOScale(1, 0.5f).SetEase(Ease.OutQuad);
    }

    private void CloseMessage()
    {
        messageObj.transform.DOScale(0, 0.5f).SetEase(Ease.OutQuad);
    }
    #endregion
}
