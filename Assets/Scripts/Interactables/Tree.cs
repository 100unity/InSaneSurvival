using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using Managers;
using Utils;

namespace Interactables
{
    public class Tree : Interactable
    {
        [SerializeField] private List<Item> drops;
        [SerializeField] private List<int> quantities;

        [SerializeField] private float gatherTime;
        [SerializeField] private float respawnTime;

        [SerializeField] private GameObject replacement;
        [SerializeField] private Camera mainCam;
        

        private Dictionary<Item, int> _itemsWithQuantities;

        private void Awake()
        {
            _itemsWithQuantities = new Dictionary<Item, int>();
            for (int i = 0; i < drops.Count; i++)
            {
                _itemsWithQuantities.Add(drops[i], quantities[i]);
            }
        }

        private double _timePassed;
        
        public override void Interact()
        {
            Debug.Log("Interact with tree");
            _timePassed = 0;
            CoroutineManager.Instance.WaitForOneFrame(() => StartCoroutine(Gather()));
            // Start a timer
            // Check that while the timer runs, the player keeps interacting
            // If enough time has passed,
            // give item(s) to player / drop them on the ground
            // remove/make the tree invisible and start respawn timer
            // after respawn timer 
        }

        private IEnumerator Gather()
        {
            while (HasInteracted && (_timePassed < gatherTime)) // making sure that gathering only happens if the player is standing still
            { 
                _timePassed += Time.deltaTime;
                if (_timePassed >= gatherTime)
                {
                    AddItems();
                    GetComponent<MeshRenderer>().enabled = false; // save references for performance
                    GetComponent<BoxCollider>().enabled = false;
                    replacement.GetComponent<MeshRenderer>().enabled = true;
                    StartCoroutine("Respawn"); // use coroutine manager

                }
                yield return null;
            }
        }

        private void AddItems()
        {
            foreach (KeyValuePair<Item, int> entry in _itemsWithQuantities)
            {
                InventoryManager.Instance.ItemHandler.AddItem(entry.Key, entry.Value);
            }
        }

        private double _respawnTimePassed;

        private Renderer _renderer;
        private Collider _coll;
        
        private IEnumerator Respawn()
        {
            _renderer = GetComponent<MeshRenderer>();
            _coll = GetComponent<BoxCollider>();
            
            while (_respawnTimePassed < respawnTime || _renderer.InFrustum(mainCam))
            {
                _respawnTimePassed += Time.deltaTime;
                yield return null;
            }

            _respawnTimePassed = 0;

            _renderer.enabled = true;
            _coll.enabled = true;
            replacement.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
