using System;
using Constants;
using Entity.Player;
using GameTime;
using Managers;
using UnityEngine;

namespace Buildings
{
    [RequireComponent(typeof(Collider))]
    public class Campfire : MonoBehaviour
    {
        [Tooltip("The amount of damage the fire deals to the player")] [SerializeField]
        private int damage;

        [Tooltip("The interval in which to apply the damage to the player")] [SerializeField]
        private float damageInterval;

        [Tooltip("The light of the fire.")]
        [SerializeField]
        private GameObject campfireFlames;

        private float _timer;
        private PlayerState _playerState;

        private void Awake() => _playerState = PlayerManager.Instance.GetPlayerState();
        
        private void OnEnable()
        {
            Clock.SunRise += DisableLight;
            Clock.SunSet += EnableLight;
        }

        private void OnDisable()
        {
            Clock.SunRise -= DisableLight;
            Clock.SunSet -= EnableLight;
        }

        private void DisableLight()
        {
            campfireFlames.SetActive(false);
        }

        private void EnableLight()
        {
            campfireFlames.SetActive(true);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag(Consts.Tags.PLAYER)) return;

            _timer += Time.deltaTime;
            if (_timer < damageInterval) return;

            _playerState.Hit(damage);
            _timer = 0;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(Consts.Tags.PLAYER)) return;
            _timer = 0;
        }
    }
}