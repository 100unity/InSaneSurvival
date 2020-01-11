using Managers;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// An object tooltip will automatically follow the building when enabled.
    /// </summary>
    public abstract class ObjectTooltip : MonoBehaviour
    {
        [Header("Tooltip-Base")] [Tooltip("Used for calculations and positioning")] [SerializeField]
        private GameObject parent;

        [Tooltip("The content to be moved and to be toggled")] [SerializeField]
        protected GameObject tooltipContent;

        [Tooltip("[CAN BE UNDEFINED]\nApplies an offset to the tooltip")] [SerializeField]
        private Vector3 offset;

        [Tooltip("Defines the distance to the player in which the tooltip is shown")] [SerializeField]
        private float distanceToPlayer;

        /// <summary>
        /// Used for moving the tooltip.
        /// </summary>
        private Camera _camera;

        protected virtual void Awake() => _camera = Camera.main;

        /// <summary>
        /// Moves the tooltip and shows/hides it.
        /// </summary>
        protected virtual void Update()
        {
            bool playerInReach = PlayerManager.Instance.PlayerInReach(parent.gameObject, distanceToPlayer);
            if (playerInReach)
                tooltipContent.transform.position = _camera.WorldToScreenPoint(parent.transform.position) + offset;

            tooltipContent.SetActive(playerInReach);
        }
    }
}