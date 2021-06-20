using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerResultCard : MonoBehaviour
{
    #region UI properties
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI playerNetworth;
    [SerializeField] GameObject merchant;
    #endregion

    public void SetInfo(Player player ,GameObject merchantPrefab)
    {
        //if(merchant != null)
        //{
        //    Destroy(merchant);
        //}
        //merchant = Instantiate(merchantPrefab, transform);
        //var merchantLocalPosition = merchant.transform.localPosition;
        //merchantLocalPosition.z = -100;
        //merchant.transform.localPosition = merchantLocalPosition;
        playerName.text = player.Name;
        playerNetworth.text = $"Networth {player.CalculateNetworth()}";
    }
}
