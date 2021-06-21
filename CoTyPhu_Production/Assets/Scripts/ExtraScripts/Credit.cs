using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credit : MonoBehaviour
{
    public void GoToCreditScene()
    {
        SceneManager.LoadScene("CreditScene");
    }

    public void GoToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}
