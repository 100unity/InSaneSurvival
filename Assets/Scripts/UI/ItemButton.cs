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

        private AUsable item;

        private Inventory inventory;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        public AUsable Item
        {
            get => item;
            set
            {
                item = value;
            }
        }

        public Inventory Inventory
        {
            get => inventory;
            set
            {
                inventory = value;
            }
        }

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                countLabel.text = _count.ToString();
            }
        }

        public void OnClick() 
        {
            if (item.Use()) 
            {
                if (item is Consumable) inventory.Remove(item);
                //else if (item is Equipable) 
            } 
        }
    }
}