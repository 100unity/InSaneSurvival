﻿using Crafting;
using Entity.Player;
using Inventory.UI;
using Managers;
using UnityEngine;

namespace UI
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private StatBar healthBar;
        [SerializeField] private StatBar saturationBar;
        [SerializeField] private StatBar hydrationBar;

        [Tooltip("The inventory UI to toggle when pressing the inventory key")] [SerializeField]
        private InventoryUI inventoryUI;

        [Tooltip("The crafting UI to be toggled when pressing the crafting key")] [SerializeField]
        private CraftingUI craftingUI;

        [Tooltip("The speech bubble to use for displaying hints")] [SerializeField]
        private SpeechBubble speechBubble;

        [Tooltip("The wearing indicator for displaying equipped items")] [SerializeField]
        private WearingIndicator wearingIndicator;

        public SpeechBubble SpeechBubble => speechBubble;
        public WearingIndicator WearingIndicator => wearingIndicator;

        private void OnEnable()
        {
            PlayerState.OnPlayerHealthUpdate += OnHealthUpdated;
            PlayerState.OnPlayerHydrationUpdate += OnHydrationUpdated;
            PlayerState.OnPlayerSaturationUpdate += OnSaturationUpdated;
        }

        private void OnDisable()
        {
            PlayerState.OnPlayerHealthUpdate -= OnHealthUpdated;
            PlayerState.OnPlayerHydrationUpdate -= OnHydrationUpdated;
            PlayerState.OnPlayerSaturationUpdate -= OnSaturationUpdated;
        }

        /// <summary>
        /// Shows the inventory UI and updates the Cursor with <see cref="CursorManager.UpdateCursor"/>.
        /// </summary>
        public void ShowInventory()
        {
            inventoryUI.ToggleInventory();
            CursorManager.Instance.UpdateCursor();
        }

        /// <summary>
        /// Shows the crafting UI and updates the Cursor with <see cref="CursorManager.UpdateCursor"/>
        /// </summary>
        public void ShowCrafting()
        {
            craftingUI.Toggle();
            CursorManager.Instance.UpdateCursor();
        }

        private void OnHealthUpdated(int value) => healthBar.UpdateBar(value);
        private void OnHydrationUpdated(int value) => hydrationBar.UpdateBar(value);
        private void OnSaturationUpdated(int value) => saturationBar.UpdateBar(value);
    }
}