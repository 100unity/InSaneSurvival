using System.Collections;
using System.Collections.Generic;
using Constants;
using Entity.Player;
using Inventory;
using Managers;
using UI;
using UnityEngine;
using Utils;

namespace Interactables
{
    [System.Serializable]
    public class Harvestable : Interactable
    {
        [Tooltip("Whether the scale should be checked.")]
        [SerializeField]
        private bool checkScale;

        [Tooltip("The maximum scale of the item in order to be gatherable.")]
        [SerializeField]
        private float maxScale;

        [Tooltip("The items being dropped on a successful harvest.")] [SerializeField]
        private List<ItemResourceData> drops;

        [Tooltip("Time in seconds needed to harvest this resource.")] [SerializeField]
        private float gatherTime;

        [Tooltip("Should this resource be only harvestable once?")] [SerializeField]
        protected bool destroyAfterHarvest;

        [Tooltip("Time in seconds needed until this resource is allowed to respawn again.")] [SerializeField]
        protected float respawnTime;

        [Tooltip("The GameObject that is supposed to replace this resource while it respawns.")] [SerializeField]
        private GameObject replacement;

        [Tooltip("Select ability needed in order to harvest this resource.")] [SerializeField]
        private Equipable.EquipableAbility neededAbility;

        [SerializeField] [HideInInspector] protected bool isRespawning;
        [SerializeField] [HideInInspector] protected double respawnTimePassed;
        
        private bool _notGatherable;

        protected GameObject Parent;
        protected Camera MainCam;
        protected Collider OwnCollider;

        private double _gatherTimePassed;
        private MeshRenderer _ownMeshRenderer;
        private MeshRenderer _replacementMeshRenderer;
        private PlayerController _playerController;

        protected virtual void Awake()
        {
            MainCam = Camera.main;

            OwnCollider = GetComponent<Collider>();
            _ownMeshRenderer = GetComponent<MeshRenderer>();
            _playerController = PlayerManager.Instance.GetPlayerController();

            // if scale should be checked, set _notGatherable
            if (checkScale)
                _notGatherable = transform.localScale.x > maxScale || transform.localScale.y > maxScale || transform.localScale.z > maxScale;

            if (destroyAfterHarvest)
                Parent = transform.parent.gameObject;
            else 
                _replacementMeshRenderer = replacement.GetComponent<MeshRenderer>();

            // If item was respawning before save, keep it respawning
            if (isRespawning) 
                CoroutineManager.Instance.WaitForSeconds(1.0f / 60.0f, () => StartCoroutine(Respawn()));
        }

        public bool IsRespawning => isRespawning;
        public double RespawnTimePassed => respawnTimePassed;

        public void SetFromSave(bool isRespawning, double respawnTimePassed)
        {
            this.isRespawning = isRespawning;
            this.respawnTimePassed = respawnTimePassed;

            if (this.isRespawning)
                CoroutineManager.Instance.WaitForSeconds(1.0f / 60.0f, () => StartCoroutine(Respawn()));
        }

        /// <summary>
        /// Checks if the player has the ability to harvest this resource.
        /// If so, it resets the <see cref="_gatherTimePassed"/> and starts the <see cref="Gather"/> coroutine.
        /// </summary>
        public override void Interact()
        {
            Equipable equipped = InventoryManager.Instance.CurrentlyEquippedItem;

            if (_notGatherable)
                return;

            if (neededAbility == Equipable.EquipableAbility.None ||
                equipped != null && equipped.HasAbility(neededAbility))
            {
                _gatherTimePassed = 0;
                _playerController.SetAnimationFloat(Consts.Animation.INTERACT_SPEED_FLOAT, 1 / gatherTime);
                _playerController.TriggerAnimation(Consts.Animation.INTERACT_TRIGGER);
                CoroutineManager.Instance.WaitForSeconds(1.0f / 60.0f, () => StartCoroutine(Gather()));
            }
            else
                CoroutineManager.Instance.WaitForSeconds(1, UIManager.Instance.SpeechBubble.ShowErrorSpeechBubble);
        }

        /// <summary>
        /// Increases the <see cref="_gatherTimePassed"/> to the limit of <see cref="gatherTime"/> as long as the
        /// player keeps interacting.
        /// If enough time has passed, it calls <see cref="AddItems"/>,
        /// and if the resource is not supposed to be destroyed after the successful harvest,
        /// it replaces the resource with the <see cref="replacement"/>
        /// and starts the coroutine <see cref="Respawn"/>.
        /// </summary>
        private IEnumerator Gather()
        {
            while (HasInteracted && _gatherTimePassed < gatherTime)
            {
                _gatherTimePassed += Time.deltaTime;

                if (_gatherTimePassed >= gatherTime)
                {
                    AddItems();
                    if (InventoryManager.Instance.CurrentlyEquippedItemButton != null)
                        InventoryManager.Instance.CurrentlyEquippedItemButton.IncreaseUses();
                    if (destroyAfterHarvest)
                        Destroy(Parent);
                    else
                        CoroutineManager.Instance.WaitForSeconds(1.0f / 60.0f, () => StartCoroutine(Respawn()));
                }

                yield return null;
            }
        }

        /// <summary>
        /// Adds each item with their respective quantity to the player's inventory as long as there is space.
        /// </summary>
        private void AddItems()
        {
            foreach (ItemResourceData drop in drops)
            {
                for (int i = 0; i < drop.amount; i++)
                {
                    if (!InventoryManager.Instance.AddItem(drop.item))
                    {
                        // TODO: Drop item to the ground
                        // For now, just skip to the next item(if there is any) and see if this one can be added
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Increases the <see cref="respawnTimePassed"/> to the limit of <see cref="respawnTime"/>.
        /// When that limit is reached and the resource is not in the camera of the player,
        /// it turns its graphics back on.
        /// </summary>
        protected virtual IEnumerator Respawn()
        {
            _ownMeshRenderer.enabled = false;
            OwnCollider.enabled = false;
            _replacementMeshRenderer.enabled = true;

            isRespawning = true;

            while (respawnTimePassed < respawnTime || _ownMeshRenderer.InFrustum(MainCam))
            {
                respawnTimePassed += Time.deltaTime;
                yield return null;
            }

            isRespawning = false;
            respawnTimePassed = 0;

            _ownMeshRenderer.enabled = true;
            OwnCollider.enabled = true;
            _replacementMeshRenderer.enabled = false;
        }
    }
}
