using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Buildings
{
    public class BlueprintTooltip : ObjectTooltip
    {
        [Header("Blueprint-Tooltip")]
        [Tooltip("The BuildingResource prefab for instantiating the list of needed resources")]
        [SerializeField]
        private BuildingResource buildingResource;

        [Tooltip("The layout group used for instantiating the resources")] [SerializeField]
        private VerticalLayoutGroup layoutGroup;

        [Tooltip("The button for building the building")] [SerializeField]
        private Button buildButton;

        [Tooltip("BuildingBlueprint reference for getting the resources and activating the building")] [SerializeField]
        private BuildingBlueprint blueprint;


        /// <summary>
        /// Used for refreshing
        /// </summary>
        private readonly List<BuildingResource> _resources = new List<BuildingResource>();

        protected override void Awake()
        {
            base.Awake();

            buildButton.gameObject.SetActive(false);
            buildButton.onClick.AddListener(BuildBuilding);

            // Setup tooltip
            foreach (ItemResourceData requiredResource in blueprint.RequiredResources)
            {
                BuildingResource resource = Instantiate(buildingResource, layoutGroup.transform);
                resource.Init(requiredResource);
                _resources.Add(resource);
            }
        }

        protected override void Update()
        {
            // If the blueprint is hidden, hide the tooltip as well
            if (!blueprint.IsActive)
            {
                tooltipContent.SetActive(false);
                return;
            }

            base.Update();
        }


        private void OnEnable() => InventoryManager.Instance.ItemHandler.ItemsUpdated += ItemsUpdated;

        private void OnDisable() => InventoryManager.Instance.ItemHandler.ItemsUpdated -= ItemsUpdated;


        /// <summary>
        /// Checks if the building can be build. If so, "activates" it and removes the items from the player's inventory.
        /// </summary>
        private void BuildBuilding()
        {
            if (!CanBuild(InventoryManager.Instance.ItemHandler)) return;

            // Deactivate this tooltip. Not needed anymore
            gameObject.SetActive(false);

            blueprint.ActivateBuilding();
            // Remove items from inventory
            foreach (ItemResourceData requiredResource in blueprint.RequiredResources)
                InventoryManager.Instance.RemoveItem(requiredResource.item, requiredResource.amount);
        }

        /// <summary>
        /// Checks if the given item handler has the needed resources.
        /// </summary>
        /// <param name="itemHandler">The item handler that holds the needed items</param>
        /// <returns>Whether the needed items are present</returns>
        private bool CanBuild(IItemHandler itemHandler) => blueprint.RequiredResources.All(resourceData =>
            itemHandler.ContainsItem(resourceData.item, resourceData.amount));

        /// <summary>
        /// Refreshes the buildButton and the resources
        /// </summary>
        private void ItemsUpdated()
        {
            buildButton.gameObject.SetActive(CanBuild(InventoryManager.Instance.ItemHandler));
            _resources.ForEach(resource => resource.Refresh());
        }
    }
}