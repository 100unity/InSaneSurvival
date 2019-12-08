using UnityEngine;

namespace ControlEffects
{
    public class ClickEffect : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;
     
        private bool _moveUp;
     
        private void FixedUpdate()
        {
            if (_moveUp)
            {
                if (transform.position.y < 5)
                    transform.position += Time.fixedDeltaTime * movementSpeed * Vector3.up;
                else
                    Destroy(gameObject);
            }
            else
            {
                if (transform.position.y > 0.5)
                    transform.position += Time.fixedDeltaTime * movementSpeed * Vector3.down;
                else
                    _moveUp = true;
            }
        }
    }
}