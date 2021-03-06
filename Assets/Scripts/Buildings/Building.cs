﻿using Managers;
using UnityEngine;

namespace Buildings
{
    /// <summary>
    /// Simple building behaviour.
    /// </summary>
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

        [Tooltip("The additional behaviour to enable after the building is built")] [SerializeField]
        private GameObject additionalBehaviour;

        public Renderer BuildingRenderer => buildingRenderer;

        /// <summary>
        /// Whether the player is in reach of this building.
        /// </summary>
        protected bool PlayerInReach { get; private set; }

        /// <summary>
        /// Whether the building is build or not. Invokes <see cref="OnBuild"/>
        /// </summary>
        public bool IsBuilt { get; set; }

        /// <summary>
        /// Can be used to temporarily disable the interact behavior.
        /// </summary>
        public bool InteractDisabled { get; protected set; }


        protected virtual void Awake() => IsBuilt = isAlreadyBuilt;

        /// <summary>
        /// Updates <see cref="PlayerInReach"/>.
        /// </summary>
        protected virtual void Update() =>
            PlayerInReach = PlayerManager.Instance.PlayerInReach(gameObject, useDistance);

        /// <summary>
        /// Can be overriden to define an interaction action.
        /// </summary>
        public void Interact()
        {
            if (!PlayerInReach || InteractDisabled) return;
            OnInteract();
        }

        protected abstract void OnInteract();

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
            if (additionalBehaviour)
                additionalBehaviour.SetActive(true);
        }
    }
}