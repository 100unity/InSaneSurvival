using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GridLayout : GridLayoutGroup
    {
        public void SwitchElements(GameObject first, GameObject second)
        {
            int firstPos = first.transform.GetSiblingIndex();
            int secondPos = second.transform.GetSiblingIndex();
            
            first.transform.SetSiblingIndex(secondPos);
            second.transform.SetSiblingIndex(firstPos);
        }
    }
}