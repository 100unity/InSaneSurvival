﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using Managers;
using Utils;

namespace Interactables
{
    public class Harvestable : Interactable
    {
        [Tooltip("The items being dropped on a successful harvest.")]
        [SerializeField] 
        private List<Item> drops;
        
        [Tooltip("How much of each item you get (the first item in the drops list is connected to the first quantity in this list.")]
        [SerializeField]
        private List<int> quantities;

        [Tooltip("Time in seconds needed to harvest this resource.")]
        [SerializeField] 
        private float gatherTime;
        
        private double _gatherTimePassed;
        
        [Tooltip("Time in seconds needed until this resource is allowed to respawn again.")]
        [SerializeField] 
        private float respawnTime;
        
        private double _respawnTimePassed;

        [Tooltip("The GameObject that is supposed to replace this resource while it respawns.")]
        [SerializeField] 
        private GameObject replacement;
        
        [Tooltip("Need a camera reference so resources don't respawn while the player can see them.")]
        [SerializeField] 
        private Camera mainCam;

        private MeshRenderer _ownMeshRenderer;
        private BoxCollider _ownCollider;
        private MeshRenderer _replacementMeshRenderer;

        private Dictionary<Item, int> _itemsWithQuantities;

        private void Awake()    
        {
            if (mainCam == null) // in case it was forgotten to set via the editor
            {
                mainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            }
            _ownMeshRenderer = GetComponent<MeshRenderer>();
            _ownCollider = GetComponent<BoxCollider>();
            _replacementMeshRenderer = replacement.GetComponent<MeshRenderer>();
            
            _itemsWithQuantities = new Dictionary<Item, int>();
            for (int i = 0; i < drops.Count; i++)
            {
                _itemsWithQuantities.Add(drops[i], quantities[i]);
            }
        }

        /// <summary>
        /// Resets the <see cref="_gatherTimePassed"/> and starts the <see cref="Gather"/> coroutine.
        /// </summary>
        public override void Interact()
        {
            _gatherTimePassed = 0;
            CoroutineManager.Instance.WaitForOneFrame(() => StartCoroutine(Gather()));
        }

        /// <summary>
        /// Increases the <see cref="_gatherTimePassed"/> to the limit of <see cref="gatherTime"/> as long as the
        /// player keeps interacting.
        /// If enough time has passed, it calls <see cref="AddItems"/>,
        /// replaces the resource with the <see cref="replacement"/>
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
                    _ownMeshRenderer.enabled = false; 
                    _ownCollider.enabled = false; 
                    _replacementMeshRenderer.enabled = true;
                    CoroutineManager.Instance.WaitForOneFrame(() => StartCoroutine(Respawn())); 

                }
                yield return null;
            }
        }

        /// <summary>
        /// Adds each item with their respective quantity to the player's inventory.
        /// </summary>
        private void AddItems()
        {
            foreach (KeyValuePair<Item, int> entry in _itemsWithQuantities)
            {
                InventoryManager.Instance.ItemHandler.AddItem(entry.Key, entry.Value);
            }
        }

        /// <summary>
        /// Increases the <see cref="_respawnTimePassed"/> to the limit of <see cref="respawnTime"/>.
        /// When that limit is reached and the resource is not in the camera of the player,
        /// it turns its graphics back on.
        /// </summary>
        private IEnumerator Respawn()
        {
            
            while (_respawnTimePassed < respawnTime || _ownMeshRenderer.InFrustum(mainCam))
            {
                _respawnTimePassed += Time.deltaTime;
                yield return null;
            }

            _respawnTimePassed = 0;

            _ownMeshRenderer.enabled = true;
            _ownCollider.enabled = true;
            _replacementMeshRenderer.enabled = false;
        }
    }
}

