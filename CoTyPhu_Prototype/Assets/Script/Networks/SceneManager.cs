using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager _ins;

    int currentScene;

    [SerializeField]
    GameObject CreateRoomCanvas;

    [SerializeField]
    GameObject WaitingRoomCanvas;

    private void Start()
    {
        _ins = this;
        MoveToCreateRoom();
    }
    public void MoveToWaitingRoom()
    {
        CreateRoomCanvas.SetActive(false);
        WaitingRoomCanvas.SetActive(true);
    }

    public void MoveToCreateRoom()
    {
        CreateRoomCanvas.SetActive(true);
        WaitingRoomCanvas.SetActive(false);
    }
}
