using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AUsable : ScriptableObject
{
    [SerializeField] private Sprite icon;
    [SerializeField] private int maxStackSize;
    public Sprite Icon => icon;
    public int MaxStackSize => maxStackSize;

    public abstract bool Use();
}
