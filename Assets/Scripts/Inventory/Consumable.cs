using System.Collections;
using System.Collections.Generic;
using Entity.Player;
using Managers;
using UnityEngine;
using Inventory;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Item/Consumable")]
public class Consumable : Item
{
    [SerializeField]
    private int healthValue;
    [SerializeField]
    private int saturationValue;
    [SerializeField]
    private int hydrationValue;

    /// <summary>
    /// Gets the PlayerState of the player, and changes its values depending on the item values.
    /// </summary>
    public new bool Use() 
    {
        PlayerState playerState = Singleton<PlayerManager>.Instance.GetPlayer().GetComponent<PlayerState>();
        if (healthValue > 0)
            playerState.Hit(-healthValue);
        if (saturationValue > 0)
            playerState.ChangePlayerSaturation(saturationValue);
        if (hydrationValue > 0)
            playerState.ChangePlayerHydration(hydrationValue);
        return true;
    }
    public override bool Equals(object other)
    {
        return other is Consumable item && item.name == name;
    }
}
