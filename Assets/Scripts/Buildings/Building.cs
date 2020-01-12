using Managers;
using UnityEngine;

namespace Buildings
{
    /// <summary>
    /// Simple building behaviour.
    /// </summary>
    [System.Serializable]
    public abstract class Building : MonoBehaviour
    {
        [Header("Building-Base")]
        [Tooltip("The renderer that holds the material of this building. Used for fading it out")]
        [SerializeField]
        private Renderer buildingRenderer;

        [Tooltip("Defines the needed distance to the player for using this")] [SerializeField]
        private float useDistance;

        [Tooltip("Whether this building is build from the start or need to be build first\n" +
                 "Make sure to add a BuildingBlueprint if false")]
        [SerializeField]
        private bool isAlreadyBuilt;

        public Renderer BuildingRenderer => buildingRenderer;

        /// <summary>
        /// Whether the player is in reach of this building.
        /// </summary>
        protected bool PlayerInReach { get; private set; }

        /// <summary>
        /// Whether the building is build or not. Invokes <see cref="OnBuild"/>
        /// </summary>
        public bool IsBuilt { get; set; }


        private void Awake() => IsBuilt = isAlreadyBuilt;

        /// <summary>
        /// Updates <see cref="PlayerInReach"/>.
        /// </summary>
        protected virtual void Update() =>
            PlayerInReach = PlayerManager.Instance.PlayerInReach(gameObject, useDistance);

        /// <summary>
        /// Can be overriden to define an interaction action.
        /// </summary>
        public virtual void Interact()
        {
            if (!PlayerInReach) return;
        }

        public void Build()
        {
            IsBuilt = true;
            OnBuild();
        }

        /// <summary>
        /// Can be overriden to define an action on building this building.
        /// </summary>
        protected virtual void OnBuild()
        {
        }
    }
}