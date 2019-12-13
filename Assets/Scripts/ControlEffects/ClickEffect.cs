using UnityEngine;

namespace ControlEffects
{

    public class ClickEffect : MonoBehaviour
    {
        [SerializeField] private Transform projector;
        [SerializeField] private float movementSpeed;

        private bool _moveUp;

        private void FixedUpdate()
        {
            if (_moveUp)
            {
                if (projector.localPosition.y <= 5)
                    projector.localPosition += Time.fixedDeltaTime * movementSpeed * Vector3.up;
                else
                    Destroy(gameObject);
            }
            else
            {
                if (projector.localPosition.y > 0.5)
                    projector.localPosition += Time.fixedDeltaTime * movementSpeed * Vector3.down;
                else
                    _moveUp = true;
            }
        }
    }
}