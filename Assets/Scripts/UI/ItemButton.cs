using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemButton : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameLabel;
        [SerializeField] private TextMeshProUGUI countLabel;

        private int _count;

        public Image Icon => icon;
        public TextMeshProUGUI NameLabel => nameLabel;

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                countLabel.text = _count.ToString();
            }
        }
    }
}