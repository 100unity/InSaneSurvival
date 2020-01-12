using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Buildings
{
    public class CampsiteTooltip : ObjectTooltip
    {
        [Header("Campsite-Tooltip")] [Tooltip("Button that unlocks the campsite")] [SerializeField]
        private Button btnUnlock;

        [Tooltip("Campsite reference. used for unlocking the buildings")] [SerializeField]
        private Campsite campsite;

        /// <summary>
        /// Check if already unlocked, else setup button.
        /// </summary>
        protected override void Awake()
        {
            if (campsite.IsUnlocked)
            {
                gameObject.SetActive(false);
                return;
            }

            base.Awake();
            btnUnlock.onClick.AddListener(campsite.UnlockBuildingBlueprints);
        }

        private void OnEnable() => campsite.OnUnlock += OnUnlock;

        private void OnDisable() => campsite.OnUnlock -= OnUnlock;

        /// <summary>
        /// Simply deactivate this.
        /// </summary>
        private void OnUnlock() => gameObject.SetActive(false);
    }
}