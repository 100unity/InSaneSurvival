using System.Collections.Generic;
using UnityEngine;
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

        public Building Building => building;

        public bool IsActive => gameObject.activeSelf;

        public List<ItemResourceData> RequiredResources => requiredResources;

        /// <summary>
        /// Old material of the building. Used for enabling it.
        /// </summary>
        private List<Material> _oldMats;

        // Transparent Shader properties
        private static readonly int SrcBlend = Shader.PropertyToID("Src_Blend");
        private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
        private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");

        /// <summary>
        /// Creates the UI and fades out the building.
        /// </summary>
        private void Awake()
        {
            // Hide by default
            gameObject.SetActive(false);

            _oldMats = new List<Material>();

            SetBlueprintMaterial();
        }

        public void ShowBlueprint() => gameObject.SetActive(true);

        public void ShowBuilding()
        {
            gameObject.SetActive(true);
            ActivateBuilding();
        }

        /// <summary>
        /// Reverts the material and disables this this.
        /// </summary>
        public void ActivateBuilding()
        {
            building.BuildingRenderer.materials = _oldMats.ToArray();
            building.IsBuild = true;
        }

        /// <summary>
        /// Takes the material from the given Building, copies it and fades it out a bit.
        /// See: https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/Inspector/StandardShaderGUI.cs
        /// </summary>
        private void SetBlueprintMaterial()
        {
            foreach (Material blueprintMat in building.BuildingRenderer.materials)
            {
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