using Constants;
using Entity.Player;
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

        private float _timer;

        private void OnTriggerStay(Collider other)
        {
            if (!other.tag.Equals(Consts.Tags.PLAYER)) return;
            
            _timer += Time.deltaTime;
            if (_timer < damageInterval) return;
            
            other.GetComponent<PlayerState>().Hit(damage);
            _timer = 0;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals(Consts.Tags.PLAYER))
                _timer = 0;
        }
    }
}