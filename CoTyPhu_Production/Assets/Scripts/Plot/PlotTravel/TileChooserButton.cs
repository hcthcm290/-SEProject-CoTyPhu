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

    private void Start()
    {
        // Align plot position.
        float blockMinorSide = 72;
        float blockMajorSide = 108;

        float sideCenter = 3.5f * blockMinorSide + blockMajorSide / 2;
        float x, y;
        int num = (int)plot;
        if (num % 16 <= 8)
            x = (num / 16 * 2 - 1) * sideCenter;
        else
            x = (Mathf.Min(num, 32 - num) % 8 - 4) * blockMinorSide;
        if (num % 16 >= 8 || num % 16 == 0)
            y = (1 - ((num + 31) % 32) / 16 * 2) * sideCenter;
        else
            y = (Mathf.Min(num, 32 - num) % 8 - 4) * blockMinorSide;

        (transform as RectTransform).anchoredPosition = new Vector3(x, y);
    }
}
