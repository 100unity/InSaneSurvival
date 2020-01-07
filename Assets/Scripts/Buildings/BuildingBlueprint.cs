using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Inventory;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Buildings
{
    /// <summary>
    /// Shows a preview of the building (slightly faded out) to indicate that it needs to be build first.
    /// It will show an UI that tells the player what resources are needed when the player is close enough
    /// </summary>
    public class BuildingBlueprint : MonoBehaviour
    {
        [Header("Building Blueprint")]
        [Tooltip("The building of which a blueprint should be created.")]
        [SerializeField]
        private Building building;

        [Tooltip("All required resources/items for building this building")] [SerializeField]
        private List<ItemResourceData> requiredResources;

        [Tooltip("How strong the blueprint should be faded out")] [SerializeField] [Range(0f, 1f)]
        private float fadeStrength;

        [Header("Tooltip")] [Tooltip("The tooltip that will be displayed")] [SerializeField]
        private GameObject tooltip;

        [Tooltip("The BuildingResource prefab for instantiating the list of needed resources")] [SerializeField]
        private BuildingResource buildingResource;

        [Tooltip("The layout group used for instantiating the resources")] [SerializeField]
        private VerticalLayoutGroup layoutGroup;

        [Tooltip("The button for building the building")] [SerializeField]
        private Button buildButton;

        [Tooltip("The offset that will be applied to the position of the tooltip")] [SerializeField]
        private Vector3 offset;


        /// <summary>
        /// Old material of the building. Used for enabling it.
        /// </summary>
        private List<Material> _oldMats;

        /// <summary>
        /// Used for calculating the tooltip-position.
        /// </summary>
        private Camera _camera;

        /// <summary>
        /// Used for refreshing.
        /// </summary>
        private readonly List<BuildingResource> _resources = new List<BuildingResource>();

        // Transparent Shader properties
        private static readonly int SrcBlend = Shader.PropertyToID("Src_Blend");
        private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
        private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");

        /// <summary>
        /// Creates the UI and fades out the building.
        /// </summary>
        private void Awake()
        {
            _oldMats = new List<Material>();

            _camera = Camera.main;
            buildButton.gameObject.SetActive(false);
            buildButton.onClick.AddListener(BuildBuilding);

            SetBlueprintMaterial();

            // Setup tooltip
            foreach (ItemResourceData requiredResource in requiredResources)
            {
                BuildingResource resource = Instantiate(buildingResource, layoutGroup.transform);
                resource.Init(requiredResource);
                _resources.Add(resource);
            }
        }

        private void OnEnable() => InventoryManager.Instance.ItemHandler.ItemsUpdated += ItemsUpdated;

        private void OnDisable() => InventoryManager.Instance.ItemHandler.ItemsUpdated -= ItemsUpdated;

        /// <summary>
        /// Checks if the player is close enough and moves the tooltip.
        /// </summary>
        private void Update()
        {
            if (building.PlayerInReach)
            {
                tooltip.transform.position = _camera.WorldToScreenPoint(building.transform.position) + offset;
                tooltip.SetActive(true);
            }
            else
                tooltip.SetActive(false);
        }

        /// <summary>
        /// Checks if the building can be build. If so, "activates" it and removes the items from the player's inventory.
        /// </summary>
        private void BuildBuilding()
        {
            if (!CanBuild(InventoryManager.Instance.ItemHandler)) return;

            ActivateBuilding();
            // Remove items from inventory
            foreach (ItemResourceData requiredResource in requiredResources)
                InventoryManager.Instance.RemoveItem(requiredResource.item, requiredResource.amount);
        }

        /// <summary>
        /// Checks if the given item handler has the needed resources.
        /// </summary>
        /// <param name="itemHandler">The item handler that holds the needed items</param>
        /// <returns>Whether the needed items are present</returns>
        private bool CanBuild(IItemHandler itemHandler) => requiredResources.All(resourceData =>
            itemHandler.ContainsItem(resourceData.item, resourceData.amount));


        /// <summary>
        /// Reverts the material and destroys this.
        /// </summary>
        private void ActivateBuilding()
        {
            building.BuildingRenderer.materials = _oldMats.ToArray();
            building.IsBuild = true;
            Destroy(gameObject);
        }

        /// <summary>
        /// Refreshes the buildButton and the resources
        /// </summary>
        private void ItemsUpdated(Item item, int amount)
        {
            buildButton.gameObject.SetActive(CanBuild(InventoryManager.Instance.ItemHandler));
            _resources.ForEach(resource => resource.Refresh());
        }

        /// <summary>
        /// Takes the material from the given Building, copies it and fades it out a bit.
        /// See: https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/Inspector/StandardShaderGUI.cs
        /// </summary>
        private void SetBlueprintMaterial()
        {
            for (int i = 0; i < building.BuildingRenderer.materials.Length; i++)
            {
                Material blueprintMat = building.BuildingRenderer.materials[i];
                // Save copy of original material
                _oldMats.Add(new Material(blueprintMat));

                blueprintMat.shader = Shader.Find("Standard");
                // Set rendering mode to transparent
                blueprintMat.SetInt(SrcBlend, (int) UnityEngine.Rendering.BlendMode.One);
                blueprintMat.SetInt(DstBlend, (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                blueprintMat.SetInt(ZWrite, 0);
                blueprintMat.DisableKeyword("_ALPHATEST_ON");
                blueprintMat.DisableKeyword("_ALPHABLEND_ON");
                blueprintMat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                blueprintMat.renderQueue = 3000;
                // Lower alpha
                Color color = blueprintMat.color;
                color.a = fadeStrength;
                blueprintMat.color = color;
            }
        }
    }
}