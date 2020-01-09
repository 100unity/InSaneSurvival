using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Buildings
{
    public class CampsiteTooltip : ObjectTooltip
    {
        [Header("Campsite-Tooltip")] [SerializeField]
        private Button btnUnlock;

        [SerializeField] private Campsite campsite;


        protected override void Awake()
        {
            base.Awake();
            // If already unlocked disable this.
            if (campsite.IsUnlocked)
            {
                gameObject.SetActive(false);
                return;
            }

            btnUnlock.onClick.AddListener(() =>
            {
                campsite.UnlockBuildingBlueprints();
                gameObject.SetActive(false);
            });
        }
    }
}