using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Buildings
{
    public class BuildingTooltip : ObjectTooltip
    {
        [Header("Building-Tooltip")]
        [Tooltip("Reference to the building. Used for checking if the player is close enough for interaction")]
        [SerializeField]
        private Building building;

        [Tooltip("The button for interacting with the building")] [SerializeField]
        private Button btnInteract;

        /// <summary>
        /// Hides the tooltip if the building is not build yet and adds the interaction action to the button.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            btnInteract.onClick.AddListener(building.Interact);
        }

        protected override void Update()
        {
            if (!building.IsBuild)
            {
                tooltipContent.SetActive(false);
                return;
            }

            base.Update();
        }
    }
}