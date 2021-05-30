using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileChooserButton : MonoBehaviour
{
    public PLOT plot;
    public Button button;

    public static Dictionary<PLOT, TileChooserButton> buttonDictionary = new Dictionary<PLOT, TileChooserButton>();

    public void OnClick()
    {
        TileChooserManager.GetInstance().ChooseTile((int)plot);
    }
    void Awake()
    {
        buttonDictionary[plot] = this;
        button = GetComponent<Button>();
    }
}
