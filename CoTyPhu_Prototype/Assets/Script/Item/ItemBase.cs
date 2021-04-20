using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    ////////////////////
    /// Helper class ///
    ////////////////////
    public struct ItemPriceCalculationInfo{
        public int PlayerCash; // How much money the player has
        public int TotalCash;  // The total amount of money all players have
        public float ItemPricePercent;   // For calculating Percentage based item prices, value 0..1
        public int FixedPrice;         // For calculating Fixed price items
    }
    public interface ItemPriceStrategy{
        public int Price(ItemPriceCalculationInfo info);
    }

    public class ItemPriceStrategyPercentTotalCash : ItemPriceStrategy{
        int ItemPriceStrategy.Price(ItemPriceCalculationInfo info)
        {
            return Mathf.RoundToInt(info.ItemPricePercent * info.TotalCash);
        }
    }

    ////////////////////
    /// Class fields ///
    ////////////////////
    public ItemPriceStrategy priceStrategy;
    public int BuyPrice;
    ///////////////////////
    /// Class Functions ///
    ///////////////////////

    // Start is called before the first frame update
    void Start()
    {
        // set price strategy
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int CalculateTotalCash()
    {
        return 0;
    }

    public virtual void PerformItemEffect(PlayerControl activator)
    {
        // Do its thing
        Debug.Log("Item " + this.ToString() + " Performinging Action.");
        // Delete self
        Destroy(gameObject);
    }

    public virtual void BuyAction(PlayerControl activator)
    {
        // Reduce player gold
        Gold gold = activator.GetGold();
        if (gold == null)
        {
            Debug.Log("Gold not found");
            return;
        }

        // Move to player item slot
    }

    public virtual void SellAction(PlayerControl activator)
    {
        // Return gold to player
        // Delete self
        Destroy(gameObject);
    }
}
