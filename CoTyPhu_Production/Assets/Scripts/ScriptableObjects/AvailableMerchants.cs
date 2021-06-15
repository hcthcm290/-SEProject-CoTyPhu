using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
		fileName = "Available Merchants Resource",
		menuName = "[MyProject]/Available Merchant",
		order = 0)]
public class AvailableMerchants : ScriptableObject
{
	[SerializeField] public List<BaseMerchant> listMerchant;
}
