using System;
using Player;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private StatBar healthBar;
        [SerializeField] private StatBar saturationBar;
        [SerializeField] private StatBar hydrationBar;

        private void Awake()
        {
            PlayerState.OnPlayerHealthUpdated += value => healthBar.UpdateBar(value);
            PlayerState.OnPlayerHydrationUpdated += value => hydrationBar.UpdateBar(value);
            PlayerState.OnPlayerSaturationUpdated += value => saturationBar.UpdateBar(value);
        }
    }
}