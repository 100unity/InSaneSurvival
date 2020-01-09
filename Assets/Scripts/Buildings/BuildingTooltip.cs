using UnityEngine;
using UnityEngine.UI;

namespace Buildings
{
    public class BuildingTooltip : MonoBehaviour
    {
        [Tooltip("Reference to the building. Used for checking if the player is close enough for interaction")]
        [SerializeField]
        private Building building;

        [Tooltip("The tooltip itself. Used for showing/hiding")] [SerializeField]
        private GameObject tooltip;

        [Tooltip("The button for interacting with the building")] [SerializeField]
        private Button btnInteract;

        [Tooltip("[CAN BE UNDEFINED]\nApplies an offset to the tooltip")] [SerializeField]
        private Vector3 offset;

        /// <summary>
        /// Used for moving the tooltip.
        /// </summary>
        private Camera _camera;

        /// <summary>
        /// Hides the tooltip if the building is not build yet and adds the interaction action to the button.
        /// </summary>
        private void Awake()
        {
            _camera = Camera.main;
            btnInteract.onClick.AddListener(building.Interact);
            tooltip.SetActive(building.IsBuild);
        }

        /// <summary>
        /// Moves the tooltip and shows/hides it.
        /// </summary>
        private void Update()
        {
            if (!building.IsBuild) return;
            if (building.PlayerInReach)
                tooltip.transform.position = _camera.WorldToScreenPoint(building.transform.position) + offset;
            ToggleTooltip(building.PlayerInReach);
        }

        /// <summary>
        /// Shows/Hides the tooltip.
        /// </summary>
        /// <param name="show">Whether to show or hide the tooltip</param>
        private void ToggleTooltip(bool show) => tooltip.SetActive(show);
    }
}