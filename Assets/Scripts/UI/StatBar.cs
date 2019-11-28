using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatBar : MonoBehaviour
    {
        //public string SpriteLocation = "Sprites/PlayerHungerStates_Sprite";
    
        private int _statusValue = 100;

        [SerializeField] private Sprite[] barStates;

        private Image[] _images;

        private Animator _animator;
        private Image _image;
    
    
        // Start is called before the first frame update
        private void Awake()
        {
            _animator = gameObject.GetComponent<Animator>();
            _image = gameObject.GetComponent<Image>();
        }
    
        public void UpdateBar(int newValue)
        {
            //int newValue = value;
            int oldValue = _statusValue;

            if (newValue > 100 || newValue < 0)
            {
                Debug.Log("Status bar value has to be between 0 and 100");
            }
            else
            {
                ChangeBarAnimation(newValue, oldValue);
                _statusValue = newValue;
                ChangeSprite();
            }
        } 
    
        private void ChangeBarAnimation(int newValue, int oldValue)
        {
            if (newValue > oldValue)
            {
                //_animator = gameObject.GetComponent<Animator>();
                if (_animator != null)
                {
                    _animator.Play("UIIncreaseStat");

                }
            }
            else
            {
                //_animator = gameObject.GetComponent<Animator>();
                if (_animator != null)
                {
                    _animator.Play("UIDecreaseStat");

                }
            }
        }

        private void ChangeSprite()
        {
            _image.sprite = barStates[_statusValue/10];
        }
    
    
    }
}
                     