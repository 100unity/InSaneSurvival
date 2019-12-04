using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Player;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")]
public class Consumable : AUsable
{
    [SerializeField]
    private int healthValue;
    [SerializeField]
    private int saturationValue;
    [SerializeField]
    private int hydrationValue;

    public override bool Use() 
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
