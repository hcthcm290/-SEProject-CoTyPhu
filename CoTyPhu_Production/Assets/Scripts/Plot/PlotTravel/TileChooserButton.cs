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
        SoundManager.Ins.Play(AudioClipEnum.Select);
    }
    void Awake()
    {
        buttonDictionary[plot] = this;
        button = GetComponent<Button>();
    }

    private void Start()
    {
        // Align plot position.
        float blockMinorSide = 48;
        float blockMajorSide = 72;
        float offsetX = 0;
        float offsetY = 24;

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

        (transform as RectTransform).anchoredPosition = new Vector3(x + offsetX, y + offsetY);
        if (num % 8 == 0)
            (transform as RectTransform).sizeDelta = new Vector2(blockMajorSide, blockMajorSide);
        else
        {
            if (num % 16 < 8)
                (transform as RectTransform).sizeDelta = new Vector2(blockMajorSide, blockMinorSide);
            else 
                (transform as RectTransform).sizeDelta = new Vector2(blockMinorSide, blockMajorSide);
        }
    }
}
