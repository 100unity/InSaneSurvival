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

        private void OnEnable()
        {
            PlayerState.OnPlayerHealthUpdated += OnHealthUpdated;
            PlayerState.OnPlayerHydrationUpdated += OnHydrationUpdated;
            PlayerState.OnPlayerSaturationUpdated += OnSaturationUpdated;
        }
        
        private void OnDisable()
        {
            PlayerState.OnPlayerHealthUpdated -= OnHealthUpdated;
            PlayerState.OnPlayerHydrationUpdated -= OnHydrationUpdated;
            PlayerState.OnPlayerSaturationUpdated -= OnSaturationUpdated;
        }
        
        private void OnHealthUpdated(int value) => healthBar.UpdateBar(value);
        private void OnHydrationUpdated(int value) => hydrationBar.UpdateBar(value);
        private void OnSaturationUpdated(int value) => saturationBar.UpdateBar(value);
    }
}