using AbstractClasses;
using Entity.Player;
using UnityEngine;

public class KillzoneController : MonoBehaviour
{
    /// <summary>
    /// Kill if player leaves zone.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            other.GetComponent<Movable>().DisableMovement();
            other.GetComponent<Damageable>().Die();
        }
    }
}
