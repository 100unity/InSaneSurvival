using System.Collections;
using System.Collections.Generic;
using Inventory;
using Managers;
using UnityEngine;
using Utils;

namespace Interactables
{
    public class Harvestable : Interactable
    {
        [Tooltip("The items being dropped on a successful harvest.")] [SerializeField]
        private List<ItemResourceData> drops;

        [Tooltip("Time in seconds needed to harvest this resource.")] [SerializeField]
        private float gatherTime;

        private double _gatherTimePassed;

        [Tooltip("Should this resource be only harvestable once?")] [SerializeField]
        private bool destroyAfterHarvest;

        private GameObject _parent;

        [Tooltip("Time in seconds needed until this resource is allowed to respawn again.")] [SerializeField]
        private float respawnTime;

        [SerializeField] [HideInInspector] private bool isRespawning;
        [SerializeField] [HideInInspector] private double respawnTimePassed;

        [Tooltip("The GameObject that is supposed to replace this resource while it respawns.")] [SerializeField]
        private GameObject replacement;

        [Tooltip("Select ability needed in order to harvest this resource.")] [SerializeField]
        private Equipable.EquipableAbility neededAbility;

        private MeshRenderer _ownMeshRenderer;
        private BoxCollider _ownCollider;
        private MeshRenderer _replacementMeshRenderer;
        private Camera _mainCam;

        private void Awake()
        {
            _mainCam = Camera.main;

            _ownMeshRenderer = GetComponent<MeshRenderer>();
            _ownCollider = GetComponent<BoxCollider>();

            // TODO: Fix this or find another solution
            float numberToUse = 0;
            float x = _ownCollider.size.x;
            float z = _ownCollider.size.z;
            float offset = 1.5f;
            if (x > z)
            {
                numberToUse = x;
            }
            else
            {
                numberToUse = z;
            }

            SetRadius((numberToUse * offset));

            _replacementMeshRenderer = replacement.GetComponent<MeshRenderer>();

            if (destroyAfterHarvest) _parent = transform.parent.gameObject;

            // if item was respawning before save, keep it respawning
            if (isRespawning) CoroutineManager.Instance.WaitForSeconds(1.0f / 60.0f, () => StartCoroutine(Respawn()));
        }

        /// <summary>
        /// Checks if the player has the ability to harvest this resource.
        /// If so, it resets the <see cref="_gatherTimePassed"/> and starts the <see cref="Gather"/> coroutine.
        /// </summary>
        public override void Interact()
        {
            Equipable equipped = InventoryManager.Instance.CurrentlyEquippedItem;
            if (equipped == null)
            {
                Debug.Log("Missing ability/No item equipped.");
                return;
            }

            if (!equipped.HasAbility(neededAbility))
            {
                // show that ability is missing
                Debug.Log("Missing ability");
                return;
            }

            _gatherTimePassed = 0;
            CoroutineManager.Instance.WaitForSeconds(1.0f / 60.0f, () => StartCoroutine(Gather()));
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
            while (HasInteracted && (_gatherTimePassed < gatherTime))
            {
                _gatherTimePassed += Time.deltaTime;
                if (_gatherTimePassed >= gatherTime)
                {
                    AddItems();
                    if (destroyAfterHarvest) GameObject.Destroy(_parent);
                    else
                    {
                        CoroutineManager.Instance.WaitForSeconds(1.0f / 60.0f, () => StartCoroutine(Respawn()));
                    }
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
                        // Item could not be added to the inventory, drop it to the ground or show an indicator for no space
                        // For now, just skip to the next item(if there is any) and see if this one can be added
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Increases the <see cref="_respawnTimePassed"/> to the limit of <see cref="respawnTime"/>.
        /// When that limit is reached and the resource is not in the camera of the player,
        /// it turns its graphics back on.
        /// </summary>
        private IEnumerator Respawn()
        {
            isRespawning = true;
            _ownMeshRenderer.enabled = false;
            _ownCollider.enabled = false;
            _replacementMeshRenderer.enabled = true;
            while (respawnTimePassed < respawnTime || _ownMeshRenderer.InFrustum(_mainCam))
            {
                respawnTimePassed += Time.deltaTime;
                yield return null;
            }

            isRespawning = false;
            respawnTimePassed = 0;

            _ownMeshRenderer.enabled = true;
            _ownCollider.enabled = true;
            _replacementMeshRenderer.enabled = false;
        }
    }
}