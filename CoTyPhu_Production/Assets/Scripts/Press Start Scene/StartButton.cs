using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void onStartClick()
    {
        SoundManager.Ins.Play(AudioClipEnum.Magic);
        SceneManager.LoadScene("MainMenuScene");
    }
}
